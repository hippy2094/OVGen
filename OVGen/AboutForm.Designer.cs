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

[global::Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
partial class AboutForm : System.Windows.Forms.Form
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
        this.Button1 = new System.Windows.Forms.Button();
        this.LabelInformation = new System.Windows.Forms.Label();
        this.LinkLabelWebsite = new System.Windows.Forms.LinkLabel();
        this.LabelVersion = new System.Windows.Forms.Label();
        this.SuspendLayout();
        // 
        // Button1
        // 
        this.Button1.Location = new System.Drawing.Point(118, 131);
        this.Button1.Name = "Button1";
        this.Button1.Size = new System.Drawing.Size(94, 29);
        this.Button1.TabIndex = 0;
        this.Button1.Text = "Close";
        this.Button1.UseVisualStyleBackColor = true;
        // 
        // LabelInformation
        // 
        this.LabelInformation.Location = new System.Drawing.Point(12, 9);
        this.LabelInformation.Name = "LabelInformation";
        this.LabelInformation.Size = new System.Drawing.Size(193, 88);
        this.LabelInformation.TabIndex = 1;
        this.LabelInformation.Text = "A software you can put WAV files and make oscilloscope view thing." + global::Microsoft.VisualBasic.ChrW(13) + global::Microsoft.VisualBasic.ChrW(10) + global::Microsoft.VisualBasic.ChrW(13) + global::Microsoft.VisualBasic.ChrW(10) + "Creator: Ze" + "inok";
        // 
        // LinkLabelWebsite
        // 
        this.LinkLabelWebsite.AutoSize = true;
        this.LinkLabelWebsite.Location = new System.Drawing.Point(12, 97);
        this.LinkLabelWebsite.Name = "LinkLabelWebsite";
        this.LinkLabelWebsite.Size = new System.Drawing.Size(153, 15);
        this.LinkLabelWebsite.TabIndex = 2;
        this.LinkLabelWebsite.TabStop = true;
        this.LinkLabelWebsite.Text = "https://zeinok.blogspot.tw";
        // 
        // LabelVersion
        // 
        this.LabelVersion.AutoSize = true;
        this.LabelVersion.Location = new System.Drawing.Point(12, 138);
        this.LabelVersion.Name = "LabelVersion";
        this.LabelVersion.Size = new System.Drawing.Size(64, 15);
        this.LabelVersion.TabIndex = 3;
        this.LabelVersion.Text = "<version>";
        // 
        // AboutForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(8.0!, 15.0!);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(224, 171);
        this.Controls.Add(this.LabelVersion);
        this.Controls.Add(this.LinkLabelWebsite);
        this.Controls.Add(this.LabelInformation);
        this.Controls.Add(this.Button1);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "AboutForm";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "About";
        this.ResumeLayout(false);
        this.PerformLayout();
    }
    private System.Windows.Forms.Button _Button1;

    internal System.Windows.Forms.Button Button1
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _Button1;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_Button1 != null)
            {
            }

            _Button1 = value;
            if (_Button1 != null)
            {
            }
        }
    }

    private System.Windows.Forms.Label _LabelInformation;

    internal System.Windows.Forms.Label LabelInformation
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelInformation;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelInformation != null)
            {
            }

            _LabelInformation = value;
            if (_LabelInformation != null)
            {
            }
        }
    }

    private System.Windows.Forms.LinkLabel _LinkLabelWebsite;

    internal System.Windows.Forms.LinkLabel LinkLabelWebsite
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LinkLabelWebsite;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LinkLabelWebsite != null)
            {
            }

            _LinkLabelWebsite = value;
            if (_LinkLabelWebsite != null)
            {
            }
        }
    }

    private System.Windows.Forms.Label _LabelVersion;

    internal System.Windows.Forms.Label LabelVersion
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelVersion;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelVersion != null)
            {
            }

            _LabelVersion = value;
            if (_LabelVersion != null)
            {
            }
        }
    }
}

