using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Argiculture.Models
{
    public class CommonEntity
    {
        public string ID { get; set; }
        public int StatusID { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }
}
