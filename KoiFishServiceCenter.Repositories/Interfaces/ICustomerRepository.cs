﻿using KoiFishServiceCenter.Repositories.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFishServiceCenter.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetCustomerAsync();
        Task<Boolean> DelCustomer(int Id);
        Task<Boolean> DelCustomer(Customer customer);
        Task<Boolean> AddCustomer(Customer customer);
        Task<Boolean> UpdateCustomer(Customer customer);
        Task<Customer> GetCustomerById(int Id);
        Task<int> CountCustomersAsync();
        Task<List<Customer>> GetAllCustomersAsync();
        Task<List<Customer>> SearcheAsync(string searchString);
        SelectList GetCustomerSelect();
        Task<int> CreateId();
        Task<Customer> GetCustomer(int Id);

    }
}
