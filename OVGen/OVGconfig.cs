using System.Drawing;
using System;

[Serializable()]
public class OVGconfig
{
    public GeneralSection General;
    public FFmpegSection FFmpeg;
    public OVGconfig()
    {
        General = new GeneralSection();
        FFmpeg = new FFmpegSection();
    }
    public class GeneralSection
    {
        public bool SmoothLine;
        public int Framerate = 60;
        public int LineWidth = 2;
        public bool ConvertVideo;
        public bool CRTStyledRender;
        public bool DottedXYmode;
        public bool DrawGrid;
        public ColorSerializable GridColor = new ColorSerializable(Color.LightGray);
        public int GridWidth = 1;
        public bool DrawBorder;
        public ColorSerializable BorderColor = new ColorSerializable(Color.LightGray);
        public int BorderWidth = 1;
        public ColorSerializable BackgroundColor = new ColorSerializable(Color.Black);
        public bool DrawMiddleLine;
        public ColorSerializable MiddleLineColor = new ColorSerializable(Color.Gray);
        public int MiddleLineWidth = 1;
        public string CanvasSize = "1280x720";
        public int FlowDirection;
        public int LabelPosition;
    }
    public class FFmpegSection
    {
        public string BinaryLocation;
        public string JoinAudioCommandLine;
        public string SilenceCommandLine;
    }
}

[Serializable()]
public struct ColorSerializable
{
    public byte R;
    public byte G;
    public byte B;
    public ColorSerializable(byte R, byte G, byte B)
    {
        this.R = R;
        this.G = G;
        this.B = B;
    }
    public ColorSerializable(Color color)
    {
        this.R = color.R;
        this.G = color.G;
        this.B = color.B;
    }
    public Color GetColor()
    {
        return Color.FromArgb(this.R, this.G, this.B);
    }
}
