using System;
using System.Collections.Generic;
using Sorting;

namespace AirportQueue
{
    public class PassengerConsumer
    {
        public List<Plane> Planes;
        public IPriorityQueue<Passenger> Queue;
        public int ProcessingTicksLeft = 0;
        private Passenger _passenger;

        public PassengerConsumer(List<Plane> planes, IPriorityQueue<Passenger> queue)
        {
            Planes = planes;
            Queue = queue;
        }

        public void Tick(Clock clock)
        {
            if (ProcessingTicksLeft > 0)
            {
                ProcessingTicksLeft--;
                return;
            }

            if (_passenger != null)
            {
                var now = clock.Time;
                if (_passenger.Plane.DepartureTime.CompareTo(now) < 0)
                {
                    _passenger.Status = Status.MissedPlane;
                    Console.WriteLine($"Passenger {_passenger} missed the plane");
                }
                else
                {
                    _passenger.Status = Status.Boarded;
                    Console.WriteLine($"Passenger {_passenger} has boarded");
                }
            }

            if (Queue.IsEmpty()) return;
            
            _passenger = Queue.Dequeue();
            switch (_passenger.Category)
            {
                case Category.LateToFlight:
                    ProcessingTicksLeft = 150;
                    break;
                case Category.BusinessClass:
                    ProcessingTicksLeft = 150;
                    break;
                case Category.Disabled:
                    ProcessingTicksLeft = 250;
                    break;
                case Category.Family:
                    ProcessingTicksLeft = 250;
                    break;
                case Category.Monkey:
                    ProcessingTicksLeft = 150;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}