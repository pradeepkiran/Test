using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Syngenta.MaizeExpert.DataAccessHelper;

/// <summary>
/// Summary description for DBLayer
/// </summary>
[DataObject(true)]
public class DBLayer
{
    [DataObjectMethod(DataObjectMethodType.Select)]
    public DataTable GetExistingData()
    {
        DataSet dsExistingData = new DataSet();
        string connectionString = ConfigurationManager.ConnectionStrings["SqlServices"].ToString();
        SqlConnection conObj = new SqlConnection(connectionString);
        SqlParameter[] arrParameterVal = new SqlParameter[0];

        dsExistingData = SqlHelper.ExecuteDataset(conObj, CommandType.StoredProcedure, "MEX_Migration_GetExistingData", arrParameterVal);
        return dsExistingData.Tables[0];
    }

    public DataTable GetFieldIds()
    {
        DataSet dsFieldIds = new DataSet();
        string connectionString = ConfigurationManager.ConnectionStrings["SqlServices"].ToString();
        SqlConnection conObj = new SqlConnection(connectionString);
        SqlParameter[] arrParameterVal = new SqlParameter[0];

        dsFieldIds = SqlHelper.ExecuteDataset(conObj, CommandType.StoredProcedure, "MEX_Migration_GetFieldIds", arrParameterVal);
        return dsFieldIds.Tables[0];
    }

    public int GetUpdateFieldTable(int fieldId)
    {
        //Initialized successUpdateFieldTable variable to 0 which is set to 1 in case of successfull execution of Stored Procedure
        int successUpdateFieldTable = 0;

        //Connection string which contains the details of database to be used
        string strConUpdateFieldTable = ConfigurationManager.ConnectionStrings["SqlServices"].ToString();
        SqlConnection conUpdateFieldTable = new SqlConnection(strConUpdateFieldTable);

        //Initializes a new instance of SqlCommand
        SqlCommand cmdUpdateFieldTable = new SqlCommand();
        //Setting the Connection property of the SqlCommand Object
        cmdUpdateFieldTable.Connection = conUpdateFieldTable;
        //Setting the CommandType property of the SqlCommand Object to StoredProcedure
        cmdUpdateFieldTable.CommandType = CommandType.StoredProcedure;
        //Setting the CommandType property of the SqlCommand Object to Name of the stored procedure i.e. usp_UpdateFieldTable
        cmdUpdateFieldTable.CommandText = "MEX_Migration_UpdateProfileId";
        //Adding parameter to StoredProcedure which will take input from track variable
        cmdUpdateFieldTable.Parameters.AddWithValue("@FieldId", fieldId);

        //prmSuccessCode stores the return value of the stored procedure
        SqlParameter prmSuccessCode = new SqlParameter();
        prmSuccessCode.Direction = ParameterDirection.ReturnValue;
        prmSuccessCode.SqlDbType = SqlDbType.Int;
        cmdUpdateFieldTable.Parameters.Add(prmSuccessCode);

        //Opens the database connetion
        conUpdateFieldTable.Open();
        try
        {
            //Execution of the stored procedure usp_UpdateFieldTable
            cmdUpdateFieldTable.ExecuteNonQuery();

            //Checks whether the value returned by stored procedure is 0 or 1(which denotes success)
            if ((int)prmSuccessCode.Value == 1)
            {
                //if the stored procedure is executed successfuly the successUpdateFieldTable is set to 1
                successUpdateFieldTable = 1;
            }
        }
        catch (Exception ex)
        {
            //In case of any exception raised during stored procedure execution the successUpdateFieldTable is set to 0
            successUpdateFieldTable = 0;
        }
        finally
        {
            //Irrespective of stored procedure execution the connection is closed
            conUpdateFieldTable.Close();
        }


        //Returns the successUpdateFieldTable to the calling method
        return successUpdateFieldTable;
    }

    public DataTable GetUserName(string profileId)
    {
        DataSet dsUserName = new DataSet();
        string connectionString = ConfigurationManager.ConnectionStrings["SqlServices"].ToString();
        SqlConnection conObj = new SqlConnection(connectionString);
        SqlParameter[] arrParameterVal = new SqlParameter[1];

        arrParameterVal[0] = new SqlParameter();
        arrParameterVal[0].ParameterName = "ProfileId";
        arrParameterVal[0].Value = profileId;

        dsUserName = SqlHelper.ExecuteDataset(conObj, CommandType.StoredProcedure, "MEX_Migration_GetUserName", arrParameterVal);
        return dsUserName.Tables[0];
    }
}