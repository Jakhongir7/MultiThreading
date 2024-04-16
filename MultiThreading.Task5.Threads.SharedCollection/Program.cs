/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static readonly List<int> sharedCollection = new List<int>();
        static readonly object lockObject = new object();

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            // Create the first task to add 10 elements to the collection
            Thread thread1 = new Thread(AddElements);
            thread1.Start();

            // Create the second task to print all elements in the collection after each adding
            Thread thread2 = new Thread(PrintCollection);
            thread2.Start();

            Console.ReadLine();
        }

        static void AddElements()
        {
            for (int i = 1; i <= 10; i++)
            {
                lock (lockObject)
                {
                    sharedCollection.Add(i);
                    Monitor.Pulse(lockObject); // Notify the other thread that a new element has been added
                }
                Thread.Sleep(100); // Sleep to simulate processing time
            }
        }

        static void PrintCollection()
        {
            while (true)
            {
                lock (lockObject)
                {
                    if (sharedCollection.Count == 0)
                    {
                        Monitor.Wait(lockObject); // Wait until there is at least one element in the collection
                    }

                    Console.Write("[");
                    for (int i = 0; i < sharedCollection.Count; i++)
                    {
                        Console.Write(sharedCollection[i]);
                        if (i < sharedCollection.Count - 1)
                        {
                            Console.Write(", ");
                        }
                    }
                    Console.WriteLine("]");

                    Thread.Sleep(500); // Sleep to allow other thread to add elements
                }
            }
        }
    }
}
