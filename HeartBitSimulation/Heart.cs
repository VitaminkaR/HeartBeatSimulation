using System;
using System.Threading;

namespace HeartBitSimulation
{
    class Heart
    {
        private TimerCallback heartBeat;
        private Timer timer;

        public Monitor monitor;

        //// основные переменные сердца
        // частота сердченых сокращений ms
        public int heartRate;

        // дебаг рисовки
        public int state;

        public Heart(Monitor _monitor)
        {
            heartRate = 75;
            heartBeat = new TimerCallback(HeartBeat);
            timer = new Timer(heartBeat, null, 0, TranslateToMiliseconds(heartRate));
            monitor = _monitor;
        }

        private void HeartBeat(object obj)
        {
            // normal rythm
            if (state == 0)
            {
                if (heartRate > 200 || heartRate < 40)
                    ChangeHR(75);
                // atria
                monitor.SetImpulse(0.2f);
                Thread.Sleep(50);
                monitor.SetImpulse(0);

                // ventricle contraction
                Thread.Sleep(50);
                monitor.SetImpulse(-0.3f);
                Thread.Sleep(25);
                monitor.SetImpulse(0.65f);
                Thread.Sleep(25);
                monitor.SetImpulse(-0.2f);
                Thread.Sleep(50);
                monitor.SetImpulse(0);

                // ventricle relaxation
                Thread.Sleep(25);
                monitor.SetImpulse(0.45f);
                Thread.Sleep(50);
                monitor.SetImpulse(0);
            }

            // big ventrical fibrillation
            if (state == 1)
            {
                if(heartRate < 100)
                    ChangeHR(200);
                // ventricle contraction
                Thread.Sleep(50);
                monitor.SetImpulse(-0.1f);
                Thread.Sleep(25);
                monitor.SetImpulse(0.45f);
                Thread.Sleep(25);
                monitor.SetImpulse(-0.05f);
                Thread.Sleep(50);
                monitor.SetImpulse(0);

                // ventricle relaxation
                Thread.Sleep(25);
                monitor.SetImpulse(0.45f);
                Thread.Sleep(50);
                monitor.SetImpulse(0);
            }

            // big ventrical fibrillation
            if (state == 2)
            {
                if (heartRate < 100)
                    ChangeHR(200);
                // ventricle contraction
                Thread.Sleep(50);
                monitor.SetImpulse(-0.1f * (float)new Random().NextDouble());
                Thread.Sleep(25);
                monitor.SetImpulse(0.1f * (float)new Random().NextDouble());
                Thread.Sleep(25);
                monitor.SetImpulse(-0.1f * (float)new Random().NextDouble());
                Thread.Sleep(50);
                monitor.SetImpulse(0);

                // ventricle relaxation
                Thread.Sleep(25);
                monitor.SetImpulse(0.1f * (float)new Random().NextDouble());
                Thread.Sleep(50);
                monitor.SetImpulse(0);
            }

            // asystol
            if (state == 3)
            {
                if (heartRate != 0)
                {
                    heartRate = 0;
                }
            }
        }

        public void Diffibrillator()
        {
            monitor.SetImpulse(1.5f);
            state = 0;
        }

        // переводит удары в минуту в удар за милисекунды
        public int TranslateToMiliseconds(float beats) => (int)(60 / beats * 1000);

        public void ChangeHR(int value)
        {
            if (value > 0)
            {
                heartRate = value;
                timer.Change(0, TranslateToMiliseconds(heartRate));
            }
        }
    }
}
