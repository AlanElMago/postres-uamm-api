using Google.Cloud.Firestore;
using NUnit.Framework;
using PostresUAMMApi.Models;
using PostresUAMMApi.Repositories;
using PostresUAMMApi.Enums;

namespace PostresUAMMApi.Tests.Repositories;

public class CustomerRepositoryTests
{
    private FirestoreDb? _firestoreDb;
    private CustomerRepository? _customerRepo;

    [SetUp]
    public void Setup()
    {
        string pathToFirebaseSecretKey = Path.Combine(
            "Secrets",
            "postres-uamm-firebase-firebase-adminsdk-ocw9l-fdf9dfbb56.json");

        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToFirebaseSecretKey);
        
        FirestoreDbBuilder builder = new() { ProjectId = "postres-uamm-firebase" };

        _firestoreDb = builder.Build();
        _customerRepo = new CustomerRepository(_firestoreDb);
    }

    [Test]
    public async Task GetCustomersAsync_ReturnsListOfCustomers()
    {
        // Arrange

        // Act
        List<Customer> customers = await _customerRepo!.GetCustomersAsync();

        // Assert
        Assert.That(customers, Is.Not.Null);
        Assert.That(customers, Is.InstanceOf<List<Customer>>());
        Assert.That(customers.Count, Is.GreaterThan(0));
    }

    [Test]
    public async Task AddCustomerAsync_ReturnsNewCustomer()
    {
        // Arrange
        Customer customer = new()
        {
            UserId = "VK3pLwneEJhi08qX4Ig7",
            CustomerType = CustomerTypesEnum.Student,
            IsVerifiedByAdmin = false
        };

        // Act
        Customer newCustomer = await _customerRepo!.AddCustomerAsync(customer);

        // Assert
        Assert.That(newCustomer, Is.Not.Null);
        Assert.That(newCustomer, Is.InstanceOf<Customer>());
        Assert.That(newCustomer.UserId, Is.EqualTo(customer.UserId));
    }

    [Test]
    public async Task GetCustomerByUserIdAsync_ReturnsCustomer()
    {
        // Arrange
        string userId = "VK3pLwneEJhi08qX4Ig7";

        // Act
        Customer customer = await _customerRepo!.GetCustomerByUserIdAsync(userId);

        // Assert
        Assert.That(customer, Is.Not.Null);
        Assert.That(customer, Is.InstanceOf<Customer>());
        Assert.That(customer.UserId, Is.EqualTo(userId));
    }

    [Test]
    public async Task UpdateCustomerAsync_ReturnsUpdatedCustomer()
    {
        string userId = "VK3pLwneEJhi08qX4Ig7";
        Customer customer = await _customerRepo!.GetCustomerByUserIdAsync(userId);

        if (customer.Id is null)
        {
            throw new InvalidOperationException("customer id is null");
        }

        customer.CustomerType = CustomerTypesEnum.Teacher;
        customer.IsVerifiedByAdmin = true;

        // Act
        Customer updatedCustomer = await _customerRepo!.UpdateCustomerAsync(customer.Id, customer);

        // Assert
        Assert.That(updatedCustomer, Is.Not.Null);
        Assert.That(updatedCustomer, Is.InstanceOf<Customer>());
        Assert.That(updatedCustomer.UserId, Is.EqualTo(customer.UserId));
        Assert.That(updatedCustomer.CustomerType, Is.EqualTo(customer.CustomerType));
        Assert.That(updatedCustomer.IsVerifiedByAdmin, Is.EqualTo(customer.IsVerifiedByAdmin));
    }
}
