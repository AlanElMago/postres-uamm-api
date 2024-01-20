using Google.Cloud.Firestore;
using NUnit.Framework;
using PostresUAMMApi.Models;
using PostresUAMMApi.Repositories;
using PostresUAMMApi.Enums;

namespace PostresUAMMApi.Tests.Repositories;

public class UserRepositoryTests
{
    private FirestoreDb? _firestoreDb;
    private UserRepository? _userRepository;

    [SetUp]
    public void Setup()
    {
        string pathToFirebaseSecretKey = Path.Combine(
            "Secrets",
            "postres-uamm-firebase-firebase-adminsdk-ocw9l-fdf9dfbb56.json");

        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToFirebaseSecretKey);
        
        FirestoreDbBuilder builder = new() { ProjectId = "postres-uamm-firebase" };

        _firestoreDb = builder.Build();
        _userRepository = new UserRepository(_firestoreDb);
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

    [Test]
    public async Task AddUserAsync_ReturnsNewUser()
    {
        // Arrange
        User user = new()
        {
            FirebaseAuthUid = "DZUGstIm8XPehNBuBEHPTpwzT1w1",
            Roles = [RolesEnum.Baker, RolesEnum.Seller]
        };

        // Act
        User newUser = await _userRepository!.AddUserAsync(user);

        // Assert
        Assert.That(newUser, Is.Not.Null);
        Assert.That(newUser, Is.InstanceOf<User>());
        Assert.That(newUser.FirebaseAuthUid, Is.EqualTo(user.FirebaseAuthUid));
    }

    [Test]
    public async Task GetUserAsync_ReturnsUser()
    {
        // Arrange
        string firebaseAuthUid = "DZUGstIm8XPehNBuBEHPTpwzT1w1";

        // Act
        User user = await _userRepository!.GetUserByFirebaseUidAsync(firebaseAuthUid);

        // Assert
        Assert.That(user, Is.Not.Null);
        Assert.That(user, Is.InstanceOf<User>());
        Assert.That(user.FirebaseAuthUid, Is.EqualTo(firebaseAuthUid));
    }

    [Test]
    public async Task UpdateUserAsync_ReturnsUpdatedUser()
    {
        // Arrange
        string firebaseAuthUid = "DZUGstIm8XPehNBuBEHPTpwzT1w1";
        User user = _userRepository!.GetUserByFirebaseUidAsync(firebaseAuthUid).Result;

        user.Roles = [RolesEnum.Admin, RolesEnum.Customer];
        user.IsEnabled = false;

        if (user.Id is null)
        {
            throw new InvalidOperationException("User id is null");
        }

        // Act
        User updatedUser = await _userRepository!.UpdateUserAsync(user.Id, user);

        // Assert
        Assert.That(updatedUser, Is.Not.Null);
        Assert.That(updatedUser, Is.InstanceOf<User>());
        Assert.That(updatedUser.FirebaseAuthUid, Is.EqualTo(firebaseAuthUid));
        Assert.That(updatedUser.Roles, Has.Member(RolesEnum.Admin).And.Member(RolesEnum.Customer));
        Assert.That(updatedUser.IsEnabled, Is.False);
    }
}
