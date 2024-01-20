using Google.Cloud.Firestore;
using NUnit.Framework;
using PostresUAMMApi.Models;
using PostresUAMMApi.Repositories;

namespace PostresUAMMApi.Tests.Repositories;

public class UserRepositoryTests
{
    private FirestoreDb? _firestoreDb;
    private UserRepository? _userRepository;
    private RoleRepository? _roleRepository;

    [SetUp]
    public void Setup()
    {
        // Set environmental variables
        string pathToFirebaseSecretKey = Path.Combine(
            "Secrets",
            "postres-uamm-firebase-firebase-adminsdk-ocw9l-fdf9dfbb56.json");

        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToFirebaseSecretKey);

        // Initialize FirestoreDb, UserRepository, and RoleRepository
        string projectId = "postres-uamm-firebase";
        FirestoreDbBuilder builder = new() { ProjectId = projectId };

        _firestoreDb = builder.Build();
        _roleRepository = new RoleRepository(_firestoreDb);
        _userRepository = new UserRepository(_firestoreDb, _roleRepository);
    }

    [Test]
    public async Task GetUsersAsync_ReturnsListOfUsers()
    {
        // Arrange

        // Act
        List<User> users = await _userRepository!.GetUsersAsync();

        // Assert
        Assert.That(users, Is.Not.Null);
        Assert.That(users, Is.InstanceOf<List<User>>());
        Assert.That(users.Count, Is.GreaterThan(0));
    }
}
