using System;
using System.Diagnostics;

namespace Sorting
{
    public class ArrayPriorityQueue<T> : IPriorityQueue<T> where T : IComparable<T>
    {
        public T[] Queue { get; }
        public int HeapSize { get; private set; }

        // Max size of the entire tree
        private int MaxHeapSize { get; }


        public ArrayPriorityQueue(int heapSize)
        {
            Queue = new T[heapSize];
            HeapSize = 0;
            MaxHeapSize = heapSize;
            // Copy contents of items array to the queue.
        }


        /// <summary>
        /// Heapsort using a lambda expression. Example: arraySorter.Sort((x,y) => x.compareTo(y) > 0); for an ascending sort
        /// </summary>
        /// <param name="lambda">A lambda expression with 2 types T and should return a bool</param>
        public void Sort(Func<T, T, bool> lambda)
        {
            var length = HeapSize;
            BuildHeap(lambda);

            for (var i = length - 1; i >= 0; i--)
            {
                // Move current root to end 
                Exchange(Queue, 0, i);

                // call heapify on the reduced heap 
                Heapify(Queue, i, 0, lambda);
            }

            // Get the sorting order by comparing root to the last element
        }

        private void BuildHeap(Func<T, T, bool> lambda)
        {
            for (var i = HeapSize / 2 - 1; i >= 0; i--)
            {
                Heapify(Queue, HeapSize, i, lambda);
            }
        }


        /// <summary>
        /// Heapifies using a lambda expression to compare children
        /// </summary>
        /// <param name="arr">Array to heapify</param>
        /// <param name="size">Size of heap</param>
        /// <param name="i">Current index</param>
        /// <param name="lambda">Lambda expression to compare children</param>
        private void Heapify(T[] arr, int size, int i, Func<T, T, bool> lambda)
        {
            var parent = i;
            var leftChild = i * 2 + 1;
            var rightChild = i * 2 + 2;

            if (leftChild < size && lambda(arr[leftChild], arr[parent])) parent = leftChild;
            if (rightChild < size && lambda(arr[rightChild], arr[parent])) parent = rightChild;
            if (parent != i)
            {
                Exchange(arr, i, parent);
                Heapify(arr, size, parent, lambda);
            }
        }

        /// <summary>
        /// Insert an item into the queue
        /// </summary>
        /// <param name="item">Item to be inserted</param>
        /// <exception cref="Exception">Throws exception if queue is full</exception>
        public void Enqueue(T item)
        {
            if (HeapSize == MaxHeapSize) throw new Exception("Queue is full!");
            Queue[HeapSize++] = item;

            BuildHeap((x, y) => x.CompareTo(y) < 0);
        }

        /// <summary>
        /// Dequeue the root of the tree
        /// </summary>
        /// <returns>Value of type T</returns>
        /// <exception cref="Exception">Throws exception if the queue is empty</exception>
        public T Dequeue()
        {
            if (HeapSize == 0)
                throw new Exception("Queue is empty, can't dequeue.");
            var root = Queue[0];

            // Move last element to the root
            Queue[0] = Queue[--HeapSize];
            Queue[HeapSize] = default;

            BuildHeap((x, y) => x.CompareTo(y) < 0);
            return root;
        }

        public T Peek()
        {
            return Queue[0];
        }

        /// <summary>
        /// Switches the index of 2 values
        /// </summary>
        /// <param name="arr">Array in which the switch should take place</param>
        /// <param name="firstIndex"></param>
        /// <param name="secondIndex"></param>
        private void Exchange(T[] arr, int firstIndex, int secondIndex)
        {
            var temp = arr[firstIndex];
            arr[firstIndex] = arr[secondIndex];
            arr[secondIndex] = temp;
        }

        public bool IsEmpty()
        {
            return HeapSize == 0;
        }
    }
}