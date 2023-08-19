using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Argiculture.Models
{
    // Single Inheritance.
    // This CropModel will have CommonEntity properties also.
    public class CropModel:CommonEntity
    {
        public string Name { get; set; }
    }
}
