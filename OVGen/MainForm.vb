Imports Microsoft.WindowsAPICodePack.Taskbar
Public Class MainForm
    Property isRunningMono As Boolean
    Dim configFileLocation As String = Environment.CurrentDirectory & "\OVG.ini"
    '== FPS counter
    Dim fpsTimer As Date
    Dim fpsFrames As ULong
    Dim realFPS As Double
    Dim averageFPS As Double
    Dim startTime As DateTime
    Dim channelFlowDirection As FlowDirection = FlowDirection.LeftToRight
    '===For Worker
    Public optionsList As New List(Of channelOptions)
    Dim canvasSize As New Size(1280, 720)
    Public wavePen As New Pen(Color.White, 2)
    Dim masterAudioFile As String = ""
    Dim outputLocation As String = ""
    Dim outputDirectory As String = ""
    Dim NoFileWriting As Boolean = False
    Dim allFilesLoaded As Boolean = False
    Dim failedFiles As New Dictionary(Of String, String)
    Dim lastFrame As Bitmap
    Dim progressList As New List(Of Progress)
    '===FFmpeg
    Dim convertVideo As Boolean = False
    Dim canceledByUser As Boolean = False
    Dim FFmpegExitCode As Integer = 0
    Public Const DefaultFFmpegCommandLineJoinAudio As String = "-f image2pipe -framerate {framerate} -c:v png -i {img} -i {audio} -c:a aac -b:a 384k -c:v libx264 -crf 18 -bf 2 -flags +cgop -pix_fmt yuv420p -movflags faststart {outfile}"
    Public Const DefaultFFmpegCommandLineSilence As String = "-f image2pipe -framerate {framerate} -c:v png -i {img} -c:v libx264 -crf 18 -bf 2 -flags +cgop -pix_fmt yuv420p -movflags faststart {outfile}"
    Public FFmpegCommandLineJoinAudio As String = DefaultFFmpegCommandLineJoinAudio
    Public FFmpegCommandLineSilence As String = DefaultFFmpegCommandLineSilence
    Public FFmpegstderr As IO.StreamReader
    '===For file operation
    Const FileFilter As String = "WAVE File(*.wav)|*.wav"
    Public currentChannelToBeSet As String = ""
    Dim ffmpegPath As String = ""

    '===GUI..etc.
    Dim formStarted As Boolean = False
    Dim originalFormSize As Size
    Dim originalTextBoxLogHeight As Integer
    Dim isProgressTaskBarSupported As Boolean = True

    Private Sub MainForm_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If Not formStarted Then
            originalFormSize = Me.Size
            Me.MinimumSize = Me.Size
        End If
        formStarted = True
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If OscilloscopeBackgroundWorker.IsBusy Then
            Dim r As DialogResult = MsgBox("Do you want to stop the worker?", MsgBoxStyle.Question + MsgBoxStyle.YesNo)
            If r = Windows.Forms.DialogResult.Yes Then
                ButtonControl_Click(Nothing, Nothing)
            Else
                e.Cancel = True
                Exit Sub
            End If
        End If
    End Sub

    Private Sub MainForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        writeConfig()
    End Sub

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.DoubleBuffer, True)
        SetStyle(ControlStyles.UserPaint, True)
        loadConfig()
        previewLayout()
        outputDirectory = IO.Path.GetTempPath() & "OVG-" & randStr(5)
        If Not CheckBoxVideo.Checked Then
            TextBoxOutputLocation.Text = outputDirectory
        End If
        LabelStatus.Text = ""
        CheckBoxNoFileWriting_CheckedChanged(Nothing, Nothing)
        originalTextBoxLogHeight = LogBox.Height
        Me.Text &= " " & Application.ProductVersion
        isRunningMono = Type.GetType("Mono.Runtime") IsNot Nothing
        If isRunningMono Then
            LogBox.AppendText("Detected running OVGen under Mono." & vbNewLine)
        End If
        isProgressTaskBarSupported = Environment.OSVersion.Version.Major >= 6 And Environment.OSVersion.Version.Minor >= 1
        'Windows 7 or up
    End Sub

    Function randStr(ByVal len As ULong) As String
        Dim rand As New Random
        Dim map As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
        randStr = ""
        For i As Integer = 1 To len
            randStr &= map.Substring(rand.Next(map.Length - 1), 1)
        Next
    End Function

    Function SafeFilename(ByVal filename As String) As String
        SafeFilename = filename
        If filename.Contains(" ") Then SafeFilename = """" & filename & """"
    End Function

    Private Sub writeConfig()
        Dim conf As New OVGconfig
        conf.General.SmoothLine = CheckBoxSmooth.Checked
        conf.General.DrawMiddleLine = CheckBoxDrawMiddleLine.Checked
        conf.General.BackgroundColor = New ColorSerializable(ButtonBackgroundColor.BackColor)
        conf.General.MiddleLineColor = New ColorSerializable(ButtonMiddleLineColor.BackColor)
        conf.General.MiddleLineWidth = NumericUpDownMiddleLine.Value
        conf.General.Framerate = NumericUpDownFrameRate.Value
        conf.General.LineWidth = NumericUpDownLineWidth.Value
        conf.General.ConvertVideo = CheckBoxVideo.Checked
        conf.General.CRTStyledRender = CheckBoxCRT.Checked
        conf.General.DottedXYmode = CheckBoxDottedXYmode.Checked
        conf.General.DrawGrid = CheckBoxGrid.Checked
        conf.General.GridColor = New ColorSerializable(ButtonGridColor.BackColor)
        conf.General.GridWidth = NumericUpDownGrid.Value
        conf.General.DrawBorder = CheckBoxBorder.Checked
        conf.General.BorderColor = New ColorSerializable(ButtonBorderColor.BackColor)
        conf.General.BorderWidth = NumericUpDownBorder.Value
        conf.General.CanvasSize = ComboBoxCanvasSize.Text
        conf.General.FlowDirection = channelFlowDirection
        conf.General.LabelPosition = ComboBoxLabelPos.SelectedIndex
        conf.FFmpeg.BinaryLocation = ffmpegPath.Trim()
        conf.FFmpeg.JoinAudioCommandLine = FFmpegCommandLineJoinAudio.Trim()
        If FFmpegCommandLineJoinAudio = DefaultFFmpegCommandLineJoinAudio Then conf.FFmpeg.JoinAudioCommandLine = ""
        conf.FFmpeg.SilenceCommandLine = FFmpegCommandLineSilence.Trim()
        If FFmpegCommandLineSilence = DefaultFFmpegCommandLineSilence Then conf.FFmpeg.SilenceCommandLine = ""
        Dim xml As New Xml.Serialization.XmlSerializer(conf.GetType())
        Try
            Dim fs As New IO.FileStream("config.xml", IO.FileMode.Create)
            xml.Serialize(fs, conf)
            fs.Close()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub loadConfig()
        If My.Computer.FileSystem.FileExists("config.xml") Then
            Dim conf As New OVGconfig
            Dim xml As New Xml.Serialization.XmlSerializer(conf.GetType())
            Try
                Dim fs As New IO.FileStream("config.xml", IO.FileMode.Open)
                conf = xml.Deserialize(fs)
                fs.Close()
                ffmpegPath = conf.FFmpeg.BinaryLocation
                FFmpegCommandLineJoinAudio = conf.FFmpeg.JoinAudioCommandLine
                If FFmpegCommandLineJoinAudio = "" Then FFmpegCommandLineJoinAudio = DefaultFFmpegCommandLineJoinAudio
                FFmpegCommandLineSilence = conf.FFmpeg.SilenceCommandLine
                If FFmpegCommandLineSilence = "" Then FFmpegCommandLineSilence = DefaultFFmpegCommandLineSilence
                CheckBoxSmooth.Checked = conf.General.SmoothLine
                ButtonBackgroundColor.BackColor = conf.General.BackgroundColor.GetColor()
                CheckBoxDrawMiddleLine.Checked = conf.General.DrawMiddleLine
                ButtonMiddleLineColor.BackColor = conf.General.MiddleLineColor.GetColor()
                NumericUpDownMiddleLine.Value = conf.General.MiddleLineWidth
                NumericUpDownFrameRate.Value = conf.General.Framerate
                NumericUpDownLineWidth.Value = conf.General.LineWidth
                CheckBoxVideo.Checked = conf.General.ConvertVideo
                CheckBoxCRT.Checked = conf.General.CRTStyledRender
                CheckBoxDottedXYmode.Checked = conf.General.DottedXYmode
                CheckBoxGrid.Checked = conf.General.DrawGrid
                ButtonGridColor.BackColor = conf.General.GridColor.GetColor()
                CheckBoxBorder.Checked = conf.General.DrawBorder
                ButtonBorderColor.BackColor = conf.General.BorderColor.GetColor()
                NumericUpDownBorder.Value = conf.General.BorderWidth
                NumericUpDownGrid.Value = conf.General.GridWidth
                ComboBoxCanvasSize.Text = conf.General.CanvasSize
                channelFlowDirection = conf.General.FlowDirection
                ComboBoxLabelPos.SelectedIndex = conf.General.LabelPosition
                ButtonFlowDirection.Invalidate()
            Catch ex As Exception
                LogBox.AppendText("Error occured while loading config:" & ex.Message & vbCrLf)
                Exit Sub
            End Try
        End If
    End Sub

    Private Sub ButtonControl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonControl.Click

        If Not OscilloscopeBackgroundWorker.IsBusy Then
            LogBox.Clear()
            Dim arg As New WorkerArguments
            'check cavnas size
            Try

                Dim userInput As String() = ComboBoxCanvasSize.Text.Split({"x", " "}, StringSplitOptions.RemoveEmptyEntries)
                If userInput.Length = 2 Then
                    canvasSize = New Size(userInput(0), userInput(1))
                Else
                    MsgBox("Invalid canvas size!", MsgBoxStyle.Critical)
                    Exit Sub
                End If
                If canvasSize.Width < 1 Or canvasSize.Height < 1 Then
                    MsgBox("Invalid canvas size!", MsgBoxStyle.Critical)
                    Exit Sub
                End If
            Catch ex As Exception
                MsgBox("Invalid canvas size!" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
                Exit Sub
            End Try
            arg.backgroundColor = ButtonBackgroundColor.BackColor
            arg.outputFile = outputLocation
            outputLocation = TextBoxOutputLocation.Text
            outputDirectory = ""
            arg.drawGrid = CheckBoxGrid.Checked
            arg.gridPen = New Pen(ButtonGridColor.BackColor, NumericUpDownGrid.Value)
            arg.drawBorder = CheckBoxBorder.Checked
            arg.borderPen = New Pen(ButtonBorderColor.BackColor, NumericUpDownBorder.Value * 2)
            arg.useAnalogOscilloscopeStyle = CheckBoxCRT.Checked
            arg.dottedXYmode = CheckBoxDottedXYmode.Checked
            If arg.useAnalogOscilloscopeStyle Then
                arg.analogOscilloscopeLineWidth = NumericUpDownLineWidth.Value
                wavePen.Width = 1
            Else
                wavePen.Width = NumericUpDownLineWidth.Value
            End If
            If ListBoxFiles.Items.Count = 0 Then
                MsgBox("Please add at least one file!", MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            Debug.WriteLine(String.Format("Output directory:{0}", outputDirectory))
            arg.FPS = NumericUpDownFrameRate.Value
            arg.smoothLine = CheckBoxSmooth.Checked
            arg.FPS = NumericUpDownFrameRate.Value
            arg.noFileWriting = CheckBoxNoFileWriting.Checked
            NoFileWriting = arg.noFileWriting
            arg.convertVideo = CheckBoxVideo.Checked
            convertVideo = arg.convertVideo
            If arg.convertVideo And Not arg.noFileWriting Then
                Debug.WriteLine(Strings.Right(outputLocation, 4).ToLower)
                If Strings.Right(outputLocation, 4).ToLower <> ".mp4" Then
                    MsgBox("Please set a proper filename!", MsgBoxStyle.Critical)
                    Exit Sub
                End If
                Try
                    Using f = New IO.FileStream(outputLocation, IO.FileMode.OpenOrCreate)
                        f.WriteByte(0)
                        f.Flush()
                    End Using
                Catch ex As Exception
                    MsgBox(ex.Message, MsgBoxStyle.Critical, "Error on creating empty file.")
                    Exit Sub
                End Try
                arg.outputDirectory = IO.Path.GetTempPath() & "OVG_" & randStr(5) & "\"
                outputDirectory = IO.Path.GetTempPath() & "OVG_" & randStr(5) & "\"
                arg.outputFile = outputLocation
                If Not BackgroundWorkerStdErrReader.IsBusy Then
                    BackgroundWorkerStdErrReader.RunWorkerAsync()
                End If
            Else
                arg.outputDirectory = TextBoxOutputLocation.Text
                outputDirectory = outputLocation
            End If
            If outputDirectory = "" And Not arg.noFileWriting And Not arg.convertVideo Then
                MsgBox("Please select a directory!", MsgBoxStyle.Exclamation)
                Exit Sub
            End If
            arg.drawMiddleLine = CheckBoxDrawMiddleLine.Checked
            arg.middleLinePen = New Pen(ButtonMiddleLineColor.BackColor, NumericUpDownMiddleLine.Value)
            arg.columns = NumericUpDownColumn.Value
            arg.flowDirection = channelFlowDirection
            arg.labelPostition = ComboBoxLabelPos.SelectedIndex
            Dim fileArray(ListBoxFiles.Items.Count - 1) As String
            For i As Integer = 0 To fileArray.Length - 1
                fileArray(i) = ListBoxFiles.Items(i)
            Next
            arg.files = fileArray
            If IO.File.Exists(masterAudioFile) Then
                arg.joinAudio = True
                arg.audioFile = masterAudioFile
            Else
                arg.joinAudio = False
            End If
            arg.ffmpegBinary = ffmpegPath
            LabelStatus.Text = "Start."
            OscilloscopeBackgroundWorker.RunWorkerAsync(arg)
            TimerStatusUpdater.Start()
            For Each ctrl As Control In TabControlRenderingFiles.Controls
                ctrl.Enabled = False
            Next
            ButtonControl.Text = "Cancel"
            ButtonControl.Update()
        Else
            OscilloscopeBackgroundWorker.CancelAsync()
            ButtonControl.Text = "Start"
        End If


    End Sub

    Private Sub CheckBoxVideo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxVideo.CheckedChanged
        If CheckBoxVideo.Checked Then
            If Not My.Computer.FileSystem.FileExists(ffmpegPath) And ffmpegPath <> "ffmpeg" Then
                Dim waitForm As New Form()
                waitForm.ShowInTaskbar = False
                waitForm.FormBorderStyle = FormBorderStyle.None
                waitForm.ControlBox = False
                waitForm.Size = New Size(100, 50)
                waitForm.Cursor = Cursors.WaitCursor
                waitForm.StartPosition = FormStartPosition.Manual
                waitForm.Location = New Point(Me.Location.X + Me.Size.Width / 2 - waitForm.Width / 2,
                                              Me.Location.Y + Me.Size.Height / 2 - waitForm.Height / 2)
                waitForm.TopMost = True
                Dim waitLabel As New Label()
                waitLabel.Text = "Please wait..."
                Dim g As Graphics = waitForm.CreateGraphics()
                Dim labelSize As SizeF = g.MeasureString(waitLabel.Text, waitLabel.Font)
                waitLabel.Location = New Point(waitForm.Width / 2 - labelSize.Width / 2, waitForm.Height / 2 - labelSize.Height / 2)
                waitForm.Controls.Add(waitLabel)
                Dim procInfo As New ProcessStartInfo("ffmpeg", "-version")
                procInfo.UseShellExecute = False
                procInfo.CreateNoWindow = True
                Dim FFmpegExist As Boolean = True
                Dim proc As Process = Nothing
                waitForm.Show()
                waitLabel.Refresh()
                ControlPaint.DrawBorder3D(g, New Rectangle(New Point, waitForm.Size), Border3DStyle.Raised)
                Try
                    proc = Process.Start(procInfo)
                Catch ex As Exception
                    FFmpegExist = False
                End Try
                If proc IsNot Nothing Then
                    Dim procStopWatch As New Stopwatch
                    procStopWatch.Start()
                    While Not proc.HasExited
                        waitLabel.Refresh()
                        ControlPaint.DrawBorder3D(g, New Rectangle(New Point, waitForm.Size), Border3DStyle.Raised)
                        Application.DoEvents()
                        If procStopWatch.Elapsed.TotalSeconds > 10 Then 'run over 10 second
                            proc.Kill()
                            Exit Sub
                        End If
                    End While
                    procStopWatch.Stop()
                    If proc.ExitCode <> 0 Then FFmpegExist = False
                End If
                waitForm.Close()
                Dim NeedToOpenDialog As Boolean = True
                If FFmpegExist Then
                    Dim useSystem As MsgBoxResult = MsgBox("We detected a copy of FFmpeg is installed in this system, do you want to use it?", MsgBoxStyle.YesNo + MsgBoxStyle.Question)
                    If useSystem = MsgBoxResult.Yes Then NeedToOpenDialog = False
                Else
                    MsgBox("FFmpeg binary is not exist or location not set!, select one.", MsgBoxStyle.Exclamation)
                End If
                If NeedToOpenDialog Then
                    Dim ofd As New OpenFileDialog
                    If isRunningMono Then
                        ofd.Filter = "FFmpeg binary|ffmpeg|All Files|*.*"
                    Else
                        ofd.Filter = "FFmpeg binary|ffmpeg.exe|All Files|*.*"
                    End If
                    If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        ffmpegPath = ofd.FileName
                        writeConfig()
                    Else
                        CheckBoxVideo.Checked = False
                        Exit Sub
                    End If
                Else
                    ffmpegPath = "ffmpeg"
                    writeConfig()
                End If
            End If
            ButtonAudio.Enabled = True
            LabelOutputLocation.Text = "Output video:"
        Else
            ButtonAudio.Enabled = False
            LabelOutputLocation.Text = "Output folder:"
            masterAudioFile = ""
            ButtonAudio.Text = "Master Audio"
        End If
    End Sub

    Private Sub ButtonAudio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAudio.Click
        Dim ofd As New OpenFileDialog
        ofd.Filter = "WAVE File(*.wav)|*.wav"
        If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            masterAudioFile = ofd.FileName
            ButtonAudio.Text = ofd.SafeFileName
        End If
    End Sub

    Private Sub ButtonSetOutputFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSetOutputFolder.Click
        If CheckBoxVideo.Checked Then
            Dim sfd As New SaveFileDialog
            sfd.Filter = "MP4 File(*.mp4)|*.mp4"
            If sfd.ShowDialog() = Windows.Forms.DialogResult.OK Then
                TextBoxOutputLocation.Text = sfd.FileName
            End If
        Else
            Dim fbd As New FolderBrowserDialog
            If fbd.ShowDialog() = Windows.Forms.DialogResult.OK Then
                TextBoxOutputLocation.Text = fbd.SelectedPath
            End If
        End If
    End Sub

    Private Sub OscilloscopeBackgroundWorker_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles OscilloscopeBackgroundWorker.DoWork
        workerReportProg(New Progress("Start."))
        startTime = Now
        Dim args As WorkerArguments = e.Argument
        canceledByUser = False
        If Not My.Computer.FileSystem.DirectoryExists(outputDirectory) And Not args.noFileWriting And Not args.convertVideo Then
            My.Computer.FileSystem.CreateDirectory(outputDirectory)
            Debug.WriteLine(outputDirectory)
        End If

        Dim wave(args.files.Length - 1) As WAV
        Dim extTrig As New Dictionary(Of Byte, WAV)
        Dim data As New List(Of Byte())
        Dim col As Byte = args.columns
        Dim sampleLength As UInteger = 0
        Dim frames As ULong = 0
        Dim totalFrame As ULong = 0
        allFilesLoaded = True
        failedFiles.Clear()
        For z As Byte = 0 To args.files.Length - 1
            workerReportProg(New Progress("Loading wav file: " & args.files(z)))
            Try
                wave(z) = New WAV(args.files(z))
            Catch ex As Exception
                allFilesLoaded = False
                failedFiles.Add(args.files(z), ex.Message)
                Continue For
            End Try
            Dim extraArg As channelOptions = optionsList(z)
            wave(z).extraArguments = extraArg
            wave(z).amplify = extraArg.amplify
            wave(z).mixChannel = extraArg.mixChannel
            wave(z).selectedChannel = extraArg.selectedChannel
            If wave(z).sampleLength > sampleLength Then
                sampleLength = wave(z).sampleLength
                totalFrame = sampleLength \ (wave(z).sampleRate \ args.FPS) + 1
            End If
            If extraArg.externalTriggerEnabled Then
                workerReportProg(New Progress("  Loading external trigger: " & extraArg.externalTriggerFile))
                Try
                    extTrig.Add(z, New WAV(extraArg.externalTriggerFile))
                    extTrig(z).extraArguments = extraArg
                    extTrig(z).amplify = extraArg.amplify
                    extTrig(z).mixChannel = extraArg.mixChannel
                    extTrig(z).selectedChannel = extraArg.selectedChannel
                Catch ex As Exception
                    allFilesLoaded = False
                    failedFiles.Add(extraArg.externalTriggerFile, ex.Message)
                    Continue For
                End Try
            End If
        Next
        If My.Computer.FileSystem.FileExists(masterAudioFile) Then 'use master audio's sample length
            Try
                Dim master As New WAV(masterAudioFile, True)
                sampleLength = master.sampleLength
                workerReportProg(New Progress("Using length of master audio."))
                totalFrame = sampleLength \ (master.sampleRate \ args.FPS) + 1
            Catch ex As Exception
                workerReportProg(New Progress("Failed to parse master audio: " & ex.Message))
            End Try
        End If
        If Not allFilesLoaded Then
            wave = Nothing
            workerReportProg(New Progress("Failed to load file(s)."))
            Exit Sub
        End If
        workerReportProg(New Progress("All file loaded."))
        Debug.WriteLine("Done loading waves.")
        fpsTimer = Now
        Debug.WriteLine(sampleLength)
        Dim bitDepth As Integer = wave(0).bitDepth
        Debug.WriteLine(bitDepth)
        Dim channels As Byte = args.files.Length
        Dim maxChannelPerColumn As Integer = Math.Ceiling(channels / col)
        Dim channelWidth As Integer = canvasSize.Width / col
        Dim channelHeight As Integer = canvasSize.Height / maxChannelPerColumn
        Dim channelSize As New Size(channelWidth, channelHeight)
        Dim channelOffset(channels - 1) As Point
        For c As Integer = 0 To channels - 1
            Dim x, y, currentColumn, currentRow As Integer
            If args.flowDirection = FlowDirection.LeftToRight Then
                currentRow = (c - (c Mod col)) / col
                y = channelHeight * currentRow
                x = channelWidth * (c Mod col)
            ElseIf args.flowDirection = FlowDirection.TopDown Then
                currentColumn = (c - (c Mod maxChannelPerColumn)) / maxChannelPerColumn
                y = channelHeight * (c Mod maxChannelPerColumn)
                x = channelWidth * currentColumn
            End If
            channelOffset(c) = New Point(x, y)
            Debug.WriteLine(channelOffset(c).ToString())
        Next



        'ffmpeg
        Dim ffmpeg As New ProcessStartInfo
        ffmpeg.FileName = args.ffmpegBinary
        ffmpeg.CreateNoWindow = True
        ffmpeg.RedirectStandardInput = True
        ffmpeg.RedirectStandardError = True
        ffmpeg.UseShellExecute = False
        If args.joinAudio Then
            'join audio
            ffmpeg.Arguments = "-y " & FFmpegCommandLineJoinAudio _
                                       .Replace("{img}", "-") _
                                       .Replace("{framerate}", args.FPS) _
                                       .Replace("{audio}", SafeFilename(args.audioFile)) _
                                       .Replace("{outfile}", SafeFilename(args.outputFile))
        Else
            'silence
            ffmpeg.Arguments = "-y " & FFmpegCommandLineSilence.Replace("{img}", "-") _
                                       .Replace("{framerate}", args.FPS) _
                                       .Replace("{outfile}", SafeFilename(args.outputFile))
        End If
        Debug.WriteLine(ffmpeg.FileName & " " & ffmpeg.Arguments)
        Dim ffmpegProc As Process = Nothing
        Dim stderr As IO.StreamReader = Nothing
        Dim stdin As IO.Stream = Nothing
        If args.convertVideo And Not args.noFileWriting Then
            workerReportProg(New Progress("Starting FFmpeg."))
            workerReportProg(New Progress("Run: " & ffmpeg.FileName & " " & ffmpeg.Arguments))
            ffmpegProc = Process.Start(ffmpeg)
            workerReportProg(New Progress("Started FFmpeg."))
            stdin = ffmpegProc.StandardInput.BaseStream
            stderr = ffmpegProc.StandardError
            FFmpegstderr = ffmpegProc.StandardError
        End If

        'start work
        workerReportProg(New Progress("Begin rendering."))
#Region "draw overlay bmp"
        Dim overlayBmp As New Bitmap(canvasSize.Width, canvasSize.Height)
        Dim overlayNeeded As Boolean = False
        For c As Byte = 0 To channels - 1
            Dim g As Graphics = Graphics.FromImage(overlayBmp)
            Dim channelArg As channelOptions = wave(c).extraArguments
            Dim labelDX, labelDY As Integer
            Dim textSize As SizeF = New SizeF(0, 0)
            If channelArg.label <> "" Then
                textSize = g.MeasureString(channelArg.label, channelArg.labelFont)
            End If
            labelDX = 0
            labelDY = 0
            Select Case args.labelPostition
                Case 0 'Top Left

                Case 1 'Top Right
                    labelDX = channelWidth - textSize.Width
                Case 2 'Bottom Left
                    labelDY = channelHeight - textSize.Height
                Case 3 'Bottom Right
                    labelDX = channelWidth - textSize.Width
                    labelDY = channelHeight - textSize.Height
            End Select
            If channelArg.label <> "" Then overlayNeeded = True
            g.DrawString(channelArg.label, channelArg.labelFont, New SolidBrush(channelArg.labelColor), New Rectangle(channelOffset(c).X + labelDX, channelOffset(c).Y + labelDY, channelSize.Width, channelSize.Height))
        Next
#End Region
        While frames < totalFrame
#Region "createBmp"
            Dim bmp As Bitmap = Nothing
            Dim createdBmp As Boolean = False
            Dim bmpCreateCount As Integer = 0
            Do
                Try
                    bmpCreateCount += 1
                    bmp = New Bitmap(canvasSize.Width, canvasSize.Height)
                    createdBmp = True
                Catch ex As Exception
                    createdBmp = False
                End Try
            Loop Until createdBmp Or bmpCreateCount > 3
            If bmpCreateCount > 3 Then Continue While
#End Region
#Region "prepare graphics object"
            Dim g As Graphics = Graphics.FromImage(bmp)
            g.Clear(args.backgroundColor)
            If args.smoothLine Then g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
#End Region
#Region "draw middle line"
            If args.drawMiddleLine Then
                For c As Byte = 0 To channels - 1
                    g.DrawLine(args.middleLinePen, channelOffset(c).X, channelOffset(c).Y + channelHeight \ 2,
                                              channelOffset(c).X + channelWidth, channelOffset(c).Y + channelHeight \ 2)
                    Dim channelArg As channelOptions = wave(c).extraArguments
                    If channelArg.XYmode Then
                        g.DrawLine(args.middleLinePen, channelOffset(c).X + channelWidth \ 2, channelOffset(c).Y,
                                              channelOffset(c).X + channelWidth \ 2, channelOffset(c).Y + channelHeight)
                    End If
                Next
            End If
#End Region
            For c As Byte = 0 To channels - 1 'for each channel
                Dim channelArg As channelOptions = wave(c).extraArguments
                Dim waveColor As Color = channelArg.waveColor
                Dim triggerOffset As Long = 0
                Dim currentWAV As WAV
                If channelArg.externalTriggerEnabled Then
                    currentWAV = extTrig(c)
                Else
                    currentWAV = wave(c)
                End If
                Dim sampleLocation As ULong = frames * currentWAV.sampleRate / args.FPS
                'trigger
#Region "trigger"
                Dim maxScanLength As ULong = currentWAV.sampleRate * channelArg.horizontalTime * channelArg.maxScan
                Dim firstScan As ULong = 0
                Dim firstSample As Double = currentWAV.getSample(sampleLocation, True)
                Dim scanRequired As Boolean = False
                Dim max As Integer = -127
                Dim low As Integer = 128
                Dim triggerValue As Integer = channelArg.trigger
                If Not channelArg.XYmode Then
                    While firstScan < maxScanLength
                        Dim sample As Integer = Math.Floor(currentWAV.getSample(sampleLocation + firstScan, True))
                        If sample > max Then max = sample
                        If sample < low Then low = sample
                        If Not currentWAV.getSample(sampleLocation + firstScan, True) = firstSample Then
                            scanRequired = True
                        End If
                        firstScan += 1
                    End While
                    If scanRequired Then
                        If channelArg.autoTriggerLevel Then
                            triggerValue = (max + low) / 2
                        End If
                        Select Case channelArg.algorithm
                            Case TriggeringAlgorithms.UseRisingEdge
                                triggerOffset = TriggeringAlgorithms.risingEdgeTrigger(currentWAV, triggerValue, sampleLocation, maxScanLength)
                            Case TriggeringAlgorithms.UsePeakSpeedScanning
                                triggerOffset = TriggeringAlgorithms.peakSpeedScanning(currentWAV, triggerValue, sampleLocation, maxScanLength)
                            Case TriggeringAlgorithms.UseMaxLengthScanning
                                Select Case channelArg.scanPhase
                                    Case 0
                                        triggerOffset = TriggeringAlgorithms.lengthScanning(currentWAV, triggerValue, sampleLocation, maxScanLength, True, False)
                                    Case 1
                                        triggerOffset = TriggeringAlgorithms.lengthScanning(currentWAV, triggerValue, sampleLocation, maxScanLength, False, True)
                                    Case 2
                                        triggerOffset = TriggeringAlgorithms.lengthScanning(currentWAV, triggerValue, sampleLocation, maxScanLength, True, True)
                                End Select
                            Case TriggeringAlgorithms.UseMaxRectifiedAreaScanning
                                Select Case channelArg.scanPhase
                                    Case 0
                                        triggerOffset = TriggeringAlgorithms.maxRectifiedArea(currentWAV, triggerValue, sampleLocation, maxScanLength, True, False)
                                    Case 1
                                        triggerOffset = TriggeringAlgorithms.maxRectifiedArea(currentWAV, triggerValue, sampleLocation, maxScanLength, False, True)
                                    Case 2
                                        triggerOffset = TriggeringAlgorithms.maxRectifiedArea(currentWAV, triggerValue, sampleLocation, maxScanLength, True, True)
                                End Select
                        End Select
                    End If
                End If
#End Region
                'pulseWidthModulatedColor
#Region "PulseWidthModulatedColor"
                If channelArg.pulseWidthModulatedColor Then
                    Dim middle As Double = (max + low) / 2
                    Dim positiveLength As ULong = 0
                    Dim totalLength As ULong = 0
                    Dim thirdScan As ULong = 1
                    While currentWAV.getSample(sampleLocation + triggerOffset + thirdScan, True) < middle And thirdScan < maxScanLength
                        thirdScan += 1
                    End While
                    While currentWAV.getSample(sampleLocation + triggerOffset + thirdScan, True) >= middle And thirdScan < maxScanLength
                        positiveLength += 1
                        thirdScan += 1
                        totalLength += 1
                    End While
                    While currentWAV.getSample(sampleLocation + triggerOffset + thirdScan, True) <= middle And thirdScan < maxScanLength
                        thirdScan += 1
                        totalLength += 1
                    End While
                    Dim hue As Integer = 0
                    If totalLength <> 0 Then
                        Dim pulseWidth As Double = positiveLength / totalLength
                        If pulseWidth > 0.5 Then pulseWidth = 1.0 - pulseWidth
                        pulseWidth *= 2
                        hue = pulseWidth * 300
                    End If
                    channelArg.waveColor = HSVtoRGB(hue, 1, 1)
                End If
#End Region
                'draw
#Region "draw wave or XY"
                If channelArg.XYmode Then
                    drawWaveXY(g, wavePen, New Rectangle(channelOffset(c), channelSize),
                         wave(c), args, currentWAV.sampleRate, currentWAV.sampleRate / args.FPS, sampleLocation)
                Else
                    drawWave(g, wavePen, New Rectangle(channelOffset(c), channelSize),
         wave(c), args, currentWAV.sampleRate, channelArg.horizontalTime, sampleLocation + triggerOffset)
                End If
                channelArg.waveColor = waveColor 'reset color
#End Region
            Next

            g.Clip = New Region() 'reset region
            If args.drawGrid Then 'draw grid
                For x As Integer = 1 To col - 1
                    g.DrawLine(args.gridPen, channelWidth * x, 0, channelWidth * x, canvasSize.Height)
                Next
                For y As Integer = 1 To maxChannelPerColumn - 1
                    g.DrawLine(args.gridPen, 0, channelHeight * y, canvasSize.Width, channelHeight * y)
                Next
            End If
            If args.drawBorder Then
                g.DrawRectangle(args.borderPen, 0, 0, canvasSize.Width, canvasSize.Height)
            End If
            If overlayNeeded Then
                g.DrawImage(overlayBmp, 0, 0)
            End If
            frames += 1
            If Not args.noFileWriting And args.convertVideo Then
                If ffmpegProc.HasExited Then
                    workerReportProg(New Progress("FFmpeg has exited, terminating render..."))
                    workerReportProg(New Progress("FFmpeg exit code:" & ffmpegProc.ExitCode))
                    Exit Sub
                End If
            End If
            Dim ok As Boolean = False
            Dim saveRetries As Integer = 0
            Do
                If args.noFileWriting Then Exit Do
                Try
                    saveRetries += 1
                    If args.convertVideo Then
                        bmp.Save(stdin, Imaging.ImageFormat.Png)
                    Else
                        bmp.Clone().Save(outputDirectory & "\" & frames & ".png", Imaging.ImageFormat.Png)
                    End If
                    ok = True
                Catch ex As InvalidOperationException
                    ok = False
                End Try
            Loop Until ok = True Or saveRetries > 10
            lastFrame = bmp.Clone()
            Dim prog As New Progress(frames, totalFrame)
            workerReportProg(prog)
            If OscilloscopeBackgroundWorker.CancellationPending Then
                workerReportProg(New Progress("Stopping!"))
                If args.convertVideo And Not args.noFileWriting Then
                    ffmpegProc.Kill()
                End If
                If BackgroundWorkerStdErrReader.IsBusy Then
                    BackgroundWorkerStdErrReader.CancelAsync()
                End If
                prog = New Progress(Nothing, 0, 0)
                prog.canceled = True
                workerReportProg(prog)
                canceledByUser = True
                Exit While
            End If
        End While
        wavePen.Color = Color.White 'reset color on end
        If args.convertVideo And Not args.noFileWriting And Not OscilloscopeBackgroundWorker.CancellationPending Then
            stdin.Flush()
            stdin.Close()
            Dim endProg As New Progress("")
            endProg.ffmpegClosed = True
            workerReportProg(endProg)
            Do Until ffmpegProc.HasExited
            Loop
            stderr.Close()
        End If
    End Sub
    Public Sub workerReportProg(ByVal userState As Object)
        progressList.Add(userState)
    End Sub

    Private Sub drawWave(ByRef g As Graphics, ByRef pen As Pen, ByVal rect As Rectangle, ByRef wave As WAV, ByVal workerArg As WorkerArguments, ByVal sampleRate As Long, ByVal timeScale As Double, ByVal offset As Long)
        Dim args As channelOptions = wave.extraArguments
        Dim points As New List(Of Point)
        Dim prevX As Integer = -1
        For i As Integer = offset - sampleRate * args.horizontalTime / 2 To offset + sampleRate * args.horizontalTime / 2 '+ sampleRate * timeScale
            Dim x As Integer = (i - (offset - sampleRate * args.horizontalTime / 2)) / sampleRate / args.horizontalTime * rect.Width + rect.X
            If prevX = x Then
                Continue For
            Else
                prevX = x
            End If
            Dim y As Integer
            y = -wave.getSample(i, True) / 256 * rect.Height + rect.Y + rect.Height / 2
            If workerArg.useAnalogOscilloscopeStyle Then
                points.Add(New Point(x, y - workerArg.analogOscilloscopeLineWidth \ 2 + workerArg.analogOscilloscopeLineWidth - 1))
                points.Add(New Point(x, y - workerArg.analogOscilloscopeLineWidth \ 2 - 1))
                Dim nextX As Integer = (i + 1 - (offset - sampleRate * args.horizontalTime / 2)) / sampleRate / args.horizontalTime * rect.Width + rect.X
                If nextX - x > 1 And x >= 0 Then
                    For dx As ULong = x To nextX
                        points.Add(New Point(dx, y - workerArg.analogOscilloscopeLineWidth \ 2 + workerArg.analogOscilloscopeLineWidth - 1))
                        points.Add(New Point(dx, y - workerArg.analogOscilloscopeLineWidth \ 2 - 1))
                    Next
                End If
            Else
                points.Add(New Point(x, y))
            End If
        Next
        pen.Color = args.waveColor
        g.Clip = New Region(rect)
        g.DrawLines(pen, points.ToArray())
    End Sub

    Private Sub drawWaveXY(ByRef g As Graphics, ByRef pen As Pen, ByVal rect As Rectangle, ByRef wave As WAV, ByVal workerArg As WorkerArguments, ByVal sampleRate As Long, ByVal frameDuration As Double, ByVal offset As Long)
        wave.mixChannel = False
        g.Clip = New Region(rect)
        Dim args As channelOptions = wave.extraArguments
        Dim points As New List(Of Point)
        Dim path As New Drawing.Drawing2D.GraphicsPath
        Dim prevX As Integer = -1
        Dim drawingSize As Size = rect.Size
        pen.Color = args.waveColor
        Dim XYpen As Pen = pen.Clone()
        If workerArg.useAnalogOscilloscopeStyle Then
            XYpen.Width = workerArg.analogOscilloscopeLineWidth
        End If
        If args.XYmodeAspectRatio Then
            If rect.Height < rect.Width Then
                drawingSize = New Size(rect.Height, rect.Height)
            Else
                drawingSize = New Size(rect.Width, rect.Width)
            End If
        End If
        For i As Integer = offset To offset + frameDuration
            wave.selectedChannel = 0
            Dim x As Integer = wave.getSample(i, True) / 256 * drawingSize.Width + rect.Width / 2 + rect.X
            wave.selectedChannel = 1
            Dim y As Integer
            y = -wave.getSample(i, True) / 256 * drawingSize.Height + rect.Height / 2 + rect.Y
            If workerArg.dottedXYmode Then
                path.AddLine(x, y, x + 1, y)
                path.CloseFigure()
            Else
                points.Add(New Point(x, y))
            End If
            'End If
        Next
        If workerArg.dottedXYmode Then
            g.DrawPath(XYpen, path)
        Else
            g.DrawLines(XYpen, points.ToArray())
        End If
    End Sub

    Function HSVtoRGB(ByVal hue As Double, ByVal saturation As Double, ByVal value As Double) As Color
        Dim h As Integer = Convert.ToInt32(Math.Floor(hue / 60)) Mod 6
        Dim f As Double = hue / 60 - Math.Floor(hue / 60)

        value = value * 255
        Dim v As Integer = Convert.ToInt32(value)
        Dim p As Integer = Convert.ToInt32(value * (1 - saturation))
        Dim q As Integer = Convert.ToInt32(value * (1 - f * saturation))
        Dim t As Integer = Convert.ToInt32(value * (1 - (1 - f) * saturation))

        If h = 0 Then
            Return Color.FromArgb(255, v, t, p)
        ElseIf h = 1 Then
            Return Color.FromArgb(255, q, v, p)
        ElseIf h = 2 Then
            Return Color.FromArgb(255, p, v, t)
        ElseIf h = 3 Then
            Return Color.FromArgb(255, p, q, v)
        ElseIf h = 4 Then
            Return Color.FromArgb(255, t, p, v)
        Else
            Return Color.FromArgb(255, v, p, q)
        End If
    End Function

    Private Sub OscilloscopeBackgroundWorker_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles OscilloscopeBackgroundWorker.ProgressChanged
        'taskbarProgress.ProgressState = Windows.Shell.TaskbarItemProgressState.Normal
        Dim prog As Progress = e.UserState
        progressUpdater(prog)
    End Sub

    Private Sub progressUpdater(ByVal prog As Progress, Optional ByVal updateImage As Boolean = False)
        If prog.message <> "" Then
            LogBox.AppendText(prog.message & vbCrLf)
            LogBox.Update()
            LogBox.Focus()
            LogBox.Select(LogBox.TextLength, 0)
        End If
        If prog.ffmpegClosed Then
            If Not isRunningMono And isProgressTaskBarSupported Then TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Indeterminate)
            LabelStatus.Text = "Waiting FFmpeg to finish..."
        End If
        If updateImage Then
            Dim ok As Boolean = False
            If CheckBoxShowOutput.Checked And lastFrame IsNot Nothing Then
                Do
                    Try
                        PictureBoxOutput.Image = lastFrame.Clone()
                        ok = True
                    Catch ex As InvalidOperationException
                        ok = False
                    End Try
                Loop Until ok = True
            End If
            Dim timeLeftSecond As ULong = 0
            If averageFPS <> 0 Then
                timeLeftSecond = (prog.TotalFrame - prog.CurrentFrame) / averageFPS
            End If
            Dim timeLeft As New TimeSpan(0, 0, timeLeftSecond)
            LabelStatus.Text = String.Format("{0:P1} {1}/{2}, {3:N1} FPS , Time left: {4}", prog.CurrentFrame / prog.TotalFrame, prog.CurrentFrame, prog.TotalFrame, realFPS, timeLeft.ToString())
            fpsFrames += 1
            Dim ms As ULong = Math.Abs((Now - fpsTimer).TotalMilliseconds)
            If ms >= 1000 Then
                fpsTimer = Now
                realFPS = fpsFrames * 1000 / ms
                averageFPS = (averageFPS + realFPS) / 2
                fpsFrames = 0
            End If
            If Not isRunningMono And isProgressTaskBarSupported Then
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal)
                TaskbarManager.Instance.SetProgressValue(prog.CurrentFrame, prog.TotalFrame)
            End If
        ElseIf prog.canceled Then 'canceled
            LabelStatus.Text = "Canceled."
            If prog.message <> "" Then
                LogBox.AppendText(prog.message & vbCrLf)
                LogBox.Update()
                LogBox.Focus()
                LogBox.Select(LogBox.TextLength, 0)
            End If
        End If
    End Sub

    Private Sub OscilloscopeBackgroundWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles OscilloscopeBackgroundWorker.RunWorkerCompleted
        Dim elapsedTime As TimeSpan = Now - startTime
        LogBox.AppendText("Total time spent: " & elapsedTime.ToString() & vbCrLf)
        CheckBoxNoFileWriting_CheckedChanged(Nothing, Nothing)
        For Each ctrl As Control In TabControlRenderingFiles.Controls
            ctrl.Enabled = True
        Next
        LabelStatus.Text = "Finished."
        ButtonControl.Text = "Start"
        ButtonControl.Enabled = True
        If Not isRunningMono And isProgressTaskBarSupported Then TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress)
        If Not allFilesLoaded Then
            For Each msg In failedFiles
                LogBox.AppendText("Failed to load " & msg.Key & ":" & msg.Value & vbCrLf)
            Next
            Exit Sub
        End If
        If BackgroundWorkerStdErrReader.IsBusy Then
            BackgroundWorkerStdErrReader.CancelAsync()
        End If
        GC.Collect()
    End Sub

    Private Sub ButtonAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click
        Dim ofd As New OpenFileDialog
        ofd.Filter = FileFilter
        ofd.Multiselect = True
        If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            For Each file In ofd.FileNames
                ListBoxFiles.Items.Add(file)
                optionsList.Add(New channelOptions)
            Next
        End If
        previewLayout()
    End Sub

    Private Sub ButtonRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRemove.Click
        If ListBoxFiles.SelectedItems.Count = 0 Then
            MsgBox("Please select at least one file!", MsgBoxStyle.Exclamation)
        End If
        While ListBoxFiles.SelectedIndices.Count > 0
            Dim index As Integer = ListBoxFiles.SelectedIndices(0)
            optionsList.RemoveAt(index)
            ListBoxFiles.Items.RemoveAt(index)
        End While
        previewLayout()
    End Sub

    Private Sub ButtonMoveUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMoveUp.Click
        If ListBoxFiles.SelectedIndices.Count > 1 Then
            MsgBox("Please select only one file.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        If ListBoxFiles.SelectedIndices.Count > 0 Then
            Dim index As Integer = ListBoxFiles.SelectedIndices(0)
            If index > 0 Then
                Dim currentOption As channelOptions = optionsList(index).Clone()
                optionsList.RemoveAt(index)
                optionsList.Insert(index - 1, currentOption)
                ListBoxFiles.Items.Insert(index - 1, ListBoxFiles.SelectedItem)
                ListBoxFiles.SetSelected(index - 1, True)
                ListBoxFiles.Items.RemoveAt(index + 1)
            End If
        Else
            MsgBox("Please select a file!", MsgBoxStyle.Exclamation)
        End If
        previewLayout()
    End Sub

    Private Sub ButtonMoveDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMoveDown.Click
        If ListBoxFiles.SelectedIndices.Count > 1 Then
            MsgBox("Please select only one file.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If ListBoxFiles.SelectedIndices.Count > 0 Then
            Dim index As Integer = ListBoxFiles.SelectedIndices(0)

            If Not index = ListBoxFiles.Items.Count - 1 Then
                Dim currentOption As channelOptions = optionsList(index).Clone()
                optionsList.RemoveAt(index)
                optionsList.Insert(index + 1, currentOption)
                ListBoxFiles.Items.Insert(index + 2, ListBoxFiles.SelectedItem)
                ListBoxFiles.SetSelected(index + 2, True)
                ListBoxFiles.Items.RemoveAt(index)
            End If
        Else
            MsgBox("Please select a file!", MsgBoxStyle.Exclamation)
        End If

        previewLayout()
    End Sub

    Private Sub ButtonOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOptions.Click
        If ListBoxFiles.SelectedIndices.Count > 0 Then
            Dim ccf As New ChannelConfigForm
            If ListBoxFiles.SelectedIndices.Count = 1 Then
                ccf.Options = optionsList(ListBoxFiles.SelectedIndex).Clone()
            Else
                Dim firstConfig As channelOptions = optionsList(ListBoxFiles.SelectedIndices(0))
                Dim isAllSame As Boolean = True
                For Each index In ListBoxFiles.SelectedIndices
                    If Not firstConfig.Equals(optionsList(index)) Then
                        isAllSame = False
                    End If
                Next
                If isAllSame Then
                    ccf.Options = firstConfig.Clone()
                Else
                    ccf.Options = New channelOptions()
                End If
            End If
            If ccf.ShowDialog() = DialogResult.OK Then
                For Each index In ListBoxFiles.SelectedIndices
                    optionsList(index) = ccf.Options
                Next
            End If
            previewLayout()
        Else
            MsgBox("Please select at least one file!", MsgBoxStyle.Exclamation)
        End If
    End Sub

    Private Sub ButtonSelAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSelectAll.Click
        If ListBoxFiles.Items.Count <> 0 Then
            Dim selected As Boolean = False
            If ListBoxFiles.SelectedIndices.Count = 0 Then
                selected = True
            End If
            For i As Integer = 0 To ListBoxFiles.Items.Count - 1
                ListBoxFiles.SetSelected(i, selected)
            Next
        End If
    End Sub

    Private Sub ListBoxFiles_DoubleClick(sender As Object, e As EventArgs) Handles ListBoxFiles.DoubleClick
        ButtonOptions.PerformClick()
    End Sub

    Sub previewLayout()

        Dim bmpLayout As New Bitmap(canvasSize.Width, canvasSize.Height)
        Dim g As Graphics = Graphics.FromImage(bmpLayout)
        g.Clear(ButtonBackgroundColor.BackColor)
        If ListBoxFiles.Items.Count <> 0 Then
            Dim col As Byte = NumericUpDownColumn.Value
            Dim channels As UInteger = ListBoxFiles.Items.Count
            Dim maxChannelPerColumn As Integer = Math.Ceiling(channels / col)
            Dim channelWidth As Integer = canvasSize.Width / col
            Dim channelHeight As Integer = canvasSize.Height / maxChannelPerColumn
            Dim channelOffset(channels - 1) As Point
            For c As Integer = 0 To channels - 1
                Dim currentChannel As channelOptions = optionsList(c)
                Dim x, y, currentColumn, currentRow As Integer
                If channelFlowDirection = FlowDirection.LeftToRight Then
                    currentRow = (c - (c Mod col)) / col
                    y = channelHeight * currentRow
                    x = channelWidth * (c Mod col)
                ElseIf channelFlowDirection = FlowDirection.TopDown Then
                    currentColumn = (c - (c Mod maxChannelPerColumn)) / maxChannelPerColumn
                    y = channelHeight * (c Mod maxChannelPerColumn)
                    x = channelWidth * currentColumn
                End If
                Dim filename As String = IO.Path.GetFileName(ListBoxFiles.Items(c))
                Dim channelColor As Color = currentChannel.waveColor
                Dim labelDX, labelDY As Integer
                Dim textSize As SizeF = New SizeF(0, 0)
                If currentChannel.label <> "" Then
                    textSize = g.MeasureString(currentChannel.label, currentChannel.labelFont)
                Else
                    textSize = g.MeasureString(filename, New Font(SystemFonts.MenuFont.FontFamily, 24))
                End If
                labelDX = 0
                labelDY = 0
                Select Case ComboBoxLabelPos.SelectedIndex
                    Case 0 'Top Left

                    Case 1 'Top Right
                        labelDX = channelWidth - textSize.Width
                    Case 2 'Bottom Left
                        labelDY = channelHeight - textSize.Height
                    Case 3 'Bottom Right
                        labelDX = channelWidth - textSize.Width
                        labelDY = channelHeight - textSize.Height
                End Select
                If currentChannel.label <> "" Then
                    g.DrawString(currentChannel.label, currentChannel.labelFont, New SolidBrush(currentChannel.labelColor), New Rectangle(x + labelDX, y + labelDY, channelWidth, channelHeight))
                Else
                    g.DrawString(filename, New Font(SystemFonts.MenuFont.FontFamily, 24), New SolidBrush(currentChannel.waveColor), New Rectangle(x + labelDX, y + labelDY, channelWidth, channelHeight))
                End If
                If CheckBoxDrawMiddleLine.Checked Then
                    g.DrawLine(New Pen(ButtonMiddleLineColor.BackColor, NumericUpDownMiddleLine.Value), x, y + channelHeight \ 2, x + channelWidth, y + channelHeight \ 2)
                    If currentChannel.XYmode Then
                        g.DrawLine(New Pen(ButtonMiddleLineColor.BackColor, NumericUpDownMiddleLine.Value), x + channelWidth \ 2, y,
                                                                                                            x + channelWidth \ 2, y + channelHeight)
                    End If
                End If
            Next
            If CheckBoxGrid.Checked Then
                For x As Integer = 1 To col - 1
                    g.DrawLine(New Pen(ButtonGridColor.BackColor, NumericUpDownGrid.Value), channelWidth * x, 0, channelWidth * x, canvasSize.Height)
                Next
                For y As Integer = 1 To maxChannelPerColumn - 1
                    g.DrawLine(New Pen(ButtonGridColor.BackColor, NumericUpDownGrid.Value), 0, channelHeight * y, canvasSize.Width, channelHeight * y)
                Next
            End If
            If CheckBoxBorder.Checked Then
                g.DrawRectangle(New Pen(ButtonBorderColor.BackColor, NumericUpDownBorder.Value * 2), 0, 0, canvasSize.Width, canvasSize.Height)
            End If
        End If

        PictureBoxOutput.Image = bmpLayout
    End Sub

    Function getTextColor(ByVal color As Color) As Color
        Dim Y As Single = 0.2126 * color.R / 255 + 0.7152 * color.G / 255 + 0.0722 * color.B / 255
        'calculate luminance
        If Y < 0.5 Then
            Return Color.White
        Else
            Return Color.Black
        End If
    End Function

    Private Sub NumericUpDownColumn_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDownColumn.ValueChanged
        'If formStarted Then
        previewLayout()
        'End If
    End Sub

    Private Sub ListBoxFiles_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        ButtonOptions.PerformClick()
    End Sub

    Private Sub CheckBoxNoFileWriting_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxNoFileWriting.CheckedChanged
        If CheckBoxNoFileWriting.Checked Then
            LabelPreviewMode.Visible = True
        Else
            LabelPreviewMode.Visible = False
        End If
        TimerLabelFlashing.Enabled = CheckBoxNoFileWriting.Checked
    End Sub

    Private Sub TimerLabelFlashing_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerLabelFlashing.Tick
        If OscilloscopeBackgroundWorker.IsBusy Then
            If CheckBoxNoFileWriting.Checked Then
                LabelPreviewMode.Visible = Not LabelPreviewMode.Visible
            Else
                LabelPreviewMode.Visible = False
            End If
        End If
    End Sub

    Private Sub ToolStripStatusLabelAbout_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ToolStripStatusLabelAbout.MouseDown
        ToolStripStatusLabelAbout.BorderStyle = Border3DStyle.SunkenOuter
    End Sub

    Private Sub ToolStripStatusLabelAbout_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripStatusLabelAbout.MouseLeave
        ToolStripStatusLabelAbout.BorderStyle = Border3DStyle.RaisedOuter
    End Sub

    Private Sub ToolStripStatusLabelAbout_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ToolStripStatusLabelAbout.MouseUp
        ToolStripStatusLabelAbout.BorderStyle = Border3DStyle.RaisedOuter
        AboutForm.ShowDialog()
    End Sub

    Private Sub LinkLabelCustomCommandLine_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabelCustomCommandLine.LinkClicked
        If CustomCommandLineForm.ShowDialog() = Windows.Forms.DialogResult.OK Then
            writeConfig()
        End If
    End Sub

    Private Sub CheckBoxGrid_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxGrid.CheckedChanged
        previewLayout()
    End Sub

    Private Sub BackgroundWorkerStdErrReader_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorkerStdErrReader.DoWork
        While convertVideo And Not NoFileWriting And Not BackgroundWorkerStdErrReader.CancellationPending
            If FFmpegstderr IsNot Nothing Then
                If FFmpegstderr.BaseStream IsNot Nothing Then
                    If FFmpegstderr.BaseStream.CanRead Then
                        BackgroundWorkerStdErrReader.ReportProgress(0, FFmpegstderr.ReadLine())
                    End If
                End If
            End If
        End While
    End Sub

    Private Sub BackgroundWorkerStdErrReader_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorkerStdErrReader.ProgressChanged
        Dim stderr As String = e.UserState
        If stderr <> "" And Not LogBox.IsDisposed Then
            LogBox.AppendText(stderr & vbCrLf)
        End If
    End Sub

    Private Sub MainForm_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        If formStarted Then
            Me.Size = New Size(originalFormSize.Width, Me.Height)
            LogBox.Size = New Size(LogBox.Width, Me.Height - originalFormSize.Height + originalTextBoxLogHeight)
        End If

    End Sub

    Private Sub ButtonFlowDirection_Click(sender As Object, e As EventArgs) Handles ButtonFlowDirection.Click
        If channelFlowDirection = FlowDirection.LeftToRight Then
            channelFlowDirection = FlowDirection.TopDown
        Else
            channelFlowDirection = FlowDirection.LeftToRight
        End If
        ButtonFlowDirection.Invalidate()
        previewLayout()
    End Sub

    Private Sub ButtonFlowDirection_Invalidated(sender As Object, e As InvalidateEventArgs) Handles ButtonFlowDirection.Invalidated
        If channelFlowDirection = FlowDirection.LeftToRight Then
            ButtonFlowDirection.Text = "Left to right"
        Else
            ButtonFlowDirection.Text = "Top to down"
        End If
    End Sub

    Private Sub CheckBoxDrawMiddleLine_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxDrawMiddleLine.CheckedChanged
        previewLayout()
    End Sub

    Private Sub ButtonGridColor_Click(sender As Object, e As EventArgs) Handles ButtonGridColor.Click
        Dim cd As New ColorDialog
        cd.Color = ButtonGridColor.BackColor
        If cd.ShowDialog() = DialogResult.OK Then
            ButtonGridColor.BackColor = cd.Color
        End If
        previewLayout()
    End Sub

    Private Sub ButtonMiddleLineColor_Click(sender As Object, e As EventArgs) Handles ButtonMiddleLineColor.Click
        Dim cd As New ColorDialog
        cd.Color = ButtonMiddleLineColor.BackColor
        If cd.ShowDialog() = DialogResult.OK Then
            ButtonMiddleLineColor.BackColor = cd.Color
        End If
        previewLayout()
    End Sub

    Private Sub ButtonBackgroundColor_Click(sender As Object, e As EventArgs) Handles ButtonBackgroundColor.Click
        Dim cd As New ColorDialog
        cd.Color = ButtonBackgroundColor.BackColor
        If cd.ShowDialog() = DialogResult.OK Then
            ButtonBackgroundColor.BackColor = cd.Color
        End If
        previewLayout()
    End Sub

    Private Sub ButtonBorderColor_Click(sender As Object, e As EventArgs) Handles ButtonBorderColor.Click
        Dim cd As New ColorDialog
        cd.Color = ButtonBorderColor.BackColor
        If cd.ShowDialog() = DialogResult.OK Then
            ButtonBorderColor.BackColor = cd.Color
        End If
        previewLayout()
    End Sub

    Private Sub ButtonBackgroundColor_BackColorChanged(sender As Object, e As EventArgs) Handles ButtonBackgroundColor.BackColorChanged
        ButtonBackgroundColor.ForeColor = getTextColor(ButtonBackgroundColor.BackColor)
    End Sub

    Private Sub ButtonMiddleLineColor_BackColorChanged(sender As Object, e As EventArgs) Handles ButtonMiddleLineColor.BackColorChanged
        ButtonMiddleLineColor.ForeColor = getTextColor(ButtonMiddleLineColor.BackColor)
    End Sub

    Private Sub ButtonGridColor_BackColorChanged(sender As Object, e As EventArgs) Handles ButtonGridColor.BackColorChanged
        ButtonGridColor.ForeColor = getTextColor(ButtonGridColor.BackColor)
    End Sub

    Private Sub ButtonBorderColor_BackColorChanged(sender As Object, e As EventArgs) Handles ButtonBorderColor.BackColorChanged
        ButtonBorderColor.ForeColor = getTextColor(ButtonBorderColor.BackColor)
    End Sub

    Private Sub CheckBoxBorder_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxBorder.CheckedChanged
        previewLayout()
    End Sub

    Private Sub NumericUpDownBorder_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDownBorder.ValueChanged
        previewLayout()
    End Sub

    Private Sub NumericUpDownGrid_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDownGrid.ValueChanged
        previewLayout()
    End Sub

    Private Sub NumericUpDownMiddleLine_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDownMiddleLine.ValueChanged
        previewLayout()
    End Sub

    Private Sub ComboBoxLabelPos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxLabelPos.SelectedIndexChanged
        previewLayout()
    End Sub

    Private Sub TimerStatusUpdater_Tick(sender As Object, e As EventArgs) Handles TimerStatusUpdater.Tick
        If OscilloscopeBackgroundWorker.IsBusy Then
            If OscilloscopeBackgroundWorker.IsBusy Then
                If progressList.Count > 0 Then
                    fpsFrames += progressList.Count - 1
                    While progressList.Count > 1
                        progressUpdater(progressList.First)
                        progressList.RemoveAt(0)
                    End While
                    progressUpdater(progressList.Last, True)
                    progressList.RemoveAt(0)
                End If
                'GC.Collect()
            End If
        Else
            TimerStatusUpdater.Stop()
        End If
    End Sub
End Class
