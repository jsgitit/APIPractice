using System;

namespace CompanyWebApi.Contracts.Models;

public class EmployeeSearchCriteria
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? BirthDate { get; set; }
    public string Department { get; set; }
    public string Username { get; set; }
}