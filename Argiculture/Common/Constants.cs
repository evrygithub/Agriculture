using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Argiculture.Common
{
    public class Constants
    {
        public const string MyDBconnection = "mydbconnection";
        public const string ID = "Please provide ID";
        public const string Name = "Please provide name";
    }
    
    public class CustomMessages
    {
        public const string SavedSuccessfully = "Record saved successfully";
        public const string DeleteSuccessfully = "Delete saved successfully";
        public const string FailedToSaveData = "Failed to save the data";
        public const string RecordExists = "Record already exists";
        public const string RecordNotExists = "Record does not exists";
        public const string DataNotExists = "Data not exists";
        public const string FailedToRetriveData = "Failed to retrive the data";
        public const string SomethingWentWrong = "Something went wrong. Please contact Admin";
    }
    public class StoreProcedures
    {
        public const string Sp_GetAllCrops = "[Sp_GetAllCrops]";
        public const string Sp_GetCropByID = "[Sp_GetCropByID]";
        public const string Sp_InsertCrop = "[Sp_InsertCrop]";
        public const string Sp_UpdateCrop = "[Sp_UpdateCrop]";
        public const string Sp_DeleteCrop = "[Sp_DeleteCrop]";
    }
    public class Tables
    {
        public const string Crop = "Crop";
    }
}
