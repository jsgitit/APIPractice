using System;

namespace CompanyWebApi.Contracts.Dto.V4
{
    /// <summary>
    /// Department Data Transfer Object
    /// </summary>
    [Serializable]
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
