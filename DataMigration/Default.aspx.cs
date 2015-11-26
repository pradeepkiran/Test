
using System;
using System.Data;
using System.Web.Security;
using Infosys.Connected.Framework.SynCryptography;
using System.Web.UI;
using System.IO;
using System.Text;


public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //For displaying the user which could not be copied
            int snoInvalidUsers = 0;
            int snoValidUsers = 0;
            DataTable dtInvalidUser = new DataTable();
            dtInvalidUser.Columns.Add("Sno");
            dtInvalidUser.Columns.Add("ProfileId");
            dtInvalidUser.Columns.Add("Error Message");
            dtInvalidUser.Columns.Add("LoginId");
            dtInvalidUser.Columns.Add("Password");
            DataRow drInvalidUser;

            DataTable dtData = new DataTable();
            DataRow drData;
            string successStatus = string.Empty;
            string successMessages = string.Empty;
            dtData = GetExistingData();

            for (int i = 0; i < dtData.Rows.Count; i++)
            {
                drData = ModifyData(dtData.Rows[i]);
                successStatus = SaveAllData(drData);

                if (successStatus != "Success")
                {
                    //successMessages = successMessages + (i + 1).ToString() + ".&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Error Message: " + successStatus + "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Username: " + drData["PROF_LoginId"].ToString() + " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Password: " + drData["PROF_Password"].ToString() + " <br/> ";
                    snoInvalidUsers += 1;
                    drInvalidUser = dtInvalidUser.NewRow();
                    drInvalidUser["Sno"] = snoInvalidUsers.ToString();
                    drInvalidUser["ProfileId"] = drData["PROF_Id"].ToString();
                    drInvalidUser["Error Message"] = successStatus;
                    drInvalidUser["LoginId"] = drData["PROF_EMailId"].ToString();
                    drInvalidUser["Password"] = drData["PROF_Password"].ToString();
                    dtInvalidUser.Rows.Add(drInvalidUser);
                }
                else if (successStatus == "Success")
                {
                    snoValidUsers += 1;
                }
            }

            Response.Write("<h2>Number of users successfully migrated= " + snoValidUsers.ToString() + "/" + dtData.Rows.Count + "</h2><br/><br/>");
            if (dtInvalidUser.Rows.Count > 0)
            {
                gvInvalidData.Caption = "Detail of users who could not be copied";
                //Response.Write(": <br/><br/>" + successMessages);
                gvInvalidData.DataSource = dtInvalidUser;
                gvInvalidData.DataBind();
            }
            else if (dtInvalidUser.Rows.Count == 0)
            {
                Response.Write("<script>alert('" + successMessages + "')</script>");
            }
        }
        catch (Exception ex)
        {
            Response.Write("Exception Occured:=" + ex.Message);
        }
    }

    //public void WriteToLog(string message)
    //{
    //    StreamWriter log;
    //    string strLogFile = System.Configuration.ConfigurationManager.AppSettings["logFilePath"].ToString();

    //    if (!File.Exists(strLogFile))
    //    {
    //        log = new StreamWriter("logfile.txt");

    //    }
    //    else
    //    {
    //        log = File.AppendText("logfile.txt");

    //    }
    //    // Write to the file:
    //    log.WriteLine(DateTime.Now);
    //    log.WriteLine(message);
    //    log.WriteLine();

    //    //Flush the stream
    //    log.Flush(); 

    //    // Close the stream:
    //    log.Close();




    //}

    public DataTable GetExistingData()
    {
        DataTable dtExistingData = new DataTable();
        DBLayer dbLayerExistingData = new DBLayer();

        dtExistingData = dbLayerExistingData.GetExistingData();

        return dtExistingData;
    }

    public string SaveAllData(DataRow drDataToSave)
    {
        string successMessage = string.Empty;

        try
        {
            successMessage = SaveMembers(drDataToSave);

            if (successMessage.Equals("Success"))
            {
                SaveRoles(drDataToSave);
                SaveProfile(drDataToSave);
            }
        }
        catch (Exception ex)
        {
            successMessage = ex.Message;
        }

        return successMessage;
    }

    public string SaveMembers(DataRow drDataToSave)
    {
        MembershipCreateStatus success = new MembershipCreateStatus();
        Membership.CreateUser(drDataToSave["PROF_EMailId"].ToString(), drDataToSave["PROF_Password"].ToString(), drDataToSave["PROF_EMailId"].ToString(), "Question", "Answer", true, out success);
        return success.ToString();
    }

    public void SaveRoles(DataRow drDataToSave)
    {
        if (!String.IsNullOrEmpty(drDataToSave["ROLE_Name"].ToString()))
        {
            Roles.AddUserToRole(drDataToSave["PROF_EMailId"].ToString(), drDataToSave["ROLE_Name"].ToString());
        }
        else
        {
            Roles.AddUserToRole(drDataToSave["PROF_EMailId"].ToString(), "User");
        }
    }

    public void SaveProfile(DataRow drDataToSave)
    {
        ProfileCommon profileCommonObj = Profile.GetProfile(drDataToSave["PROF_EMailId"].ToString());

        profileCommonObj.FirstName = drDataToSave["PROF_FirstName"].ToString();
        profileCommonObj.LastName = drDataToSave["PROF_LastName"].ToString();
        profileCommonObj.Company = drDataToSave["PROF_Company"].ToString();
        profileCommonObj.Department = drDataToSave["PROF_Department"].ToString();
        profileCommonObj.Address = drDataToSave["PROF_Address"].ToString();
        profileCommonObj.City = drDataToSave["PROF_City"].ToString();
        profileCommonObj.State = drDataToSave["PROF_State"].ToString();
        profileCommonObj.PostalCode = drDataToSave["PROF_PostalCode"].ToString();
        profileCommonObj.Country = drDataToSave["PROF_Country"].ToString();
        profileCommonObj.Fax = drDataToSave["PROF_Fax"].ToString();
        profileCommonObj.Mobile = drDataToSave["PROF_Mobile"].ToString();
        profileCommonObj.Phone = drDataToSave["PROF_Phone"].ToString();
        profileCommonObj.ProfileType = drDataToSave["PROF_PRTY_Id"].ToString();
        profileCommonObj.DefaultField = drDataToSave["PROF_Default_FIEL_Id"].ToString();
        profileCommonObj.ExpiryDate = DateTime.Now.AddYears(10).ToShortDateString();
        profileCommonObj.Owner = GetUserName(drDataToSave["PROF_Owner"].ToString());
        profileCommonObj.Deleted = drDataToSave["PROF_Deleted"].ToString();
        profileCommonObj.CreatedBy = GetUserName(drDataToSave["PROF_CreatedBy"].ToString());
        profileCommonObj.ModifiedBy = GetUserName(drDataToSave["PROF_ModifiedBy"].ToString());
        profileCommonObj.ModifiedOn = drDataToSave["PROF_ModifiedOn"].ToString();
        profileCommonObj.RoleCntryCat_CatId = drDataToSave["PROF_RoleCntryCat_CatId"].ToString();

        profileCommonObj.TotalHt = drDataToSave["FASI_TotalHa"].ToString();
        profileCommonObj.CornHt = drDataToSave["FASI_CornHa"].ToString();
        profileCommonObj.CornIrrHt = drDataToSave["FASI_CornIrrigatedHa"].ToString();
        profileCommonObj.WheatHt = drDataToSave["FASI_WheatHa"].ToString();
        profileCommonObj.SunflowerHt = drDataToSave["FASI_SunFlowerHa"].ToString();
        profileCommonObj.RapesHt = drDataToSave["FASI_RapeHa"].ToString();
        profileCommonObj.Dosespurchased = drDataToSave["FASI_DosesPurchased"].ToString();

        profileCommonObj.Save();
    }

    public DataRow ModifyData(DataRow drDataToModify)
    {
        string strModifiedPassword = "";
        try
        {
            strModifiedPassword = GetDecryptedString(drDataToModify["PROF_Password"].ToString());
        }
        catch (Exception ex)
        {
            //Response.Write("Exception in Modify data Method " + ex.Message);
            strModifiedPassword = drDataToModify["PROF_Password"].ToString();
        }
        finally
        {
            drDataToModify["PROF_Password"] = strModifiedPassword;
        }

        return drDataToModify;
    }

    public static string GetDecryptedString(string strText)
    {
        SynEncryptionManager encryptionManager = new SynEncryptionManager("091008104335AM");

        encryptionManager.CryptographyStrategy = new DesCryptography();
        encryptionManager.EncryptedValue = strText;
        return encryptionManager.CreateDecryptor();
    }

    public string GetUserName(string profileId)
    {
        string userName = string.Empty;
        DBLayer dbLayer = new DBLayer();
        try
        {
            userName = dbLayer.GetUserName(profileId).Rows[0][0].ToString();
        }
        catch (Exception ex)
        {
            userName = string.Empty;
        }

        return userName;
    }
}