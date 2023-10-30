using System;
using System.Threading;

namespace PolyglotPal_KimRozenberg
{
    public class Timer
    {
        private System.Threading.Timer _timer;
        private TimeSpan _interval;
        private TimeSpan _currentTime;
        private bool _isRunning;

        public Timer(TimeSpan interval)
        {
            _interval = interval;
            _currentTime = TimeSpan.Zero;
            _isRunning = false;
        }

        public void Start()
        {
            _isRunning = true;
            _timer = new System.Threading.Timer(TimerCallback, null, 0, (int)_interval.TotalMilliseconds);
        }

        public void Start(TimeSpan startTime)
        {
            _currentTime = startTime;
            Start();
        }

        public void Pause()
        {
            _isRunning = false;
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public void Resume()
        {
            _isRunning = true;
            _timer.Change(0, (int)_interval.TotalMilliseconds);
        }

        public TimeSpan GetCurrentTime()
        {
            return _currentTime;
        }

        private void TimerCallback(object o)
        {
            if (_isRunning)
            {
                _currentTime = _currentTime.Add(_interval);
            }
        }

        public void Reset()
        {
            _currentTime = TimeSpan.Zero;
            _isRunning = false;
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
        }
    }
}