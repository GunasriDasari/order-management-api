
namespace OrderManagement.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(int customerId, string name, string email);
    }
}
