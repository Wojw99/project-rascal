using System;
using System.Threading;

public class PlayerTimer
{
    private Timer timer;
    private readonly object timerLock = new object();

    public event Action OnTimerElapsed;

    public PlayerTimer(int interval)
    {
        timer = new Timer(TimerCallback, null, Timeout.Infinite, Timeout.Infinite);
        Interval = interval;
    }

    public int Interval { get; }

    public void Start()
    {
        lock (timerLock)
        {
            timer.Change(Interval, Timeout.Infinite);
        }
    }

    public void Reset()
    {
        lock (timerLock)
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            Start();
        }
    }

    public void Stop()
    {
        lock (timerLock)
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
        }
    }

    private void TimerCallback(object state)
    {
        lock (timerLock)
        {
            OnTimerElapsed?.Invoke();
        }
    }
}