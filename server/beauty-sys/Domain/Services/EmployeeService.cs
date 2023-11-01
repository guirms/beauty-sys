﻿using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Objects.Reponses;
using Domain.Objects.Requests;
using System.Xml.Linq;

namespace Domain.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
        }

        public async Task CreateEmployee(CreateEmployeeRequest createEmployeeRequest)
        {
            var employee = new Employee
            {
                Name = createEmployeeRequest.Name,
                Office = createEmployeeRequest.Office,
                Cpf = createEmployeeRequest.Cpf,
                Procedures = _mapper.ProjectTo<Procedure>(createEmployeeRequest.Procedures.AsQueryable()).ToList()
            };

            await _employeeRepository.SaveAsync(employee);
        }
        public async Task<EmployeeResponse> GetById(int id)
        {
            var employee = await _employeeRepository.GetById(id) ?? throw new InvalidOperationException("Nenhum funcionário encontrado.");

            var employeeResponse = new EmployeeResponse(employee.EmployeeId, employee.Name, employee.Cpf, employee.Office);

            return employeeResponse;
        }
        public async Task<List<EmployeeResponse>> GetEmployees()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeeResponse = new List<EmployeeResponse>();

            foreach (var employee in employees)
            {
                employeeResponse.Add(new EmployeeResponse(employee.EmployeeId, employee.Name, employee.Cpf, employee.Office));
            }

            return employeeResponse;
        }

        public async Task UpdateEmployee(int id, UpdateEmployeeRequest updateEmployeeRequest)
        {
            var employee = await _employeeRepository.GetById(id) ?? throw new InvalidOperationException("Nenhum cliente encontrado.");

            if (updateEmployeeRequest.Name != null)
                employee.Name = updateEmployeeRequest.Name;

            if (updateEmployeeRequest.Office != null)
                employee.Office = updateEmployeeRequest.Office;

            employee.UpdatedAt = DateTime.Now;

            await _employeeRepository.UpdateAsync(employee);
        }
    }
}
