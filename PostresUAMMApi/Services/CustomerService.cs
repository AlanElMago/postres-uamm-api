using PostresUAMMApi.Models;
using PostresUAMMApi.Repositories;

namespace PostresUAMMApi.Services;

public interface ICustomerService
{
    Task<Customer> RegisterCustomerAsync(Customer customer);
}

public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;

    public async Task<Customer> RegisterCustomerAsync(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);

        Customer newCustomer = await _customerRepository.AddCustomerAsync(customer);

        return newCustomer;
    }
}
