using Microsoft.AspNetCore.Identity;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IJwtService _jwtService;
        private readonly PasswordHasher<Customer> _passwordHasher;

        public AuthService(ICustomerRepository customerRepository, IJwtService jwtService)
        {
            _customerRepository = customerRepository;
            _jwtService = jwtService;
            _passwordHasher = new PasswordHasher<Customer>();
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existingCustomer = await _customerRepository.GetByEmailAsync(request.Email);

            if(existingCustomer != null)
            {
                throw new InvalidOperationException("Email is already registered.");
            }

            var customer = new Customer
            {
                Name = request.Name,
                Email = request.Email,
            };

            customer.PasswordHash = _passwordHasher.HashPassword(customer, request.Password);

            var createdCustomer = await _customerRepository.CreateAsync(customer);

            var token = _jwtService.GenerateToken(
                createdCustomer.Id,
                createdCustomer.Name,
                createdCustomer.Email
                );

            return new AuthResponse
            {
                CustomerId = createdCustomer.Id,
                Name = createdCustomer.Name,
                Email = createdCustomer.Email,
                Token = token
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var customer = await _customerRepository.GetByEmailAsync(request.Email);

            if(customer == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");

            }

            var result = _passwordHasher.VerifyHashedPassword(
                customer,
                customer.PasswordHash,
                request.Password
                );

            if(result == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var token = _jwtService.GenerateToken(
                customer.Id,
                customer.Name,
                customer.Email
                );

            return new AuthResponse
            {
                CustomerId = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Token = token
            };
        }
    }
}
