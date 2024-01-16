using System;
using System.Diagnostics;

namespace Digimon_Project.Utils
{
    public class TimerPlus : System.Timers.Timer
    {
        Stopwatch stopWatch = new Stopwatch();

        public TimerPlus() : base()
        {
            Elapsed += ElapsedAction;
            stopWatch.Start();
        }

        public TimerPlus(int time) : base(time)
        {
            Elapsed += ElapsedAction;
            stopWatch.Start();
        }

        public TimerPlus(double time) : base(time)
        {
            Elapsed += ElapsedAction;
            stopWatch.Start();
        }

        protected new void Dispose()
        {
            Elapsed -= ElapsedAction;
            base.Dispose();
        }

        public double TimeLeft
        {
            get
            {
                return stopWatch.ElapsedMilliseconds;
            }
        }

        public void Pause()
        {
            stopWatch.Stop();
        }

        public void Resume()
        {
            stopWatch.Start();
        }

        public new void Start()
        {
            stopWatch.Restart();
            base.Start();
        }

        private void ElapsedAction(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (AutoReset)
            {
                stopWatch.Restart();
            }
        }
    }
}
