using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Argiculture.Extensions
{
    // https://stackoverflow.com/questions/208532/how-do-you-convert-a-datatable-into-a-generic-list
    public static class Extension
    {
        //For getting all crops or single crop details..etc.(Globally) this methods handle any List of entity result(s).
        public static List<T> ToListof<T>(this DataTable dt)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();
            var objectProperties = typeof(T).GetProperties(flags);
            var targetList = dt.AsEnumerable().Select(dataRow =>
            {
                var instanceOfT = Activator.CreateInstance<T>();// Create an object to the entity (Crop)

                foreach (var properties in objectProperties.Where(properties => columnNames.Contains(properties.Name) && dataRow[properties.Name] != DBNull.Value))
                {
                    properties.SetValue(instanceOfT, dataRow[properties.Name], null);
                }
                return instanceOfT;
            }).ToList();

            return targetList;
        }

        public static DataSet ExecuteDataSet(string storedProcedureName, SqlConnection sqlconnection)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(storedProcedureName, sqlconnection);
                cmd.CommandType = CommandType.StoredProcedure;
                DataSet dataSet = new DataSet();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                sqlconnection.Open();
                sqlDataAdapter.Fill(dataSet);
                sqlconnection.Close();
                return dataSet;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if(sqlconnection.State!=ConnectionState.Open)
                {
                    sqlconnection.Close();
                    sqlconnection.Dispose();
                }
            }
        } 

        public static string ExecuteScalar(List<SqlParameter> sqlParameters,string storedProcedure,SqlConnection sqlConnection)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand(storedProcedure, sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                foreach (var item in sqlParameters)
                {
                    sqlCommand.Parameters.Add(item);
                }
                sqlConnection.Open();
                var result =(string) sqlCommand.ExecuteScalar(); // ExecuteScalar returns object type. For this we are doing explict casting. Our stored procedure return PrimaryKey column (ID value) of Table. 
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Close();
                    sqlConnection.Dispose();
                }
            }
        }
    }
}
