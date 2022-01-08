using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;

namespace HeartBitSimulation
{
    class Heart
    {
        // оклонение для рандоматизации показателя чсс. на n * 2 относительно центра(начального сердцебиения(80 - offset))
        const int HEART_DEVIATION = 5;

        // популярные сердечные ритмы
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
        private Random rand;

        //// основные переменные сердца
        // частота сердченых сокращений ms
        public int heartRate;
        // ритм сердца
        public Rythms heartState;
        // значение разницы чсс от нормального выского уровня для рандоматизации(индивидуальности) чсс
        public int heartOffset;
        // значение повреждения сердца
        public float heartDamage;
        // мертв ли миокард
        public bool isDeath;

        public Heart(Monitor _monitor)
        {
            rand = new Random();
            monitor = _monitor;

            heartOffset = rand.Next(0, 21);
            heartRate = 80 - heartOffset;
            heartState = Rythms.sinusRhythm;

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
                    HeartDeviation();

                    // QRS сокращение желудочков
                    Thread.Sleep(75 + TranslateToMiliseconds(heartRate));
                    mutex.WaitOne();
                    monitor.SetImpulse(-0.4f + (float)(rand.NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(0.75f + (float)(rand.NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(-0.3f + (float)(rand.NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(0f);
                    // T расслабление желудочков
                    Thread.Sleep(100);
                    monitor.SetImpulse(0.3f + (float)(rand.NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(0f);
                    mutex.ReleaseMutex();
                }

                // аритмия
                if (heartState == Rythms.arrhythmia)
                {
                    HeartDeviation();

                    // QRS сокращение желудочков
                    Thread.Sleep(75 + TranslateToMiliseconds(heartRate) + rand.Next(-400, 400));
                    mutex.WaitOne();
                    monitor.SetImpulse(-0.4f + (float)(rand.NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(0.75f + (float)(rand.NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(-0.3f + (float)(rand.NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(0f);
                    // T расслабление желудочков
                    Thread.Sleep(100);
                    monitor.SetImpulse(0.3f + (float)rand.NextDouble() / 50);
                    Thread.Sleep(25);
                    monitor.SetImpulse(0f);
                    mutex.ReleaseMutex();
                }

                // мерцательная аритмия
                if (heartState == Rythms.atrialFibrillation)
                {
                    HeartDeviation();

                    // QRS сокращение желудочков
                    Thread.Sleep(75 + TranslateToMiliseconds(heartRate));
                    mutex.WaitOne();
                    monitor.SetImpulse(-0.4f + (float)(rand.NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(0.75f + (float)(rand.NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(-0.3f + (float)(rand.NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(0f);
                    // T расслабление желудочков
                    Thread.Sleep(100);
                    monitor.SetImpulse(0.3f + (float)(rand.NextDouble() / 50));
                    Thread.Sleep(25);
                    monitor.SetImpulse(0f);
                    mutex.ReleaseMutex();
                }

                // крупноволновая фибрилляция желудочков
                if (heartState == Rythms.coarseVentricularFibrillation)
                {
                    HeartDeviation();

                    // сокращение желудочков
                    mutex.WaitOne();
                    monitor.SetImpulse(0.5f + (float)(rand.Next(-10,10)) / 50);
                    Thread.Sleep(25);
                    monitor.SetImpulse(0);
                    Thread.Sleep(rand.Next(0,heartRate));
                    monitor.SetImpulse((0.5f + (float)(rand.Next(-10, 10)) / (4000 / heartRate)) * -1);
                    Thread.Sleep(25);
                    monitor.SetImpulse(0.5f);
                    mutex.ReleaseMutex();
                }

                // мелковолновая фибрилляция желудочоков
                if (heartState == Rythms.ventricularFibrillation)
                {
                    HeartDeviation();

                    // сокращение желудочков
                    mutex.WaitOne();
                    monitor.SetImpulse((float)(rand.NextDouble() / (400 / heartRate)) * new Random().Next(-1,2));
                    Thread.Sleep(25);
                    monitor.SetImpulse(0f);
                }

                Thread.Sleep(1);
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
                    monitor.SetImpulse(0.2f + (float)(rand.NextDouble() / 50));
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
                    Thread.Sleep(TranslateToMiliseconds(heartRate) + rand.Next(-300, 300));
                    monitor.SetImpulse(0.2f + (float)(rand.NextDouble() / 50));
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
                    Thread.Sleep(TranslateToMiliseconds(heartRate + rand.Next(200, 500)));
                    monitor.SetImpulse(0.2f + (float)(rand.NextDouble() / 15));
                    Thread.Sleep(25);
                    monitor.SetImpulse(0f);
                    Thread.Sleep(100);
                    mutex.ReleaseMutex();
                }

                Thread.Sleep(1);
            }
        }

        private void HeartDeviation()
        {
            int bHeartRate = 80 - heartOffset; // стандартный ритм сердца (base)
            int dHeartRate = 200 - heartOffset - (int)heartDamage; // серьезно нарушенный ритм сердца (damaged)
            if(heartState == Rythms.sinusRhythm)
            {
                heartRate = rand.Next(bHeartRate - HEART_DEVIATION, bHeartRate + HEART_DEVIATION);
            }
            if(heartState == Rythms.arrhythmia)
            {
                heartRate = rand.Next(bHeartRate - HEART_DEVIATION * 4, bHeartRate + HEART_DEVIATION * 4);
            }
            if(heartState == Rythms.atrialFibrillation)
            {
                heartRate = rand.Next(bHeartRate - HEART_DEVIATION, bHeartRate + HEART_DEVIATION * 3);
            }
            if(heartState == Rythms.coarseVentricularFibrillation)
            {
                heartRate = rand.Next(dHeartRate - HEART_DEVIATION * 4, dHeartRate + HEART_DEVIATION * 4);
                heartDamage += 0.1f;
            }
            if(heartState == Rythms.ventricularFibrillation)
            {
                heartRate = rand.Next(dHeartRate - HEART_DEVIATION * 4, dHeartRate + HEART_DEVIATION * 4);
                heartDamage += 0.05f;
            }
        }

        // переводит удары в минуту в удар за милисекунды
        public int TranslateToMiliseconds(float beats) => (int)(60 / beats * 1000);

        internal void Debug(KeyboardState ks)
        {
            if (ks.IsKeyDown(Keys.Up))
                heartDamage++;
            if (ks.IsKeyDown(Keys.Down) && heartRate > 1)
                heartDamage--;

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

            // повреждение тканей сердца
            if (heartDamage > 25)
                heartState = Rythms.coarseVentricularFibrillation;
            if (heartDamage > 50)
                heartState = Rythms.ventricularFibrillation;
            if (heartDamage > 100)
                heartState = Rythms.asystole;
            if (heartDamage > 200)
                isDeath = true;

            if (heartState == Rythms.asystole)
            {
                if (heartRate > 0)
                    heartRate--;

                heartDamage += 0.05f;
            }
        }
    }
}
