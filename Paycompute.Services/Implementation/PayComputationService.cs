﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Paycompute.Entity;
using Paycompute.Persistence;

namespace Paycompute.Services.Implementation
{
    public class PayComputationService : IPayComputationService
    {
        private decimal contractualEarnings;
        private decimal overtimeHours;
        private ApplicationDbContext _context;

        public PayComputationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public decimal ContractualEarnings(decimal contractualHours, decimal hoursWorked, decimal hourlyRate)
        {
            if (hoursWorked < contractualHours)
            {
                contractualEarnings = hoursWorked * hourlyRate;
            }
            else
            {
                contractualEarnings = contractualHours * hourlyRate;
            }
            return contractualEarnings;
        }

        public async Task CreateAsync(PaymentRecord paymentRecord)
        {
            await _context.PaymentRecords.AddAsync(paymentRecord);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<PaymentRecord> GetAll() => _context.PaymentRecords.OrderBy(p => p.EmployeeId);

        public IEnumerable<SelectListItem> GetAllTaxYear()
        {
            var allTaxYear = _context.TaxYears.Select(taxYear => new SelectListItem
            {
                Text = taxYear.YearOfTax,
                Value = taxYear.Id.ToString()
            });
            return allTaxYear;
        }

        public PaymentRecord GetById(int id) => _context.PaymentRecords.Where(pay => pay.Id == id).FirstOrDefault();

        public TaxYear GetTaxYearById(int id) => _context.TaxYears.Where(year => year.Id == id).FirstOrDefault();

        public decimal NetPay(decimal totalEarnings, decimal totalDeduction) => totalEarnings - totalDeduction;

        public decimal OvertimeEarnings(decimal overtimeRate, decimal overtimeHours) => overtimeHours * overtimeRate;

        public decimal OvertimeHours(decimal hoursWorked, decimal contractualHours)
        {
            overtimeHours = 0.0m;
            if (hoursWorked > contractualHours)
            {
                overtimeHours = hoursWorked - contractualHours;
            }
            return overtimeHours;
        }

        public decimal OvertimeRate(decimal hourlyRate) => hourlyRate * 1.5m;

        public decimal TotalDeduction(decimal tax, decimal nic, decimal studentLoanPayment, decimal unionFees)
        => tax + nic + studentLoanPayment + unionFees;

        public decimal TotalEarnings(decimal overtimeEarnings, decimal contractualEarnings)
        => overtimeEarnings + contractualEarnings;
    }
}
