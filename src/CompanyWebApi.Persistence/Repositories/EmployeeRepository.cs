﻿using CompanyWebApi.Contracts.Dto;
using CompanyWebApi.Contracts.Entities;
using CompanyWebApi.Contracts.Models;
using CompanyWebApi.Core.Extensions;
using CompanyWebApi.Persistence.DbContexts;
using CompanyWebApi.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CompanyWebApi.Persistence.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee, ApplicationDbContext>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee, bool tracking = true)
        {
            await AddAsync(employee).ConfigureAwait(false);
            await SaveAsync().ConfigureAwait(false);
            return await GetEmployeeAsync(employee.EmployeeId, tracking).ConfigureAwait(false);
        }

        public async Task<Employee> GetEmployeeAsync(int id, bool tracking = false)
        {
            var result = await GetSingleOrDefaultAsync<Employee>(
                emp => emp.EmployeeId == id,
                source => source
                      .Include(emp => emp.Company)
                      .Include(emp => emp.Department)
                      .Include(emp => emp.EmployeeAddresses)
                      .Include(emp => emp.User),
                tracking).ConfigureAwait(false);
            return result;
        }

        public async Task<Employee> GetEmployeeAsync(Expression<Func<Employee, bool>> predicate, bool tracking = false)
        {
            var result = await GetSingleOrDefaultAsync<Employee>(
                predicate,
                source => source
                      .Include(emp => emp.Company)
                      .Include(emp => emp.Department)
                      .Include(emp => emp.EmployeeAddresses)
                      .Include(emp => emp.User),
                tracking).ConfigureAwait(false);
            return result;
        }

        public async Task<IList<Employee>> GetEmployeesAsync(Expression<Func<Employee, bool>> predicate = null, bool tracking = false)
        {
            var result = await GetAsync<Employee>(predicate,
                include: source => source
                    .Include(emp => emp.Company)
                    .Include(emp => emp.Department)
                    .Include(emp => emp.EmployeeAddresses)
                    .Include(emp => emp.User),
                orderBy: emp => emp
                    .OrderBy(o => o.LastName)
                    .ThenBy(tb => tb.FirstName),
                tracking: tracking).ConfigureAwait(false);
            return result;
        }

        public async Task<IList<Employee>> SearchEmployeesAsync(EmployeeSearchCriteria searchCriteria, bool tracking = false)
        {
            var employees = await GetEmployeesAsync(null, tracking).ConfigureAwait(false);
            employees = employees.If(!string.IsNullOrEmpty(searchCriteria.FirstName), q => q.Where(x =>
                    x.FirstName.Contains(searchCriteria.FirstName, StringComparison.InvariantCultureIgnoreCase)))
                .If(!string.IsNullOrEmpty(searchCriteria.LastName), q => q.Where(x =>
                    x.LastName.Contains(searchCriteria.LastName, StringComparison.InvariantCultureIgnoreCase)))
                .If(!string.IsNullOrEmpty(searchCriteria.Department), q => q.Where(x =>
                    x.Department.Name.Contains(searchCriteria.Department, StringComparison.InvariantCultureIgnoreCase)))
                .If(!string.IsNullOrEmpty(searchCriteria.Username), q => q.Where(x =>
                    x.User.Username.Contains(searchCriteria.Username, StringComparison.InvariantCultureIgnoreCase)))
                .If(searchCriteria.BirthDate.HasValue, q => q.Where(x => x.BirthDate == searchCriteria.BirthDate))
                .ToList();
            return employees;
        }

        public async Task UpdateEmployeeAsync(Employee employee, bool tracking = true)
        {
            await UpdateAsync(employee, tracking).ConfigureAwait(false);
            await SaveAsync().ConfigureAwait(false);
        }

    }
}
