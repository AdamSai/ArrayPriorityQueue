using System;
using System.Collections.Generic;
using System.Threading;
using Sorting;

namespace AirportQueue
{
    class Program
    {
        private static readonly List<Plane> Planes = new List<Plane>();
        private static PassengerProducer _producer;
        private static List<PassengerConsumer> _consumers;
        private static IPriorityQueue<Passenger> _queue;
        private static Clock _clock;

        private static void Setup()
        {
            for (var hour = 7; hour <= 22; hour++)
            {
                Planes.Add(new Plane(new Time(hour, 00, 00)));
            }

            _queue = new ArrayPriorityQueue<Passenger>(10000);
//            _queue = new NotPrioritisingPassengerArrayQueue(10000);
            _producer = new PassengerProducer(Planes, _queue);
            _consumers = new List<PassengerConsumer>()
            {
                new PassengerConsumer(Planes, _queue), new PassengerConsumer(Planes, _queue)
            };
            _clock = new Clock(_producer, _consumers, new Time(05, 00, 00));
        }

        public static void Main(string[] args)
        {
            Setup();
            Console.WriteLine("Hello Airport");
            Thread newThread = new Thread(new ThreadStart(_clock.Run));
            newThread.Start();
        }
    }
}