using System;

namespace CompanyWebApi.Contracts.Dto.V4
{
    /// <summary>
    /// Company Data Transfer Object
    /// </summary>

    public class CompanyDto
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}