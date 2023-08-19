using Argiculture.Common;
using Argiculture.Extensions;
using Argiculture.IRepository;
using Argiculture.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Argiculture.Repository
{
    public class Crop : ICrop
    {
        private readonly IConfiguration _configuration;
        private SqlConnection sqlConnection;
        public Crop(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlConnection = new SqlConnection(_configuration.GetConnectionString(Constants.MyDBconnection));
        }
        
        public List<CropModel> GetAllCrops()
        {
            try
            {
                var dataset = Extension.ExecuteDataSet(StoreProcedures.Sp_GetAllCrops, sqlConnection);
                dataset.Tables[0].TableName = Tables.Crop; // Filling or Adding the Table name to Dataset
                var result = dataset.Tables[Tables.Crop].ToListof<CropModel>().OrderByDescending(x => x.CreatedDate).ToList();
                return result;                          // ToListof is user defined generic extension method.
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public CropModel GetCropByID(string ID)
        {
            try
            {
                var dataset = Extension.ExecuteDataSet(StoreProcedures.Sp_GetCropByID, sqlConnection);
                dataset.Tables[0].TableName = Tables.Crop;
                var result = dataset.Tables[Tables.Crop].ToListof<CropModel>().OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string AddCrop(CropModel cropModel)
        {
            try
            {
                cropModel.StatusID = 1;
                List<SqlParameter> lst = new List<SqlParameter>();
                lst.Add(!string.IsNullOrEmpty(cropModel.Name) ? new SqlParameter("@Name", cropModel.Name) : new SqlParameter("@Name", DBNull.Value));
                lst.Add(new SqlParameter("@StatusID", cropModel.StatusID));
                var result = Extension.ExecuteScalar(lst, StoreProcedures.Sp_InsertCrop, sqlConnection);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
        public string UpdateCrop(CropModel cropModel)
        {
            try
            {
                cropModel.StatusID = 1;
                List<SqlParameter> lst = new List<SqlParameter>();
                lst.Add(!string.IsNullOrEmpty(cropModel.ID) ? new SqlParameter("@ID", cropModel.ID) : new SqlParameter("@ID", DBNull.Value));
                lst.Add(!string.IsNullOrEmpty(cropModel.Name) ? new SqlParameter("@Name", cropModel.Name) : new SqlParameter("@Name", DBNull.Value));
                lst.Add(new SqlParameter("@StatusID", cropModel.StatusID));
                var result = Extension.ExecuteScalar(lst, StoreProcedures.Sp_UpdateCrop, sqlConnection);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        public string DeleteCrop(string ID)
        {
            try
            {
                List<SqlParameter> lst = new List<SqlParameter>();
                lst.Add(!string.IsNullOrEmpty(ID) ? new SqlParameter("@ID", ID) : new SqlParameter("@ID", DBNull.Value));
                var result = Extension.ExecuteScalar(lst, StoreProcedures.Sp_DeleteCrop, sqlConnection);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
