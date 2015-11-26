using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class UpdateFieldTable : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DBLayer dbLayer = new DBLayer();
        DataTable dtFieldIds = new DataTable();
        int updateFieldSuccess = 0;
        int countFieldUpdated = 0;

        dtFieldIds = dbLayer.GetFieldIds();

        for (int i = 0; i < dtFieldIds.Rows.Count; i++)
        {
            updateFieldSuccess = UpdateFields(Convert.ToInt32(dtFieldIds.Rows[i]["FIEL_Id"]));

            if (updateFieldSuccess == 1)
            {
                countFieldUpdated += 1;
            }
        }

        Response.Write("<h2>Number of fields updated= " + countFieldUpdated.ToString() + "</h2><br/><br/>");


    }

    public int UpdateFields(int fieldId)
    {
        int successStatusCode = 0;
        DBLayer dbLayerUpdateFieldTable = new DBLayer();
        successStatusCode = dbLayerUpdateFieldTable.GetUpdateFieldTable(fieldId);
        return successStatusCode;
    }

}