﻿using CompanyWebApi.Contracts.Entities;
using CompanyWebApi.Persistence.DbContexts;
using CompanyWebApi.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace CompanyWebApi.Persistence.Repositories
{
    public class CompanyRepository : BaseRepository<Company, ApplicationDbContext>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<Company> AddCompanyAsync(Company company, bool tracking = true)
        {
            await AddAsync(company).ConfigureAwait(false);
            await SaveAsync().ConfigureAwait(false);
            return await GetCompanyAsync(company.CompanyId, tracking).ConfigureAwait(false);
        }

        public async Task<IList<Company>> GetCompaniesAsync(bool tracking = false)
        {
            var result = await GetAsync<Company>(
                include: source => source
                    .Include(cmp => cmp.Departments)
                        .ThenInclude(cmp => cmp.Employees)
                            .ThenInclude(cmp => cmp.EmployeeAddresses)
                    .Include(cmp => cmp.Departments)
                        .ThenInclude(cmp => cmp.Employees)
                            .ThenInclude(cmp => cmp.User)
                    .Include(cmp => cmp.Departments)
                        .ThenInclude(dept => dept.Employees)
                            .ThenInclude(emp => emp.Company), 
                orderBy: cmp => cmp
                    .OrderBy(o => o.CompanyId),
                tracking: tracking).ConfigureAwait(false);
            return result;
        }

        public async Task<Company> GetCompanyAsync(int id, bool tracking = false)
        {
            var result = await GetSingleOrDefaultAsync<Company>(
                cmp => cmp.CompanyId == id,
                source => source
                    .Include(cmp => cmp.Departments)
                        .ThenInclude(dept => dept.Employees)
                            .ThenInclude(emp => emp.EmployeeAddresses)
                    .Include(cmp => cmp.Departments)
                        .ThenInclude(dept => dept.Employees)
                            .ThenInclude(emp => emp.User)
                    .Include(cmp => cmp.Departments)
                        .ThenInclude(dept => dept.Employees)
                            .ThenInclude(emp => emp.Company), 
                    tracking)
                .ConfigureAwait(false);
                    
            return result;
        }

        public async Task<Company> GetCompanyAsync(Expression<Func<Company, bool>> predicate, bool tracking = false)
        {
            var result = await GetSingleOrDefaultAsync<Company>(
                predicate,
                source => source
                    .Include(cmp => cmp.Departments).ThenInclude(cmp => cmp.Employees).ThenInclude(cmp => cmp.EmployeeAddresses)
                    .Include(cmp => cmp.Departments).ThenInclude(cmp => cmp.Employees).ThenInclude(cmp => cmp.User)
                    .Include(cmp => cmp.Departments)
                        .ThenInclude(dept => dept.Employees)
                            .ThenInclude(emp => emp.Company),
                tracking).ConfigureAwait(false);
            return result;
        }
    }
}
