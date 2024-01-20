using Google.Cloud.Firestore;
using NUnit.Framework;
using PostresUAMMApi.Models;
using PostresUAMMApi.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PostresUAMMApi.Tests.Repositories;

public class RoleRepositoryTests
{
    private FirestoreDb? _firestoreDb;
    private RoleRepository? _roleRepository;

    [SetUp]
    public void Setup()
    {
        // Set environmental variables
        string pathToFirebaseSecretKey = Path.Combine(
            "Secrets",
            "postres-uamm-firebase-firebase-adminsdk-ocw9l-fdf9dfbb56.json");

        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToFirebaseSecretKey);

        // Initialize FirestoreDb and RoleRepository
        string projectId = "postres-uamm-firebase";
        FirestoreDbBuilder builder = new() { ProjectId = projectId };

        _firestoreDb = builder.Build();
        _roleRepository = new RoleRepository(_firestoreDb);
    }

    [Test]
    public async Task GetRolesAsync_ReturnsListOfRoles()
    {
        // Arrange

        // Act
        List<Role> roles = await _roleRepository!.GetRolesAsync();

        // Assert
        Assert.That(roles, Is.Not.Null);
        Assert.That(roles, Is.InstanceOf<List<Role>>());
        Assert.That(roles.Count, Is.GreaterThan(0));
    }

    [Test]
    public async Task GetRoleAsync_ExistingRoleId_ReturnsRole()
    {
        // Arrange
        string roleId = "zwuGP4cf1i9MfWi9WsQ7";

        // Act
        Role? role = await _roleRepository!.GetRoleAsync(roleId);

        // Assert
        Assert.That(role, Is.Not.Null);
        Assert.That(role, Is.InstanceOf<Role>());
        Assert.That(role.Id, Is.EqualTo(roleId));
    }

    [Test]
    public void GetRoleAsync_NonExistingRoleId_ThrowsException()
    {
        // Arrange
        string roleId = "aaaaaaaaaaaaaaaaaaaa";

        // Act and Assert
        Assert.ThrowsAsync<Exception>(async () => await _roleRepository!.GetRoleAsync(roleId));
    }
}
