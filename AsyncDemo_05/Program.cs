using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncDemo_05
{
    //在模拟买盐的基础上，同时开启多个Task的情况
    class Program
    {
        static void Main(string[] args)
        {
            AsyncTest();
            Console.ReadKey();
        }
        public static void AsyncTest()
        {
            Console.WriteLine($"AsyncTest()方法开始执行,线程：{Thread.CurrentThread.ManagedThreadId}");
            Task task = Test1();
            Console.WriteLine($"AsyncTest()方法继续执行,线程：{Thread.CurrentThread.ManagedThreadId}");
            task.Wait();
            Console.WriteLine($"AsyncTest()方法结束执行,线程：{Thread.CurrentThread.ManagedThreadId}");
        }
        //当方法中有多个 wait 时，会依次执行所有的Task，只有当所有的Task执行完成后，才表示异步方法执行完成，当前线程才得以执行
        public static async Task Test1()
        {
            Console.WriteLine($"Test1(1),线程：{Thread.CurrentThread.ManagedThreadId}");
            await Task.Factory.StartNew((state) =>
            {
                Console.WriteLine($"Test1({ state } 开始执行),线程：{Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);
                Console.WriteLine($"Test1({ state } 结束执行),线程：{Thread.CurrentThread.ManagedThreadId}");
            },"task1");

            await Task.Factory.StartNew((state) =>
            {
                Console.WriteLine($"Test1({ state } 开始执行),线程：{Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);
                Console.WriteLine($"Test1({ state } 结束执行),线程：{Thread.CurrentThread.ManagedThreadId}");
            }, "task2");

            Console.WriteLine($"Test1（）方法执行结束,线程：{Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
