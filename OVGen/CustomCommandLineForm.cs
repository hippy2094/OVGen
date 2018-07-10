using Microsoft.VisualBasic;

public partial class CustomCommandLineForm
{
    private void CustomCommandLineForm_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
    {
        Interaction.MsgBox("If you messed up something, press [Default] button.");
        e.Cancel = true;
    }

    private void CustomCommandLineForm_Load(System.Object sender, System.EventArgs e)
    {
        TextBoxJoinAudioCommandLine.Text = My.MyProject.MyForms.MainForm.FFmpegCommandLineJoinAudio;
        TextBoxSilenceCommandLine.Text = My.MyProject.MyForms.MainForm.FFmpegCommandLineSilence;
    }

    private void ButtonOK_Click(System.Object sender, System.EventArgs e)
    {
        My.MyProject.MyForms.MainForm.FFmpegCommandLineJoinAudio = TextBoxJoinAudioCommandLine.Text;
        My.MyProject.MyForms.MainForm.FFmpegCommandLineSilence = TextBoxSilenceCommandLine.Text;
        this.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.Close();
    }

    private void ButtonCancel_Click(System.Object sender, System.EventArgs e)
    {
        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.Close();
    }

    private void ButtonDefault_Click(System.Object sender, System.EventArgs e)
    {
        if (Interaction.MsgBox("Set to default commandline?", MsgBoxStyle.YesNo) == MsgBoxResult.Yes)
        {
            TextBoxJoinAudioCommandLine.Text = MainForm.DefaultFFmpegCommandLineJoinAudio;
            TextBoxSilenceCommandLine.Text = MainForm.DefaultFFmpegCommandLineSilence;
        }
    }
}
