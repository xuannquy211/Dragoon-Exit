using System;

public abstract class LoopDayPref<T> : DataPref<T> where T : class, new()
{
    public long lastDay;

    public LoopDayPref()
    {
        lastDay = DateTime.Now.Ticks;
    }

    public static bool IsPastDay(long lastValue)
    {
        var now = DateTime.Now;
        var last = new DateTime(lastValue);

        return now.Date > last.Date;
    }
}