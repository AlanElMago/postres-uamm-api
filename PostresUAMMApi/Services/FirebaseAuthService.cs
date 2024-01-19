using Firebase.Auth;

namespace PostresUAMMApi.Services;

public class FirebaseAuthService(FirebaseAuthClient firebaseAuthClient)
{
    private readonly FirebaseAuthClient _firebaseAuthClient = firebaseAuthClient;

    public async Task<string> SignUp(string email, string password)
    {
        UserCredential userCredential = await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, password);

        return await userCredential.User.GetIdTokenAsync();
    }

    public async Task<string> Login(string email, string password)
    {
        UserCredential userCredential = await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);

        return await userCredential.User.GetIdTokenAsync();
    }

    public void SignOut() => _firebaseAuthClient.SignOut();
}
