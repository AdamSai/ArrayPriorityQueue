using System;
using System.Collections.Generic;
using Sorting;

namespace AirportQueue
{
    public class PassengerProducer
    {
        public static int NextPassengerId = 1;
        public List<Plane> Planes;
        public IPriorityQueue<Passenger> Queue;
        public int ProcessingTicksLeft = 0;
        public Random Randomizer = new Random();
        public Time LastDepartureTime;

        public PassengerProducer(List<Plane> planes, IPriorityQueue<Passenger> queue)
        {
            Planes = planes;
            Queue = queue;
            LastDepartureTime = planes[planes.Count - 1].DepartureTime;
        }

        public void Tick(Clock clock)
        {
            if (ProcessingTicksLeft > 0)
            {
                ProcessingTicksLeft--;
                return;
            }

            var now = clock.Time;
            if (now.CompareTo(LastDepartureTime) >= 0)
            {
                clock.Stop();
                return;
            }

            Plane plane = null;
            while (plane == null)
            {
                foreach (var p in Planes)
                {
                    if(p.DepartureTime.CompareTo(now) < 0) continue;
                    if (Randomizer.Next(3) == 0)
                    {
                        plane = p;
                        break;
                    }
                }
            }

            var c = Randomizer.Next(100);
            Category category;
            if (plane.DepartureTime.Millis - now.Millis < 15 * 60 * 1000)
            {
                category = Category.LateToFlight;
            }
            else if (c < 50) category = Category.BusinessClass;
            else if (c < 70) category = Category.Disabled;
            else if (c < 90) category = Category.Family;
            else category = Category.Monkey;
            
            Passenger passenger = new Passenger(NextPassengerId++, now, category, plane);
            Console.WriteLine($"Passenger {passenger} added to queue");
            Queue.Enqueue(passenger);

            ProcessingTicksLeft = Randomizer.Next(120);

        }
    }
}