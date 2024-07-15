using CompanyWebApi.Contracts.Entities;
using CompanyWebApi.Persistence.DbContexts;
using CompanyWebApi.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CompanyWebApi.Persistence.Repositories;
public class EmployeeAddressRepository : BaseRepository<EmployeeAddress, ApplicationDbContext>, IEmployeeAddressRepository
{
    public EmployeeAddressRepository(ApplicationDbContext dbContext)
    : base(dbContext)
    {
    }

    public async Task<EmployeeAddress> AddEmployeeAddressAsync(EmployeeAddress employeeAddress, bool tracking = true)
    {
        await AddAsync(employeeAddress).ConfigureAwait(false);
        await SaveAsync().ConfigureAwait(false);
        return await GetEmployeeAddressAsync(employeeAddress.EmployeeId, employeeAddress.AddressTypeId, tracking).ConfigureAwait(false);
    }

    public async Task<EmployeeAddress> GetEmployeeAddressAsync(int employeeId, AddressType addressTypeId, bool tracking = false)
    {
        var result = await GetSingleOrDefaultAsync<EmployeeAddress>(
         ea => ea.EmployeeId == employeeId && ea.AddressTypeId == addressTypeId,
            source => source
                .Include(ea => ea.Employee),
            tracking).ConfigureAwait(false);
        return result;
    }

    public async Task<EmployeeAddress> GetEmployeeAddressAsync(Expression<Func<EmployeeAddress, bool>> predicate, bool tracking = false)
    {
        var result = await GetSingleOrDefaultAsync<EmployeeAddress>(
            predicate,
            source => source
                .Include(ea => ea.Employee),
            tracking).ConfigureAwait(false);
        return result;
    }

    public async Task<IList<EmployeeAddress>> GetEmployeeAddressesAsync(Expression<Func<EmployeeAddress, bool>> predicate = null, bool tracking = false)
    {
        var result = await GetAsync<EmployeeAddress>(
           predicate,
           include: source => source
                .Include(ea => ea.Employee),
           orderBy: ea => ea
                .OrderBy(ea => ea.EmployeeId)
                .ThenBy(ea => ea.AddressTypeId),
           tracking: tracking).ConfigureAwait(false);
        return result;
    }

    public async Task UpdateEmployeeAddressAsync(EmployeeAddress employeeAddress, bool tracking = true)
    {
        await UpdateAsync(employeeAddress).ConfigureAwait(false);
        await SaveAsync().ConfigureAwait(false);

    }

    public async Task UpsertEmployeeAddressesAsync(IList<EmployeeAddress> employeeAddresses, bool tracking = true)
    {
        await UpsertAsync(employeeAddresses).ConfigureAwait(false);
        await SaveAsync().ConfigureAwait(false);
    }
}
