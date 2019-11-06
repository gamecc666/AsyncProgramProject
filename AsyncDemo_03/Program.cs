using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo_02
{
    //模拟打开电源开关（关心是否完成，返回Task类型）
    class Program
    {
        static void Main(string[] args)
        {
            OpenMainSwitch();
            Console.ReadKey();
        }

        public static void OpenMainSwitch()
        {
            Console.WriteLine($"我和老婆正在看电视，线程：{Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("突然断电了，快看下是不是跳闸了");
            Task task = CommandOpenMainSwitch();
            Console.WriteLine($"没电了先玩会手机吧，线程：{Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(1000);
            Console.WriteLine($"手机也没电了等着吧！线程：{Thread.CurrentThread.ManagedThreadId}");
            while (!task.IsCompleted)
            {
                Thread.Sleep(100);
            }
            Console.WriteLine($"又有电了我们继续玩吧！线程{Thread.CurrentThread.ManagedThreadId}");
        }

        public static async Task CommandOpenMainSwitch()
        {
            Console.WriteLine($"这个时候我准备去打开电源开关，线程：{Thread.CurrentThread.ManagedThreadId}");
            await Task.Run(() => {
                Console.WriteLine($"屁颠屁颠的去开电源，线程：{Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);
            });
            Console.WriteLine($"电源开关开了，线程：{Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
