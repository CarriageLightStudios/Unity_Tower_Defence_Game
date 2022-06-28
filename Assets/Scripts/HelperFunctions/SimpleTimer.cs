using System;
using System.Timers;


public class SimpleTimer
{
    private Timer Timer;
    public bool Finished;


    public SimpleTimer(float time, bool autoReset)
    {
        Timer = new Timer(time);
        Finished = false;
        Timer.Elapsed += SimpleTimer_Elapsed;
        Timer.AutoReset = autoReset;
        Timer.Enabled = true;
    }

    private void SimpleTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
        Finished = true;
    }


}