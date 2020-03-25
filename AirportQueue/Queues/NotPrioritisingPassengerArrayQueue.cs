using System;
using AirportQueue;

namespace Sorting
{
    public class NotPrioritisingPassengerArrayQueue : IPriorityQueue<Passenger>
    {
        private Passenger[] items;
        public int Size { get; private set; } = 0;
        private int _head = 0;  // index of the current front item, if one exists
        private int _tail = 0; // index of next item to be added

        public NotPrioritisingPassengerArrayQueue(int capacity)
        {
            items = new Passenger[capacity];
        }
        
        public void Enqueue(Passenger item)
        {
            if (Size == items.Length)
                throw new Exception("Cannot add to full queue");
            items[_tail] = item;
            _tail = (_tail + 1) % items.Length;
            Size++;
        }
        
        public Passenger Dequeue() {
            if (Size == 0)
                throw new InvalidOperationException("Cannot remove from empty queue");
            var item = items[_head];
            items[_head] = null;
            _head = (_head + 1) % items.Length;
            Size--;
            return item;
        }
        
        public Passenger Peek() {
            if (Size == 0)
                throw new InvalidOperationException("Cannot peek into empty queue");
            return items[_head];
        }

        public bool IsEmpty()
        {
            return Size == 0;
        }
    }
}