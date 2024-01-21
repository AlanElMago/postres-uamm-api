using Firebase.Auth;
using PostresUAMMApi.Models.Forms;
using System.Net.Http.Headers;

namespace PostresUAMMApi.Services;

public interface IFirebaseAuthService
{
    Task<UserCredential> SignUpAsync(string email, string password);

    Task<UserCredential> SignUpCustomerAsync(UserRegistrationForm form);

    Task<string> LoginAsync(string email, string password);

    void Logout();

    Task SendVerificationEmailAsync(UserCredential userCredential);
}

public class FirebaseAuthService(
    FirebaseAuthClient firebaseAuthClient,
    FirebaseAuthConfig firebaseAuthConfig) : IFirebaseAuthService
{
    private readonly FirebaseAuthClient _firebaseAuthClient = firebaseAuthClient;
    private readonly FirebaseAuthConfig _firebaseAuthConfig = firebaseAuthConfig;

    public async Task<UserCredential> SignUpAsync(string email, string password)
    {
        UserCredential userCredential = await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, password);

        await SendVerificationEmailAsync(userCredential);

        return userCredential;
    }

    public async Task<UserCredential> SignUpCustomerAsync(UserRegistrationForm form)
    {
        ArgumentNullException.ThrowIfNull(form);
        ArgumentNullException.ThrowIfNull(form.Email);
        ArgumentNullException.ThrowIfNull(form.Password);

        if (!form.Email.EndsWith("@alumnos.uat.edu.mx") && !form.Email.EndsWith("@docentes.uat.edu.mx"))
        {
            throw new ArgumentException("El correo electrónico debe ser de la UAT");
        }

        return await SignUpAsync(form.Email, form.Password);
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        UserCredential userCredential = await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);

        return await userCredential.User.GetIdTokenAsync();
    }

    public void Logout() => _firebaseAuthClient.SignOut();

    // Note: The domain "@alumnos.uat.edu.mx" seems to block the email verification.
    // TODO: Investigate why.
    public async Task SendVerificationEmailAsync(UserCredential userCredential)
    {
        using HttpClient httpClient = new();

        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        string userIdToken = await userCredential.User.GetIdTokenAsync();
        string requestUri = $"https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key={_firebaseAuthConfig.ApiKey}";
        StringContent stringContent = new(
            $@"{{""requestType"":""VERIFY_EMAIL"",""idToken"":""{userIdToken}""}}",
            new MediaTypeHeaderValue("application/json"));

        HttpResponseMessage response = await httpClient.PostAsync(requestUri, stringContent);

        response.EnsureSuccessStatusCode();
    }
}
