using Google.Cloud.Firestore;
using PostresUAMMApi.Models;

namespace PostresUAMMApi.Repositories;

public interface ICustomerRepository
{
    Task<List<Customer>> GetCustomersAsync();

    Task<Customer> GetCustomerAsync(string id);

    Task<Customer> GetCustomerByUserIdAsync(string userId);

    Task<Customer> AddCustomerAsync(Customer customer);

    Task<Customer> UpdateCustomerAsync(string id, Customer customer);
}

public class CustomerRepository(FirestoreDb firestoreDb) : ICustomerRepository
{
    private readonly FirestoreDb _firestoreDb = firestoreDb;

    public async Task<List<Customer>> GetCustomersAsync()
    {
        Query customersCollQuery = _firestoreDb.Collection("customers");
        QuerySnapshot allCustomersQuerySnapshot = await customersCollQuery.GetSnapshotAsync();
        List<Customer> customers = allCustomersQuerySnapshot
            .Select(customerDocSnapshot => customerDocSnapshot.ConvertTo<Customer>())
            .ToList();

        return customers;
    }

    public async Task<Customer> GetCustomerAsync(string id)
    {
        DocumentSnapshot customerDocSnapshot = await _firestoreDb
            .Collection("customers")
            .Document(id)
            .GetSnapshotAsync();

        if (!customerDocSnapshot.Exists)
        {
            throw new InvalidOperationException($"Customer with id {id} does not exist in the database");
        }

        Customer customer = customerDocSnapshot.ConvertTo<Customer>();

        return customer;
    }

    public async Task<Customer> GetCustomerByUserIdAsync(string userId)
    {
        Query customerQuery = _firestoreDb.Collection("customers").WhereEqualTo("userId", userId);
        QuerySnapshot customerQuerySnapshot = await customerQuery.GetSnapshotAsync();

        if (customerQuerySnapshot.Count <= 0)
        {
            throw new InvalidOperationException($"Customer with userId {userId} does not exist in the database");
        }

        Customer customer = customerQuerySnapshot.Documents[0].ConvertTo<Customer>();

        return customer;
    }

    public async Task<Customer> AddCustomerAsync(Customer customer)
    {
        CollectionReference customersCollRef = _firestoreDb.Collection("customers");
        DocumentReference customerDocRef = await customersCollRef.AddAsync(customer);
        DocumentSnapshot customerDocSnapshot = await customerDocRef.GetSnapshotAsync();
        Customer newCustomer = customerDocSnapshot.ConvertTo<Customer>();

        return newCustomer;
    }

    public async Task<Customer> UpdateCustomerAsync(string id, Customer customer)
    {
        DocumentReference customerDocRef = _firestoreDb.Collection("customers").Document(id);
        await customerDocRef.SetAsync(customer, SetOptions.Overwrite);

        DocumentSnapshot customerDocSnapshot = await customerDocRef.GetSnapshotAsync();
        Customer updatedCustomer = customerDocSnapshot.ConvertTo<Customer>();

        return updatedCustomer;
    }
}
