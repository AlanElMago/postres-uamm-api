using Google.Cloud.Firestore;
using NUnit.Framework;
using PostresUAMMApi.Models;
using PostresUAMMApi.Repositories;
using PostresUAMMApi.Enums;

namespace PostresUAMMApi.Tests.Repositories;

public class UserRepositoryTests
{
    private FirestoreDb? _firestoreDb;
    private UserRepository? _userRepo;

    [SetUp]
    public void Setup()
    {
        string pathToFirebaseSecretKey = Path.Combine(
            "Secrets",
            "postres-uamm-firebase-firebase-adminsdk-ocw9l-fdf9dfbb56.json");

        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToFirebaseSecretKey);
        
        FirestoreDbBuilder builder = new() { ProjectId = "postres-uamm-firebase" };

        _firestoreDb = builder.Build();
        _userRepo = new UserRepository(_firestoreDb);
    }

    [Test]
    public async Task GetUsersAsync_ReturnsListOfUsers()
    {
        // Arrange

        // Act
        List<User> users = await _userRepo!.GetUsersAsync();

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
            FullName = "John Smith",
            Roles = [UserRoles.Baker, UserRoles.Seller]
        };

        // Act
        User newUser = await _userRepo!.AddUserAsync(user);

        // Assert
        Assert.That(newUser, Is.Not.Null);
        Assert.That(newUser, Is.InstanceOf<User>());
        Assert.That(newUser.FirebaseAuthUid, Is.EqualTo(user.FirebaseAuthUid));
        Assert.That(newUser.FullName, Is.EqualTo(user.FullName));
        Assert.That(newUser.Roles, Has.Member(UserRoles.Baker).And.Member(UserRoles.Seller));
    }

    [Test]
    public async Task GetUserAsync_ReturnsUser()
    {
        // Arrange
        string firebaseAuthUid = "DZUGstIm8XPehNBuBEHPTpwzT1w1";

        // Act
        User user = await _userRepo!.GetUserByFirebaseUidAsync(firebaseAuthUid);

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
        User user = _userRepo!.GetUserByFirebaseUidAsync(firebaseAuthUid).Result;

        user.FullName = "John Doe";
        user.Roles = [UserRoles.Admin, UserRoles.Customer];
        user.IsEnabled = false;

        if (user.Id is null)
        {
            throw new InvalidOperationException("User id is null");
        }

        // Act
        User updatedUser = await _userRepo!.UpdateUserAsync(user.Id, user);

        // Assert
        Assert.That(updatedUser, Is.Not.Null);
        Assert.That(updatedUser, Is.InstanceOf<User>());
        Assert.That(updatedUser.FirebaseAuthUid, Is.EqualTo(firebaseAuthUid));
        Assert.That(updatedUser.FullName, Is.EqualTo("John Doe"));
        Assert.That(updatedUser.Roles, Has.Member(UserRoles.Admin).And.Member(UserRoles.Customer));
        Assert.That(updatedUser.IsEnabled, Is.False);
    }
}
