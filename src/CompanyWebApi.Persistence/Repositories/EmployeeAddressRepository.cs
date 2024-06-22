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

    // public async Task<IList<EmployeeAddressDto>> UpsertAsync(IList<EmployeeAddressUpdateDto> addresses)
    //{
    //    var updatedAddresses = new List<EmployeeAddressDto>();

    //    foreach (var addressDto in addresses)
    //    {
    //        var existingAddress = await _addressRepository.GetByIdAsync(addressDto.AddressId);

    //        if (existingAddress != null)
    //        {
    //            // Check if an update is necessary
    //            if (existingAddress.Address != addressDto.Address || existingAddress.Type != addressDto.Type)
    //            {
    //                // Update the existing address
    //                existingAddress.Address = addressDto.Address;
    //                existingAddress.Type = addressDto.Type;
    //                existingAddress.ModifiedDate = DateTime.UtcNow;

    //                await _addressRepository.UpdateAsync(existingAddress);
    //            }

    //            var updatedAddressDto = new EmployeeAddressDto
    //            {
    //                AddressId = existingAddress.AddressId,
    //                EmployeeId = existingAddress.EmployeeId,
    //                Address = existingAddress.Address,
    //                Type = existingAddress.Type,
    //                ModifiedDate = existingAddress.ModifiedDate
    //            };

    //            updatedAddresses.Add(updatedAddressDto);
    //        }
    //        else
    //        {
    //            // Add new address
    //            var newAddress = new EmployeeAddress
    //            {
    //                EmployeeId = addressDto.EmployeeId,
    //                Address = addressDto.Address,
    //                Type = addressDto.Type,
    //                CreatedDate = DateTime.UtcNow,
    //                ModifiedDate = DateTime.UtcNow
    //            };

    //            await _addressRepository.AddAsync(newAddress);

    //            var newAddressDto = new EmployeeAddressDto
    //            {
    //                AddressId = newAddress.AddressId,
    //                EmployeeId = newAddress.EmployeeId,
    //                Address = newAddress.Address,
    //                Type = newAddress.Type,
    //                ModifiedDate = newAddress.ModifiedDate
    //            };

    //            updatedAddresses.Add(newAddressDto);
    //        }
    //    }

    //    return updatedAddresses;
    //}
}
