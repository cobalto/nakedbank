using System;
using System.Collections.Generic;

namespace NakedBank.Shared.Models.Responses
{
    public class BaseResponse
    {
        public DateTime ExecutionTime { get; set; }
        public List<BusinessError> Errors { get; set; }

        public BaseResponse()
        {
            ExecutionTime = DateTime.UtcNow;
            Errors = new List<BusinessError>();
        }
    }
}
