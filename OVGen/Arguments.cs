
using System.Drawing;
using System.Windows.Forms;
using System;

public class Progress
{
    public ulong CurrentFrame;
    public ulong TotalFrame;
    public string message;
    public bool canceled = false;
    public bool ffmpegClosed = false;
    public Progress(ulong NewCurrentFrame, ulong NewTotalFrame, string newMessage = "")
    {
        CurrentFrame = NewCurrentFrame;
        TotalFrame = NewTotalFrame;
        message = newMessage;
    }
    public Progress(string message)
    {
        this.message = message;
    }
}

public class WorkerArguments
{
    public string[] files;
    public byte columns;
    public bool noFileWriting;
    public bool convertVideo;
    public string ffmpegBinary;
    public int FPS;
    public string outputFile;
    public string outputDirectory;
    public bool joinAudio;
    public string audioFile;
    public bool canceled = false;
    public bool smoothLine;
    public Color backgroundColor = Color.Black;
    public bool drawMiddleLine;
    public Pen middleLinePen;
    public bool useAnalogOscilloscopeStyle = false;
    public byte analogOscilloscopeLineWidth = 4;
    public bool dottedXYmode = false;
    public bool drawGrid;
    public Pen gridPen;
    public bool drawBorder;
    public Pen borderPen;
    public FlowDirection flowDirection;
    public int labelPostition;
}

public class channelOptions : ICloneable
{
    public Pen pen = My.MyProject.MyForms.MainForm.wavePen;
    public Color waveColor = My.MyProject.MyForms.MainForm.wavePen.Color;
    public bool pulseWidthModulatedColor = false;
    public double horizontalTime = 0.025;
    public float amplify = 1;
    public int trigger = 0;
    public bool autoTriggerLevel = true;
    public bool externalTriggerEnabled = false;
    public string externalTriggerFile = "";
    public byte algorithm = TriggeringAlgorithms.UsePeakSpeedScanning;
    public byte scanPhase = 0;
    public string label;
    public Font labelFont = new Font(SystemFonts.MenuFont.FontFamily, 24);
    public Color labelColor = Color.White;
    public float maxScan = 1.0F;
    public bool mixChannel = true;
    public byte selectedChannel = 0;
    public bool XYmode = false;
    public bool XYmodeAspectRatio = true;

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
