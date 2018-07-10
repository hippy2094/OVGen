using System;
using System.Text;

public class WAV
{
    public System.IO.Stream Stream;
    // Public rawSample As Byte()
    private System.IO.MemoryMappedFiles.MemoryMappedFile mmf;
    public UInt16 channels;
    public UInt32 sampleRate;
    public UInt32 byteRate;
    public UInt16 blockAlign;
    public UInt16 bitDepth;
    public UInt32 sampleLength;
    public UInt32 totalSamples;
    public object extraArguments;
    public float amplify = 1;
    public uint limit = 0;
    public bool mixChannel = true;
    public byte selectedChannel = 0;
    private UInt32 sampleBegin;

    public WAV(string filename, bool checkHeadersOnly = false)
    {
        UInt32 offset = 0;
        mmf = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(System.IO.File.Open(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read), null, 0, System.IO.MemoryMappedFiles.MemoryMappedFileAccess.Read, null, System.IO.HandleInheritability.None, false);
        try
        {
            Stream = mmf.CreateViewStream(0, 0, System.IO.MemoryMappedFiles.MemoryMappedFileAccess.Read);
        }
        catch (Exception ex)
        {
            mmf.Dispose();
            Stream = null;
            throw ex;
        }
        byte[] buffer4 = new byte[4];
        byte[] buffer2 = new byte[2];
        offset += Stream.Read(buffer4, 0, 4);
        if (Encoding.ASCII.GetString(buffer4) != "RIFF")
        {
            System.Exception ex = new System.Exception("Bad header at " + Stream.Position + ", expected \"RIFF\"");
            Stream.Close();
            mmf.Dispose();
            throw ex;
        }
        UInt32 chunkSize;
        offset += Stream.Read(buffer4, 0, 4);
        chunkSize = BitConverter.ToUInt32(buffer4, 0);
        offset += Stream.Read(buffer4, 0, 4);
        if (Encoding.ASCII.GetString(buffer4) != "WAVE")
        {
            System.Exception ex = new System.Exception("Bad header at " + Stream.Position + ", expected \"WAVE\".");
            Stream.Close();
            mmf.Dispose();
            throw ex;
        }
        offset += Stream.Read(buffer4, 0, 4);
        if (Encoding.ASCII.GetString(buffer4) != "fmt ")
        {
            System.Exception ex = new System.Exception("Bad header at " + Stream.Position + ", expected \"fmt \".");
            Stream.Close();
            mmf.Dispose();
            throw ex;
        }
        offset += Stream.Read(buffer4, 0, 4);
        UInt32 SubChunkSize1 = BitConverter.ToUInt32(buffer4, 0);
        offset += Stream.Read(buffer2, 0, 2);
        if (BitConverter.ToUInt16(buffer2, 0) != 1)
        {
            System.Exception ex = new System.Exception("Not a PCM encoded audio.");
            Stream.Close();
            mmf.Dispose();
            throw ex;
        }
        offset += Stream.Read(buffer2, 0, 2);
        channels = BitConverter.ToUInt16(buffer2, 0);
        offset += Stream.Read(buffer4, 0, 4);
        sampleRate = BitConverter.ToUInt32(buffer4, 0);
        offset += Stream.Read(buffer4, 0, 4);
        byteRate = BitConverter.ToUInt32(buffer4, 0);
        offset += Stream.Read(buffer2, 0, 2);
        blockAlign = BitConverter.ToUInt16(buffer2, 0);
        offset += Stream.Read(buffer2, 0, 2);
        bitDepth = BitConverter.ToUInt16(buffer2, 0);
        if (SubChunkSize1 > 16)
        {
            offset += Stream.Read(buffer2, 0, 2);
            var extraParamsSize = BitConverter.ToUInt16(buffer2, 0);
            offset += extraParamsSize;
        }
        offset += Stream.Read(buffer4, 0, 4);
        if (Encoding.ASCII.GetString(buffer4) != "data")
        {
            System.Exception ex = new System.Exception("Bad header at " + Stream.Position + ", expected \"data\".");
            Stream.Close();
            mmf.Dispose();
            throw ex;
        }
        offset += Stream.Read(buffer4, 0, 4);
        totalSamples = BitConverter.ToUInt32(buffer4, 0);
        sampleLength = totalSamples / (double)channels;
        if (bitDepth == 16)
            sampleLength /= (double)2;
        sampleBegin = Stream.Position;
        if (checkHeadersOnly)
        {
            Stream.Close();
            mmf.Dispose();
            return;
        }
    }
    /// <summary>
    /// Returns value between -128 to 127
    /// If unsigned, 0 to 255
    /// </summary>
    /// <param name="index">Sample index</param>
    /// <param name="signed">Should return signed value.</param>
    /// <returns>Double</returns>
    /// <remarks></remarks>
    public double getSample(long index, bool signed)
    {
        // returns value between -128 to 128
        // if unsigned, 0 to 256
        index *= channels;
        if (!mixChannel)
            index += selectedChannel;
        switch (bitDepth)
        {
            case 16:
                {
                    index *= 2;
                    if (index < 0 | index > totalSamples)
                    {
                        if (signed)
                            return 0;
                        else
                            return sbyte.MaxValue;
                    }
                    Stream.Position = sampleBegin + index;
                    byte[] buffer = new byte[2];
                    Stream.Read(buffer, 0, 2);
                    double value = BitConverter.ToInt16(buffer, 0) / (double)32767 * 127 * amplify;
                    if (mixChannel)
                    {
                        for (int i = 2; i <= channels * 2; i += 2)
                        {
                            Stream.Position += i;
                            Stream.Read(buffer, 0, 2);
                            value += BitConverter.ToInt16(buffer, 0) / (double)32768 * 128 * amplify;
                            value /= 2;
                        }
                    }
                    if (signed)
                    {
                        switch (value)
                        {
                            case object _ when value < -128 + limit:
                                {
                                    return -128 + limit;
                                }

                            case object _ when value > 127 - limit:
                                {
                                    return 128 - limit;
                                }

                            default:
                                {
                                    return value;
                                }
                        }
                    }
                    else
                        switch (value)
                        {
                            case object _ when value < -128 + limit:
                                {
                                    return 0 + limit;
                                }

                            case object _ when value > 127 - limit:
                                {
                                    return 255 - limit;
                                }

                            default:
                                {
                                    return value + 127;
                                }
                        }

                    break;
                }

            case 8:
                {
                    if (index < 0 | index > totalSamples)
                    {
                        if (signed)
                            return 0;
                        else
                            return sbyte.MaxValue + 1;
                    }
                    Stream.Position = sampleBegin + index;
                    double value = (Stream.ReadByte() - 128) * amplify;
                    if (mixChannel)
                    {
                        for (int i = 2; i <= channels; i++)
                        {
                            value += (Stream.ReadByte() - 128) * amplify;
                            value /= 2;
                        }
                    }
                    if (signed)
                    {
                        switch (value)
                        {
                            case object _ when value < -128:
                                {
                                    return -128;
                                }

                            case object _ when value > 127 - limit:
                                {
                                    return 128 - limit;
                                }

                            default:
                                {
                                    return value;
                                }
                        }
                    }
                    else
                    {
                        value += 128;

                        switch (value)
                        {
                            case object _ when value < 0 + limit:
                                {
                                    return 0 + limit;
                                }

                            case object _ when value > 255 - limit:
                                {
                                    return 255 - limit;
                                }

                            default:
                                {
                                    return value;
                                }
                        }
                    }

                    break;
                }

            default:
                {
                    // how
                    return 0;
                }
        }
    }
}
