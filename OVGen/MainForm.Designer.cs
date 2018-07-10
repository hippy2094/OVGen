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


partial class MainForm : System.Windows.Forms.Form
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
        this.components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        this.OscilloscopeBackgroundWorker = new System.ComponentModel.BackgroundWorker();
        this.ButtonSetOutputFolder = new System.Windows.Forms.Button();
        this.LabelOutputLocation = new System.Windows.Forms.Label();
        this.TextBoxOutputLocation = new System.Windows.Forms.TextBox();
        this.ButtonControl = new System.Windows.Forms.Button();
        this.CheckBoxShowOutput = new System.Windows.Forms.CheckBox();
        this.ButtonBackgroundColor = new System.Windows.Forms.Button();
        this.NumericUpDownMiddleLine = new System.Windows.Forms.NumericUpDown();
        this.NumericUpDownGrid = new System.Windows.Forms.NumericUpDown();
        this.ButtonMiddleLineColor = new System.Windows.Forms.Button();
        this.ButtonGridColor = new System.Windows.Forms.Button();
        this.CheckBoxDrawMiddleLine = new System.Windows.Forms.CheckBox();
        this.ButtonFlowDirection = new System.Windows.Forms.Button();
        this.LabelFlowDirecton = new System.Windows.Forms.Label();
        this.CheckBoxGrid = new System.Windows.Forms.CheckBox();
        this.LabelCanvasSize = new System.Windows.Forms.Label();
        this.ComboBoxCanvasSize = new System.Windows.Forms.ComboBox();
        this.CheckBoxCRT = new System.Windows.Forms.CheckBox();
        this.NumericUpDownLineWidth = new System.Windows.Forms.NumericUpDown();
        this.CheckBoxNoFileWriting = new System.Windows.Forms.CheckBox();
        this.LabelLineWidth = new System.Windows.Forms.Label();
        this.NumericUpDownColumn = new System.Windows.Forms.NumericUpDown();
        this.LabelColumn = new System.Windows.Forms.Label();
        this.CheckBoxSmooth = new System.Windows.Forms.CheckBox();
        this.NumericUpDownFrameRate = new System.Windows.Forms.NumericUpDown();
        this.LabelFrameRate = new System.Windows.Forms.Label();
        this.LinkLabelCustomCommandLine = new System.Windows.Forms.LinkLabel();
        this.ButtonSelectAll = new System.Windows.Forms.Button();
        this.ButtonOptions = new System.Windows.Forms.Button();
        this.ButtonMoveDown = new System.Windows.Forms.Button();
        this.ButtonMoveUp = new System.Windows.Forms.Button();
        this.ButtonRemove = new System.Windows.Forms.Button();
        this.ButtonAdd = new System.Windows.Forms.Button();
        this.ButtonAudio = new System.Windows.Forms.Button();
        this.CheckBoxVideo = new System.Windows.Forms.CheckBox();
        this.ToolTips = new System.Windows.Forms.ToolTip(this.components);
        this.PictureBoxOutput = new System.Windows.Forms.PictureBox();
        this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
        this.LabelStatus = new System.Windows.Forms.ToolStripStatusLabel();
        this.ToolStripStatusLabelPadding = new System.Windows.Forms.ToolStripStatusLabel();
        this.ToolStripStatusLabelAbout = new System.Windows.Forms.ToolStripStatusLabel();
        this.LabelPreviewMode = new System.Windows.Forms.Label();
        this.TimerLabelFlashing = new System.Windows.Forms.Timer(this.components);
        this.BackgroundWorkerStdErrReader = new System.ComponentModel.BackgroundWorker();
        this.LogBox = new System.Windows.Forms.RichTextBox();
        this.TabControlRenderingFiles = new System.Windows.Forms.TabControl();
        this.TabPageRendering = new System.Windows.Forms.TabPage();
        this.CheckBoxDottedXYmode = new System.Windows.Forms.CheckBox();
        this.ComboBoxLabelPos = new System.Windows.Forms.ComboBox();
        this.LabelChannelLabelPos = new System.Windows.Forms.Label();
        this.NumericUpDownBorder = new System.Windows.Forms.NumericUpDown();
        this.ButtonBorderColor = new System.Windows.Forms.Button();
        this.CheckBoxBorder = new System.Windows.Forms.CheckBox();
        this.TabPageFiles = new System.Windows.Forms.TabPage();
        this.ListBoxFiles = new System.Windows.Forms.ListBox();
        this.TimerStatusUpdater = new System.Windows.Forms.Timer(this.components);
        (System.ComponentModel.ISupportInitialize)this.NumericUpDownMiddleLine.BeginInit();
        (System.ComponentModel.ISupportInitialize)this.NumericUpDownGrid.BeginInit();
        (System.ComponentModel.ISupportInitialize)this.NumericUpDownLineWidth.BeginInit();
        (System.ComponentModel.ISupportInitialize)this.NumericUpDownColumn.BeginInit();
        (System.ComponentModel.ISupportInitialize)this.NumericUpDownFrameRate.BeginInit();
        (System.ComponentModel.ISupportInitialize)this.PictureBoxOutput.BeginInit();
        this.StatusStrip1.SuspendLayout();
        this.TabControlRenderingFiles.SuspendLayout();
        this.TabPageRendering.SuspendLayout();
        (System.ComponentModel.ISupportInitialize)this.NumericUpDownBorder.BeginInit();
        this.TabPageFiles.SuspendLayout();
        this.SuspendLayout();
        // 
        // OscilloscopeBackgroundWorker
        // 
        this.OscilloscopeBackgroundWorker.WorkerReportsProgress = true;
        this.OscilloscopeBackgroundWorker.WorkerSupportsCancellation = true;
        // 
        // ButtonSetOutputFolder
        // 
        this.ButtonSetOutputFolder.Location = new System.Drawing.Point(786, 18);
        this.ButtonSetOutputFolder.Margin = new System.Windows.Forms.Padding(4);
        this.ButtonSetOutputFolder.Name = "ButtonSetOutputFolder";
        this.ButtonSetOutputFolder.Size = new System.Drawing.Size(31, 29);
        this.ButtonSetOutputFolder.TabIndex = 2;
        this.ButtonSetOutputFolder.Text = "...";
        this.ToolTips.SetToolTip(this.ButtonSetOutputFolder, "Browse...");
        this.ButtonSetOutputFolder.UseVisualStyleBackColor = true;
        // 
        // LabelOutputLocation
        // 
        this.LabelOutputLocation.AutoSize = true;
        this.LabelOutputLocation.Location = new System.Drawing.Point(15, 22);
        this.LabelOutputLocation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        this.LabelOutputLocation.Name = "LabelOutputLocation";
        this.LabelOutputLocation.Size = new System.Drawing.Size(88, 15);
        this.LabelOutputLocation.TabIndex = 0;
        this.LabelOutputLocation.Text = "Output folder:";
        // 
        // TextBoxOutputLocation
        // 
        this.TextBoxOutputLocation.Location = new System.Drawing.Point(105, 18);
        this.TextBoxOutputLocation.Margin = new System.Windows.Forms.Padding(4);
        this.TextBoxOutputLocation.Name = "TextBoxOutputLocation";
        this.TextBoxOutputLocation.Size = new System.Drawing.Size(673, 25);
        this.TextBoxOutputLocation.TabIndex = 1;
        // 
        // ButtonControl
        // 
        this.ButtonControl.Location = new System.Drawing.Point(724, 52);
        this.ButtonControl.Margin = new System.Windows.Forms.Padding(4);
        this.ButtonControl.Name = "ButtonControl";
        this.ButtonControl.Size = new System.Drawing.Size(94, 29);
        this.ButtonControl.TabIndex = 4;
        this.ButtonControl.Text = "Start";
        this.ToolTips.SetToolTip(this.ButtonControl, "Start drawing.");
        this.ButtonControl.UseVisualStyleBackColor = true;
        // 
        // CheckBoxShowOutput
        // 
        this.CheckBoxShowOutput.AutoSize = true;
        this.CheckBoxShowOutput.BackColor = System.Drawing.SystemColors.Control;
        this.CheckBoxShowOutput.Checked = true;
        this.CheckBoxShowOutput.CheckState = System.Windows.Forms.CheckState.Checked;
        this.CheckBoxShowOutput.Location = new System.Drawing.Point(718, 89);
        this.CheckBoxShowOutput.Margin = new System.Windows.Forms.Padding(4);
        this.CheckBoxShowOutput.Name = "CheckBoxShowOutput";
        this.CheckBoxShowOutput.Size = new System.Drawing.Size(101, 19);
        this.CheckBoxShowOutput.TabIndex = 5;
        this.CheckBoxShowOutput.Text = "Show output";
        this.ToolTips.SetToolTip(this.CheckBoxShowOutput, "Uncheck this if you are worried about speed dropping.");
        this.CheckBoxShowOutput.UseVisualStyleBackColor = false;
        // 
        // ButtonBackgroundColor
        // 
        this.ButtonBackgroundColor.BackColor = System.Drawing.Color.Black;
        this.ButtonBackgroundColor.ForeColor = System.Drawing.Color.White;
        this.ButtonBackgroundColor.Location = new System.Drawing.Point(6, 135);
        this.ButtonBackgroundColor.Name = "ButtonBackgroundColor";
        this.ButtonBackgroundColor.Size = new System.Drawing.Size(234, 25);
        this.ButtonBackgroundColor.TabIndex = 8;
        this.ButtonBackgroundColor.Text = "Background Color";
        this.ButtonBackgroundColor.UseVisualStyleBackColor = false;
        // 
        // NumericUpDownMiddleLine
        // 
        this.NumericUpDownMiddleLine.Location = new System.Drawing.Point(185, 254);
        this.NumericUpDownMiddleLine.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
        this.NumericUpDownMiddleLine.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.NumericUpDownMiddleLine.Name = "NumericUpDownMiddleLine";
        this.NumericUpDownMiddleLine.Size = new System.Drawing.Size(48, 25);
        this.NumericUpDownMiddleLine.TabIndex = 19;
        this.NumericUpDownMiddleLine.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // NumericUpDownGrid
        // 
        this.NumericUpDownGrid.Location = new System.Drawing.Point(148, 225);
        this.NumericUpDownGrid.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
        this.NumericUpDownGrid.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.NumericUpDownGrid.Name = "NumericUpDownGrid";
        this.NumericUpDownGrid.Size = new System.Drawing.Size(48, 25);
        this.NumericUpDownGrid.TabIndex = 16;
        this.NumericUpDownGrid.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // ButtonMiddleLineColor
        // 
        this.ButtonMiddleLineColor.BackColor = System.Drawing.Color.Gray;
        this.ButtonMiddleLineColor.Location = new System.Drawing.Point(104, 254);
        this.ButtonMiddleLineColor.Name = "ButtonMiddleLineColor";
        this.ButtonMiddleLineColor.Size = new System.Drawing.Size(75, 25);
        this.ButtonMiddleLineColor.TabIndex = 18;
        this.ButtonMiddleLineColor.Text = "Color";
        this.ButtonMiddleLineColor.UseVisualStyleBackColor = false;
        // 
        // ButtonGridColor
        // 
        this.ButtonGridColor.BackColor = System.Drawing.Color.LightGray;
        this.ButtonGridColor.Location = new System.Drawing.Point(67, 225);
        this.ButtonGridColor.Name = "ButtonGridColor";
        this.ButtonGridColor.Size = new System.Drawing.Size(75, 25);
        this.ButtonGridColor.TabIndex = 15;
        this.ButtonGridColor.Text = "Color";
        this.ButtonGridColor.UseVisualStyleBackColor = false;
        // 
        // CheckBoxDrawMiddleLine
        // 
        this.CheckBoxDrawMiddleLine.AutoSize = true;
        this.CheckBoxDrawMiddleLine.Location = new System.Drawing.Point(5, 258);
        this.CheckBoxDrawMiddleLine.Name = "CheckBoxDrawMiddleLine";
        this.CheckBoxDrawMiddleLine.Size = new System.Drawing.Size(100, 19);
        this.CheckBoxDrawMiddleLine.TabIndex = 17;
        this.CheckBoxDrawMiddleLine.Text = "Middle Line";
        this.ToolTips.SetToolTip(this.CheckBoxDrawMiddleLine, "Draws a horizontal line at 0V");
        this.CheckBoxDrawMiddleLine.UseVisualStyleBackColor = true;
        // 
        // ButtonFlowDirection
        // 
        this.ButtonFlowDirection.Location = new System.Drawing.Point(110, 315);
        this.ButtonFlowDirection.Name = "ButtonFlowDirection";
        this.ButtonFlowDirection.Size = new System.Drawing.Size(129, 26);
        this.ButtonFlowDirection.TabIndex = 23;
        this.ButtonFlowDirection.Text = "FlowDirection";
        this.ButtonFlowDirection.UseVisualStyleBackColor = true;
        // 
        // LabelFlowDirecton
        // 
        this.LabelFlowDirecton.AutoSize = true;
        this.LabelFlowDirecton.Location = new System.Drawing.Point(6, 321);
        this.LabelFlowDirecton.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        this.LabelFlowDirecton.Name = "LabelFlowDirecton";
        this.LabelFlowDirecton.Size = new System.Drawing.Size(97, 15);
        this.LabelFlowDirecton.TabIndex = 22;
        this.LabelFlowDirecton.Text = "Flow Direction:";
        // 
        // CheckBoxGrid
        // 
        this.CheckBoxGrid.AutoSize = true;
        this.CheckBoxGrid.Location = new System.Drawing.Point(5, 229);
        this.CheckBoxGrid.Margin = new System.Windows.Forms.Padding(4);
        this.CheckBoxGrid.Name = "CheckBoxGrid";
        this.CheckBoxGrid.Size = new System.Drawing.Size(55, 19);
        this.CheckBoxGrid.TabIndex = 14;
        this.CheckBoxGrid.Text = "Grid";
        this.CheckBoxGrid.UseVisualStyleBackColor = true;
        // 
        // LabelCanvasSize
        // 
        this.LabelCanvasSize.AutoSize = true;
        this.LabelCanvasSize.Location = new System.Drawing.Point(6, 289);
        this.LabelCanvasSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        this.LabelCanvasSize.Name = "LabelCanvasSize";
        this.LabelCanvasSize.Size = new System.Drawing.Size(79, 15);
        this.LabelCanvasSize.TabIndex = 20;
        this.LabelCanvasSize.Text = "Canvas Size:";
        // 
        // ComboBoxCanvasSize
        // 
        this.ComboBoxCanvasSize.FormattingEnabled = true;
        this.ComboBoxCanvasSize.Items.AddRange(new object[] { "1280x720", "1920x1080" });
        this.ComboBoxCanvasSize.Location = new System.Drawing.Point(93, 285);
        this.ComboBoxCanvasSize.Margin = new System.Windows.Forms.Padding(4);
        this.ComboBoxCanvasSize.Name = "ComboBoxCanvasSize";
        this.ComboBoxCanvasSize.Size = new System.Drawing.Size(145, 23);
        this.ComboBoxCanvasSize.TabIndex = 21;
        this.ComboBoxCanvasSize.Text = "1280x720";
        // 
        // CheckBoxCRT
        // 
        this.CheckBoxCRT.AutoSize = true;
        this.CheckBoxCRT.Location = new System.Drawing.Point(120, 31);
        this.CheckBoxCRT.Margin = new System.Windows.Forms.Padding(2);
        this.CheckBoxCRT.Name = "CheckBoxCRT";
        this.CheckBoxCRT.Size = new System.Drawing.Size(93, 19);
        this.CheckBoxCRT.TabIndex = 2;
        this.CheckBoxCRT.Text = "CRT  Style";
        this.CheckBoxCRT.UseVisualStyleBackColor = true;
        // 
        // NumericUpDownLineWidth
        // 
        this.NumericUpDownLineWidth.Location = new System.Drawing.Point(85, 165);
        this.NumericUpDownLineWidth.Margin = new System.Windows.Forms.Padding(2);
        this.NumericUpDownLineWidth.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
        this.NumericUpDownLineWidth.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.NumericUpDownLineWidth.Name = "NumericUpDownLineWidth";
        this.NumericUpDownLineWidth.Size = new System.Drawing.Size(49, 25);
        this.NumericUpDownLineWidth.TabIndex = 10;
        this.NumericUpDownLineWidth.Value = new decimal(new int[] { 2, 0, 0, 0 });
        // 
        // CheckBoxNoFileWriting
        // 
        this.CheckBoxNoFileWriting.AutoSize = true;
        this.CheckBoxNoFileWriting.BackColor = System.Drawing.SystemColors.ControlLightLight;
        this.CheckBoxNoFileWriting.Checked = true;
        this.CheckBoxNoFileWriting.CheckState = System.Windows.Forms.CheckState.Checked;
        this.CheckBoxNoFileWriting.Location = new System.Drawing.Point(6, 5);
        this.CheckBoxNoFileWriting.Margin = new System.Windows.Forms.Padding(2);
        this.CheckBoxNoFileWriting.Name = "CheckBoxNoFileWriting";
        this.CheckBoxNoFileWriting.Size = new System.Drawing.Size(202, 19);
        this.CheckBoxNoFileWriting.TabIndex = 0;
        this.CheckBoxNoFileWriting.Text = "No file writing (preview only)";
        this.ToolTips.SetToolTip(this.CheckBoxNoFileWriting, "Good for previewing your settings.");
        this.CheckBoxNoFileWriting.UseVisualStyleBackColor = false;
        // 
        // LabelLineWidth
        // 
        this.LabelLineWidth.AutoSize = true;
        this.LabelLineWidth.Location = new System.Drawing.Point(5, 169);
        this.LabelLineWidth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.LabelLineWidth.Name = "LabelLineWidth";
        this.LabelLineWidth.Size = new System.Drawing.Size(76, 15);
        this.LabelLineWidth.TabIndex = 9;
        this.LabelLineWidth.Text = "Line Width:";
        // 
        // NumericUpDownColumn
        // 
        this.NumericUpDownColumn.Location = new System.Drawing.Point(77, 104);
        this.NumericUpDownColumn.Margin = new System.Windows.Forms.Padding(2);
        this.NumericUpDownColumn.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.NumericUpDownColumn.Name = "NumericUpDownColumn";
        this.NumericUpDownColumn.Size = new System.Drawing.Size(48, 25);
        this.NumericUpDownColumn.TabIndex = 7;
        this.ToolTips.SetToolTip(this.NumericUpDownColumn, "Change column count");
        this.NumericUpDownColumn.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // LabelColumn
        // 
        this.LabelColumn.AutoSize = true;
        this.LabelColumn.Location = new System.Drawing.Point(5, 106);
        this.LabelColumn.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.LabelColumn.Name = "LabelColumn";
        this.LabelColumn.Size = new System.Drawing.Size(61, 15);
        this.LabelColumn.TabIndex = 6;
        this.LabelColumn.Text = "Columns:";
        // 
        // CheckBoxSmooth
        // 
        this.CheckBoxSmooth.AutoSize = true;
        this.CheckBoxSmooth.Location = new System.Drawing.Point(6, 31);
        this.CheckBoxSmooth.Margin = new System.Windows.Forms.Padding(2);
        this.CheckBoxSmooth.Name = "CheckBoxSmooth";
        this.CheckBoxSmooth.Size = new System.Drawing.Size(103, 19);
        this.CheckBoxSmooth.TabIndex = 1;
        this.CheckBoxSmooth.Text = "Smooth Line";
        this.ToolTips.SetToolTip(this.CheckBoxSmooth, "Anti-alias drawing, slower.");
        this.CheckBoxSmooth.UseVisualStyleBackColor = true;
        // 
        // NumericUpDownFrameRate
        // 
        this.NumericUpDownFrameRate.Location = new System.Drawing.Point(80, 75);
        this.NumericUpDownFrameRate.Margin = new System.Windows.Forms.Padding(2);
        this.NumericUpDownFrameRate.Maximum = new decimal(new int[] { 600, 0, 0, 0 });
        this.NumericUpDownFrameRate.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.NumericUpDownFrameRate.Name = "NumericUpDownFrameRate";
        this.NumericUpDownFrameRate.Size = new System.Drawing.Size(48, 25);
        this.NumericUpDownFrameRate.TabIndex = 5;
        this.NumericUpDownFrameRate.Value = new decimal(new int[] { 60, 0, 0, 0 });
        // 
        // LabelFrameRate
        // 
        this.LabelFrameRate.AutoSize = true;
        this.LabelFrameRate.Location = new System.Drawing.Point(5, 77);
        this.LabelFrameRate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.LabelFrameRate.Name = "LabelFrameRate";
        this.LabelFrameRate.Size = new System.Drawing.Size(68, 15);
        this.LabelFrameRate.TabIndex = 4;
        this.LabelFrameRate.Text = "Framerate:";
        // 
        // LinkLabelCustomCommandLine
        // 
        this.LinkLabelCustomCommandLine.AutoSize = true;
        this.LinkLabelCustomCommandLine.Location = new System.Drawing.Point(112, 6);
        this.LinkLabelCustomCommandLine.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.LinkLabelCustomCommandLine.Name = "LinkLabelCustomCommandLine";
        this.LinkLabelCustomCommandLine.Size = new System.Drawing.Size(111, 15);
        this.LinkLabelCustomCommandLine.TabIndex = 1;
        this.LinkLabelCustomCommandLine.TabStop = true;
        this.LinkLabelCustomCommandLine.Text = "Edit commandline";
        // 
        // ButtonSelectAll
        // 
        this.ButtonSelectAll.Location = new System.Drawing.Point(173, 463);
        this.ButtonSelectAll.Margin = new System.Windows.Forms.Padding(2);
        this.ButtonSelectAll.Name = "ButtonSelectAll";
        this.ButtonSelectAll.Size = new System.Drawing.Size(68, 30);
        this.ButtonSelectAll.TabIndex = 9;
        this.ButtonSelectAll.Text = "Sel. All";
        this.ButtonSelectAll.UseVisualStyleBackColor = true;
        // 
        // ButtonOptions
        // 
        this.ButtonOptions.Image = global::OVG.My.Resources.Resources.gear_16xLG;
        this.ButtonOptions.Location = new System.Drawing.Point(140, 463);
        this.ButtonOptions.Margin = new System.Windows.Forms.Padding(2);
        this.ButtonOptions.Name = "ButtonOptions";
        this.ButtonOptions.Size = new System.Drawing.Size(28, 30);
        this.ButtonOptions.TabIndex = 8;
        this.ToolTips.SetToolTip(this.ButtonOptions, "Channel config");
        this.ButtonOptions.UseVisualStyleBackColor = true;
        // 
        // ButtonMoveDown
        // 
        this.ButtonMoveDown.Image = global::OVG.My.Resources.Resources.arrow_Down_16xLG;
        this.ButtonMoveDown.Location = new System.Drawing.Point(106, 463);
        this.ButtonMoveDown.Margin = new System.Windows.Forms.Padding(2);
        this.ButtonMoveDown.Name = "ButtonMoveDown";
        this.ButtonMoveDown.Size = new System.Drawing.Size(28, 30);
        this.ButtonMoveDown.TabIndex = 7;
        this.ToolTips.SetToolTip(this.ButtonMoveDown, "Move down");
        this.ButtonMoveDown.UseVisualStyleBackColor = true;
        // 
        // ButtonMoveUp
        // 
        this.ButtonMoveUp.Image = global::OVG.My.Resources.Resources.arrow_Up_16xLG;
        this.ButtonMoveUp.Location = new System.Drawing.Point(72, 463);
        this.ButtonMoveUp.Margin = new System.Windows.Forms.Padding(2);
        this.ButtonMoveUp.Name = "ButtonMoveUp";
        this.ButtonMoveUp.Size = new System.Drawing.Size(28, 30);
        this.ButtonMoveUp.TabIndex = 6;
        this.ToolTips.SetToolTip(this.ButtonMoveUp, "Move up");
        this.ButtonMoveUp.UseVisualStyleBackColor = true;
        // 
        // ButtonRemove
        // 
        this.ButtonRemove.Image = global::OVG.My.Resources.Resources.action_Cancel_16xLG;
        this.ButtonRemove.Location = new System.Drawing.Point(39, 463);
        this.ButtonRemove.Margin = new System.Windows.Forms.Padding(2);
        this.ButtonRemove.Name = "ButtonRemove";
        this.ButtonRemove.Size = new System.Drawing.Size(28, 30);
        this.ButtonRemove.TabIndex = 5;
        this.ToolTips.SetToolTip(this.ButtonRemove, "Remove file");
        this.ButtonRemove.UseVisualStyleBackColor = true;
        // 
        // ButtonAdd
        // 
        this.ButtonAdd.Image = global::OVG.My.Resources.Resources.action_add_16xLG;
        this.ButtonAdd.Location = new System.Drawing.Point(5, 463);
        this.ButtonAdd.Margin = new System.Windows.Forms.Padding(2);
        this.ButtonAdd.Name = "ButtonAdd";
        this.ButtonAdd.Size = new System.Drawing.Size(28, 30);
        this.ButtonAdd.TabIndex = 4;
        this.ToolTips.SetToolTip(this.ButtonAdd, "Add file");
        this.ButtonAdd.UseVisualStyleBackColor = true;
        // 
        // ButtonAudio
        // 
        this.ButtonAudio.Enabled = false;
        this.ButtonAudio.Location = new System.Drawing.Point(5, 28);
        this.ButtonAudio.Margin = new System.Windows.Forms.Padding(2);
        this.ButtonAudio.Name = "ButtonAudio";
        this.ButtonAudio.Size = new System.Drawing.Size(236, 26);
        this.ButtonAudio.TabIndex = 2;
        this.ButtonAudio.Text = "Master Audio";
        this.ButtonAudio.UseVisualStyleBackColor = true;
        // 
        // CheckBoxVideo
        // 
        this.CheckBoxVideo.AutoSize = true;
        this.CheckBoxVideo.Location = new System.Drawing.Point(5, 5);
        this.CheckBoxVideo.Margin = new System.Windows.Forms.Padding(2);
        this.CheckBoxVideo.Name = "CheckBoxVideo";
        this.CheckBoxVideo.Size = new System.Drawing.Size(103, 19);
        this.CheckBoxVideo.TabIndex = 0;
        this.CheckBoxVideo.Text = "Output video";
        this.ToolTips.SetToolTip(this.CheckBoxVideo, "Output as video instead of png frame sequences." + global::Microsoft.VisualBasic.ChrW(13) + global::Microsoft.VisualBasic.ChrW(10) + "Requires FFmpeg.");
        this.CheckBoxVideo.UseVisualStyleBackColor = true;
        // 
        // PictureBoxOutput
        // 
        this.PictureBoxOutput.Location = new System.Drawing.Point(18, 89);
        this.PictureBoxOutput.Margin = new System.Windows.Forms.Padding(4);
        this.PictureBoxOutput.Name = "PictureBoxOutput";
        this.PictureBoxOutput.Size = new System.Drawing.Size(800, 450);
        this.PictureBoxOutput.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        this.PictureBoxOutput.TabIndex = 4;
        this.PictureBoxOutput.TabStop = false;
        // 
        // StatusStrip1
        // 
        this.StatusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
        this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.LabelStatus, this.ToolStripStatusLabelPadding, this.ToolStripStatusLabelAbout });
        this.StatusStrip1.Location = new System.Drawing.Point(0, 637);
        this.StatusStrip1.Name = "StatusStrip1";
        this.StatusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 18, 0);
        this.StatusStrip1.Size = new System.Drawing.Size(1098, 28);
        this.StatusStrip1.SizingGrip = false;
        this.StatusStrip1.TabIndex = 8;
        this.StatusStrip1.Text = "StatusStrip1";
        // 
        // LabelStatus
        // 
        this.LabelStatus.Name = "LabelStatus";
        this.LabelStatus.Size = new System.Drawing.Size(89, 23);
        this.LabelStatus.Text = "LabelStatus";
        // 
        // ToolStripStatusLabelPadding
        // 
        this.ToolStripStatusLabelPadding.Name = "ToolStripStatusLabelPadding";
        this.ToolStripStatusLabelPadding.Size = new System.Drawing.Size(935, 23);
        this.ToolStripStatusLabelPadding.Spring = true;
        // 
        // ToolStripStatusLabelAbout
        // 
        this.ToolStripStatusLabelAbout.BorderSides = (System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom);
        this.ToolStripStatusLabelAbout.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
        this.ToolStripStatusLabelAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.ToolStripStatusLabelAbout.Name = "ToolStripStatusLabelAbout";
        this.ToolStripStatusLabelAbout.Size = new System.Drawing.Size(55, 23);
        this.ToolStripStatusLabelAbout.Text = "About";
        this.ToolStripStatusLabelAbout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // LabelPreviewMode
        // 
        this.LabelPreviewMode.AutoSize = true;
        this.LabelPreviewMode.BackColor = System.Drawing.SystemColors.Control;
        this.LabelPreviewMode.ForeColor = System.Drawing.Color.Red;
        this.LabelPreviewMode.Location = new System.Drawing.Point(620, 59);
        this.LabelPreviewMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        this.LabelPreviewMode.Name = "LabelPreviewMode";
        this.LabelPreviewMode.Size = new System.Drawing.Size(90, 15);
        this.LabelPreviewMode.TabIndex = 3;
        this.LabelPreviewMode.Text = "Preview Mode";
        this.LabelPreviewMode.Visible = false;
        // 
        // TimerLabelFlashing
        // 
        this.TimerLabelFlashing.Enabled = true;
        this.TimerLabelFlashing.Interval = 1000;
        // 
        // BackgroundWorkerStdErrReader
        // 
        this.BackgroundWorkerStdErrReader.WorkerReportsProgress = true;
        this.BackgroundWorkerStdErrReader.WorkerSupportsCancellation = true;
        // 
        // LogBox
        // 
        this.LogBox.Location = new System.Drawing.Point(18, 545);
        this.LogBox.Name = "LogBox";
        this.LogBox.ReadOnly = true;
        this.LogBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
        this.LogBox.Size = new System.Drawing.Size(1060, 92);
        this.LogBox.TabIndex = 7;
        this.LogBox.Text = "";
        // 
        // TabControlRenderingFiles
        // 
        this.TabControlRenderingFiles.Controls.Add(this.TabPageRendering);
        this.TabControlRenderingFiles.Controls.Add(this.TabPageFiles);
        this.TabControlRenderingFiles.Location = new System.Drawing.Point(826, 12);
        this.TabControlRenderingFiles.Name = "TabControlRenderingFiles";
        this.TabControlRenderingFiles.SelectedIndex = 0;
        this.TabControlRenderingFiles.Size = new System.Drawing.Size(254, 527);
        this.TabControlRenderingFiles.TabIndex = 6;
        // 
        // TabPageRendering
        // 
        this.TabPageRendering.BackColor = System.Drawing.SystemColors.Control;
        this.TabPageRendering.Controls.Add(this.CheckBoxDottedXYmode);
        this.TabPageRendering.Controls.Add(this.ComboBoxLabelPos);
        this.TabPageRendering.Controls.Add(this.LabelChannelLabelPos);
        this.TabPageRendering.Controls.Add(this.NumericUpDownBorder);
        this.TabPageRendering.Controls.Add(this.ButtonBorderColor);
        this.TabPageRendering.Controls.Add(this.CheckBoxBorder);
        this.TabPageRendering.Controls.Add(this.ButtonBackgroundColor);
        this.TabPageRendering.Controls.Add(this.CheckBoxNoFileWriting);
        this.TabPageRendering.Controls.Add(this.NumericUpDownMiddleLine);
        this.TabPageRendering.Controls.Add(this.LabelFrameRate);
        this.TabPageRendering.Controls.Add(this.NumericUpDownGrid);
        this.TabPageRendering.Controls.Add(this.NumericUpDownFrameRate);
        this.TabPageRendering.Controls.Add(this.ButtonMiddleLineColor);
        this.TabPageRendering.Controls.Add(this.CheckBoxSmooth);
        this.TabPageRendering.Controls.Add(this.ButtonGridColor);
        this.TabPageRendering.Controls.Add(this.LabelColumn);
        this.TabPageRendering.Controls.Add(this.CheckBoxDrawMiddleLine);
        this.TabPageRendering.Controls.Add(this.NumericUpDownColumn);
        this.TabPageRendering.Controls.Add(this.ButtonFlowDirection);
        this.TabPageRendering.Controls.Add(this.LabelLineWidth);
        this.TabPageRendering.Controls.Add(this.LabelFlowDirecton);
        this.TabPageRendering.Controls.Add(this.NumericUpDownLineWidth);
        this.TabPageRendering.Controls.Add(this.CheckBoxGrid);
        this.TabPageRendering.Controls.Add(this.CheckBoxCRT);
        this.TabPageRendering.Controls.Add(this.LabelCanvasSize);
        this.TabPageRendering.Controls.Add(this.ComboBoxCanvasSize);
        this.TabPageRendering.Location = new System.Drawing.Point(4, 25);
        this.TabPageRendering.Name = "TabPageRendering";
        this.TabPageRendering.Padding = new System.Windows.Forms.Padding(3);
        this.TabPageRendering.Size = new System.Drawing.Size(246, 498);
        this.TabPageRendering.TabIndex = 0;
        this.TabPageRendering.Text = "Rendering";
        // 
        // CheckBoxDottedXYmode
        // 
        this.CheckBoxDottedXYmode.AutoSize = true;
        this.CheckBoxDottedXYmode.Location = new System.Drawing.Point(6, 55);
        this.CheckBoxDottedXYmode.Name = "CheckBoxDottedXYmode";
        this.CheckBoxDottedXYmode.Size = new System.Drawing.Size(126, 19);
        this.CheckBoxDottedXYmode.TabIndex = 3;
        this.CheckBoxDottedXYmode.Text = "Dotted XY mode";
        this.CheckBoxDottedXYmode.UseVisualStyleBackColor = true;
        // 
        // ComboBoxLabelPos
        // 
        this.ComboBoxLabelPos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.ComboBoxLabelPos.FormattingEnabled = true;
        this.ComboBoxLabelPos.Items.AddRange(new object[] { "Top Left", "Top Right", "Bottom Left", "Bottom Right" });
        this.ComboBoxLabelPos.Location = new System.Drawing.Point(105, 347);
        this.ComboBoxLabelPos.Name = "ComboBoxLabelPos";
        this.ComboBoxLabelPos.Size = new System.Drawing.Size(133, 23);
        this.ComboBoxLabelPos.TabIndex = 25;
        // 
        // LabelChannelLabelPos
        // 
        this.LabelChannelLabelPos.AutoSize = true;
        this.LabelChannelLabelPos.Location = new System.Drawing.Point(6, 350);
        this.LabelChannelLabelPos.Name = "LabelChannelLabelPos";
        this.LabelChannelLabelPos.Size = new System.Drawing.Size(93, 15);
        this.LabelChannelLabelPos.TabIndex = 24;
        this.LabelChannelLabelPos.Text = "Label Position:";
        // 
        // NumericUpDownBorder
        // 
        this.NumericUpDownBorder.Location = new System.Drawing.Point(160, 196);
        this.NumericUpDownBorder.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
        this.NumericUpDownBorder.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.NumericUpDownBorder.Name = "NumericUpDownBorder";
        this.NumericUpDownBorder.Size = new System.Drawing.Size(48, 25);
        this.NumericUpDownBorder.TabIndex = 13;
        this.NumericUpDownBorder.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // ButtonBorderColor
        // 
        this.ButtonBorderColor.BackColor = System.Drawing.Color.LightGray;
        this.ButtonBorderColor.Location = new System.Drawing.Point(79, 196);
        this.ButtonBorderColor.Name = "ButtonBorderColor";
        this.ButtonBorderColor.Size = new System.Drawing.Size(75, 25);
        this.ButtonBorderColor.TabIndex = 12;
        this.ButtonBorderColor.Text = "Color";
        this.ButtonBorderColor.UseVisualStyleBackColor = false;
        // 
        // CheckBoxBorder
        // 
        this.CheckBoxBorder.AutoSize = true;
        this.CheckBoxBorder.Location = new System.Drawing.Point(5, 200);
        this.CheckBoxBorder.Name = "CheckBoxBorder";
        this.CheckBoxBorder.Size = new System.Drawing.Size(68, 19);
        this.CheckBoxBorder.TabIndex = 11;
        this.CheckBoxBorder.Text = "Border";
        this.CheckBoxBorder.UseVisualStyleBackColor = true;
        // 
        // TabPageFiles
        // 
        this.TabPageFiles.BackColor = System.Drawing.SystemColors.Control;
        this.TabPageFiles.Controls.Add(this.ListBoxFiles);
        this.TabPageFiles.Controls.Add(this.ButtonAudio);
        this.TabPageFiles.Controls.Add(this.CheckBoxVideo);
        this.TabPageFiles.Controls.Add(this.ButtonOptions);
        this.TabPageFiles.Controls.Add(this.LinkLabelCustomCommandLine);
        this.TabPageFiles.Controls.Add(this.ButtonMoveDown);
        this.TabPageFiles.Controls.Add(this.ButtonMoveUp);
        this.TabPageFiles.Controls.Add(this.ButtonAdd);
        this.TabPageFiles.Controls.Add(this.ButtonSelectAll);
        this.TabPageFiles.Controls.Add(this.ButtonRemove);
        this.TabPageFiles.Location = new System.Drawing.Point(4, 25);
        this.TabPageFiles.Name = "TabPageFiles";
        this.TabPageFiles.Padding = new System.Windows.Forms.Padding(3);
        this.TabPageFiles.Size = new System.Drawing.Size(246, 498);
        this.TabPageFiles.TabIndex = 1;
        this.TabPageFiles.Text = "Files";
        // 
        // ListBoxFiles
        // 
        this.ListBoxFiles.FormattingEnabled = true;
        this.ListBoxFiles.HorizontalScrollbar = true;
        this.ListBoxFiles.ItemHeight = 15;
        this.ListBoxFiles.Location = new System.Drawing.Point(5, 59);
        this.ListBoxFiles.Name = "ListBoxFiles";
        this.ListBoxFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
        this.ListBoxFiles.Size = new System.Drawing.Size(235, 394);
        this.ListBoxFiles.TabIndex = 10;
        // 
        // TimerStatusUpdater
        // 
        // 
        // MainForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(8.0f, 15.0f);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1098, 665);
        this.Controls.Add(this.TabControlRenderingFiles);
        this.Controls.Add(this.LogBox);
        this.Controls.Add(this.CheckBoxShowOutput);
        this.Controls.Add(this.LabelPreviewMode);
        this.Controls.Add(this.StatusStrip1);
        this.Controls.Add(this.PictureBoxOutput);
        this.Controls.Add(this.ButtonControl);
        this.Controls.Add(this.TextBoxOutputLocation);
        this.Controls.Add(this.LabelOutputLocation);
        this.Controls.Add(this.ButtonSetOutputFolder);
        this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
        this.Margin = new System.Windows.Forms.Padding(4);
        this.MaximizeBox = false;
        this.Name = "MainForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Oscilloscope View Generator";
        (System.ComponentModel.ISupportInitialize)this.NumericUpDownMiddleLine.EndInit();
        (System.ComponentModel.ISupportInitialize)this.NumericUpDownGrid.EndInit();
        (System.ComponentModel.ISupportInitialize)this.NumericUpDownLineWidth.EndInit();
        (System.ComponentModel.ISupportInitialize)this.NumericUpDownColumn.EndInit();
        (System.ComponentModel.ISupportInitialize)this.NumericUpDownFrameRate.EndInit();
        (System.ComponentModel.ISupportInitialize)this.PictureBoxOutput.EndInit();
        this.StatusStrip1.ResumeLayout(false);
        this.StatusStrip1.PerformLayout();
        this.TabControlRenderingFiles.ResumeLayout(false);
        this.TabPageRendering.ResumeLayout(false);
        this.TabPageRendering.PerformLayout();
        (System.ComponentModel.ISupportInitialize)this.NumericUpDownBorder.EndInit();
        this.TabPageFiles.ResumeLayout(false);
        this.TabPageFiles.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }
    private System.ComponentModel.BackgroundWorker _OscilloscopeBackgroundWorker;

    internal System.ComponentModel.BackgroundWorker OscilloscopeBackgroundWorker
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _OscilloscopeBackgroundWorker;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_OscilloscopeBackgroundWorker != null)
            {
            }

            _OscilloscopeBackgroundWorker = value;
            if (_OscilloscopeBackgroundWorker != null)
            {
            }
        }
    }

    private System.Windows.Forms.Button _ButtonSetOutputFolder;

    internal System.Windows.Forms.Button ButtonSetOutputFolder
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ButtonSetOutputFolder;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ButtonSetOutputFolder != null)
            {
            }

            _ButtonSetOutputFolder = value;
            if (_ButtonSetOutputFolder != null)
            {
            }
        }
    }

    private System.Windows.Forms.Label _LabelOutputLocation;

    internal System.Windows.Forms.Label LabelOutputLocation
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelOutputLocation;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelOutputLocation != null)
            {
            }

            _LabelOutputLocation = value;
            if (_LabelOutputLocation != null)
            {
            }
        }
    }

    private System.Windows.Forms.TextBox _TextBoxOutputLocation;

    internal System.Windows.Forms.TextBox TextBoxOutputLocation
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _TextBoxOutputLocation;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_TextBoxOutputLocation != null)
            {
            }

            _TextBoxOutputLocation = value;
            if (_TextBoxOutputLocation != null)
            {
            }
        }
    }

    private System.Windows.Forms.Button _ButtonControl;

    internal System.Windows.Forms.Button ButtonControl
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ButtonControl;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ButtonControl != null)
            {
            }

            _ButtonControl = value;
            if (_ButtonControl != null)
            {
            }
        }
    }

    private System.Windows.Forms.PictureBox _PictureBoxOutput;

    internal System.Windows.Forms.PictureBox PictureBoxOutput
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _PictureBoxOutput;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_PictureBoxOutput != null)
            {
            }

            _PictureBoxOutput = value;
            if (_PictureBoxOutput != null)
            {
            }
        }
    }

    private System.Windows.Forms.CheckBox _CheckBoxShowOutput;

    internal System.Windows.Forms.CheckBox CheckBoxShowOutput
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _CheckBoxShowOutput;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_CheckBoxShowOutput != null)
            {
            }

            _CheckBoxShowOutput = value;
            if (_CheckBoxShowOutput != null)
            {
            }
        }
    }

    private System.Windows.Forms.NumericUpDown _NumericUpDownFrameRate;

    internal System.Windows.Forms.NumericUpDown NumericUpDownFrameRate
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _NumericUpDownFrameRate;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_NumericUpDownFrameRate != null)
            {
            }

            _NumericUpDownFrameRate = value;
            if (_NumericUpDownFrameRate != null)
            {
            }
        }
    }

    private System.Windows.Forms.Label _LabelFrameRate;

    internal System.Windows.Forms.Label LabelFrameRate
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelFrameRate;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelFrameRate != null)
            {
            }

            _LabelFrameRate = value;
            if (_LabelFrameRate != null)
            {
            }
        }
    }

    private System.Windows.Forms.CheckBox _CheckBoxVideo;

    internal System.Windows.Forms.CheckBox CheckBoxVideo
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _CheckBoxVideo;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_CheckBoxVideo != null)
            {
            }

            _CheckBoxVideo = value;
            if (_CheckBoxVideo != null)
            {
            }
        }
    }

    private System.Windows.Forms.Button _ButtonAudio;

    internal System.Windows.Forms.Button ButtonAudio
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ButtonAudio;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ButtonAudio != null)
            {
            }

            _ButtonAudio = value;
            if (_ButtonAudio != null)
            {
            }
        }
    }

    private System.Windows.Forms.CheckBox _CheckBoxSmooth;

    internal System.Windows.Forms.CheckBox CheckBoxSmooth
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _CheckBoxSmooth;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_CheckBoxSmooth != null)
            {
            }

            _CheckBoxSmooth = value;
            if (_CheckBoxSmooth != null)
            {
            }
        }
    }

    private System.Windows.Forms.NumericUpDown _NumericUpDownColumn;

    internal System.Windows.Forms.NumericUpDown NumericUpDownColumn
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _NumericUpDownColumn;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_NumericUpDownColumn != null)
            {
            }

            _NumericUpDownColumn = value;
            if (_NumericUpDownColumn != null)
            {
            }
        }
    }

    private System.Windows.Forms.Label _LabelColumn;

    internal System.Windows.Forms.Label LabelColumn
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelColumn;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelColumn != null)
            {
            }

            _LabelColumn = value;
            if (_LabelColumn != null)
            {
            }
        }
    }

    private System.Windows.Forms.CheckBox _CheckBoxNoFileWriting;

    internal System.Windows.Forms.CheckBox CheckBoxNoFileWriting
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _CheckBoxNoFileWriting;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_CheckBoxNoFileWriting != null)
            {
            }

            _CheckBoxNoFileWriting = value;
            if (_CheckBoxNoFileWriting != null)
            {
            }
        }
    }

    private System.Windows.Forms.Button _ButtonAdd;

    internal System.Windows.Forms.Button ButtonAdd
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ButtonAdd;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ButtonAdd != null)
            {
            }

            _ButtonAdd = value;
            if (_ButtonAdd != null)
            {
            }
        }
    }

    private System.Windows.Forms.Button _ButtonRemove;

    internal System.Windows.Forms.Button ButtonRemove
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ButtonRemove;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ButtonRemove != null)
            {
            }

            _ButtonRemove = value;
            if (_ButtonRemove != null)
            {
            }
        }
    }

    private System.Windows.Forms.Button _ButtonMoveDown;

    internal System.Windows.Forms.Button ButtonMoveDown
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ButtonMoveDown;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ButtonMoveDown != null)
            {
            }

            _ButtonMoveDown = value;
            if (_ButtonMoveDown != null)
            {
            }
        }
    }

    private System.Windows.Forms.Button _ButtonMoveUp;

    internal System.Windows.Forms.Button ButtonMoveUp
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ButtonMoveUp;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ButtonMoveUp != null)
            {
            }

            _ButtonMoveUp = value;
            if (_ButtonMoveUp != null)
            {
            }
        }
    }

    private System.Windows.Forms.Button _ButtonOptions;

    internal System.Windows.Forms.Button ButtonOptions
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ButtonOptions;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ButtonOptions != null)
            {
            }

            _ButtonOptions = value;
            if (_ButtonOptions != null)
            {
            }
        }
    }

    private System.Windows.Forms.ToolTip _ToolTips;

    internal System.Windows.Forms.ToolTip ToolTips
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ToolTips;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ToolTips != null)
            {
            }

            _ToolTips = value;
            if (_ToolTips != null)
            {
            }
        }
    }

    private System.Windows.Forms.Label _LabelLineWidth;

    internal System.Windows.Forms.Label LabelLineWidth
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelLineWidth;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelLineWidth != null)
            {
            }

            _LabelLineWidth = value;
            if (_LabelLineWidth != null)
            {
            }
        }
    }

    private System.Windows.Forms.NumericUpDown _NumericUpDownLineWidth;

    internal System.Windows.Forms.NumericUpDown NumericUpDownLineWidth
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _NumericUpDownLineWidth;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_NumericUpDownLineWidth != null)
            {
            }

            _NumericUpDownLineWidth = value;
            if (_NumericUpDownLineWidth != null)
            {
            }
        }
    }

    private System.Windows.Forms.CheckBox _CheckBoxCRT;

    internal System.Windows.Forms.CheckBox CheckBoxCRT
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _CheckBoxCRT;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_CheckBoxCRT != null)
            {
            }

            _CheckBoxCRT = value;
            if (_CheckBoxCRT != null)
            {
            }
        }
    }

    private System.Windows.Forms.Button _ButtonSelectAll;

    internal System.Windows.Forms.Button ButtonSelectAll
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ButtonSelectAll;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ButtonSelectAll != null)
            {
            }

            _ButtonSelectAll = value;
            if (_ButtonSelectAll != null)
            {
            }
        }
    }

    private System.Windows.Forms.StatusStrip _StatusStrip1;

    internal System.Windows.Forms.StatusStrip StatusStrip1
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _StatusStrip1;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_StatusStrip1 != null)
            {
            }

            _StatusStrip1 = value;
            if (_StatusStrip1 != null)
            {
            }
        }
    }

    private System.Windows.Forms.ToolStripStatusLabel _LabelStatus;

    internal System.Windows.Forms.ToolStripStatusLabel LabelStatus
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelStatus;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelStatus != null)
            {
            }

            _LabelStatus = value;
            if (_LabelStatus != null)
            {
            }
        }
    }

    private System.Windows.Forms.Label _LabelPreviewMode;

    internal System.Windows.Forms.Label LabelPreviewMode
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelPreviewMode;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelPreviewMode != null)
            {
            }

            _LabelPreviewMode = value;
            if (_LabelPreviewMode != null)
            {
            }
        }
    }

    private System.Windows.Forms.Timer _TimerLabelFlashing;

    internal System.Windows.Forms.Timer TimerLabelFlashing
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _TimerLabelFlashing;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_TimerLabelFlashing != null)
            {
            }

            _TimerLabelFlashing = value;
            if (_TimerLabelFlashing != null)
            {
            }
        }
    }

    private System.Windows.Forms.Label _LabelCanvasSize;

    internal System.Windows.Forms.Label LabelCanvasSize
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelCanvasSize;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelCanvasSize != null)
            {
            }

            _LabelCanvasSize = value;
            if (_LabelCanvasSize != null)
            {
            }
        }
    }

    private System.Windows.Forms.ComboBox _ComboBoxCanvasSize;

    internal System.Windows.Forms.ComboBox ComboBoxCanvasSize
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ComboBoxCanvasSize;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ComboBoxCanvasSize != null)
            {
            }

            _ComboBoxCanvasSize = value;
            if (_ComboBoxCanvasSize != null)
            {
            }
        }
    }

    private System.Windows.Forms.CheckBox _CheckBoxGrid;

    internal System.Windows.Forms.CheckBox CheckBoxGrid
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _CheckBoxGrid;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_CheckBoxGrid != null)
            {
            }

            _CheckBoxGrid = value;
            if (_CheckBoxGrid != null)
            {
            }
        }
    }

    private System.Windows.Forms.ToolStripStatusLabel _ToolStripStatusLabelPadding;

    internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabelPadding
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ToolStripStatusLabelPadding;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ToolStripStatusLabelPadding != null)
            {
            }

            _ToolStripStatusLabelPadding = value;
            if (_ToolStripStatusLabelPadding != null)
            {
            }
        }
    }

    private System.Windows.Forms.ToolStripStatusLabel _ToolStripStatusLabelAbout;

    internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabelAbout
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ToolStripStatusLabelAbout;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ToolStripStatusLabelAbout != null)
            {
            }

            _ToolStripStatusLabelAbout = value;
            if (_ToolStripStatusLabelAbout != null)
            {
            }
        }
    }

    private System.Windows.Forms.LinkLabel _LinkLabelCustomCommandLine;

    internal System.Windows.Forms.LinkLabel LinkLabelCustomCommandLine
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LinkLabelCustomCommandLine;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LinkLabelCustomCommandLine != null)
            {
            }

            _LinkLabelCustomCommandLine = value;
            if (_LinkLabelCustomCommandLine != null)
            {
            }
        }
    }

    private System.ComponentModel.BackgroundWorker _BackgroundWorkerStdErrReader;

    internal System.ComponentModel.BackgroundWorker BackgroundWorkerStdErrReader
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _BackgroundWorkerStdErrReader;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_BackgroundWorkerStdErrReader != null)
            {
            }

            _BackgroundWorkerStdErrReader = value;
            if (_BackgroundWorkerStdErrReader != null)
            {
            }
        }
    }

    private System.Windows.Forms.Label _LabelFlowDirecton;

    internal System.Windows.Forms.Label LabelFlowDirecton
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelFlowDirecton;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelFlowDirecton != null)
            {
            }

            _LabelFlowDirecton = value;
            if (_LabelFlowDirecton != null)
            {
            }
        }
    }

    private Button _ButtonFlowDirection;

    internal Button ButtonFlowDirection
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ButtonFlowDirection;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ButtonFlowDirection != null)
            {
            }

            _ButtonFlowDirection = value;
            if (_ButtonFlowDirection != null)
            {
            }
        }
    }

    private CheckBox _CheckBoxDrawMiddleLine;

    internal CheckBox CheckBoxDrawMiddleLine
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _CheckBoxDrawMiddleLine;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_CheckBoxDrawMiddleLine != null)
            {
            }

            _CheckBoxDrawMiddleLine = value;
            if (_CheckBoxDrawMiddleLine != null)
            {
            }
        }
    }

    private NumericUpDown _NumericUpDownMiddleLine;

    internal NumericUpDown NumericUpDownMiddleLine
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _NumericUpDownMiddleLine;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_NumericUpDownMiddleLine != null)
            {
            }

            _NumericUpDownMiddleLine = value;
            if (_NumericUpDownMiddleLine != null)
            {
            }
        }
    }

    private NumericUpDown _NumericUpDownGrid;

    internal NumericUpDown NumericUpDownGrid
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _NumericUpDownGrid;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_NumericUpDownGrid != null)
            {
            }

            _NumericUpDownGrid = value;
            if (_NumericUpDownGrid != null)
            {
            }
        }
    }

    private Button _ButtonMiddleLineColor;

    internal Button ButtonMiddleLineColor
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ButtonMiddleLineColor;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ButtonMiddleLineColor != null)
            {
            }

            _ButtonMiddleLineColor = value;
            if (_ButtonMiddleLineColor != null)
            {
            }
        }
    }

    private Button _ButtonGridColor;

    internal Button ButtonGridColor
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ButtonGridColor;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ButtonGridColor != null)
            {
            }

            _ButtonGridColor = value;
            if (_ButtonGridColor != null)
            {
            }
        }
    }

    private Button _ButtonBackgroundColor;

    internal Button ButtonBackgroundColor
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ButtonBackgroundColor;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ButtonBackgroundColor != null)
            {
            }

            _ButtonBackgroundColor = value;
            if (_ButtonBackgroundColor != null)
            {
            }
        }
    }

    private RichTextBox _LogBox;

    internal RichTextBox LogBox
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LogBox;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LogBox != null)
            {
            }

            _LogBox = value;
            if (_LogBox != null)
            {
            }
        }
    }

    private TabControl _TabControlRenderingFiles;

    internal TabControl TabControlRenderingFiles
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _TabControlRenderingFiles;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_TabControlRenderingFiles != null)
            {
            }

            _TabControlRenderingFiles = value;
            if (_TabControlRenderingFiles != null)
            {
            }
        }
    }

    private TabPage _TabPageRendering;

    internal TabPage TabPageRendering
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _TabPageRendering;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_TabPageRendering != null)
            {
            }

            _TabPageRendering = value;
            if (_TabPageRendering != null)
            {
            }
        }
    }

    private TabPage _TabPageFiles;

    internal TabPage TabPageFiles
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _TabPageFiles;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_TabPageFiles != null)
            {
            }

            _TabPageFiles = value;
            if (_TabPageFiles != null)
            {
            }
        }
    }

    private CheckBox _CheckBoxBorder;

    internal CheckBox CheckBoxBorder
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _CheckBoxBorder;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_CheckBoxBorder != null)
            {
            }

            _CheckBoxBorder = value;
            if (_CheckBoxBorder != null)
            {
            }
        }
    }

    private NumericUpDown _NumericUpDownBorder;

    internal NumericUpDown NumericUpDownBorder
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _NumericUpDownBorder;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_NumericUpDownBorder != null)
            {
            }

            _NumericUpDownBorder = value;
            if (_NumericUpDownBorder != null)
            {
            }
        }
    }

    private Button _ButtonBorderColor;

    internal Button ButtonBorderColor
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ButtonBorderColor;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ButtonBorderColor != null)
            {
            }

            _ButtonBorderColor = value;
            if (_ButtonBorderColor != null)
            {
            }
        }
    }

    private ComboBox _ComboBoxLabelPos;

    internal ComboBox ComboBoxLabelPos
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ComboBoxLabelPos;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ComboBoxLabelPos != null)
            {
            }

            _ComboBoxLabelPos = value;
            if (_ComboBoxLabelPos != null)
            {
            }
        }
    }

    private Label _LabelChannelLabelPos;

    internal Label LabelChannelLabelPos
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _LabelChannelLabelPos;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_LabelChannelLabelPos != null)
            {
            }

            _LabelChannelLabelPos = value;
            if (_LabelChannelLabelPos != null)
            {
            }
        }
    }

    private ListBox _ListBoxFiles;

    internal ListBox ListBoxFiles
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _ListBoxFiles;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_ListBoxFiles != null)
            {
            }

            _ListBoxFiles = value;
            if (_ListBoxFiles != null)
            {
            }
        }
    }

    private CheckBox _CheckBoxDottedXYmode;

    internal CheckBox CheckBoxDottedXYmode
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _CheckBoxDottedXYmode;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_CheckBoxDottedXYmode != null)
            {
            }

            _CheckBoxDottedXYmode = value;
            if (_CheckBoxDottedXYmode != null)
            {
            }
        }
    }

    private Timer _TimerStatusUpdater;

    internal Timer TimerStatusUpdater
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _TimerStatusUpdater;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_TimerStatusUpdater != null)
            {
            }

            _TimerStatusUpdater = value;
            if (_TimerStatusUpdater != null)
            {
            }
        }
    }
}

