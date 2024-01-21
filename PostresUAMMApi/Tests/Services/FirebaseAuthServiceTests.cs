using Firebase.Auth;
using Firebase.Auth.Providers;
using NUnit.Framework;

using PostresUAMMApi.Services;

namespace PostresUAMMApi.Tests.Services;

public class FirebaseAuthServiceTests()
{
    private FirebaseAuthConfig? _firebaseAuthConfig;
    private FirebaseAuthService? _firebaseAuthService;

    [SetUp]
    public void Setup()
    {
        WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder();
        webApplicationBuilder.Configuration.AddUserSecrets("382d69e6-8486-43ba-a6b4-edb9cde00db0");

        // FirebaseWebApiKey is located in secrets.json
        string? firebaseWebApiKey = webApplicationBuilder.Configuration["FirebaseWebApiKey"];
        string firebaseProjectName = "postres-uamm-firebase";

        if (string.IsNullOrWhiteSpace(firebaseWebApiKey) ) {
            throw new Exception("FirebaseWebApiKey is null or empty");
        }

        _firebaseAuthConfig = new FirebaseAuthConfig
        {
            ApiKey = firebaseWebApiKey,
            AuthDomain = $"{firebaseProjectName}.firebaseapp.com",
            Providers = new FirebaseAuthProvider[]
            {
                new EmailProvider()
            }
        };

        FirebaseAuthClient firebaseAuthClient = new (_firebaseAuthConfig);
        FirebaseAuthConfig firebaseAuthConfig = new FirebaseAuthConfig()
        {
            ApiKey = firebaseWebApiKey,
            AuthDomain = $"{firebaseProjectName}.firebaseapp.com",
            Providers = new FirebaseAuthProvider[] { new EmailProvider() }
        };

        _firebaseAuthService = new(firebaseAuthClient, firebaseAuthConfig);
    }

    [Test]
    public async Task SignUp_ValidCredentials_ReturnsIdToken()
    {
        // Arrange
        string email = "test@example.com";
        string fullName = "Test User";
        string password = "password";

        // Act
        UserCredential userCredential = await _firebaseAuthService!.SignUpAsync(email, password, fullName);

        // Assert
        Assert.That(userCredential, Is.Not.Null);
    }

    [Test]
    public async Task Login_ValidCredentials_ReturnsIdToken()
    {
        // Arrange
        string email = "test@example.com";
        string password = "password";

        // Act
        string idToken = await _firebaseAuthService!.LoginAsync(email, password);

        // Assert
        Assert.That(idToken, Is.Not.Null);
    }

    [Test]
    public async Task SignOut_CallsFirebaseAuthClientSignOut()
    {
        // Arrange
        string email = "test@example.com";
        string password = "password";

        // Act
        await _firebaseAuthService!.LoginAsync(email, password);

        // Assert
        Assert.DoesNotThrow(() => _firebaseAuthService.Logout());
    }
}
