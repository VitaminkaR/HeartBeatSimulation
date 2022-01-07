using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;

namespace HeartBitSimulation
{
    class Heart
    {
        public enum Rythms
        {
            sinusRhythm,
            arrhythmia,
            atrialFibrillation,
            coarseVentricularFibrillation,
            ventricularFibrillation,
            asystole,
        }


        // сокращения
        private Thread ventricleThread;
        private Thread atrialThread;
        private Mutex mutex = new Mutex();

        public Monitor monitor;

        //// основные переменные сердца
        // частота сердченых сокращений ms
        public int heartRate;
        public Rythms heartState;

        public Heart(Monitor _monitor)
        {
            heartRate = 75;
            heartState = Rythms.sinusRhythm;
            monitor = _monitor;

            ventricleThread = new Thread(new ThreadStart(VentricleWork));
            atrialThread = new Thread(new ThreadStart(AtrialWork));
            atrialThread.Start();
            ventricleThread.Start();
        }

        private void VentricleWork()
        {
            while (true)
            {
                // синусовый ритм
                if (heartState == Rythms.sinusRhythm)
                {
                    // QRS сокращение желудочков
                    Thread.Sleep(75 + TranslateToMiliseconds(heartRate));
                    mutex.WaitOne();
                    monitor.SetImpulse(-0.4f + (float)(new Random().NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(0.75f + (float)(new Random().NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(-0.3f + (float)(new Random().NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(0f);
                    // T расслабление желудочков
                    Thread.Sleep(100);
                    monitor.SetImpulse(0.3f + (float)(new Random().NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(0f);
                    mutex.ReleaseMutex();
                }

                // аритмия
                if (heartState == Rythms.arrhythmia)
                {
                    // QRS сокращение желудочков
                    Thread.Sleep(75 + TranslateToMiliseconds(heartRate) + new Random().Next(-400, 400));
                    mutex.WaitOne();
                    monitor.SetImpulse(-0.4f + (float)(new Random().NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(0.75f + (float)(new Random().NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(-0.3f + (float)(new Random().NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(0f);
                    // T расслабление желудочков
                    Thread.Sleep(100);
                    monitor.SetImpulse(0.3f + (float)(new Random().NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(0f);
                    mutex.ReleaseMutex();
                }

                // мерцательная аритмия
                if (heartState == Rythms.atrialFibrillation)
                {
                    // QRS сокращение желудочков
                    Thread.Sleep(75 + TranslateToMiliseconds(heartRate));
                    mutex.WaitOne();
                    monitor.SetImpulse(-0.4f + (float)(new Random().NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(0.75f + (float)(new Random().NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(-0.3f + (float)(new Random().NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(0f);
                    // T расслабление желудочков
                    Thread.Sleep(100);
                    monitor.SetImpulse(0.3f + (float)(new Random().NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(0f);
                    mutex.ReleaseMutex();
                }

                // крупноволновая фибрилляция желудочков
                if (heartState == Rythms.coarseVentricularFibrillation)
                {
                    // сокращение желудочков
                    mutex.WaitOne();
                    monitor.SetImpulse(0.5f + (float)(new Random().Next(-10,10)) / 50);
                    Thread.Sleep(25);
                    monitor.SetImpulse(0);
                    Thread.Sleep(TranslateToMiliseconds(heartRate));
                    monitor.SetImpulse((0.5f + (float)(new Random().Next(-10, 10)) / 50) * -1);
                    Thread.Sleep(25);
                    monitor.SetImpulse(5);
                    mutex.ReleaseMutex();
                }

                // мелковолновая фибрилляция желудочоков
                if (heartState == Rythms.ventricularFibrillation)
                {
                    // сокращение желудочков
                    mutex.WaitOne();
                    monitor.SetImpulse((float)(new Random().NextDouble() / (400 / heartRate)) * new Random().Next(-1,2));
                    Thread.Sleep(25);
                    monitor.SetImpulse(0f);
                }
            }
        }

        private void AtrialWork()
        {
            while (true)
            {
                // синусовый ритм
                if (heartState == Rythms.sinusRhythm)
                {
                    // P сокращение предсердий
                    mutex.WaitOne();
                    Thread.Sleep(TranslateToMiliseconds(heartRate));
                    monitor.SetImpulse(0.2f + (float)(new Random().NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(0f);
                    Thread.Sleep(100);
                    mutex.ReleaseMutex();
                }

                // аритмия
                if (heartState == Rythms.arrhythmia)
                {
                    // P сокращение предсердий
                    mutex.WaitOne();
                    Thread.Sleep(TranslateToMiliseconds(heartRate) + new Random().Next(-300, 300));
                    monitor.SetImpulse(0.2f + (float)(new Random().NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(0f);
                    Thread.Sleep(100);
                    mutex.ReleaseMutex();
                }

                // мерцательная аритмия
                if (heartState == Rythms.atrialFibrillation)
                {
                    // P сокращение предсердий
                    mutex.WaitOne();
                    Thread.Sleep(TranslateToMiliseconds(heartRate + new Random().Next(200, 500)));
                    monitor.SetImpulse(0.2f + (float)(new Random().NextDouble() / 15));
                    Thread.Sleep(25);
                    monitor.SetImpulse(0f);
                    Thread.Sleep(100);
                    mutex.ReleaseMutex();
                }
            }
        }

        // переводит удары в минуту в удар за милисекунды
        public int TranslateToMiliseconds(float beats) => (int)(60 / beats * 1000);

        internal void Debug(KeyboardState ks)
        {
            if (ks.IsKeyDown(Keys.Up))
                heartRate++;
            if (ks.IsKeyDown(Keys.Down) && heartRate > 1)
                heartRate--;

            if (ks.IsKeyDown(Keys.D1))
                heartState = Rythms.sinusRhythm;
            if (ks.IsKeyDown(Keys.D2))
                heartState = Rythms.arrhythmia;
            if (ks.IsKeyDown(Keys.D3))
                heartState = Rythms.atrialFibrillation;
            if (ks.IsKeyDown(Keys.D4))
                heartState = Rythms.coarseVentricularFibrillation;
            if (ks.IsKeyDown(Keys.D5))
                heartState = Rythms.ventricularFibrillation;
            if (ks.IsKeyDown(Keys.D6))
                heartState = Rythms.asystole;
        }

        internal void Update()
        {
            // если серцде бьется быстрее 240 bmp или медленее 20 bmp, оно останавливается
            if (heartRate > 240 || heartRate < 20)
                heartState = Rythms.asystole;

            if(heartState == Rythms.asystole)
            {
                if (heartRate > 0)
                    heartRate--;
            }
        }
    }
}
