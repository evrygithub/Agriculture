using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Argiculture.Common
{
    public class AGResult
    {
        public bool IsSuccess { get; set; }// To check whether record success or fail of a record.
        public object Result { get; set; }// To store result which comes from database.
        public string Message { get; set; }// User attention.
        
        
    }
}
