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
        int REoffset = risingEdgeTrigger(ref wave, triggerValue, offset, maxScanLength);
        offset += REoffset;

        // Sample time
        ulong x = 0;
        ulong zeroCrossingX = 0;    // Positive-slope zero-crossing
        long retX = 0;

        double get(long x) {
            return Math.Floor(wave.getSample(offset + x, True))
        }

        // Distance from zero-crossing
        ulong deltaX = 0;
        ulong shortestDeltaX = maxScanLength;

        int peakY = -127;

        while (x < maxScanLength)
        {
            deltaX = 0;
            zeroCrossingX = x;

            // invariants:
            // if x != 0, get(x) is a positive zero-crossing.
            // get(zeroCrossingX) is (usually) a positive zero-crossing.
            // deltaX = 0.

            while (get(x) >= triggerValue)
            {
                // invariants:
                // get(x) >= 0
                // deltaX = x - zeroCrossingX
                // peakY = largest value

                int y = get(x);

                if (y > peakY)
                {
                    peakY = y;
                    retX = zeroCrossingX;
                    shortestDeltaX = deltaX;
                }
                else if (y == peakY && deltaX < shortestDeltaX)
                {
                    retX = zeroCrossingX;
                    shortestDeltaX = deltaX;
                }

                deltaX += 1;
                x += 1;
                if (x >= maxScanLength) goto End;
            }

            while (get(x) < triggerValue)
            {
                x += 1;
                if (x >= maxScanLength) goto End;
            }
        }

    End:
        return retX + REoffset;
    }

    public static long lengthScanning(ref WAV wave, int triggerValue, long offset, long maxScanLength, bool scanPositive, bool scanNegative)
    {
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

