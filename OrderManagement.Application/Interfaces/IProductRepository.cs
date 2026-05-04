using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> CreateAsync(Product product);

        Task<IEnumerable<Product>> GetAllAsync();

        Task<Product?> GetByIdAsync(int id);
    }
}