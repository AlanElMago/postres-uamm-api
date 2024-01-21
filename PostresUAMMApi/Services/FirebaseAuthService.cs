using Firebase.Auth;

namespace PostresUAMMApi.Services;

public interface IFirebaseAuthService
{
    Task<UserCredential> SignUp(string email, string password);

    Task<string> Login(string email, string password);

    void SignOut();
}

public class FirebaseAuthService(FirebaseAuthClient firebaseAuthClient) : IFirebaseAuthService
{
    private readonly FirebaseAuthClient _firebaseAuthClient = firebaseAuthClient;

    public async Task<UserCredential> SignUp(string email, string password)
    {
        UserCredential userCredential = await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, password);

        // return await userCredential.User.GetIdTokenAsync();
        return userCredential;
    }

    public async Task<string> Login(string email, string password)
    {
        UserCredential userCredential = await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);

        return await userCredential.User.GetIdTokenAsync();
    }

    public void SignOut() => _firebaseAuthClient.SignOut();
}
