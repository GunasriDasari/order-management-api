using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Application.DTOs;

namespace OrderManagement.API.Controllers
{
    [ApiController]
    [Route("customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerRequest request)
        {
            var customer = new Customer
            {
                Name = request.Name,
                Email = request.Email
            };

            var createdCustomer = await _customerRepository.CreateAsync(customer);

            var response = new CustomerResponse
            {
                Id = createdCustomer.Id,
                Name = createdCustomer.Name,
                Email = createdCustomer.Email
            };

            return CreatedAtAction(nameof(GetCustomerById), new { id = response.Id }, response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if(customer == null)
            {
                return NotFound();
            }

            var response = new CustomerResponse
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email
            };

            return Ok(response);
        }
    }
}
