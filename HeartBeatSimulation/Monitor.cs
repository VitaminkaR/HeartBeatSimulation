using Microsoft.Xna.Framework;
using System;
using System.Threading;

namespace HeartBitSimulation
{
    class Monitor
    {
        public const float GRAPH_SPEED = 0.01f; // на сколько будет сдвигатся
        public const int GRAPH_UPDATE = 5; // var/1000 sec, работа таймера(период)

        private Mutex mutex = new Mutex();

        // движение графа
        private TimerCallback graphUpdate;
        private Timer timer;

        // переменная электрического импульса
        private float electricImpulse;

        // доступ к рисованию
        Graphics graphics;

        // отчистка монитора
        private void ClearMonitor() => graphics.DelAllVertex();

        // начало нового графа
        private void NewGraph()
        {
            ClearMonitor();
            graphics.AddVertex(new Vector3(-2.1f, 0, 0), Color.Green);
        }

        public void Init()
        {
            graphUpdate = new TimerCallback(GraphUpdate);
            timer = new Timer(graphUpdate, null, 0, GRAPH_UPDATE);
            graphics = HBSimGame.graphics;
            NewGraph();
        }

        private void GraphUpdate(object obj)
        {
            mutex.WaitOne();
            if (graphics.VNotNull())
            {
                Vector3 v_position = graphics.GetLastVertex().Position;
                Vector3 nv_position = new Vector3(v_position.X + GRAPH_SPEED, electricImpulse, 0);
                graphics.AddVertex(nv_position, Color.Green);

                if (v_position.X > 2.1)
                {
                    NewGraph();
                }
            }

            HBSimGame.meter?.Update();
            mutex.ReleaseMutex();
        }

        public void SetImpulse(float impulse)
        {
            electricImpulse = impulse;
        }
    }
}
