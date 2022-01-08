using System;
using System.Collections.Generic;
using System.Text;

namespace HeartBitSimulation
{
    class RateMeter
    {
        const int VALUES_COUNT = 100;

        private int[] values; // список значений
        private Heart heart;

        public RateMeter(Heart _heart)
        {
            heart = _heart;
            values = new int[VALUES_COUNT];
        }

        public string GetRate()
        {
            int meanValue = 0;
            for (int i = 0; i < values.Length; i++)
            {
                meanValue += values[i];
            }
            meanValue /= VALUES_COUNT;

            if (meanValue > 0)
                return meanValue.ToString();
            else
                return "--";
        }

        private int id;

        public void Update()
        {
            values[id] = heart.heartRate;

            if (id < VALUES_COUNT - 1)
                id++;
            else
                id = 0; 
        }
    }
}
