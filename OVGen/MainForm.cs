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
using Microsoft.WindowsAPICodePack.Taskbar;

public partial class MainForm
{
    public bool isRunningMono { get; set; }
    private string configFileLocation = Environment.CurrentDirectory + @"\OVG.ini";
    // == FPS counter
    private DateTime fpsTimer;
    private ulong fpsFrames;
    private double realFPS;
    private double averageFPS;
    private DateTime startTime;
    private FlowDirection channelFlowDirection = FlowDirection.LeftToRight;
    // ===For Worker
    public List<channelOptions> optionsList = new List<channelOptions>();
    private Size canvasSize = new Size(1280, 720);
    public Pen wavePen = new Pen(Color.White, 2);
    private string masterAudioFile = "";
    private string outputLocation = "";
    private string outputDirectory = "";
    private bool NoFileWriting = false;
    private bool allFilesLoaded = false;
    private Dictionary<string, string> failedFiles = new Dictionary<string, string>();
    private Bitmap lastFrame;
    private List<Progress> progressList = new List<Progress>();
    // ===FFmpeg
    private bool convertVideo = false;
    private bool canceledByUser = false;
    private int FFmpegExitCode = 0;
    public const string DefaultFFmpegCommandLineJoinAudio = "-f image2pipe -framerate {framerate} -c:v png -i {img} -i {audio} -c:a aac -b:a 384k -c:v libx264 -crf 18 -bf 2 -flags +cgop -pix_fmt yuv420p -movflags faststart {outfile}";
    public const string DefaultFFmpegCommandLineSilence = "-f image2pipe -framerate {framerate} -c:v png -i {img} -c:v libx264 -crf 18 -bf 2 -flags +cgop -pix_fmt yuv420p -movflags faststart {outfile}";
    public string FFmpegCommandLineJoinAudio = DefaultFFmpegCommandLineJoinAudio;
    public string FFmpegCommandLineSilence = DefaultFFmpegCommandLineSilence;
    public System.IO.StreamReader FFmpegstderr;
    // ===For file operation
    const string FileFilter = "WAVE File(*.wav)|*.wav";
    public string currentChannelToBeSet = "";
    private string ffmpegPath = "";

    // ===GUI..etc.
    private bool formStarted = false;
    private Size originalFormSize;
    private int originalTextBoxLogHeight;
    private bool isProgressTaskBarSupported = true;

    private void MainForm_Activated(object sender, System.EventArgs e)
    {
        if (!formStarted)
        {
            originalFormSize = this.Size;
            this.MinimumSize = this.Size;
        }
        formStarted = true;
    }

    private void MainForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
    {
        if (OscilloscopeBackgroundWorker.IsBusy)
        {
            DialogResult r = Interaction.MsgBox("Do you want to stop the worker?", MsgBoxStyle.Question + MsgBoxStyle.YesNo);
            if (r == System.Windows.Forms.DialogResult.Yes)
                ButtonControl_Click(null, null);
            else
            {
                e.Cancel = true;
                return;
            }
        }
    }

    private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
    {
        writeConfig();
    }

    private void MainForm_Load(System.Object sender, System.EventArgs e)
    {
        Control.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        Control.SetStyle(ControlStyles.DoubleBuffer, true);
        Control.SetStyle(ControlStyles.UserPaint, true);
        loadConfig();
        previewLayout();
        outputDirectory = System.IO.Path.GetTempPath() + "OVG-" + randStr(5);
        if (!CheckBoxVideo.Checked)
            TextBoxOutputLocation.Text = outputDirectory;
        LabelStatus.Text = "";
        CheckBoxNoFileWriting_CheckedChanged(null, null);
        originalTextBoxLogHeight = LogBox.Height;
        this.Text += " " + Application.ProductVersion;
        isRunningMono = Type.GetType("Mono.Runtime") != null;
        if (isRunningMono)
            LogBox.AppendText("Detected running OVGen under Mono." + Constants.vbNewLine);
        isProgressTaskBarSupported = Environment.OSVersion.Version.Major >= 6 & Environment.OSVersion.Version.Minor >= 1;
    }

    public string randStr(ulong len)
    {
        Random rand = new Random();
        string map = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        randStr = "";
        for (int i = 1; i <= len; i++)
            randStr += map.Substring(rand.Next(map.Length - 1), 1);
    }

    public string SafeFilename(string filename)
    {
        SafeFilename = filename;
        if (filename.Contains(" "))
            SafeFilename = "\"" + filename + "\"";
    }

    private void writeConfig()
    {
        OVGconfig conf = new OVGconfig();
        conf.General.SmoothLine = CheckBoxSmooth.Checked;
        conf.General.DrawMiddleLine = CheckBoxDrawMiddleLine.Checked;
        conf.General.BackgroundColor = new ColorSerializable(ButtonBackgroundColor.BackColor);
        conf.General.MiddleLineColor = new ColorSerializable(ButtonMiddleLineColor.BackColor);
        conf.General.MiddleLineWidth = NumericUpDownMiddleLine.Value;
        conf.General.Framerate = NumericUpDownFrameRate.Value;
        conf.General.LineWidth = NumericUpDownLineWidth.Value;
        conf.General.ConvertVideo = CheckBoxVideo.Checked;
        conf.General.CRTStyledRender = CheckBoxCRT.Checked;
        conf.General.DottedXYmode = CheckBoxDottedXYmode.Checked;
        conf.General.DrawGrid = CheckBoxGrid.Checked;
        conf.General.GridColor = new ColorSerializable(ButtonGridColor.BackColor);
        conf.General.GridWidth = NumericUpDownGrid.Value;
        conf.General.DrawBorder = CheckBoxBorder.Checked;
        conf.General.BorderColor = new ColorSerializable(ButtonBorderColor.BackColor);
        conf.General.BorderWidth = NumericUpDownBorder.Value;
        conf.General.CanvasSize = ComboBoxCanvasSize.Text;
        conf.General.FlowDirection = channelFlowDirection;
        conf.General.LabelPosition = ComboBoxLabelPos.SelectedIndex;
        conf.FFmpeg.BinaryLocation = ffmpegPath.Trim();
        conf.FFmpeg.JoinAudioCommandLine = FFmpegCommandLineJoinAudio.Trim();
        if (FFmpegCommandLineJoinAudio == DefaultFFmpegCommandLineJoinAudio)
            conf.FFmpeg.JoinAudioCommandLine = "";
        conf.FFmpeg.SilenceCommandLine = FFmpegCommandLineSilence.Trim();
        if (FFmpegCommandLineSilence == DefaultFFmpegCommandLineSilence)
            conf.FFmpeg.SilenceCommandLine = "";
        System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(conf.GetType());
        try
        {
            System.IO.FileStream fs = new System.IO.FileStream("config.xml", System.IO.FileMode.Create);
            xml.Serialize(fs, conf);
            fs.Close();
        }
        catch (Exception ex)
        {
        }
    }

    private void loadConfig()
    {
        if (My.Computer.FileSystem.FileExists("config.xml"))
        {
            OVGconfig conf = new OVGconfig();
            System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(conf.GetType());
            try
            {
                System.IO.FileStream fs = new System.IO.FileStream("config.xml", System.IO.FileMode.Open);
                conf = xml.Deserialize(fs);
                fs.Close();
                ffmpegPath = conf.FFmpeg.BinaryLocation;
                FFmpegCommandLineJoinAudio = conf.FFmpeg.JoinAudioCommandLine;
                if (FFmpegCommandLineJoinAudio == "")
                    FFmpegCommandLineJoinAudio = DefaultFFmpegCommandLineJoinAudio;
                FFmpegCommandLineSilence = conf.FFmpeg.SilenceCommandLine;
                if (FFmpegCommandLineSilence == "")
                    FFmpegCommandLineSilence = DefaultFFmpegCommandLineSilence;
                CheckBoxSmooth.Checked = conf.General.SmoothLine;
                ButtonBackgroundColor.BackColor = conf.General.BackgroundColor.GetColor();
                CheckBoxDrawMiddleLine.Checked = conf.General.DrawMiddleLine;
                ButtonMiddleLineColor.BackColor = conf.General.MiddleLineColor.GetColor();
                NumericUpDownMiddleLine.Value = conf.General.MiddleLineWidth;
                NumericUpDownFrameRate.Value = conf.General.Framerate;
                NumericUpDownLineWidth.Value = conf.General.LineWidth;
                CheckBoxVideo.Checked = conf.General.ConvertVideo;
                CheckBoxCRT.Checked = conf.General.CRTStyledRender;
                CheckBoxDottedXYmode.Checked = conf.General.DottedXYmode;
                CheckBoxGrid.Checked = conf.General.DrawGrid;
                ButtonGridColor.BackColor = conf.General.GridColor.GetColor();
                CheckBoxBorder.Checked = conf.General.DrawBorder;
                ButtonBorderColor.BackColor = conf.General.BorderColor.GetColor();
                NumericUpDownBorder.Value = conf.General.BorderWidth;
                NumericUpDownGrid.Value = conf.General.GridWidth;
                ComboBoxCanvasSize.Text = conf.General.CanvasSize;
                channelFlowDirection = conf.General.FlowDirection;
                ComboBoxLabelPos.SelectedIndex = conf.General.LabelPosition;
                ButtonFlowDirection.Invalidate();
            }
            catch (Exception ex)
            {
                LogBox.AppendText("Error occured while loading config:" + ex.Message + Constants.vbCrLf);
                return;
            }
        }
    }

    private void ButtonControl_Click(System.Object sender, System.EventArgs e)
    {
        if (!OscilloscopeBackgroundWorker.IsBusy)
        {
            LogBox.Clear();
            WorkerArguments arg = new WorkerArguments();
            // check cavnas size
            try
            {
                string[] userInput = ComboBoxCanvasSize.Text.Split(new[] { "x", " " }, StringSplitOptions.RemoveEmptyEntries);
                if (userInput.Length == 2)
                    canvasSize = new Size(userInput[0], userInput[1]);
                else
                {
                    Interaction.MsgBox("Invalid canvas size!", MsgBoxStyle.Critical);
                    return;
                }
                if (canvasSize.Width < 1 | canvasSize.Height < 1)
                {
                    Interaction.MsgBox("Invalid canvas size!", MsgBoxStyle.Critical);
                    return;
                }
            }
            catch (Exception ex)
            {
                Interaction.MsgBox("Invalid canvas size!" + Constants.vbCrLf + ex.Message, MsgBoxStyle.Critical);
                return;
            }
            arg.backgroundColor = ButtonBackgroundColor.BackColor;
            arg.outputFile = outputLocation;
            outputLocation = TextBoxOutputLocation.Text;
            outputDirectory = "";
            arg.drawGrid = CheckBoxGrid.Checked;
            arg.gridPen = new Pen(ButtonGridColor.BackColor, NumericUpDownGrid.Value);
            arg.drawBorder = CheckBoxBorder.Checked;
            arg.borderPen = new Pen(ButtonBorderColor.BackColor, NumericUpDownBorder.Value * 2);
            arg.useAnalogOscilloscopeStyle = CheckBoxCRT.Checked;
            arg.dottedXYmode = CheckBoxDottedXYmode.Checked;
            if (arg.useAnalogOscilloscopeStyle)
            {
                arg.analogOscilloscopeLineWidth = NumericUpDownLineWidth.Value;
                wavePen.Width = 1;
            }
            else
                wavePen.Width = NumericUpDownLineWidth.Value;
            if (ListBoxFiles.Items.Count == 0)
            {
                Interaction.MsgBox("Please add at least one file!", MsgBoxStyle.Exclamation);
                return;
            }

            Debug.WriteLine(string.Format("Output directory:{0}", outputDirectory));
            arg.FPS = NumericUpDownFrameRate.Value;
            arg.smoothLine = CheckBoxSmooth.Checked;
            arg.FPS = NumericUpDownFrameRate.Value;
            arg.noFileWriting = CheckBoxNoFileWriting.Checked;
            NoFileWriting = arg.noFileWriting;
            arg.convertVideo = CheckBoxVideo.Checked;
            convertVideo = arg.convertVideo;
            if (arg.convertVideo & !arg.noFileWriting)
            {
                Debug.WriteLine(Strings.Right(outputLocation, 4).ToLower());
                if (Strings.Right(outputLocation, 4).ToLower() != ".mp4")
                {
                    Interaction.MsgBox("Please set a proper filename!", MsgBoxStyle.Critical);
                    return;
                }
                try
                {
                    using (var f = new System.IO.FileStream(outputLocation, System.IO.FileMode.OpenOrCreate))
                    {
                        f.WriteByte(0);
                        f.Flush();
                    }
                }
                catch (Exception ex)
                {
                    Interaction.MsgBox(ex.Message, MsgBoxStyle.Critical, "Error on creating empty file.");
                    return;
                }
                arg.outputDirectory = System.IO.Path.GetTempPath() + "OVG_" + randStr(5) + @"\";
                outputDirectory = System.IO.Path.GetTempPath() + "OVG_" + randStr(5) + @"\";
                arg.outputFile = outputLocation;
                if (!BackgroundWorkerStdErrReader.IsBusy)
                    BackgroundWorkerStdErrReader.RunWorkerAsync();
            }
            else
            {
                arg.outputDirectory = TextBoxOutputLocation.Text;
                outputDirectory = outputLocation;
            }
            if (outputDirectory == "" & !arg.noFileWriting & !arg.convertVideo)
            {
                Interaction.MsgBox("Please select a directory!", MsgBoxStyle.Exclamation);
                return;
            }
            arg.drawMiddleLine = CheckBoxDrawMiddleLine.Checked;
            arg.middleLinePen = new Pen(ButtonMiddleLineColor.BackColor, NumericUpDownMiddleLine.Value);
            arg.columns = NumericUpDownColumn.Value;
            arg.flowDirection = channelFlowDirection;
            arg.labelPostition = ComboBoxLabelPos.SelectedIndex;
            string[] fileArray = new string[ListBoxFiles.Items.Count - 1 + 1];
            for (int i = 0; i <= fileArray.Length - 1; i++)
                fileArray[i] = ListBoxFiles.Items[i];
            arg.files = fileArray;
            if (System.IO.File.Exists(masterAudioFile))
            {
                arg.joinAudio = true;
                arg.audioFile = masterAudioFile;
            }
            else
                arg.joinAudio = false;
            arg.ffmpegBinary = ffmpegPath;
            LabelStatus.Text = "Start.";
            OscilloscopeBackgroundWorker.RunWorkerAsync(arg);
            TimerStatusUpdater.Start();
            foreach (Control ctrl in TabControlRenderingFiles.Controls)
                ctrl.Enabled = false;
            ButtonControl.Text = "Cancel";
            ButtonControl.Update();
        }
        else
        {
            OscilloscopeBackgroundWorker.CancelAsync();
            ButtonControl.Text = "Start";
        }
    }

    private void CheckBoxVideo_CheckedChanged(System.Object sender, System.EventArgs e)
    {
        if (CheckBoxVideo.Checked)
        {
            if (!My.Computer.FileSystem.FileExists(ffmpegPath) & ffmpegPath != "ffmpeg")
            {
                Form waitForm = new Form();
                waitForm.ShowInTaskbar = false;
                waitForm.FormBorderStyle = FormBorderStyle.None;
                waitForm.ControlBox = false;
                waitForm.Size = new Size(100, 50);
                waitForm.Cursor = Cursors.WaitCursor;
                waitForm.StartPosition = FormStartPosition.Manual;
                waitForm.Location = new Point(this.Location.X + this.Size.Width / (double)2 - waitForm.Width / (double)2, this.Location.Y + this.Size.Height / (double)2 - waitForm.Height / (double)2);
                waitForm.TopMost = true;
                Label waitLabel = new Label();
                waitLabel.Text = "Please wait...";
                Graphics g = waitForm.CreateGraphics();
                SizeF labelSize = g.MeasureString(waitLabel.Text, waitLabel.Font);
                waitLabel.Location = new Point(waitForm.Width / (double)2 - labelSize.Width / (double)2, waitForm.Height / (double)2 - labelSize.Height / (double)2);
                waitForm.Controls.Add(waitLabel);
                ProcessStartInfo procInfo = new ProcessStartInfo("ffmpeg", "-version");
                procInfo.UseShellExecute = false;
                procInfo.CreateNoWindow = true;
                bool FFmpegExist = true;
                Process proc = null;
                waitForm.Show();
                waitLabel.Refresh();
                ControlPaint.DrawBorder3D(g, new Rectangle(new Point(), waitForm.Size), Border3DStyle.Raised);
                try
                {
                    proc = Process.Start(procInfo);
                }
                catch (Exception ex)
                {
                    FFmpegExist = false;
                }
                if (proc != null)
                {
                    Stopwatch procStopWatch = new Stopwatch();
                    procStopWatch.Start();
                    while (!proc.HasExited)
                    {
                        waitLabel.Refresh();
                        ControlPaint.DrawBorder3D(g, new Rectangle(new Point(), waitForm.Size), Border3DStyle.Raised);
                        Application.DoEvents();
                        if (procStopWatch.Elapsed.TotalSeconds > 10)
                        {
                            proc.Kill();
                            return;
                        }
                    }
                    procStopWatch.Stop();
                    if (proc.ExitCode != 0)
                        FFmpegExist = false;
                }
                waitForm.Close();
                bool NeedToOpenDialog = true;
                if (FFmpegExist)
                {
                    MsgBoxResult useSystem = Interaction.MsgBox("We detected a copy of FFmpeg is installed in this system, do you want to use it?", MsgBoxStyle.YesNo + MsgBoxStyle.Question);
                    if (useSystem == MsgBoxResult.Yes)
                        NeedToOpenDialog = false;
                }
                else
                    Interaction.MsgBox("FFmpeg binary is not exist or location not set!, select one.", MsgBoxStyle.Exclamation);
                if (NeedToOpenDialog)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    if (isRunningMono)
                        ofd.Filter = "FFmpeg binary|ffmpeg|All Files|*.*";
                    else
                        ofd.Filter = "FFmpeg binary|ffmpeg.exe|All Files|*.*";
                    if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        ffmpegPath = ofd.FileName;
                        writeConfig();
                    }
                    else
                    {
                        CheckBoxVideo.Checked = false;
                        return;
                    }
                }
                else
                {
                    ffmpegPath = "ffmpeg";
                    writeConfig();
                }
            }
            ButtonAudio.Enabled = true;
            LabelOutputLocation.Text = "Output video:";
        }
        else
        {
            ButtonAudio.Enabled = false;
            LabelOutputLocation.Text = "Output folder:";
            masterAudioFile = "";
            ButtonAudio.Text = "Master Audio";
        }
    }

    private void ButtonAudio_Click(System.Object sender, System.EventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "WAVE File(*.wav)|*.wav";
        if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            masterAudioFile = ofd.FileName;
            ButtonAudio.Text = ofd.SafeFileName;
        }
    }

    private void ButtonSetOutputFolder_Click(System.Object sender, System.EventArgs e)
    {
        if (CheckBoxVideo.Checked)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "MP4 File(*.mp4)|*.mp4";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                TextBoxOutputLocation.Text = sfd.FileName;
        }
        else
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                TextBoxOutputLocation.Text = fbd.SelectedPath;
        }
    }

    private void OscilloscopeBackgroundWorker_DoWork(System.Object sender, System.ComponentModel.DoWorkEventArgs e)
    {
        workerReportProg(new Progress("Start."));
        startTime = DateTime.Now;
        WorkerArguments args = e.Argument;
        canceledByUser = false;
        if (!My.Computer.FileSystem.DirectoryExists(outputDirectory) & !args.noFileWriting & !args.convertVideo)
        {
            My.Computer.FileSystem.CreateDirectory(outputDirectory);
            Debug.WriteLine(outputDirectory);
        }

        WAV[] wave = new WAV[args.files.Length - 1 + 1];
        Dictionary<byte, WAV> extTrig = new Dictionary<byte, WAV>();
        List<byte[]> data = new List<byte[]>();
        byte col = args.columns;
        uint sampleLength = 0;
        ulong frames = 0;
        ulong totalFrame = 0;
        allFilesLoaded = true;
        failedFiles.Clear();
        for (byte z = 0; z <= args.files.Length - 1; z++)
        {
            workerReportProg(new Progress("Loading wav file: " + args.files[z]));
            try
            {
                wave[z] = new WAV(args.files[z]);
            }
            catch (Exception ex)
            {
                allFilesLoaded = false;
                failedFiles.Add(args.files[z], ex.Message);
                continue;
            }
            channelOptions extraArg = optionsList[z];
            wave[z].extraArguments = extraArg;
            wave[z].amplify = extraArg.amplify;
            wave[z].mixChannel = extraArg.mixChannel;
            wave[z].selectedChannel = extraArg.selectedChannel;
            if (wave[z].sampleLength > sampleLength)
            {
                sampleLength = wave[z].sampleLength;
                totalFrame = sampleLength / (wave[z].sampleRate / args.FPS) + 1;
            }
            if (extraArg.externalTriggerEnabled)
            {
                workerReportProg(new Progress("  Loading external trigger: " + extraArg.externalTriggerFile));
                try
                {
                    extTrig.Add(z, new WAV(extraArg.externalTriggerFile));
                    extTrig[z].extraArguments = extraArg;
                    extTrig[z].amplify = extraArg.amplify;
                    extTrig[z].mixChannel = extraArg.mixChannel;
                    extTrig[z].selectedChannel = extraArg.selectedChannel;
                }
                catch (Exception ex)
                {
                    allFilesLoaded = false;
                    failedFiles.Add(extraArg.externalTriggerFile, ex.Message);
                    continue;
                }
            }
        }
        if (My.Computer.FileSystem.FileExists(masterAudioFile))
        {
            try
            {
                WAV master = new WAV(masterAudioFile, true);
                sampleLength = master.sampleLength;
                workerReportProg(new Progress("Using length of master audio."));
                totalFrame = sampleLength / (master.sampleRate / args.FPS) + 1;
            }
            catch (Exception ex)
            {
                workerReportProg(new Progress("Failed to parse master audio: " + ex.Message));
            }
        }
        if (!allFilesLoaded)
        {
            wave = null;
            workerReportProg(new Progress("Failed to load file(s)."));
            return;
        }
        workerReportProg(new Progress("All file loaded."));
        Debug.WriteLine("Done loading waves.");
        fpsTimer = DateTime.Now;
        Debug.WriteLine(sampleLength);
        int bitDepth = wave[0].bitDepth;
        Debug.WriteLine(bitDepth);
        byte channels = args.files.Length;
        int maxChannelPerColumn = Math.Ceiling(channels / (double)col);
        int channelWidth = canvasSize.Width / (double)col;
        int channelHeight = canvasSize.Height / (double)maxChannelPerColumn;
        Size channelSize = new Size(channelWidth, channelHeight);
        Point[] channelOffset = new Point[channels - 1 + 1];
        for (int c = 0; c <= channels - 1; c++)
        {
            int x, y, currentColumn, currentRow;
            if (args.flowDirection == FlowDirection.LeftToRight)
            {
                currentRow = (c - (c % col)) / (double)col;
                y = channelHeight * currentRow;
                x = channelWidth * (c % col);
            }
            else if (args.flowDirection == FlowDirection.TopDown)
            {
                currentColumn = (c - (c % maxChannelPerColumn)) / (double)maxChannelPerColumn;
                y = channelHeight * (c % maxChannelPerColumn);
                x = channelWidth * currentColumn;
            }
            channelOffset[c] = new Point(x, y);
            Debug.WriteLine(channelOffset[c].ToString());
        }



        // ffmpeg
        ProcessStartInfo ffmpeg = new ProcessStartInfo();
        ffmpeg.FileName = args.ffmpegBinary;
        ffmpeg.CreateNoWindow = true;
        ffmpeg.RedirectStandardInput = true;
        ffmpeg.RedirectStandardError = true;
        ffmpeg.UseShellExecute = false;
        if (args.joinAudio)
            // join audio
            ffmpeg.Arguments = "-y " + FFmpegCommandLineJoinAudio
                                       .Replace("{img}", "-")
                                       .Replace("{framerate}", args.FPS)
                                       .Replace("{audio}", SafeFilename(args.audioFile))
                                       .Replace("{outfile}", SafeFilename(args.outputFile));
        else
            // silence
            ffmpeg.Arguments = "-y " + FFmpegCommandLineSilence.Replace("{img}", "-")
                                       .Replace("{framerate}", args.FPS)
                                       .Replace("{outfile}", SafeFilename(args.outputFile));
        Debug.WriteLine(ffmpeg.FileName + " " + ffmpeg.Arguments);
        Process ffmpegProc = null;
        System.IO.StreamReader stderr = null;
        System.IO.Stream stdin = null;
        if (args.convertVideo & !args.noFileWriting)
        {
            workerReportProg(new Progress("Starting FFmpeg."));
            workerReportProg(new Progress("Run: " + ffmpeg.FileName + " " + ffmpeg.Arguments));
            ffmpegProc = Process.Start(ffmpeg);
            workerReportProg(new Progress("Started FFmpeg."));
            stdin = ffmpegProc.StandardInput.BaseStream;
            stderr = ffmpegProc.StandardError;
            FFmpegstderr = ffmpegProc.StandardError;
        }

        // start work
        workerReportProg(new Progress("Begin rendering."));
        Bitmap overlayBmp = new Bitmap(canvasSize.Width, canvasSize.Height);
        bool overlayNeeded = false;
        for (byte c = 0; c <= channels - 1; c++)
        {
            Graphics g = Graphics.FromImage(overlayBmp);
            channelOptions channelArg = wave[c].extraArguments;
            int labelDX, labelDY;
            SizeF textSize = new SizeF(0, 0);
            if (channelArg.label != "")
                textSize = g.MeasureString(channelArg.label, channelArg.labelFont);
            labelDX = 0;
            labelDY = 0;
            switch (args.labelPostition)
            {
                case 0 // Top Left
               :
                    {
                        break;
                    }

                case 1 // Top Right
       :
                    {
                        labelDX = channelWidth - textSize.Width;
                        break;
                    }

                case 2 // Bottom Left
         :
                    {
                        labelDY = channelHeight - textSize.Height;
                        break;
                    }

                case 3 // Bottom Right
         :
                    {
                        labelDX = channelWidth - textSize.Width;
                        labelDY = channelHeight - textSize.Height;
                        break;
                    }
            }
            if (channelArg.label != "")
                overlayNeeded = true;
            g.DrawString(channelArg.label, channelArg.labelFont, new SolidBrush(channelArg.labelColor), new Rectangle(channelOffset[c].X + labelDX, channelOffset[c].Y + labelDY, channelSize.Width, channelSize.Height));
        }
        while (frames < totalFrame)
        {
            Bitmap bmp = null;
            bool createdBmp = false;
            int bmpCreateCount = 0;
            do
            {
                try
                {
                    bmpCreateCount += 1;
                    bmp = new Bitmap(canvasSize.Width, canvasSize.Height);
                    createdBmp = true;
                }
                catch (Exception ex)
                {
                    createdBmp = false;
                }
            }
            while (!createdBmp | bmpCreateCount > 3);
            if (bmpCreateCount > 3)
                continue;
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(args.backgroundColor);
            if (args.smoothLine)
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            if (args.drawMiddleLine)
            {
                for (byte c = 0; c <= channels - 1; c++)
                {
                    g.DrawLine(args.middleLinePen, channelOffset[c].X, channelOffset[c].Y + channelHeight / 2, channelOffset[c].X + channelWidth, channelOffset[c].Y + channelHeight / 2);
                    channelOptions channelArg = wave[c].extraArguments;
                    if (channelArg.XYmode)
                        g.DrawLine(args.middleLinePen, channelOffset[c].X + channelWidth / 2, channelOffset[c].Y, channelOffset[c].X + channelWidth / 2, channelOffset[c].Y + channelHeight);
                }
            }
            for (byte c = 0; c <= channels - 1; c++) // for each channel
            {
                channelOptions channelArg = wave[c].extraArguments;
                Color waveColor = channelArg.waveColor;
                long triggerOffset = 0;
                WAV currentWAV;
                if (channelArg.externalTriggerEnabled)
                    currentWAV = extTrig[c];
                else
                    currentWAV = wave[c];
                ulong sampleLocation = frames * currentWAV.sampleRate / (double)args.FPS;
                // trigger
                ulong maxScanLength = currentWAV.sampleRate * channelArg.horizontalTime * channelArg.maxScan;
                ulong firstScan = 0;
                double firstSample = currentWAV.getSample(sampleLocation, true);
                bool scanRequired = false;
                int max = -127;
                int low = 128;
                int triggerValue = channelArg.trigger;
                if (!channelArg.XYmode)
                {
                    while (firstScan < maxScanLength)
                    {
                        int sample = Math.Floor(currentWAV.getSample(sampleLocation + firstScan, true));
                        if (sample > max)
                            max = sample;
                        if (sample < low)
                            low = sample;
                        if (!currentWAV.getSample(sampleLocation + firstScan, true) == firstSample)
                            scanRequired = true;
                        firstScan += 1;
                    }
                    if (scanRequired)
                    {
                        if (channelArg.autoTriggerLevel)
                            triggerValue = (max + low) / (double)2;
                        switch (channelArg.algorithm)
                        {
                            case TriggeringAlgorithms.UseRisingEdge:
                                {
                                    triggerOffset = TriggeringAlgorithms.risingEdgeTrigger(ref currentWAV, triggerValue, sampleLocation, maxScanLength);
                                    break;
                                }

                            case TriggeringAlgorithms.UsePeakSpeedScanning:
                                {
                                    triggerOffset = TriggeringAlgorithms.peakSpeedScanning(ref currentWAV, triggerValue, sampleLocation, maxScanLength);
                                    break;
                                }

                            case TriggeringAlgorithms.UseMaxLengthScanning:
                                {
                                    switch (channelArg.scanPhase)
                                    {
                                        case 0:
                                            {
                                                triggerOffset = TriggeringAlgorithms.lengthScanning(ref currentWAV, triggerValue, sampleLocation, maxScanLength, true, false);
                                                break;
                                            }

                                        case 1:
                                            {
                                                triggerOffset = TriggeringAlgorithms.lengthScanning(ref currentWAV, triggerValue, sampleLocation, maxScanLength, false, true);
                                                break;
                                            }

                                        case 2:
                                            {
                                                triggerOffset = TriggeringAlgorithms.lengthScanning(ref currentWAV, triggerValue, sampleLocation, maxScanLength, true, true);
                                                break;
                                            }
                                    }

                                    break;
                                }

                            case TriggeringAlgorithms.UseMaxRectifiedAreaScanning:
                                {
                                    switch (channelArg.scanPhase)
                                    {
                                        case 0:
                                            {
                                                triggerOffset = TriggeringAlgorithms.maxRectifiedArea(ref currentWAV, triggerValue, sampleLocation, maxScanLength, true, false);
                                                break;
                                            }

                                        case 1:
                                            {
                                                triggerOffset = TriggeringAlgorithms.maxRectifiedArea(ref currentWAV, triggerValue, sampleLocation, maxScanLength, false, true);
                                                break;
                                            }

                                        case 2:
                                            {
                                                triggerOffset = TriggeringAlgorithms.maxRectifiedArea(ref currentWAV, triggerValue, sampleLocation, maxScanLength, true, true);
                                                break;
                                            }
                                    }

                                    break;
                                }
                        }
                    }
                }
                // pulseWidthModulatedColor
                if (channelArg.pulseWidthModulatedColor)
                {
                    double middle = (max + low) / (double)2;
                    ulong positiveLength = 0;
                    ulong totalLength = 0;
                    ulong thirdScan = 1;
                    while (currentWAV.getSample(sampleLocation + triggerOffset + thirdScan, true) < middle & thirdScan < maxScanLength)
                        thirdScan += 1;
                    while (currentWAV.getSample(sampleLocation + triggerOffset + thirdScan, true) >= middle & thirdScan < maxScanLength)
                    {
                        positiveLength += 1;
                        thirdScan += 1;
                        totalLength += 1;
                    }
                    while (currentWAV.getSample(sampleLocation + triggerOffset + thirdScan, true) <= middle & thirdScan < maxScanLength)
                    {
                        thirdScan += 1;
                        totalLength += 1;
                    }
                    int hue = 0;
                    if (totalLength != 0)
                    {
                        double pulseWidth = positiveLength / (double)totalLength;
                        if (pulseWidth > 0.5)
                            pulseWidth = 1.0 - pulseWidth;
                        pulseWidth *= 2;
                        hue = pulseWidth * 300;
                    }
                    channelArg.waveColor = HSVtoRGB(hue, 1, 1);
                }
                // draw
                if (channelArg.XYmode)
                    drawWaveXY(ref g, ref wavePen, new Rectangle(channelOffset[c], channelSize), ref wave[c], args, currentWAV.sampleRate, currentWAV.sampleRate / (double)args.FPS, sampleLocation);
                else
                    drawWave(ref g, ref wavePen, new Rectangle(channelOffset[c], channelSize), ref wave[c], args, currentWAV.sampleRate, channelArg.horizontalTime, sampleLocation + triggerOffset);
                channelArg.waveColor = waveColor; // reset color
            }

            g.Clip = new Region(); // reset region
            if (args.drawGrid)
            {
                for (int x = 1; x <= col - 1; x++)
                    g.DrawLine(args.gridPen, channelWidth * x, 0, channelWidth * x, canvasSize.Height);
                for (int y = 1; y <= maxChannelPerColumn - 1; y++)
                    g.DrawLine(args.gridPen, 0, channelHeight * y, canvasSize.Width, channelHeight * y);
            }
            if (args.drawBorder)
                g.DrawRectangle(args.borderPen, 0, 0, canvasSize.Width, canvasSize.Height);
            if (overlayNeeded)
                g.DrawImage(overlayBmp, 0, 0);
            frames += 1;
            if (!args.noFileWriting & args.convertVideo)
            {
                if (ffmpegProc.HasExited)
                {
                    workerReportProg(new Progress("FFmpeg has exited, terminating render..."));
                    workerReportProg(new Progress("FFmpeg exit code:" + ffmpegProc.ExitCode));
                    return;
                }
            }
            bool ok = false;
            int saveRetries = 0;
            do
            {
                if (args.noFileWriting)
                    break;
                try
                {
                    saveRetries += 1;
                    if (args.convertVideo)
                        bmp.Save(stdin, System.Drawing.Imaging.ImageFormat.Png);
                    else
                        bmp.Clone().Save(outputDirectory + @"\" + frames + ".png", System.Drawing.Imaging.ImageFormat.Png);
                    ok = true;
                }
                catch (InvalidOperationException ex)
                {
                    ok = false;
                }
            }
            while (!ok == true | saveRetries > 10);
            lastFrame = bmp.Clone();
            Progress prog = new Progress(frames, totalFrame);
            workerReportProg(prog);
            if (OscilloscopeBackgroundWorker.CancellationPending)
            {
                workerReportProg(new Progress("Stopping!"));
                if (args.convertVideo & !args.noFileWriting)
                    ffmpegProc.Kill();
                if (BackgroundWorkerStdErrReader.IsBusy)
                    BackgroundWorkerStdErrReader.CancelAsync();
                prog = new Progress(default(ULong), 0, 0);
                prog.canceled = true;
                workerReportProg(prog);
                canceledByUser = true;
                break;
            }
        }
        wavePen.Color = Color.White; // reset color on end
        if (args.convertVideo & !args.noFileWriting & !OscilloscopeBackgroundWorker.CancellationPending)
        {
            stdin.Flush();
            stdin.Close();
            Progress endProg = new Progress("");
            endProg.ffmpegClosed = true;
            workerReportProg(endProg);
            while (!ffmpegProc.HasExited)
            {
            }
            stderr.Close();
        }
    }
    public void workerReportProg(object userState)
    {
        progressList.Add(userState);
    }

    private void drawWave(ref Graphics g, ref Pen pen, Rectangle rect, ref WAV wave, WorkerArguments workerArg, long sampleRate, double timeScale, long offset)
    {
        channelOptions args = wave.extraArguments;
        List<Point> points = new List<Point>();
        int prevX = -1;
        for (int i = offset - sampleRate * args.horizontalTime / 2; i <= offset + sampleRate * args.horizontalTime / 2; i++) // + sampleRate * timeScale
        {
            int x = (i - (offset - sampleRate * args.horizontalTime / 2)) / sampleRate / args.horizontalTime * rect.Width + rect.X;
            if (prevX == x)
                continue;
            else
                prevX = x;
            int y;
            y = -wave.getSample(i, true) / 256 * rect.Height + rect.Y + rect.Height / (double)2;
            if (workerArg.useAnalogOscilloscopeStyle)
            {
                points.Add(new Point(x, y - workerArg.analogOscilloscopeLineWidth / 2 + workerArg.analogOscilloscopeLineWidth - 1));
                points.Add(new Point(x, y - workerArg.analogOscilloscopeLineWidth / 2 - 1));
                int nextX = (i + 1 - (offset - sampleRate * args.horizontalTime / 2)) / sampleRate / args.horizontalTime * rect.Width + rect.X;
                if (nextX - x > 1 & x >= 0)
                {
                    for (ulong dx = x; dx <= nextX; dx++)
                    {
                        points.Add(new Point(dx, y - workerArg.analogOscilloscopeLineWidth / 2 + workerArg.analogOscilloscopeLineWidth - 1));
                        points.Add(new Point(dx, y - workerArg.analogOscilloscopeLineWidth / 2 - 1));
                    }
                }
            }
            else
                points.Add(new Point(x, y));
        }
        pen.Color = args.waveColor;
        g.Clip = new Region(rect);
        g.DrawLines(pen, points.ToArray());
    }

    private void drawWaveXY(ref Graphics g, ref Pen pen, Rectangle rect, ref WAV wave, WorkerArguments workerArg, long sampleRate, double frameDuration, long offset)
    {
        wave.mixChannel = false;
        g.Clip = new Region(rect);
        channelOptions args = wave.extraArguments;
        List<Point> points = new List<Point>();
        System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
        int prevX = -1;
        Size drawingSize = rect.Size;
        pen.Color = args.waveColor;
        Pen XYpen = pen.Clone();
        if (workerArg.useAnalogOscilloscopeStyle)
            XYpen.Width = workerArg.analogOscilloscopeLineWidth;
        if (args.XYmodeAspectRatio)
        {
            if (rect.Height < rect.Width)
                drawingSize = new Size(rect.Height, rect.Height);
            else
                drawingSize = new Size(rect.Width, rect.Width);
        }
        for (int i = offset; i <= offset + frameDuration; i++)
        {
            wave.selectedChannel = 0;
            int x = wave.getSample(i, true) / 256 * drawingSize.Width + rect.Width / (double)2 + rect.X;
            wave.selectedChannel = 1;
            int y;
            y = -wave.getSample(i, true) / 256 * drawingSize.Height + rect.Height / (double)2 + rect.Y;
            if (workerArg.dottedXYmode)
            {
                path.AddLine(x, y, x + 1, y);
                path.CloseFigure();
            }
            else
                points.Add(new Point(x, y));
        }
        if (workerArg.dottedXYmode)
            g.DrawPath(XYpen, path);
        else
            g.DrawLines(XYpen, points.ToArray());
    }

    public Color HSVtoRGB(double hue, double saturation, double value)
    {
        int h = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
        double f = hue / 60 - Math.Floor(hue / 60);

        value = value * 255;
        int v = Convert.ToInt32(value);
        int p = Convert.ToInt32(value * (1 - saturation));
        int q = Convert.ToInt32(value * (1 - f * saturation));
        int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

        if (h == 0)
            return Color.FromArgb(255, v, t, p);
        else if (h == 1)
            return Color.FromArgb(255, q, v, p);
        else if (h == 2)
            return Color.FromArgb(255, p, v, t);
        else if (h == 3)
            return Color.FromArgb(255, p, q, v);
        else if (h == 4)
            return Color.FromArgb(255, t, p, v);
        else
            return Color.FromArgb(255, v, p, q);
    }

    private void OscilloscopeBackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
    {
        // taskbarProgress.ProgressState = Windows.Shell.TaskbarItemProgressState.Normal
        Progress prog = e.UserState;
        progressUpdater(prog);
    }

    private void progressUpdater(Progress prog, bool updateImage = false)
    {
        if (prog.message != "")
        {
            LogBox.AppendText(prog.message + Constants.vbCrLf);
            LogBox.Update();
            LogBox.Focus();
            LogBox.Select(LogBox.TextLength, 0);
        }
        if (prog.ffmpegClosed)
        {
            if (!isRunningMono & isProgressTaskBarSupported)
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Indeterminate);
            LabelStatus.Text = "Waiting FFmpeg to finish...";
        }
        if (updateImage)
        {
            bool ok = false;
            if (CheckBoxShowOutput.Checked & lastFrame != null)
            {
                do
                {
                    try
                    {
                        PictureBoxOutput.Image = lastFrame.Clone();
                        ok = true;
                    }
                    catch (InvalidOperationException ex)
                    {
                        ok = false;
                    }
                }
                while (!ok == true);
            }
            ulong timeLeftSecond = 0;
            if (averageFPS != 0)
                timeLeftSecond = (prog.TotalFrame - prog.CurrentFrame) / averageFPS;
            TimeSpan timeLeft = new TimeSpan(0, 0, timeLeftSecond);
            LabelStatus.Text = string.Format("{0:P1} {1}/{2}, {3:N1} FPS , Time left: {4}", prog.CurrentFrame / (double)prog.TotalFrame, prog.CurrentFrame, prog.TotalFrame, realFPS, timeLeft.ToString());
            fpsFrames += 1;
            ulong ms = Math.Abs((DateTime.Now - fpsTimer).TotalMilliseconds);
            if (ms >= 1000)
            {
                fpsTimer = DateTime.Now;
                realFPS = fpsFrames * 1000 / (double)ms;
                averageFPS = (averageFPS + realFPS) / 2;
                fpsFrames = 0;
            }
            if (!isRunningMono & isProgressTaskBarSupported)
            {
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                TaskbarManager.Instance.SetProgressValue(prog.CurrentFrame, prog.TotalFrame);
            }
        }
        else if (prog.canceled)
        {
            LabelStatus.Text = "Canceled.";
            if (prog.message != "")
            {
                LogBox.AppendText(prog.message + Constants.vbCrLf);
                LogBox.Update();
                LogBox.Focus();
                LogBox.Select(LogBox.TextLength, 0);
            }
        }
    }

    private void OscilloscopeBackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
    {
        TimeSpan elapsedTime = DateTime.Now - startTime;
        LogBox.AppendText("Total time spent: " + elapsedTime.ToString() + Constants.vbCrLf);
        CheckBoxNoFileWriting_CheckedChanged(null, null);
        foreach (Control ctrl in TabControlRenderingFiles.Controls)
            ctrl.Enabled = true;
        LabelStatus.Text = "Finished.";
        ButtonControl.Text = "Start";
        ButtonControl.Enabled = true;
        if (!isRunningMono & isProgressTaskBarSupported)
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
        if (!allFilesLoaded)
        {
            foreach (var msg in failedFiles)
                LogBox.AppendText("Failed to load " + msg.Key + ":" + msg.Value + Constants.vbCrLf);
            return;
        }
        if (BackgroundWorkerStdErrReader.IsBusy)
            BackgroundWorkerStdErrReader.CancelAsync();
        GC.Collect();
    }

    private void ButtonAdd_Click(System.Object sender, System.EventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = FileFilter;
        ofd.Multiselect = true;
        if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            foreach (var file in ofd.FileNames)
            {
                ListBoxFiles.Items.Add(file);
                optionsList.Add(new channelOptions());
            }
        }
        previewLayout();
    }

    private void ButtonRemove_Click(System.Object sender, System.EventArgs e)
    {
        if (ListBoxFiles.SelectedItems.Count == 0)
            Interaction.MsgBox("Please select at least one file!", MsgBoxStyle.Exclamation);
        while (ListBoxFiles.SelectedIndices.Count > 0)
        {
            int index = ListBoxFiles.SelectedIndices[0];
            optionsList.RemoveAt(index);
            ListBoxFiles.Items.RemoveAt(index);
        }
        previewLayout();
    }

    private void ButtonMoveUp_Click(System.Object sender, System.EventArgs e)
    {
        if (ListBoxFiles.SelectedIndices.Count > 1)
        {
            Interaction.MsgBox("Please select only one file.", MsgBoxStyle.Exclamation);
            return;
        }

        if (ListBoxFiles.SelectedIndices.Count > 0)
        {
            int index = ListBoxFiles.SelectedIndices[0];
            if (index > 0)
            {
                channelOptions currentOption = optionsList[index].Clone();
                optionsList.RemoveAt(index);
                optionsList.Insert(index - 1, currentOption);
                ListBoxFiles.Items.Insert(index - 1, ListBoxFiles.SelectedItem);
                ListBoxFiles.SetSelected(index - 1, true);
                ListBoxFiles.Items.RemoveAt(index + 1);
            }
        }
        else
            Interaction.MsgBox("Please select a file!", MsgBoxStyle.Exclamation);
        previewLayout();
    }

    private void ButtonMoveDown_Click(System.Object sender, System.EventArgs e)
    {
        if (ListBoxFiles.SelectedIndices.Count > 1)
        {
            Interaction.MsgBox("Please select only one file.", MsgBoxStyle.Exclamation);
            return;
        }
        if (ListBoxFiles.SelectedIndices.Count > 0)
        {
            int index = ListBoxFiles.SelectedIndices[0];

            if (!index == ListBoxFiles.Items.Count - 1)
            {
                channelOptions currentOption = optionsList[index].Clone();
                optionsList.RemoveAt(index);
                optionsList.Insert(index + 1, currentOption);
                ListBoxFiles.Items.Insert(index + 2, ListBoxFiles.SelectedItem);
                ListBoxFiles.SetSelected(index + 2, true);
                ListBoxFiles.Items.RemoveAt(index);
            }
        }
        else
            Interaction.MsgBox("Please select a file!", MsgBoxStyle.Exclamation);

        previewLayout();
    }

    private void ButtonOptions_Click(System.Object sender, System.EventArgs e)
    {
        if (ListBoxFiles.SelectedIndices.Count > 0)
        {
            ChannelConfigForm ccf = new ChannelConfigForm();
            if (ListBoxFiles.SelectedIndices.Count == 1)
                ccf.Options = optionsList[ListBoxFiles.SelectedIndex].Clone();
            else
            {
                channelOptions firstConfig = optionsList[ListBoxFiles.SelectedIndices[0]];
                bool isAllSame = true;
                foreach (var index in ListBoxFiles.SelectedIndices)
                {
                    if (!firstConfig.Equals(optionsList[index]))
                        isAllSame = false;
                }
                if (isAllSame)
                    ccf.Options = firstConfig.Clone();
                else
                    ccf.Options = new channelOptions();
            }
            if (ccf.ShowDialog() == DialogResult.OK)
            {
                foreach (var index in ListBoxFiles.SelectedIndices)
                    optionsList[index] = ccf.Options;
            }
            previewLayout();
        }
        else
            Interaction.MsgBox("Please select at least one file!", MsgBoxStyle.Exclamation);
    }

    private void ButtonSelAll_Click(System.Object sender, System.EventArgs e)
    {
        if (ListBoxFiles.Items.Count != 0)
        {
            bool selected = false;
            if (ListBoxFiles.SelectedIndices.Count == 0)
                selected = true;
            for (int i = 0; i <= ListBoxFiles.Items.Count - 1; i++)
                ListBoxFiles.SetSelected(i, selected);
        }
    }

    private void ListBoxFiles_DoubleClick(object sender, EventArgs e)
    {
        ButtonOptions.PerformClick();
    }

    public void previewLayout()
    {
        Bitmap bmpLayout = new Bitmap(canvasSize.Width, canvasSize.Height);
        Graphics g = Graphics.FromImage(bmpLayout);
        g.Clear(ButtonBackgroundColor.BackColor);
        if (ListBoxFiles.Items.Count != 0)
        {
            byte col = NumericUpDownColumn.Value;
            uint channels = ListBoxFiles.Items.Count;
            int maxChannelPerColumn = Math.Ceiling(channels / (double)col);
            int channelWidth = canvasSize.Width / (double)col;
            int channelHeight = canvasSize.Height / (double)maxChannelPerColumn;
            Point[] channelOffset = new Point[channels - 1 + 1];
            for (int c = 0; c <= channels - 1; c++)
            {
                channelOptions currentChannel = optionsList[c];
                int x, y, currentColumn, currentRow;
                if (channelFlowDirection == FlowDirection.LeftToRight)
                {
                    currentRow = (c - (c % col)) / (double)col;
                    y = channelHeight * currentRow;
                    x = channelWidth * (c % col);
                }
                else if (channelFlowDirection == FlowDirection.TopDown)
                {
                    currentColumn = (c - (c % maxChannelPerColumn)) / (double)maxChannelPerColumn;
                    y = channelHeight * (c % maxChannelPerColumn);
                    x = channelWidth * currentColumn;
                }
                string filename = System.IO.Path.GetFileName(ListBoxFiles.Items[c]);
                Color channelColor = currentChannel.waveColor;
                int labelDX, labelDY;
                SizeF textSize = new SizeF(0, 0);
                if (currentChannel.label != "")
                    textSize = g.MeasureString(currentChannel.label, currentChannel.labelFont);
                else
                    textSize = g.MeasureString(filename, new Font(SystemFonts.MenuFont.FontFamily, 24));
                labelDX = 0;
                labelDY = 0;
                switch (ComboBoxLabelPos.SelectedIndex)
                {
                    case 0 // Top Left
                   :
                        {
                            break;
                        }

                    case 1 // Top Right
           :
                        {
                            labelDX = channelWidth - textSize.Width;
                            break;
                        }

                    case 2 // Bottom Left
             :
                        {
                            labelDY = channelHeight - textSize.Height;
                            break;
                        }

                    case 3 // Bottom Right
             :
                        {
                            labelDX = channelWidth - textSize.Width;
                            labelDY = channelHeight - textSize.Height;
                            break;
                        }
                }
                if (currentChannel.label != "")
                    g.DrawString(currentChannel.label, currentChannel.labelFont, new SolidBrush(currentChannel.labelColor), new Rectangle(x + labelDX, y + labelDY, channelWidth, channelHeight));
                else
                    g.DrawString(filename, new Font(SystemFonts.MenuFont.FontFamily, 24), new SolidBrush(currentChannel.waveColor), new Rectangle(x + labelDX, y + labelDY, channelWidth, channelHeight));
                if (CheckBoxDrawMiddleLine.Checked)
                {
                    g.DrawLine(new Pen(ButtonMiddleLineColor.BackColor, NumericUpDownMiddleLine.Value), x, y + channelHeight / 2, x + channelWidth, y + channelHeight / 2);
                    if (currentChannel.XYmode)
                        g.DrawLine(new Pen(ButtonMiddleLineColor.BackColor, NumericUpDownMiddleLine.Value), x + channelWidth / 2, y, x + channelWidth / 2, y + channelHeight);
                }
            }
            if (CheckBoxGrid.Checked)
            {
                for (int x = 1; x <= col - 1; x++)
                    g.DrawLine(new Pen(ButtonGridColor.BackColor, NumericUpDownGrid.Value), channelWidth * x, 0, channelWidth * x, canvasSize.Height);
                for (int y = 1; y <= maxChannelPerColumn - 1; y++)
                    g.DrawLine(new Pen(ButtonGridColor.BackColor, NumericUpDownGrid.Value), 0, channelHeight * y, canvasSize.Width, channelHeight * y);
            }
            if (CheckBoxBorder.Checked)
                g.DrawRectangle(new Pen(ButtonBorderColor.BackColor, NumericUpDownBorder.Value * 2), 0, 0, canvasSize.Width, canvasSize.Height);
        }

        PictureBoxOutput.Image = bmpLayout;
    }

    public Color getTextColor(Color color)
    {
        float Y = 0.2126 * color.R / 255 + 0.7152 * color.G / 255 + 0.0722 * color.B / 255;
        // calculate luminance
        if (Y < 0.5)
            return Color.White;
        else
            return Color.Black;
    }

    private void NumericUpDownColumn_ValueChanged(System.Object sender, System.EventArgs e)
    {
        // If formStarted Then
        previewLayout();
    }

    private void ListBoxFiles_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
    {
        ButtonOptions.PerformClick();
    }

    private void CheckBoxNoFileWriting_CheckedChanged(System.Object sender, System.EventArgs e)
    {
        if (CheckBoxNoFileWriting.Checked)
            LabelPreviewMode.Visible = true;
        else
            LabelPreviewMode.Visible = false;
        TimerLabelFlashing.Enabled = CheckBoxNoFileWriting.Checked;
    }

    private void TimerLabelFlashing_Tick(System.Object sender, System.EventArgs e)
    {
        if (OscilloscopeBackgroundWorker.IsBusy)
        {
            if (CheckBoxNoFileWriting.Checked)
                LabelPreviewMode.Visible = !LabelPreviewMode.Visible;
            else
                LabelPreviewMode.Visible = false;
        }
    }

    private void ToolStripStatusLabelAbout_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
    {
        ToolStripStatusLabelAbout.BorderStyle = Border3DStyle.SunkenOuter;
    }

    private void ToolStripStatusLabelAbout_MouseLeave(object sender, System.EventArgs e)
    {
        ToolStripStatusLabelAbout.BorderStyle = Border3DStyle.RaisedOuter;
    }

    private void ToolStripStatusLabelAbout_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
    {
        ToolStripStatusLabelAbout.BorderStyle = Border3DStyle.RaisedOuter;
        My.MyProject.MyForms.AboutForm.ShowDialog();
    }

    private void LinkLabelCustomCommandLine_LinkClicked(System.Object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
    {
        if (My.MyProject.MyForms.CustomCommandLineForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            writeConfig();
    }

    private void CheckBoxGrid_CheckedChanged(System.Object sender, System.EventArgs e)
    {
        previewLayout();
    }

    private void BackgroundWorkerStdErrReader_DoWork(System.Object sender, System.ComponentModel.DoWorkEventArgs e)
    {
        while (convertVideo & !NoFileWriting & !BackgroundWorkerStdErrReader.CancellationPending)
        {
            if (FFmpegstderr != null)
            {
                if (FFmpegstderr.BaseStream != null)
                {
                    if (FFmpegstderr.BaseStream.CanRead)
                        BackgroundWorkerStdErrReader.ReportProgress(0, FFmpegstderr.ReadLine());
                }
            }
        }
    }

    private void BackgroundWorkerStdErrReader_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
    {
        string stderr = e.UserState;
        if (stderr != "" & !LogBox.IsDisposed)
            LogBox.AppendText(stderr + Constants.vbCrLf);
    }

    private void MainForm_Resize(object sender, System.EventArgs e)
    {
        if (formStarted)
        {
            this.Size = new Size(originalFormSize.Width, this.Height);
            LogBox.Size = new Size(LogBox.Width, this.Height - originalFormSize.Height + originalTextBoxLogHeight);
        }
    }

    private void ButtonFlowDirection_Click(object sender, EventArgs e)
    {
        if (channelFlowDirection == FlowDirection.LeftToRight)
            channelFlowDirection = FlowDirection.TopDown;
        else
            channelFlowDirection = FlowDirection.LeftToRight;
        ButtonFlowDirection.Invalidate();
        previewLayout();
    }

    private void ButtonFlowDirection_Invalidated(object sender, InvalidateEventArgs e)
    {
        if (channelFlowDirection == FlowDirection.LeftToRight)
            ButtonFlowDirection.Text = "Left to right";
        else
            ButtonFlowDirection.Text = "Top to down";
    }

    private void CheckBoxDrawMiddleLine_CheckedChanged(object sender, EventArgs e)
    {
        previewLayout();
    }

    private void ButtonGridColor_Click(object sender, EventArgs e)
    {
        ColorDialog cd = new ColorDialog();
        cd.Color = ButtonGridColor.BackColor;
        if (cd.ShowDialog() == DialogResult.OK)
            ButtonGridColor.BackColor = cd.Color;
        previewLayout();
    }

    private void ButtonMiddleLineColor_Click(object sender, EventArgs e)
    {
        ColorDialog cd = new ColorDialog();
        cd.Color = ButtonMiddleLineColor.BackColor;
        if (cd.ShowDialog() == DialogResult.OK)
            ButtonMiddleLineColor.BackColor = cd.Color;
        previewLayout();
    }

    private void ButtonBackgroundColor_Click(object sender, EventArgs e)
    {
        ColorDialog cd = new ColorDialog();
        cd.Color = ButtonBackgroundColor.BackColor;
        if (cd.ShowDialog() == DialogResult.OK)
            ButtonBackgroundColor.BackColor = cd.Color;
        previewLayout();
    }

    private void ButtonBorderColor_Click(object sender, EventArgs e)
    {
        ColorDialog cd = new ColorDialog();
        cd.Color = ButtonBorderColor.BackColor;
        if (cd.ShowDialog() == DialogResult.OK)
            ButtonBorderColor.BackColor = cd.Color;
        previewLayout();
    }

    private void ButtonBackgroundColor_BackColorChanged(object sender, EventArgs e)
    {
        ButtonBackgroundColor.ForeColor = getTextColor(ButtonBackgroundColor.BackColor);
    }

    private void ButtonMiddleLineColor_BackColorChanged(object sender, EventArgs e)
    {
        ButtonMiddleLineColor.ForeColor = getTextColor(ButtonMiddleLineColor.BackColor);
    }

    private void ButtonGridColor_BackColorChanged(object sender, EventArgs e)
    {
        ButtonGridColor.ForeColor = getTextColor(ButtonGridColor.BackColor);
    }

    private void ButtonBorderColor_BackColorChanged(object sender, EventArgs e)
    {
        ButtonBorderColor.ForeColor = getTextColor(ButtonBorderColor.BackColor);
    }

    private void CheckBoxBorder_CheckedChanged(object sender, EventArgs e)
    {
        previewLayout();
    }

    private void NumericUpDownBorder_ValueChanged(object sender, EventArgs e)
    {
        previewLayout();
    }

    private void NumericUpDownGrid_ValueChanged(object sender, EventArgs e)
    {
        previewLayout();
    }

    private void NumericUpDownMiddleLine_ValueChanged(object sender, EventArgs e)
    {
        previewLayout();
    }

    private void ComboBoxLabelPos_SelectedIndexChanged(object sender, EventArgs e)
    {
        previewLayout();
    }

    private void TimerStatusUpdater_Tick(object sender, EventArgs e)
    {
        if (OscilloscopeBackgroundWorker.IsBusy)
        {
            if (OscilloscopeBackgroundWorker.IsBusy)
            {
                if (progressList.Count > 0)
                {
                    fpsFrames += progressList.Count - 1;
                    while (progressList.Count > 1)
                    {
                        progressUpdater(progressList.First());
                        progressList.RemoveAt(0);
                    }
                    progressUpdater(progressList.Last(), true);
                    progressList.RemoveAt(0);
                }
            }
        }
        else
            TimerStatusUpdater.Stop();
    }
}

