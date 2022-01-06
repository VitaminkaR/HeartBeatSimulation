﻿using System;
using System.Threading;

namespace HeartBitSimulation
{
    class Heart
    {
        // сокращения
        private Thread ventricleThread;
        private Thread atrialThread;
        private Mutex mutex = new Mutex();

        public Monitor monitor;

        //// основные переменные сердца
        // частота сердченых сокращений ms
        public int heartRate;

        public Heart(Monitor _monitor)
        {
            heartRate = 75;
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
        }

        private void AtrialWork()
        {
            while (true)
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
        }

        // переводит удары в минуту в удар за милисекунды
        public int TranslateToMiliseconds(float beats) => (int)(60 / beats * 1000);
    }
}