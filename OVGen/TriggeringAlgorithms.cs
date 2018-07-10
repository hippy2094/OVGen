using System;

static class TriggeringAlgorithms
{
    public readonly static string[] Algorithms = new[] { "Rising Edge", "Peak Speed", "Max Length", "Max Rectified Area", "No Trigger" };

    public const byte UseRisingEdge = 0;
    public const byte UsePeakSpeedScanning = 1;
    public const byte UseMaxLengthScanning = 2;
    public const byte UseMaxRectifiedAreaScanning = 3;
    public static long risingEdgeTrigger(ref WAV wave, int triggerValue, long offset, long maxScanLength)
    {
        channelOptions args = wave.extraArguments;
        long risingEdgeTrigger = 0;
        while (Math.Floor(wave.getSample(offset + risingEdgeTrigger, true)) > triggerValue & risingEdgeTrigger < maxScanLength) // postive
            risingEdgeTrigger += 1;
        while (Math.Floor(wave.getSample(offset + risingEdgeTrigger, true)) <= triggerValue & risingEdgeTrigger < maxScanLength) // negative
            risingEdgeTrigger += 1;
        return risingEdgeTrigger;
    }

    public static long peakSpeedScanning(ref WAV wave, int triggerValue, long offset, long maxScanLength)
    {
        // OPNA2608: scans how fast a peak is reached centers there
        channelOptions args = wave.extraArguments;
        int REoffset = risingEdgeTrigger(ref wave, triggerValue, offset, maxScanLength);
        offset += REoffset;
        long peakSpeedScanning = 0;
        int peak = -127;
        ulong distance = 0;
        ulong shortestDistance = maxScanLength;
        ulong dy = 0;
        ulong tempTrigger = 0;

        while (dy < maxScanLength)
        {
            distance = 0;
            tempTrigger = dy;
            while (Math.Floor(wave.getSample(offset + dy, true)) >= triggerValue & dy < maxScanLength)
            {
                int currentSample = Math.Floor(wave.getSample(offset + dy, true));
                if (currentSample == peak & distance < shortestDistance)
                {
                    peakSpeedScanning = tempTrigger;
                    shortestDistance = distance;
                }
                else if (currentSample > peak)
                {
                    peak = currentSample;
                    peakSpeedScanning = tempTrigger;
                    shortestDistance = distance;
                }
                distance += 1;
                dy += 1;
            }
            while (Math.Floor(wave.getSample(offset + dy, true)) < triggerValue & dy < maxScanLength)
                dy += 1;
        }
        peakSpeedScanning += REoffset;
        return peakSpeedScanning;
    }

    public static long lengthScanning(ref WAV wave, int triggerValue, long offset, long maxScanLength, bool scanPositive, bool scanNegative)
    {
        channelOptions args = wave.extraArguments;
        long lengthScanning = 0;
        long scanLocation = 0;
        long maxLength = 0;
        long tempTrigger = 0;
        while (scanLocation < maxScanLength)
        {
            long currentLength = 0;
            tempTrigger = scanLocation;
            while (Math.Floor(wave.getSample(offset + scanLocation, true)) > triggerValue & scanLocation < maxScanLength)
            {
                scanLocation += 1;
                if (scanPositive)
                    currentLength += 1;
            }
            while (Math.Floor(wave.getSample(offset + scanLocation, true)) <= triggerValue & scanLocation < maxScanLength)
            {
                scanLocation += 1;
                if (scanNegative)
                    currentLength += 1;
            }
            if (currentLength > maxLength)
            {
                maxLength = currentLength;
                lengthScanning = tempTrigger;
            }
        }
        return lengthScanning;
    }

    public static long maxRectifiedArea(ref WAV wave, int triggerValue, long offset, long maxScanLength, bool scanPositive, bool scanNegative)
    {
        channelOptions args = wave.extraArguments;
        long maxRectifiedArea = 0;
        long scanLocation = 0;
        long totalSample = 0;
        long tempTrigger = 0;
        while (scanLocation < maxScanLength)
        {
            long currentTotalSample = 0;
            tempTrigger = scanLocation;
            while (Math.Floor(wave.getSample(offset + scanLocation, true)) > triggerValue & scanLocation < maxScanLength)
            {
                scanLocation += 1;
                if (scanPositive)
                    currentTotalSample += wave.getSample(offset + scanLocation, true);
            }
            while (Math.Floor(wave.getSample(offset + scanLocation, true)) <= triggerValue & scanLocation < maxScanLength)
            {
                scanLocation += 1;
                if (scanNegative)
                    currentTotalSample -= wave.getSample(offset + scanLocation, true);
            }
            if (currentTotalSample > totalSample)
            {
                totalSample = currentTotalSample;
                maxRectifiedArea = tempTrigger;
            }
        }
        return maxRectifiedArea;
    }
}

