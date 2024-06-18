using CompanyWebApi.Persistence.DbContexts;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace CompanyWebApi.Persistence.Repositories.Factory
{
    public class RepositoryFactory : IRepositoryFactory 
    { 
        private readonly ApplicationDbContext _dbContext;

        public RepositoryFactory(ApplicationDbContext dbContext,
            ICompanyRepository companyRepository,
            IDepartmentRepository departmentRepository,
            IEmployeeRepository employeeRepository,
            IEmployeeAddressRepository employeeAddressRepository,
            IUserRepository userRepository)
        {
            _dbContext = dbContext ?? throw new ArgumentException("DbContext is null", nameof(dbContext));
            CompanyRepository = companyRepository ?? throw new ArgumentException("Repository is null", nameof(dbContext));
            DepartmentRepository = departmentRepository ?? throw new ArgumentException("Repository is null", nameof(departmentRepository));
            EmployeeRepository = employeeRepository ?? throw new ArgumentException("Repository is null", nameof(employeeRepository));
            EmployeeAddressRepository = employeeAddressRepository ?? throw new ArgumentException("Repository is null", nameof(employeeAddressRepository));
            UserRepository = userRepository ?? throw new ArgumentException("Repository is null", nameof(userRepository));
        }

        public ICompanyRepository CompanyRepository { get; }

        public IDepartmentRepository DepartmentRepository { get; }

        public IEmployeeRepository EmployeeRepository { get; }

        public IEmployeeAddressRepository EmployeeAddressRepository { get; }

        public IUserRepository UserRepository { get; }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
