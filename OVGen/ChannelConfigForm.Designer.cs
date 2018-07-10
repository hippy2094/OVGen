using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Xml.Linq;


partial class ChannelConfigForm : System.Windows.Forms.Form
{

    // Form 覆寫 Dispose 以清除元件清單。
    [System.Diagnostics.DebuggerNonUserCode()]
    protected override void Dispose(bool disposing)
    {
        try
        {
            if (disposing && components != null)
                components.Dispose();
        }
        finally
        {
            base.Dispose(disposing);
        }
    }

    // 為 Windows Form 設計工具的必要項
    private System.ComponentModel.IContainer components;

    // 注意: 以下為 Windows Form 設計工具所需的程序
    // 可以使用 Windows Form 設計工具進行修改。
    // 請不要使用程式碼編輯器進行修改。
    [System.Diagnostics.DebuggerStepThrough()]
    private void InitializeComponent()
    {
        this.ButtonColor = new System.Windows.Forms.Button();
        this.ComboBoxAlgorithm = new System.Windows.Forms.ComboBox();
        this.TextBoxHorizontalTime = new System.Windows.Forms.TextBox();
        this.LabelMS = new System.Windows.Forms.Label();
        this.LabelAmplify = new System.Windows.Forms.Label();
        this.TextBoxAmplify = new System.Windows.Forms.TextBox();
        this.LabelX = new System.Windows.Forms.Label();
        this.LabelTimeScale = new System.Windows.Forms.Label();
        this.ButtonOK = new System.Windows.Forms.Button();
        this.ButtonCancel = new System.Windows.Forms.Button();
        this.TextBoxLabel = new System.Windows.Forms.TextBox();
        this.ButtonFont = new System.Windows.Forms.Button();
        this.LabelChannelLabel = new System.Windows.Forms.Label();
        this.ButtonFontColor = new System.Windows.Forms.Button();
        this.LabelAlgorithm = new System.Windows.Forms.Label();
        this.LabelScanTime = new System.Windows.Forms.Label();
        this.RadioButton1x = new System.Windows.Forms.RadioButton();
        this.RadioButton2x = new System.Windows.Forms.RadioButton();
        this.CheckBoxMixAudioChannel = new System.Windows.Forms.CheckBox();
        this.LabelAudioChannel = new System.Windows.Forms.Label();
        this.NumericUpDownAudioChannel = new System.Windows.Forms.NumericUpDown();
        this.RadioButton1dot5x = new System.Windows.Forms.RadioButton();
        this.LabelTriggerLevel = new System.Windows.Forms.Label();
        this.NumericUpDownTriggerLevel = new System.Windows.Forms.NumericUpDown();
        this.CheckBoxExternalTrigger = new System.Windows.Forms.CheckBox();
        this.ButtonExternalTrigger = new System.Windows.Forms.Button();
        this.CheckBoxPulseWidthModulatedColor = new System.Windows.Forms.CheckBox();
        this.CheckBoxAutoTriggerLevel = new System.Windows.Forms.CheckBox();
        this.ButtonAutoAmplify = new System.Windows.Forms.Button();
        this.LabelScanPhase = new System.Windows.Forms.Label();
        this.ComboBoxScanPhase = new System.Windows.Forms.ComboBox();
        this.CheckBoxXYmode = new System.Windows.Forms.CheckBox();
        this.CheckBoxXYaspectRatio = new System.Windows.Forms.CheckBox();
        (System.ComponentModel.ISupportInitialize)this.NumericUpDownAudioChannel.BeginInit();
        (System.ComponentModel.ISupportInitialize)this.NumericUpDownTriggerLevel.BeginInit();
        this.SuspendLayout();
        // 
        // ButtonColor
        // 
        this.ButtonColor.Location = new System.Drawing.Point(11, 57);
        this.ButtonColor.Margin = new System.Windows.Forms.Padding(2);
        this.ButtonColor.Name = "ButtonColor";
        this.ButtonColor.Size = new System.Drawing.Size(253, 26);
        this.ButtonColor.TabIndex = 4;
        this.ButtonColor.Text = "Set Wave Color";
        this.ButtonColor.UseVisualStyleBackColor = true;
        // 
        // ComboBoxAlgorithm
        // 
        this.ComboBoxAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.ComboBoxAlgorithm.FormattingEnabled = true;
        this.ComboBoxAlgorithm.Location = new System.Drawing.Point(81, 112);
        this.ComboBoxAlgorithm.Margin = new System.Windows.Forms.Padding(2);
        this.ComboBoxAlgorithm.Name = "ComboBoxAlgorithm";
        this.ComboBoxAlgorithm.Size = new System.Drawing.Size(183, 23);
        this.ComboBoxAlgorithm.TabIndex = 7;
        // 
        // TextBoxHorizontalTime
        // 
        this.TextBoxHorizontalTime.ImeMode = System.Windows.Forms.ImeMode.Off;
        this.TextBoxHorizontalTime.Location = new System.Drawing.Point(109, 168);
        this.TextBoxHorizontalTime.Margin = new System.Windows.Forms.Padding(2);
        this.TextBoxHorizontalTime.Name = "TextBoxHorizontalTime";
        this.TextBoxHorizontalTime.Size = new System.Drawing.Size(33, 25);
        this.TextBoxHorizontalTime.TabIndex = 11;
        // 
        // LabelMS
        // 
        this.LabelMS.AutoSize = true;
        this.LabelMS.Location = new System.Drawing.Point(146, 171);
        this.LabelMS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.LabelMS.Name = "LabelMS";
        this.LabelMS.Size = new System.Drawing.Size(23, 15);
        this.LabelMS.TabIndex = 12;
        this.LabelMS.Text = "ms";
        // 
        // LabelAmplify
        // 
        this.LabelAmplify.AutoSize = true;
        this.LabelAmplify.Location = new System.Drawing.Point(6, 264);
        this.LabelAmplify.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.LabelAmplify.Name = "LabelAmplify";
        this.LabelAmplify.Size = new System.Drawing.Size(59, 15);
        this.LabelAmplify.TabIndex = 18;
        this.LabelAmplify.Text = "Amplify:";
        // 
        // TextBoxAmplify
        // 
        this.TextBoxAmplify.ImeMode = System.Windows.Forms.ImeMode.Off;
        this.TextBoxAmplify.Location = new System.Drawing.Point(68, 260);
        this.TextBoxAmplify.Margin = new System.Windows.Forms.Padding(2);
        this.TextBoxAmplify.Name = "TextBoxAmplify";
        this.TextBoxAmplify.Size = new System.Drawing.Size(50, 25);
        this.TextBoxAmplify.TabIndex = 19;
        // 
        // LabelX
        // 
        this.LabelX.AutoSize = true;
        this.LabelX.Location = new System.Drawing.Point(122, 264);
        this.LabelX.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.LabelX.Name = "LabelX";
        this.LabelX.Size = new System.Drawing.Size(14, 15);
        this.LabelX.TabIndex = 20;
        this.LabelX.Text = "x";
        // 
        // LabelTimeScale
        // 
        this.LabelTimeScale.AutoSize = true;
        this.LabelTimeScale.Location = new System.Drawing.Point(5, 171);
        this.LabelTimeScale.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.LabelTimeScale.Name = "LabelTimeScale";
        this.LabelTimeScale.Size = new System.Drawing.Size(100, 15);
        this.LabelTimeScale.TabIndex = 10;
        this.LabelTimeScale.Text = "Horizontal time:";
        // 
        // ButtonOK
        // 
        this.ButtonOK.Location = new System.Drawing.Point(110, 373);
        this.ButtonOK.Margin = new System.Windows.Forms.Padding(2);
        this.ButtonOK.Name = "ButtonOK";
        this.ButtonOK.Size = new System.Drawing.Size(75, 25);
        this.ButtonOK.TabIndex = 29;
        this.ButtonOK.Text = "OK";
        this.ButtonOK.UseVisualStyleBackColor = true;
        // 
        // ButtonCancel
        // 
        this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.ButtonCancel.Location = new System.Drawing.Point(189, 373);
        this.ButtonCancel.Margin = new System.Windows.Forms.Padding(2);
        this.ButtonCancel.Name = "ButtonCancel";
        this.ButtonCancel.Size = new System.Drawing.Size(75, 25);
        this.ButtonCancel.TabIndex = 30;
        this.ButtonCancel.Text = "Cancel";
        this.ButtonCancel.UseVisualStyleBackColor = true;
        // 
        // TextBoxLabel
        // 
        this.TextBoxLabel.Location = new System.Drawing.Point(12, 27);
        this.TextBoxLabel.Name = "TextBoxLabel";
        this.TextBoxLabel.Size = new System.Drawing.Size(140, 25);
        this.TextBoxLabel.TabIndex = 1;
        // 
        // ButtonFont
        // 
        this.ButtonFont.Location = new System.Drawing.Point(158, 24);
        this.ButtonFont.Name = "ButtonFont";
        this.ButtonFont.Size = new System.Drawing.Size(50, 25);
        this.ButtonFont.TabIndex = 2;
        this.ButtonFont.Text = "Font";
        this.ButtonFont.UseVisualStyleBackColor = true;
        // 
        // LabelChannelLabel
        // 
        this.LabelChannelLabel.AutoSize = true;
        this.LabelChannelLabel.Location = new System.Drawing.Point(12, 9);
        this.LabelChannelLabel.Name = "LabelChannelLabel";
        this.LabelChannelLabel.Size = new System.Drawing.Size(93, 15);
        this.LabelChannelLabel.TabIndex = 0;
        this.LabelChannelLabel.Text = "Channel Label:";
        // 
        // ButtonFontColor
        // 
        this.ButtonFontColor.Location = new System.Drawing.Point(214, 24);
        this.ButtonFontColor.Name = "ButtonFontColor";
        this.ButtonFontColor.Size = new System.Drawing.Size(50, 25);
        this.ButtonFontColor.TabIndex = 3;
        this.ButtonFontColor.Text = "Color";
        this.ButtonFontColor.UseVisualStyleBackColor = true;
        // 
        // LabelAlgorithm
        // 
        this.LabelAlgorithm.AutoSize = true;
        this.LabelAlgorithm.Location = new System.Drawing.Point(5, 115);
        this.LabelAlgorithm.Name = "LabelAlgorithm";
        this.LabelAlgorithm.Size = new System.Drawing.Size(70, 15);
        this.LabelAlgorithm.TabIndex = 6;
        this.LabelAlgorithm.Text = "Algorithm:";
        // 
        // LabelScanTime
        // 
        this.LabelScanTime.AutoSize = true;
        this.LabelScanTime.Location = new System.Drawing.Point(5, 292);
        this.LabelScanTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.LabelScanTime.Name = "LabelScanTime";
        this.LabelScanTime.Size = new System.Drawing.Size(102, 15);
        this.LabelScanTime.TabIndex = 22;
        this.LabelScanTime.Text = "Max Scan Time:";
        // 
        // RadioButton1x
        // 
        this.RadioButton1x.AutoSize = true;
        this.RadioButton1x.Location = new System.Drawing.Point(112, 290);
        this.RadioButton1x.Name = "RadioButton1x";
        this.RadioButton1x.Size = new System.Drawing.Size(42, 19);
        this.RadioButton1x.TabIndex = 23;
        this.RadioButton1x.TabStop = true;
        this.RadioButton1x.Text = "1x";
        this.RadioButton1x.UseVisualStyleBackColor = true;
        // 
        // RadioButton2x
        // 
        this.RadioButton2x.AutoSize = true;
        this.RadioButton2x.Location = new System.Drawing.Point(219, 290);
        this.RadioButton2x.Name = "RadioButton2x";
        this.RadioButton2x.Size = new System.Drawing.Size(42, 19);
        this.RadioButton2x.TabIndex = 25;
        this.RadioButton2x.TabStop = true;
        this.RadioButton2x.Text = "2x";
        this.RadioButton2x.UseVisualStyleBackColor = true;
        // 
        // CheckBoxMixAudioChannel
        // 
        this.CheckBoxMixAudioChannel.AutoSize = true;
        this.CheckBoxMixAudioChannel.Location = new System.Drawing.Point(171, 317);
        this.CheckBoxMixAudioChannel.Name = "CheckBoxMixAudioChannel";
        this.CheckBoxMixAudioChannel.Size = new System.Drawing.Size(90, 19);
        this.CheckBoxMixAudioChannel.TabIndex = 28;
        this.CheckBoxMixAudioChannel.Text = "Mix stereo";
        this.CheckBoxMixAudioChannel.UseVisualStyleBackColor = true;
        // 
        // LabelAudioChannel
        // 
        this.LabelAudioChannel.AutoSize = true;
        this.LabelAudioChannel.Location = new System.Drawing.Point(5, 318);
        this.LabelAudioChannel.Name = "LabelAudioChannel";
        this.LabelAudioChannel.Size = new System.Drawing.Size(93, 15);
        this.LabelAudioChannel.TabIndex = 26;
        this.LabelAudioChannel.Text = "Audio channel:";
        // 
        // NumericUpDownAudioChannel
        // 
        this.NumericUpDownAudioChannel.Location = new System.Drawing.Point(104, 316);
        this.NumericUpDownAudioChannel.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
        this.NumericUpDownAudioChannel.Name = "NumericUpDownAudioChannel";
        this.NumericUpDownAudioChannel.Size = new System.Drawing.Size(61, 25);
        this.NumericUpDownAudioChannel.TabIndex = 27;
        // 
        // RadioButton1dot5x
        // 
        this.RadioButton1dot5x.AutoSize = true;
        this.RadioButton1dot5x.Location = new System.Drawing.Point(160, 290);
        this.RadioButton1dot5x.Name = "RadioButton1dot5x";
        this.RadioButton1dot5x.Size = new System.Drawing.Size(53, 19);
        this.RadioButton1dot5x.TabIndex = 24;
        this.RadioButton1dot5x.TabStop = true;
        this.RadioButton1dot5x.Text = "1.5x";
        this.RadioButton1dot5x.UseVisualStyleBackColor = true;
        // 
        // LabelTriggerLevel
        // 
        this.LabelTriggerLevel.AutoSize = true;
        this.LabelTriggerLevel.Location = new System.Drawing.Point(5, 201);
        this.LabelTriggerLevel.Name = "LabelTriggerLevel";
        this.LabelTriggerLevel.Size = new System.Drawing.Size(85, 15);
        this.LabelTriggerLevel.TabIndex = 13;
        this.LabelTriggerLevel.Text = "Trigger level:";
        // 
        // NumericUpDownTriggerLevel
        // 
        this.NumericUpDownTriggerLevel.Location = new System.Drawing.Point(96, 198);
        this.NumericUpDownTriggerLevel.Maximum = new decimal(new int[] { 127, 0, 0, 0 });
        this.NumericUpDownTriggerLevel.Minimum = new decimal(new int[] { 128, 0, 0, -2147483648 });
        this.NumericUpDownTriggerLevel.Name = "NumericUpDownTriggerLevel";
        this.NumericUpDownTriggerLevel.Size = new System.Drawing.Size(60, 25);
        this.NumericUpDownTriggerLevel.TabIndex = 14;
        // 
        // CheckBoxExternalTrigger
        // 
        this.CheckBoxExternalTrigger.AutoSize = true;
        this.CheckBoxExternalTrigger.Location = new System.Drawing.Point(9, 232);
        this.CheckBoxExternalTrigger.Name = "CheckBoxExternalTrigger";
        this.CheckBoxExternalTrigger.Size = new System.Drawing.Size(90, 19);
        this.CheckBoxExternalTrigger.TabIndex = 16;
        this.CheckBoxExternalTrigger.Text = "Ext. Trig.:";
        this.CheckBoxExternalTrigger.UseVisualStyleBackColor = true;
        // 
        // ButtonExternalTrigger
        // 
        this.ButtonExternalTrigger.Location = new System.Drawing.Point(101, 230);
        this.ButtonExternalTrigger.Name = "ButtonExternalTrigger";
        this.ButtonExternalTrigger.Size = new System.Drawing.Size(156, 23);
        this.ButtonExternalTrigger.TabIndex = 17;
        this.ButtonExternalTrigger.Text = "(None)";
        this.ButtonExternalTrigger.UseVisualStyleBackColor = true;
        // 
        // CheckBoxPulseWidthModulatedColor
        // 
        this.CheckBoxPulseWidthModulatedColor.AutoSize = true;
        this.CheckBoxPulseWidthModulatedColor.Location = new System.Drawing.Point(11, 88);
        this.CheckBoxPulseWidthModulatedColor.Name = "CheckBoxPulseWidthModulatedColor";
        this.CheckBoxPulseWidthModulatedColor.Size = new System.Drawing.Size(191, 19);
        this.CheckBoxPulseWidthModulatedColor.TabIndex = 5;
        this.CheckBoxPulseWidthModulatedColor.Text = "Pulse width modulated color";
        this.CheckBoxPulseWidthModulatedColor.UseVisualStyleBackColor = true;
        // 
        // CheckBoxAutoTriggerLevel
        // 
        this.CheckBoxAutoTriggerLevel.AutoSize = true;
        this.CheckBoxAutoTriggerLevel.Location = new System.Drawing.Point(162, 201);
        this.CheckBoxAutoTriggerLevel.Name = "CheckBoxAutoTriggerLevel";
        this.CheckBoxAutoTriggerLevel.Size = new System.Drawing.Size(57, 19);
        this.CheckBoxAutoTriggerLevel.TabIndex = 15;
        this.CheckBoxAutoTriggerLevel.Text = "Auto";
        this.CheckBoxAutoTriggerLevel.UseVisualStyleBackColor = true;
        // 
        // ButtonAutoAmplify
        // 
        this.ButtonAutoAmplify.Location = new System.Drawing.Point(141, 260);
        this.ButtonAutoAmplify.Name = "ButtonAutoAmplify";
        this.ButtonAutoAmplify.Size = new System.Drawing.Size(60, 25);
        this.ButtonAutoAmplify.TabIndex = 21;
        this.ButtonAutoAmplify.Text = "Auto";
        this.ButtonAutoAmplify.UseVisualStyleBackColor = true;
        // 
        // LabelScanPhase
        // 
        this.LabelScanPhase.AutoSize = true;
        this.LabelScanPhase.Location = new System.Drawing.Point(5, 143);
        this.LabelScanPhase.Name = "LabelScanPhase";
        this.LabelScanPhase.Size = new System.Drawing.Size(74, 15);
        this.LabelScanPhase.TabIndex = 8;
        this.LabelScanPhase.Text = "Scan Phase:";
        // 
        // ComboBoxScanPhase
        // 
        this.ComboBoxScanPhase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.ComboBoxScanPhase.FormattingEnabled = true;
        this.ComboBoxScanPhase.Items.AddRange(new object[] { "Positive", "Negative", "Both" });
        this.ComboBoxScanPhase.Location = new System.Drawing.Point(81, 140);
        this.ComboBoxScanPhase.Name = "ComboBoxScanPhase";
        this.ComboBoxScanPhase.Size = new System.Drawing.Size(97, 23);
        this.ComboBoxScanPhase.TabIndex = 9;
        // 
        // CheckBoxXYmode
        // 
        this.CheckBoxXYmode.AutoSize = true;
        this.CheckBoxXYmode.Location = new System.Drawing.Point(8, 347);
        this.CheckBoxXYmode.Name = "CheckBoxXYmode";
        this.CheckBoxXYmode.Size = new System.Drawing.Size(84, 19);
        this.CheckBoxXYmode.TabIndex = 31;
        this.CheckBoxXYmode.Text = "XY mode";
        this.CheckBoxXYmode.UseVisualStyleBackColor = true;
        // 
        // CheckBoxXYaspectRatio
        // 
        this.CheckBoxXYaspectRatio.AutoSize = true;
        this.CheckBoxXYaspectRatio.Checked = true;
        this.CheckBoxXYaspectRatio.CheckState = System.Windows.Forms.CheckState.Checked;
        this.CheckBoxXYaspectRatio.Location = new System.Drawing.Point(98, 347);
        this.CheckBoxXYaspectRatio.Name = "CheckBoxXYaspectRatio";
        this.CheckBoxXYaspectRatio.Size = new System.Drawing.Size(174, 19);
        this.CheckBoxXYaspectRatio.TabIndex = 32;
        this.CheckBoxXYaspectRatio.Text = "XY mode 1:1 aspect ratio";
        this.CheckBoxXYaspectRatio.UseVisualStyleBackColor = true;
        // 
        // ChannelConfigForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(8.0f, 15.0f);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.ButtonCancel;
        this.ClientSize = new System.Drawing.Size(272, 409);
        this.Controls.Add(this.CheckBoxXYaspectRatio);
        this.Controls.Add(this.CheckBoxXYmode);
        this.Controls.Add(this.ComboBoxScanPhase);
        this.Controls.Add(this.LabelScanPhase);
        this.Controls.Add(this.ButtonAutoAmplify);
        this.Controls.Add(this.CheckBoxAutoTriggerLevel);
        this.Controls.Add(this.CheckBoxPulseWidthModulatedColor);
        this.Controls.Add(this.ButtonExternalTrigger);
        this.Controls.Add(this.CheckBoxExternalTrigger);
        this.Controls.Add(this.NumericUpDownTriggerLevel);
        this.Controls.Add(this.LabelTriggerLevel);
        this.Controls.Add(this.RadioButton1dot5x);
        this.Controls.Add(this.NumericUpDownAudioChannel);
        this.Controls.Add(this.LabelAudioChannel);
        this.Controls.Add(this.CheckBoxMixAudioChannel);
        this.Controls.Add(this.RadioButton2x);
        this.Controls.Add(this.RadioButton1x);
        this.Controls.Add(this.LabelScanTime);
        this.Controls.Add(this.LabelAlgorithm);
        this.Controls.Add(this.ButtonFontColor);
        this.Controls.Add(this.LabelChannelLabel);
        this.Controls.Add(this.ButtonFont);
        this.Controls.Add(this.TextBoxLabel);
        this.Controls.Add(this.ButtonCancel);
        this.Controls.Add(this.ButtonOK);
        this.Controls.Add(this.LabelTimeScale);
        this.Controls.Add(this.LabelX);
        this.Controls.Add(this.TextBoxAmplify);
        this.Controls.Add(this.LabelAmplify);
        this.Controls.Add(this.LabelMS);
        this.Controls.Add(this.TextBoxHorizontalTime);
        this.Controls.Add(this.ComboBoxAlgorithm);
        this.Controls.Add(this.ButtonColor);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        this.Margin = new System.Windows.Forms.Padding(2);
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "ChannelConfigForm";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Channel config";
        (System.ComponentModel.ISupportInitialize)this.NumericUpDownAudioChannel.EndInit();
        (System.ComponentModel.ISupportInitialize)this.NumericUpDownTriggerLevel.EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }
    private System.Windows.Forms.Button _ButtonColor;

    internal System.Windows.Forms.Button ButtonColor
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ButtonColor;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ButtonColor != null)
            {
            }

            _ButtonColor = value;
            if (_ButtonColor != null)
            {
            }
        }
    }

    private System.Windows.Forms.ComboBox _ComboBoxAlgorithm;

    internal System.Windows.Forms.ComboBox ComboBoxAlgorithm
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ComboBoxAlgorithm;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ComboBoxAlgorithm != null)
            {
            }

            _ComboBoxAlgorithm = value;
            if (_ComboBoxAlgorithm != null)
            {
            }
        }
    }

    private System.Windows.Forms.TextBox _TextBoxHorizontalTime;

    internal System.Windows.Forms.TextBox TextBoxHorizontalTime
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _TextBoxHorizontalTime;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_TextBoxHorizontalTime != null)
            {
            }

            _TextBoxHorizontalTime = value;
            if (_TextBoxHorizontalTime != null)
            {
            }
        }
    }

    private System.Windows.Forms.Label _LabelMS;

    internal System.Windows.Forms.Label LabelMS
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelMS;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelMS != null)
            {
            }

            _LabelMS = value;
            if (_LabelMS != null)
            {
            }
        }
    }

    private System.Windows.Forms.Label _LabelAmplify;

    internal System.Windows.Forms.Label LabelAmplify
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelAmplify;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelAmplify != null)
            {
            }

            _LabelAmplify = value;
            if (_LabelAmplify != null)
            {
            }
        }
    }

    private System.Windows.Forms.TextBox _TextBoxAmplify;

    internal System.Windows.Forms.TextBox TextBoxAmplify
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _TextBoxAmplify;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_TextBoxAmplify != null)
            {
            }

            _TextBoxAmplify = value;
            if (_TextBoxAmplify != null)
            {
            }
        }
    }

    private System.Windows.Forms.Label _LabelX;

    internal System.Windows.Forms.Label LabelX
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelX;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelX != null)
            {
            }

            _LabelX = value;
            if (_LabelX != null)
            {
            }
        }
    }

    private System.Windows.Forms.Label _LabelTimeScale;

    internal System.Windows.Forms.Label LabelTimeScale
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelTimeScale;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelTimeScale != null)
            {
            }

            _LabelTimeScale = value;
            if (_LabelTimeScale != null)
            {
            }
        }
    }

    private System.Windows.Forms.Button _ButtonOK;

    internal System.Windows.Forms.Button ButtonOK
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ButtonOK;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ButtonOK != null)
            {
            }

            _ButtonOK = value;
            if (_ButtonOK != null)
            {
            }
        }
    }

    private System.Windows.Forms.Button _ButtonCancel;

    internal System.Windows.Forms.Button ButtonCancel
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ButtonCancel;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ButtonCancel != null)
            {
            }

            _ButtonCancel = value;
            if (_ButtonCancel != null)
            {
            }
        }
    }

    private TextBox _TextBoxLabel;

    internal TextBox TextBoxLabel
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _TextBoxLabel;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_TextBoxLabel != null)
            {
            }

            _TextBoxLabel = value;
            if (_TextBoxLabel != null)
            {
            }
        }
    }

    private Button _ButtonFont;

    internal Button ButtonFont
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ButtonFont;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ButtonFont != null)
            {
            }

            _ButtonFont = value;
            if (_ButtonFont != null)
            {
            }
        }
    }

    private Label _LabelChannelLabel;

    internal Label LabelChannelLabel
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelChannelLabel;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelChannelLabel != null)
            {
            }

            _LabelChannelLabel = value;
            if (_LabelChannelLabel != null)
            {
            }
        }
    }

    private Button _ButtonFontColor;

    internal Button ButtonFontColor
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ButtonFontColor;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ButtonFontColor != null)
            {
            }

            _ButtonFontColor = value;
            if (_ButtonFontColor != null)
            {
            }
        }
    }

    private Label _LabelAlgorithm;

    internal Label LabelAlgorithm
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelAlgorithm;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelAlgorithm != null)
            {
            }

            _LabelAlgorithm = value;
            if (_LabelAlgorithm != null)
            {
            }
        }
    }

    private Label _LabelScanTime;

    internal Label LabelScanTime
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelScanTime;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelScanTime != null)
            {
            }

            _LabelScanTime = value;
            if (_LabelScanTime != null)
            {
            }
        }
    }

    private RadioButton _RadioButton1x;

    internal RadioButton RadioButton1x
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _RadioButton1x;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_RadioButton1x != null)
            {
            }

            _RadioButton1x = value;
            if (_RadioButton1x != null)
            {
            }
        }
    }

    private RadioButton _RadioButton2x;

    internal RadioButton RadioButton2x
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _RadioButton2x;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_RadioButton2x != null)
            {
            }

            _RadioButton2x = value;
            if (_RadioButton2x != null)
            {
            }
        }
    }

    private CheckBox _CheckBoxMixAudioChannel;

    internal CheckBox CheckBoxMixAudioChannel
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _CheckBoxMixAudioChannel;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_CheckBoxMixAudioChannel != null)
            {
            }

            _CheckBoxMixAudioChannel = value;
            if (_CheckBoxMixAudioChannel != null)
            {
            }
        }
    }

    private Label _LabelAudioChannel;

    internal Label LabelAudioChannel
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelAudioChannel;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelAudioChannel != null)
            {
            }

            _LabelAudioChannel = value;
            if (_LabelAudioChannel != null)
            {
            }
        }
    }

    private NumericUpDown _NumericUpDownAudioChannel;

    internal NumericUpDown NumericUpDownAudioChannel
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _NumericUpDownAudioChannel;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_NumericUpDownAudioChannel != null)
            {
            }

            _NumericUpDownAudioChannel = value;
            if (_NumericUpDownAudioChannel != null)
            {
            }
        }
    }

    private RadioButton _RadioButton1dot5x;

    internal RadioButton RadioButton1dot5x
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _RadioButton1dot5x;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_RadioButton1dot5x != null)
            {
            }

            _RadioButton1dot5x = value;
            if (_RadioButton1dot5x != null)
            {
            }
        }
    }

    private Label _LabelTriggerLevel;

    internal Label LabelTriggerLevel
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelTriggerLevel;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelTriggerLevel != null)
            {
            }

            _LabelTriggerLevel = value;
            if (_LabelTriggerLevel != null)
            {
            }
        }
    }

    private NumericUpDown _NumericUpDownTriggerLevel;

    internal NumericUpDown NumericUpDownTriggerLevel
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _NumericUpDownTriggerLevel;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_NumericUpDownTriggerLevel != null)
            {
            }

            _NumericUpDownTriggerLevel = value;
            if (_NumericUpDownTriggerLevel != null)
            {
            }
        }
    }

    private CheckBox _CheckBoxExternalTrigger;

    internal CheckBox CheckBoxExternalTrigger
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _CheckBoxExternalTrigger;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_CheckBoxExternalTrigger != null)
            {
            }

            _CheckBoxExternalTrigger = value;
            if (_CheckBoxExternalTrigger != null)
            {
            }
        }
    }

    private Button _ButtonExternalTrigger;

    internal Button ButtonExternalTrigger
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ButtonExternalTrigger;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ButtonExternalTrigger != null)
            {
            }

            _ButtonExternalTrigger = value;
            if (_ButtonExternalTrigger != null)
            {
            }
        }
    }

    private CheckBox _CheckBoxPulseWidthModulatedColor;

    internal CheckBox CheckBoxPulseWidthModulatedColor
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _CheckBoxPulseWidthModulatedColor;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_CheckBoxPulseWidthModulatedColor != null)
            {
            }

            _CheckBoxPulseWidthModulatedColor = value;
            if (_CheckBoxPulseWidthModulatedColor != null)
            {
            }
        }
    }

    private CheckBox _CheckBoxAutoTriggerLevel;

    internal CheckBox CheckBoxAutoTriggerLevel
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _CheckBoxAutoTriggerLevel;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_CheckBoxAutoTriggerLevel != null)
            {
            }

            _CheckBoxAutoTriggerLevel = value;
            if (_CheckBoxAutoTriggerLevel != null)
            {
            }
        }
    }

    private Button _ButtonAutoAmplify;

    internal Button ButtonAutoAmplify
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ButtonAutoAmplify;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ButtonAutoAmplify != null)
            {
            }

            _ButtonAutoAmplify = value;
            if (_ButtonAutoAmplify != null)
            {
            }
        }
    }

    private Label _LabelScanPhase;

    internal Label LabelScanPhase
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelScanPhase;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelScanPhase != null)
            {
            }

            _LabelScanPhase = value;
            if (_LabelScanPhase != null)
            {
            }
        }
    }

    private ComboBox _ComboBoxScanPhase;

    internal ComboBox ComboBoxScanPhase
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ComboBoxScanPhase;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ComboBoxScanPhase != null)
            {
            }

            _ComboBoxScanPhase = value;
            if (_ComboBoxScanPhase != null)
            {
            }
        }
    }

    private CheckBox _CheckBoxXYmode;

    internal CheckBox CheckBoxXYmode
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _CheckBoxXYmode;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_CheckBoxXYmode != null)
            {
            }

            _CheckBoxXYmode = value;
            if (_CheckBoxXYmode != null)
            {
            }
        }
    }

    private CheckBox _CheckBoxXYaspectRatio;

    internal CheckBox CheckBoxXYaspectRatio
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _CheckBoxXYaspectRatio;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_CheckBoxXYaspectRatio != null)
            {
            }

            _CheckBoxXYaspectRatio = value;
            if (_CheckBoxXYaspectRatio != null)
            {
            }
        }
    }
}

