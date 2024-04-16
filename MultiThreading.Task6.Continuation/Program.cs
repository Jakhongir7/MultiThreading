/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading.Tasks;
using System.Threading;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            // feel free to add your code
            // Create the parent task
            Task parentTask = Task.Run(async () =>
            {
                // Simulate some work
                await Task.Delay(2000); // Simulate 2 seconds of work
                Console.WriteLine("Parent task completed");
                throw new Exception("Parent task failed");
            });

            // Continuation task should be executed regardless of the result of the parent task
            Task continuationTask1 = parentTask.ContinueWith((prevTask) =>
            {
                Console.WriteLine("Continuation task 1 executed regardless of the result of the parent task");
            });

            // Continuation task should be executed when the parent task was completed without success
            Task continuationTask2 = parentTask.ContinueWith((prevTask) =>
            {
                if (prevTask.IsFaulted)
                {
                    Console.WriteLine("Continuation task 2 executed when the parent task was completed without success");
                }
            }, TaskContinuationOptions.OnlyOnFaulted);

            // Continuation task should be executed when the parent task failed and parent task thread should be reused for continuation
            Task continuationTask3 = parentTask.ContinueWith((prevTask) =>
            {
                if (prevTask.IsFaulted)
                {
                    Console.WriteLine("Continuation task 3 executed when the parent task failed and parent task thread was reused for continuation");
                }
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            // Continuation task should be executed outside of the thread pool when the parent task is cancelled
            CancellationTokenSource cts = new CancellationTokenSource();
            Task continuationTask4 = parentTask.ContinueWith((prevTask) =>
            {
                if (prevTask.IsCanceled)
                {
                    Console.WriteLine("Continuation task 4 executed outside of the thread pool when the parent task is cancelled");
                }
            }, cts.Token, TaskContinuationOptions.RunContinuationsAsynchronously, TaskScheduler.Default);

            // Cancel the parent task after 1 second
            await Task.Delay(1000);
            cts.Cancel();

            // Wait for all tasks to complete
            await Task.WhenAll(parentTask, continuationTask1, continuationTask2, continuationTask3, continuationTask4);
            Console.ReadLine();
        }
    }
}
