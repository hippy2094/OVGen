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
partial class AutoAmplifyWorkerForm : System.Windows.Forms.Form
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
    // 請勿使用程式碼編輯器進行修改。
    [System.Diagnostics.DebuggerStepThrough()]
    private void InitializeComponent()
    {
        this.ButtonCancel = new System.Windows.Forms.Button();
        this.BackgroundWorkerAutoAmplify = new System.ComponentModel.BackgroundWorker();
        this.ProgressBar1 = new System.Windows.Forms.ProgressBar();
        this.SuspendLayout();
        // 
        // ButtonCancel
        // 
        this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.ButtonCancel.Location = new System.Drawing.Point(84, 43);
        this.ButtonCancel.Name = "ButtonCancel";
        this.ButtonCancel.Size = new System.Drawing.Size(75, 26);
        this.ButtonCancel.TabIndex = 1;
        this.ButtonCancel.Text = "Cancel";
        this.ButtonCancel.UseVisualStyleBackColor = true;
        // 
        // BackgroundWorkerAutoAmplify
        // 
        this.BackgroundWorkerAutoAmplify.WorkerReportsProgress = true;
        this.BackgroundWorkerAutoAmplify.WorkerSupportsCancellation = true;
        // 
        // ProgressBar1
        // 
        this.ProgressBar1.Location = new System.Drawing.Point(12, 12);
        this.ProgressBar1.Name = "ProgressBar1";
        this.ProgressBar1.Size = new System.Drawing.Size(208, 25);
        this.ProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
        this.ProgressBar1.TabIndex = 0;
        // 
        // AutoAmplifyWorkerForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(8.0!, 15.0!);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.ButtonCancel;
        this.ClientSize = new System.Drawing.Size(232, 77);
        this.Controls.Add(this.ProgressBar1);
        this.Controls.Add(this.ButtonCancel);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "AutoAmplifyWorkerForm";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Auto Amplify";
        this.ResumeLayout(false);
    }
    private Button _ButtonCancel;

    internal Button ButtonCancel
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

    private System.ComponentModel.BackgroundWorker _BackgroundWorkerAutoAmplify;

    internal System.ComponentModel.BackgroundWorker BackgroundWorkerAutoAmplify
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _BackgroundWorkerAutoAmplify;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_BackgroundWorkerAutoAmplify != null)
            {
            }

            _BackgroundWorkerAutoAmplify = value;
            if (_BackgroundWorkerAutoAmplify != null)
            {
            }
        }
    }

    private ProgressBar _ProgressBar1;

    internal ProgressBar ProgressBar1
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ProgressBar1;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ProgressBar1 != null)
            {
            }

            _ProgressBar1 = value;
            if (_ProgressBar1 != null)
            {
            }
        }
    }
}

