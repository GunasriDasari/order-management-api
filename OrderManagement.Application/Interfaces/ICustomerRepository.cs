using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> CreateAsync(Customer customer);

        Task<Customer?> GetByIdAsync(int id);

        Task<Customer?> GetByEmailAsync(string email);
    }
}