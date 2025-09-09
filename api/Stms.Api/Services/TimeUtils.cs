using System.Text.RegularExpressions;
using System.Globalization;

namespace Stms.Api.Services;

public static class TimeUtils
{
    // "mm:ss.mmm" -> ms
    public static int ParseTimingMs(string t)
    {
        var m = Regex.Match(t, @"^(\d{1,2}):([0-5]?\d)\.(\d{1,3})$");
        if (!m.Success) throw new ArgumentException("Timing must be mm:ss.mmm");
        int min = int.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture);
        int sec = int.Parse(m.Groups[2].Value, CultureInfo.InvariantCulture);
        int ms  = int.Parse(m.Groups[3].Value.PadRight(3, '0'), CultureInfo.InvariantCulture);
        return (min * 60 + sec) * 1000 + ms;
    }

    // ms -> "mm:ss.mmm"
    public static string ToTimingString(int ms)
    {
        int totalSec = ms / 1000;
        int minutes = totalSec / 60;
        int seconds = totalSec % 60;
        int millis  = ms % 1000;
        return $"{minutes:00}:{seconds:00}.{millis:000}";
    }
}
