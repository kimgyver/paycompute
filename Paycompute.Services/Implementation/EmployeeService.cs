﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Paycompute.Entity;
using Paycompute.Persistence;

namespace Paycompute.Services.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Employee nweEmployee)
        {
            await _context.Employees.AddAsync(nweEmployee);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int employeeId)
        {
            var employee = GetById(employeeId);
            _context.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Employee> GetAll() => _context.Employees.AsNoTracking().OrderBy(emp => emp.FullName);
        //{
        //    return _context.Employees;
        //}

        public Employee GetById(int employeeId) =>
            _context.Employees.Where(e => e.Id == employeeId).FirstOrDefault();
      
        public async Task UpdateAsync(Employee employee)
        {
            _context.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id)
        {
            var employee = GetById(id);
            _context.Update(employee);
            await _context.SaveChangesAsync();
        }

        public decimal StudentLoanRepaymentAmount(int id, decimal totalAmount)
        {
            var employee = GetById(id);
            if (employee.StudentLoan != StudentLoan.Yes || totalAmount < 1750)
                return 0m;

            decimal studentLoanAmount = 0m;
            if (totalAmount >= 1750 && totalAmount < 2000)
                studentLoanAmount = 15m;
            else if (totalAmount >= 2000 && totalAmount < 2250)
                studentLoanAmount = 38m;
            else if (totalAmount >= 2250 && totalAmount < 2500)
                studentLoanAmount = 60m;
            else if (totalAmount >= 2500)
                studentLoanAmount = 83m;

            return studentLoanAmount;
        }

        public decimal UnionFees(int id)
        {
            var employee = GetById(id);
            return (employee.UnionMember == UnionMember.Yes) ? 10m : 0m;
        }

        public IEnumerable<SelectListItem> GetAllEmployeesForPayroll()
        {
            return GetAll().Select(emp => new SelectListItem()
            {
                Text = emp.FullName,
                Value = emp.Id.ToString()
            });
        }
    }
}
