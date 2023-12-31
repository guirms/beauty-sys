﻿using Application.Interfaces;
using Domain.Interfaces.Services;
using Domain.Objects.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController, Authorize]
    [Route("Customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ICustomerAppService _customerAppService;

        public CustomerController(ICustomerService customerService, ICustomerAppService customerAppService)
        {
            _customerService = customerService;
            _customerAppService = customerAppService;
        }

        [HttpGet("GetCustomers")]
        public IActionResult GetCustomers(int? id, string? name, int currentPage = 1, int takeQuantity = 10)
        {
            try
            {
                return Ok(_customerService.GetCustomers(currentPage, takeQuantity, id, name));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteCustomer/{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                await _customerService.DeleteCustomer(id);

                return Ok("Cliente deletado com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("UpdateCustomer")]
        public async Task<IActionResult> UpdateCustomer(UpdateCustomerRequest updateCustomerRequest)
        {
            try
            {
                await _customerAppService.UpdateCustomer(updateCustomerRequest);

                return Ok("Cliente atualizado com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SaveCustomer")]
        public async Task<IActionResult> SaveCustomer(CreateCustomerRequest createCustomerRequest)
        {
            try
            {
                await _customerAppService.CreateCustomer(createCustomerRequest);

                return Ok("Cliente salvo com sucesso");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
