using System;
using Newtonsoft.Json;

namespace CompanyWebApi.Contracts.Entities.Base
{
	public abstract class BaseAuditEntity : IBaseAuditEntity    
	{
		public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime Modified { get; set; } = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
	}
}
