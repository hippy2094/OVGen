using System.Diagnostics;
using System.Windows.Forms;

public partial class AboutForm
{
    private void AboutForm_Load(System.Object sender, System.EventArgs e)
    {
        LabelVersion.Text = Application.ProductVersion;
    }

    private void Button1_Click(System.Object sender, System.EventArgs e)
    {
        Form.Close();
    }

    private void LinkLabelWebsite_LinkClicked(System.Object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
    {
        Process.Start("https://zeinok.blogspot.tw");
    }
}
