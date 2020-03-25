using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.ConstrainedExecution;
using System.Threading;

namespace AirportQueue
{
    public class Clock
    {
        private const long SleepingTime = 100;
        private bool _running = true;
        private PassengerProducer _producer;
        private List<PassengerConsumer> _consumers;
        private long _millis;

        public Time Time => new Time(_millis);

        public Clock(PassengerProducer producer, List<PassengerConsumer> consumers, Time startTime)
        {
            _producer = producer;
            _consumers = consumers;
            _millis = startTime.Millis;
        }

        public void Stop()
        {
            _running = false;
        }

        public void Run()
        {
            try
            {
                while (_running)
                {
                    Thread.Sleep((int) SleepingTime);
                    _producer.Tick(this);
                    _consumers.ForEach(consumer => consumer.Tick(this));
                    _millis += 1000;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}