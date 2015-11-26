using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infosys.Connected.Framework.SynCryptography;

public partial class Encryption : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnEncrypt_Click(object sender, EventArgs e)
    {
        lblResult.Text = GetEncryptedString(txtOriginal.Text);
    }
    protected void btnDecrypt_Click(object sender, EventArgs e)
    {
        lblResult.Text = GetDecryptedString(txtOriginal.Text);
    }

    public static string GetDecryptedString(string strText)
    {
        SynEncryptionManager encryptionManager = new SynEncryptionManager("091008104335AM");

        encryptionManager.CryptographyStrategy = new DesCryptography();
        encryptionManager.EncryptedValue = strText;
        return encryptionManager.CreateDecryptor();
    }

    public static string GetEncryptedString(string strText)
    {
        SynEncryptionManager encryptionManager = new SynEncryptionManager("091008104335AM");
        encryptionManager.CryptographyStrategy = new DesCryptography();
        encryptionManager.EncryptionValue = strText;
        return encryptionManager.CreateEncryptor();
    }
}