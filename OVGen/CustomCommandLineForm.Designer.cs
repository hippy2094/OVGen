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


partial class CustomCommandLineForm : System.Windows.Forms.Form
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
        this.LabelWithAudio = new System.Windows.Forms.Label();
        this.TextBoxJoinAudioCommandLine = new System.Windows.Forms.TextBox();
        this.LabelWithoutAudio = new System.Windows.Forms.Label();
        this.TextBoxSilenceCommandLine = new System.Windows.Forms.TextBox();
        this.LabelVariables = new System.Windows.Forms.Label();
        this.ButtonOK = new System.Windows.Forms.Button();
        this.ButtonCancel = new System.Windows.Forms.Button();
        this.LabelFFmpeg1 = new System.Windows.Forms.Label();
        this.LabelFFmpeg2 = new System.Windows.Forms.Label();
        this.ButtonDefault = new System.Windows.Forms.Button();
        this.SuspendLayout();
        // 
        // LabelWithAudio
        // 
        this.LabelWithAudio.AutoSize = true;
        this.LabelWithAudio.Location = new System.Drawing.Point(11, 9);
        this.LabelWithAudio.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.LabelWithAudio.Name = "LabelWithAudio";
        this.LabelWithAudio.Size = new System.Drawing.Size(63, 12);
        this.LabelWithAudio.TabIndex = 0;
        this.LabelWithAudio.Text = "With Audio:";
        // 
        // TextBoxJoinAudioCommandLine
        // 
        this.TextBoxJoinAudioCommandLine.Location = new System.Drawing.Point(86, 22);
        this.TextBoxJoinAudioCommandLine.Margin = new System.Windows.Forms.Padding(2);
        this.TextBoxJoinAudioCommandLine.Name = "TextBoxJoinAudioCommandLine";
        this.TextBoxJoinAudioCommandLine.Size = new System.Drawing.Size(497, 22);
        this.TextBoxJoinAudioCommandLine.TabIndex = 1;
        // 
        // LabelWithoutAudio
        // 
        this.LabelWithoutAudio.AutoSize = true;
        this.LabelWithoutAudio.Location = new System.Drawing.Point(11, 46);
        this.LabelWithoutAudio.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.LabelWithoutAudio.Name = "LabelWithoutAudio";
        this.LabelWithoutAudio.Size = new System.Drawing.Size(78, 12);
        this.LabelWithoutAudio.TabIndex = 2;
        this.LabelWithoutAudio.Text = "Without Audio:";
        // 
        // TextBoxSilenceCommandLine
        // 
        this.TextBoxSilenceCommandLine.Location = new System.Drawing.Point(86, 60);
        this.TextBoxSilenceCommandLine.Margin = new System.Windows.Forms.Padding(2);
        this.TextBoxSilenceCommandLine.Name = "TextBoxSilenceCommandLine";
        this.TextBoxSilenceCommandLine.Size = new System.Drawing.Size(497, 22);
        this.TextBoxSilenceCommandLine.TabIndex = 3;
        // 
        // LabelVariables
        // 
        this.LabelVariables.AutoSize = true;
        this.LabelVariables.Location = new System.Drawing.Point(11, 102);
        this.LabelVariables.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.LabelVariables.Name = "LabelVariables";
        this.LabelVariables.Size = new System.Drawing.Size(96, 60);
        this.LabelVariables.TabIndex = 4;
        this.LabelVariables.Text = "Available variables:" + global::Microsoft.VisualBasic.ChrW(13) + global::Microsoft.VisualBasic.ChrW(10) + "{framerate}" + global::Microsoft.VisualBasic.ChrW(13) + global::Microsoft.VisualBasic.ChrW(10) + "{img}" + global::Microsoft.VisualBasic.ChrW(13) + global::Microsoft.VisualBasic.ChrW(10) + "{audio}" + global::Microsoft.VisualBasic.ChrW(13) + global::Microsoft.VisualBasic.ChrW(10) + "{outfile}";
        // 
        // ButtonOK
        // 
        this.ButtonOK.Location = new System.Drawing.Point(438, 137);
        this.ButtonOK.Margin = new System.Windows.Forms.Padding(2);
        this.ButtonOK.Name = "ButtonOK";
        this.ButtonOK.Size = new System.Drawing.Size(70, 23);
        this.ButtonOK.TabIndex = 5;
        this.ButtonOK.Text = "OK";
        this.ButtonOK.UseVisualStyleBackColor = true;
        // 
        // ButtonCancel
        // 
        this.ButtonCancel.Location = new System.Drawing.Point(513, 137);
        this.ButtonCancel.Margin = new System.Windows.Forms.Padding(2);
        this.ButtonCancel.Name = "ButtonCancel";
        this.ButtonCancel.Size = new System.Drawing.Size(70, 23);
        this.ButtonCancel.TabIndex = 6;
        this.ButtonCancel.Text = "Cancel";
        this.ButtonCancel.UseVisualStyleBackColor = true;
        // 
        // LabelFFmpeg1
        // 
        this.LabelFFmpeg1.AutoSize = true;
        this.LabelFFmpeg1.Location = new System.Drawing.Point(11, 25);
        this.LabelFFmpeg1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.LabelFFmpeg1.Name = "LabelFFmpeg1";
        this.LabelFFmpeg1.Size = new System.Drawing.Size(71, 12);
        this.LabelFFmpeg1.TabIndex = 7;
        this.LabelFFmpeg1.Text = "ffmpeg.exe -y";
        // 
        // LabelFFmpeg2
        // 
        this.LabelFFmpeg2.AutoSize = true;
        this.LabelFFmpeg2.Location = new System.Drawing.Point(11, 63);
        this.LabelFFmpeg2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.LabelFFmpeg2.Name = "LabelFFmpeg2";
        this.LabelFFmpeg2.Size = new System.Drawing.Size(71, 12);
        this.LabelFFmpeg2.TabIndex = 8;
        this.LabelFFmpeg2.Text = "ffmpeg.exe -y";
        // 
        // ButtonDefault
        // 
        this.ButtonDefault.Location = new System.Drawing.Point(363, 137);
        this.ButtonDefault.Margin = new System.Windows.Forms.Padding(2);
        this.ButtonDefault.Name = "ButtonDefault";
        this.ButtonDefault.Size = new System.Drawing.Size(70, 23);
        this.ButtonDefault.TabIndex = 9;
        this.ButtonDefault.Text = "Default";
        this.ButtonDefault.UseVisualStyleBackColor = true;
        // 
        // CustomCommandLineForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6.0f, 12.0f);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(594, 171);
        this.Controls.Add(this.ButtonDefault);
        this.Controls.Add(this.LabelFFmpeg2);
        this.Controls.Add(this.LabelFFmpeg1);
        this.Controls.Add(this.ButtonCancel);
        this.Controls.Add(this.ButtonOK);
        this.Controls.Add(this.LabelVariables);
        this.Controls.Add(this.TextBoxSilenceCommandLine);
        this.Controls.Add(this.LabelWithoutAudio);
        this.Controls.Add(this.TextBoxJoinAudioCommandLine);
        this.Controls.Add(this.LabelWithAudio);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.HelpButton = true;
        this.Margin = new System.Windows.Forms.Padding(2);
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "CustomCommandLineForm";
        this.ShowIcon = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Custom commandline (experts only!)";
        this.ResumeLayout(false);
        this.PerformLayout();
    }
    private System.Windows.Forms.Label _LabelWithAudio;

    internal System.Windows.Forms.Label LabelWithAudio
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelWithAudio;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelWithAudio != null)
            {
            }

            _LabelWithAudio = value;
            if (_LabelWithAudio != null)
            {
            }
        }
    }

    private System.Windows.Forms.TextBox _TextBoxJoinAudioCommandLine;

    internal System.Windows.Forms.TextBox TextBoxJoinAudioCommandLine
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _TextBoxJoinAudioCommandLine;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_TextBoxJoinAudioCommandLine != null)
            {
            }

            _TextBoxJoinAudioCommandLine = value;
            if (_TextBoxJoinAudioCommandLine != null)
            {
            }
        }
    }

    private System.Windows.Forms.Label _LabelWithoutAudio;

    internal System.Windows.Forms.Label LabelWithoutAudio
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelWithoutAudio;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelWithoutAudio != null)
            {
            }

            _LabelWithoutAudio = value;
            if (_LabelWithoutAudio != null)
            {
            }
        }
    }

    private System.Windows.Forms.TextBox _TextBoxSilenceCommandLine;

    internal System.Windows.Forms.TextBox TextBoxSilenceCommandLine
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _TextBoxSilenceCommandLine;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_TextBoxSilenceCommandLine != null)
            {
            }

            _TextBoxSilenceCommandLine = value;
            if (_TextBoxSilenceCommandLine != null)
            {
            }
        }
    }

    private System.Windows.Forms.Label _LabelVariables;

    internal System.Windows.Forms.Label LabelVariables
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelVariables;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelVariables != null)
            {
            }

            _LabelVariables = value;
            if (_LabelVariables != null)
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

    private System.Windows.Forms.Label _LabelFFmpeg1;

    internal System.Windows.Forms.Label LabelFFmpeg1
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelFFmpeg1;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelFFmpeg1 != null)
            {
            }

            _LabelFFmpeg1 = value;
            if (_LabelFFmpeg1 != null)
            {
            }
        }
    }

    private System.Windows.Forms.Label _LabelFFmpeg2;

    internal System.Windows.Forms.Label LabelFFmpeg2
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelFFmpeg2;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelFFmpeg2 != null)
            {
            }

            _LabelFFmpeg2 = value;
            if (_LabelFFmpeg2 != null)
            {
            }
        }
    }

    private System.Windows.Forms.Button _ButtonDefault;

    internal System.Windows.Forms.Button ButtonDefault
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ButtonDefault;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ButtonDefault != null)
            {
            }

            _ButtonDefault = value;
            if (_ButtonDefault != null)
            {
            }
        }
    }
}

