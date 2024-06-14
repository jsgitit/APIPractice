using CompanyWebApi.Contracts.Entities;
using CompanyWebApi.Persistence.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CompanyWebApi.Persistence.Repositories;
public interface IEmployeeAddressRepository : IBaseRepository<EmployeeAddress>
{
    /// <summary>
    /// Add a new employee address
    /// </summary>
    /// <param name="employeeAddress">EmployeeAddress model</param>
    /// <param name="tracking">Tracking changes</param>
    /// <returns></returns>
    Task<EmployeeAddress> AddEmployeeAddressAsync(EmployeeAddress employeeAddress, bool tracking = true);

    /// <summary>
    /// Get employee address by employee id and address type id
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="addressTypeId"></param>
    /// <param name="tracking">Tracking changes</param>
    /// <returns></returns>
    Task<EmployeeAddress> GetEmployeeAddressAsync(int employeeId, AddressType addressTypeId, bool tracking = false);

    /// <summary>
    /// Get employee address by predicate
    /// </summary>
    /// <param name="predicate">Where conditions</param>
    /// <param name="tracking">Tracking changes</param>
    /// <returns></returns>
    Task<EmployeeAddress> GetEmployeeAddressAsync(Expression<Func<EmployeeAddress, bool>> predicate, bool tracking = false);

    /// <summary>
    /// Get all employees addresses by predicate
    /// </summary>
    /// <param name="predicate">Where conditions</param>
    /// <param name="tracking">Tracking changes</param>
    /// <returns></returns>
    Task<IList<EmployeeAddress>> GetEmployeeAddressesAsync(Expression<Func<EmployeeAddress, bool>> predicate = null, bool tracking = false);
}
