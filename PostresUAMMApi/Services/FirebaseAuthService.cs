using Firebase.Auth;
using FirebaseAdmin.Auth;
using Newtonsoft.Json;
using PostresUAMMApi.Models.Forms;
using PostresUAMMApi.Repositories;
using System.Net.Http.Headers;

namespace PostresUAMMApi.Services;

public interface IFirebaseAuthService
{
    Task<UserCredential> SignUpAsync(string email, string password, string fullName);

    Task<UserCredential> SignUpCustomerAsync(UserRegistrationForm form);

    Task<string> LoginAsync(UserLoginForm form);

    void Logout();

    Task SendVerificationEmailAsync(UserCredential userCredential);
}

public class FirebaseAuthService(
    FirebaseAuthClient firebaseAuthClient,
    FirebaseAuthConfig firebaseAuthConfig,
    IUserRepository userRepository) : IFirebaseAuthService
{
    private readonly FirebaseAuthClient _firebaseAuthClient = firebaseAuthClient;
    private readonly FirebaseAuthConfig _firebaseAuthConfig = firebaseAuthConfig;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<UserCredential> SignUpAsync(string email, string password, string fullName)
    {
        UserCredential userCredential = await _firebaseAuthClient
            .CreateUserWithEmailAndPasswordAsync(email, password, fullName);

        await SendVerificationEmailAsync(userCredential);

        return userCredential;
    }

    public async Task<UserCredential> SignUpCustomerAsync(UserRegistrationForm form)
    {
        ArgumentNullException.ThrowIfNull(form);
        ArgumentException.ThrowIfNullOrWhiteSpace(form.Email);
        ArgumentException.ThrowIfNullOrWhiteSpace(form.Password);
        ArgumentException.ThrowIfNullOrWhiteSpace(form.FullName);

        if (!form.Email.EndsWith("@alumnos.uat.edu.mx") && !form.Email.EndsWith("@docentes.uat.edu.mx"))
        {
            throw new ArgumentException("El correo electrónico debe ser de la UAT");
        }

        return await SignUpAsync(form.Email, form.Password, form.FullName);
    }

    public async Task<string> LoginAsync(UserLoginForm form)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(form.Email);
        ArgumentException.ThrowIfNullOrWhiteSpace(form.Password);

        UserCredential userCredential = await _firebaseAuthClient
            .SignInWithEmailAndPasswordAsync(form.Email, form.Password);

        Models.User user = await _userRepository.GetUserByFirebaseUidAsync(userCredential.User.Uid);

        Dictionary<string, object>? userAsDictionary = JsonConvert
            .DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(user));

        string customToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(
            userCredential.User.Uid, userAsDictionary
        );

        return customToken;
    }

    public void Logout() => _firebaseAuthClient.SignOut();

    // Note: The domain "@alumnos.uat.edu.mx" seems to block the email verification.
    // TODO: Investigate why.
    public async Task SendVerificationEmailAsync(UserCredential userCredential)
    {
        using HttpClient httpClient = new();

        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        string userIdToken = await userCredential.User.GetIdTokenAsync();

        StringContent stringContent = new(
            content: $@"{{""requestType"":""VERIFY_EMAIL"",""idToken"":""{userIdToken}""}}",
            mediaType: new MediaTypeHeaderValue("application/json"));

        HttpResponseMessage response = await httpClient.PostAsync(
            requestUri: $"https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key={_firebaseAuthConfig.ApiKey}",
            content: stringContent);

        response.EnsureSuccessStatusCode();
    }
}
