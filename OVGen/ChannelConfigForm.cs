using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Linq;
using System.Collections.Generic;
using System;

public partial class ChannelConfigForm
{
    public channelOptions Options { get; set; } = new channelOptions();
    public bool SetAll { get; set; } = false;
    private channelOptions globalOptions = new channelOptions();
    private Font labelFont;
    private string externalTrigger;

    private void ChannelConfigForm_Load(System.Object sender, System.EventArgs e)
    {
        ComboBoxAlgorithm.Items.Clear();
        ComboBoxAlgorithm.Items.AddRange(TriggeringAlgorithms.Algorithms);
        ButtonColor.BackColor = Options.waveColor;
        CheckBoxPulseWidthModulatedColor.Checked = Options.pulseWidthModulatedColor;
        ComboBoxAlgorithm.SelectedIndex = Options.algorithm;
        ComboBoxScanPhase.SelectedIndex = Options.scanPhase;
        TextBoxHorizontalTime.Text = Options.horizontalTime * 1000;
        NumericUpDownTriggerLevel.Value = Options.trigger;
        CheckBoxAutoTriggerLevel.Checked = Options.autoTriggerLevel;
        CheckBoxExternalTrigger.Checked = Options.externalTriggerEnabled;
        ButtonExternalTrigger.Enabled = Options.externalTriggerEnabled;
        if (System.IO.File.Exists(Options.externalTriggerFile))
        {
            ButtonExternalTrigger.Text = new System.IO.FileInfo(Options.externalTriggerFile).Name;
            externalTrigger = Options.externalTriggerFile;
        }
        else
        {
            ButtonExternalTrigger.Text = "(None)";
            externalTrigger = "";
        }
        TextBoxAmplify.Text = Options.amplify;
        TextBoxLabel.Text = Options.label;
        labelFont = Options.labelFont;
        ButtonFontColor.BackColor = Options.labelColor;
        switch (Options.maxScan)
        {
            case 1.0F:
                {
                    RadioButton1x.Checked = true;
                    break;
                }

            case 1.5F:
                {
                    RadioButton1dot5x.Checked = true;
                    break;
                }

            case 2.0F:
                {
                    RadioButton2x.Checked = true;
                    break;
                }
        }
        NumericUpDownAudioChannel.Value = Options.selectedChannel;
        CheckBoxMixAudioChannel.Checked = Options.mixChannel;
        CheckBoxXYmode.Checked = Options.XYmode;
        CheckBoxXYaspectRatio.Checked = Options.XYmodeAspectRatio;
        CheckBoxXYmode_CheckedChanged(null, null);
    }

    private void ChannelConfigForm_KeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
            ButtonOK_Click(null, null);
    }

    public void checkValueValid()
    {
        if (Information.IsNumeric(TextBoxHorizontalTime.Text) & Information.IsNumeric(TextBoxAmplify.Text))
        {
            if (TextBoxHorizontalTime.Text >= 1)
                ButtonOK.Enabled = true;
            else
                ButtonOK.Enabled = false;
        }
        else
            ButtonOK.Enabled = false;
    }

    private void ButtonFont_Click(object sender, EventArgs e)
    {
        FontDialog fd = new FontDialog();
        fd.Font = labelFont;
        if (fd.ShowDialog() == DialogResult.OK)
            labelFont = fd.Font;
    }

    private void ButtonFontColor_Click(object sender, EventArgs e)
    {
        ColorDialog cd = new ColorDialog();
        cd.Color = ButtonColor.BackColor;
        if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            ButtonFontColor.BackColor = cd.Color;
    }

    private void ButtonColor_Click(System.Object sender, System.EventArgs e)
    {
        ColorDialog cd = new ColorDialog();
        cd.Color = ButtonColor.BackColor;
        if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            ButtonColor.BackColor = cd.Color;
    }

    private void ButtonColor_BackColorChanged(object sender, EventArgs e)
    {
        ButtonColor.ForeColor = My.MyProject.MyForms.MainForm.getTextColor(ButtonColor.BackColor);
    }

    private void TextBoxTimeScale_TextChanged(System.Object sender, System.EventArgs e)
    {
        checkValueValid();
    }

    private void TextBoxAmplify_TextChanged(System.Object sender, System.EventArgs e)
    {
        checkValueValid();
    }

    private void ButtonOK_Click(System.Object sender, System.EventArgs e)
    {
        if (CheckBoxExternalTrigger.Checked)
            CheckBoxExternalTrigger.Checked = System.IO.File.Exists(externalTrigger);
        if (TextBoxLabel.Text.Replace(" ", "").Length == 0)
            TextBoxLabel.Text = "";
        Options.waveColor = ButtonColor.BackColor;
        Options.pulseWidthModulatedColor = CheckBoxPulseWidthModulatedColor.Checked;
        Options.algorithm = ComboBoxAlgorithm.SelectedIndex;
        Options.horizontalTime = TextBoxHorizontalTime.Text / (double)1000;
        Options.trigger = NumericUpDownTriggerLevel.Value;
        Options.scanPhase = ComboBoxScanPhase.SelectedIndex;
        Options.autoTriggerLevel = CheckBoxAutoTriggerLevel.Checked;
        Options.externalTriggerEnabled = CheckBoxExternalTrigger.Checked;
        Options.externalTriggerFile = externalTrigger;
        Options.amplify = TextBoxAmplify.Text;
        Options.label = TextBoxLabel.Text;
        Options.labelFont = labelFont;
        Options.labelColor = ButtonFontColor.BackColor;
        switch (true)
        {
            case object _ when RadioButton1x.Checked:
                {
                    Options.maxScan = 1.0;
                    break;
                }

            case object _ when RadioButton1dot5x.Checked:
                {
                    Options.maxScan = 1.5;
                    break;
                }

            case object _ when RadioButton2x.Checked:
                {
                    Options.maxScan = 2.0;
                    break;
                }
        }
        Options.selectedChannel = NumericUpDownAudioChannel.Value;
        Options.mixChannel = CheckBoxMixAudioChannel.Checked;
        Options.XYmode = CheckBoxXYmode.Checked;
        Options.XYmodeAspectRatio = CheckBoxXYaspectRatio.Checked;
        this.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.Close();
    }

    private void ButtonCancel_Click(System.Object sender, System.EventArgs e)
    {
        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.Close();
    }

    private void CheckBoxStereo_CheckedChanged(object sender, EventArgs e)
    {
        LabelAudioChannel.Enabled = !CheckBoxMixAudioChannel.Checked;
        NumericUpDownAudioChannel.Enabled = !CheckBoxMixAudioChannel.Checked;
    }

    private void ButtonExternalTrigger_Click(object sender, EventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            ButtonExternalTrigger.Text = ofd.SafeFileName;
            externalTrigger = ofd.FileName;
        }
    }

    private void CheckBoxExternalTrigger_CheckedChanged(object sender, EventArgs e)
    {
        ButtonExternalTrigger.Enabled = CheckBoxExternalTrigger.Checked;
    }

    private void CheckBoxAutoTriggerLevel_CheckedChanged(object sender, EventArgs e)
    {
        NumericUpDownTriggerLevel.Enabled = !CheckBoxAutoTriggerLevel.Checked;
    }

    private void ButtonAutoAmplify_Click(object sender, EventArgs e)
    {
        double smallestAmplifyValue = double.MaxValue;
        List<string> alreadyScannedFilesList = new List<string>();
        foreach (var file in My.MyProject.MyForms.MainForm.ListBoxFiles.SelectedItems)
        {
            if (alreadyScannedFilesList.Contains(file))
                continue;
            else
                alreadyScannedFilesList.Add(file);
            AutoAmplifyWorkerForm aawf = new AutoAmplifyWorkerForm();
            aawf.Filename = file;
            if (aawf.ShowDialog() == DialogResult.Cancel)
                break;
            double val = aawf.Result;
            if (val < 1)
                val = 1;
            if (val < smallestAmplifyValue)
                smallestAmplifyValue = val;
        }
        if (!CheckBoxMixAudioChannel.Checked)
            smallestAmplifyValue /= 2;
        if (smallestAmplifyValue != double.MaxValue)
            TextBoxAmplify.Text = Math.Round(smallestAmplifyValue, 1);
    }

    private void ComboBoxAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
    {
        LabelScanPhase.Enabled = ComboBoxAlgorithm.SelectedIndex == TriggeringAlgorithms.UseMaxLengthScanning
                                | ComboBoxAlgorithm.SelectedIndex == TriggeringAlgorithms.UseMaxRectifiedAreaScanning;
        ComboBoxScanPhase.Enabled = LabelScanPhase.Enabled;
    }

    private void CheckBoxXYmode_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBoxXYmode.Checked)
        {
            ComboBoxAlgorithm.SelectedIndex = TriggeringAlgorithms.Algorithms.Length - 1;
            CheckBoxMixAudioChannel.Checked = false;
        }
        Control[] excludedControls = new[] { LabelChannelLabel, TextBoxLabel, ButtonFont, ButtonFontColor, ButtonColor, LabelAmplify, TextBoxAmplify, LabelX, ButtonAutoAmplify, CheckBoxXYmode, CheckBoxXYaspectRatio, ButtonOK, ButtonCancel };
        foreach (Control ctrl in this.Controls)
        {
            if (!excludedControls.Contains(ctrl))
                ctrl.Enabled = !CheckBoxXYmode.Checked;
        }
        CheckBoxXYaspectRatio.Enabled = CheckBoxXYmode.Checked;
    }
}
