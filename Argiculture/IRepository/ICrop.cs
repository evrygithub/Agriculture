using Argiculture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Argiculture.IRepository
{
    public interface ICrop
    {
        public List<CropModel> GetAllCrops();

        public CropModel GetCropByID(string ID);

        public string AddCrop(CropModel cropModel);// return type is string becoz we have taken NEWID in DB tables.

        public string UpdateCrop(CropModel cropModel);

        public string DeleteCrop(string ID);
    }
}
