using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;

public partial class DeleteAllUsers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dtData = new DataTable();
        string successStatus = string.Empty;
        dtData = GetExistingData();
        int countDeletion = 0;

        for (int i = 0; i < dtData.Rows.Count; i++)
        {
            if (DeleteUser(dtData.Rows[i]["PROF_EMailId"].ToString()))
            {
                successStatus = "Success";
                countDeletion += 1;
            }
            else
            {
                successStatus = "Partial Failure";
            }
        }

        Response.Write("<h2>Number of users successfully deleted= " + countDeletion.ToString() + "(" + successStatus + ")</h2><br/><br/>");

        //Response.Write("<script>alert('" + successStatus + "')</script>");
    }


    public DataTable GetExistingData()
    {
        DataTable dtExistingData = new DataTable();
        DBLayer dbLayerExistingData = new DBLayer();

        dtExistingData = dbLayerExistingData.GetExistingData();

        return dtExistingData;
    }

    public Boolean DeleteUser(string userName)
    {
        Boolean deleted = false;
        try
        {
            deleted = Membership.DeleteUser(userName, true);
        }
        catch (Exception)
        {
        }
        return deleted;
    }
}