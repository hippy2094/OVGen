using System.Data;
using System.Windows.Forms;
using System;
using System.ComponentModel;

public partial class AutoAmplifyWorkerForm
{
    public string Filename { get; set; }
    private double _result = 1;
    private bool finished = false;
    public double Result
    {
        get
        {
            return _result;
        }
        set
        {
            throw new ReadOnlyException();
        }
    }

    private void AutoAmplifyWorkerForm_Load(object sender, EventArgs e)
    {
        try
        {
            this.Text += string.Format(" ({0})", new System.IO.FileInfo(Filename).Name);
        }
        catch (Exception ex)
        {
        }
        BackgroundWorkerAutoAmplify.RunWorkerAsync();
    }

    private void ButtonCancel_Click(object sender, EventArgs e)
    {
        if (BackgroundWorkerAutoAmplify.IsBusy)
            BackgroundWorkerAutoAmplify.CancelAsync();
        this.DialogResult = DialogResult.Cancel;
        Form.Close();
    }

    private void BackgroundWorkerAutoAmplify_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    {
        WAV wav;
        try
        {
            wav = new WAV(Filename);
        }
        catch (Exception ex)
        {
            return;
        }
        double biggestSample = 0;
        int progress = 0;
        for (int i = 0; i <= wav.sampleLength - 1; i++)
        {
            int tempProgress = (i + 1) / (double)wav.sampleLength * 100;
            if (tempProgress != progress)
            {
                progress = tempProgress;
                BackgroundWorkerAutoAmplify.ReportProgress(progress);
            }
            if (BackgroundWorkerAutoAmplify.CancellationPending)
                break;
            if (Math.Abs(wav.getSample(i, true)) > biggestSample)
                biggestSample = Math.Abs(wav.getSample(i, true));
        }
        _result = 127 / biggestSample;
        if (_result < 1)
            _result = 1;
    }

    private void BackgroundWorkerAutoAmplify_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        finished = true;
        this.DialogResult = DialogResult.OK;
        Form.Close();
    }

    private void BackgroundWorkerAutoAmplify_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        ProgressBar1.Value = e.ProgressPercentage;
    }

    private void AutoAmplifyWorkerForm_Closing(object sender, CancelEventArgs e)
    {
        if (BackgroundWorkerAutoAmplify.IsBusy)
            BackgroundWorkerAutoAmplify.CancelAsync();
        if (!finished)
            this.DialogResult = DialogResult.Cancel;
    }
}
