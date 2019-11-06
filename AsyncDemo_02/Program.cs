using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo_01
{
    //模拟扔垃圾(不关心结果，返回void类型)
    public class Program
    {
        static void Main()
        {
            DropLitter();
            Console.ReadLine();
        }

        public static void DropLitter()
        {
            Console.WriteLine($"----------1---------线程id：{Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("垃圾满了扔了吧！");
            CommandDropLitter();
            Console.WriteLine($"不管了我继续打扫，线程：{Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(100);
            Console.WriteLine($"扫完了，线程：{Thread.CurrentThread.ManagedThreadId}");
        }

        public static async void CommandDropLitter()
        {
            Console.WriteLine($"这时候我准备扔垃圾，线程：{Thread.CurrentThread.ManagedThreadId}");
            await Task.Run(() => {
                Console.WriteLine($"屁颠屁颠扔垃圾,线程：{Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);
            });
            Console.WriteLine($"扔完了还有啥吩咐：线程id:{Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
