using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo_03
{
    //模拟去买盐（不关心是否完成，还要获取执行结果，返回Task<TResult>类型）
    class Program
    {
        static void Main(string[] args)
        {
            CookDinner();
            Console.ReadKey();
        }

        public static void CookDinner()
        {
            Console.WriteLine($"--老婆开始做饭了，线程：{Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("--哎呀没盐了");
            Task<string> task = CommandBuySalt();
            Console.WriteLine($"--不管他，继续炒菜，线程：{Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(1000);
            /*
             *  1：此处代码不严谨应该通过Task.IsComplote来判断；如果任务取消的话会报错
             *  2：task.Result=>会阻塞当前代码的执行到异步函数返回结果
             *  3：task.wait()=>会阻塞当前线程直到异步函数返回结果
             */
            string result = task.Result;              //必须等到买盐回来，（停止炒菜（阻塞线程))；
            Console.WriteLine($"--用盐炒菜就是香,{result}，线程：{Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"--老婆做好饭了，线程：{Thread.CurrentThread.ManagedThreadId}");
        }

        public static async Task<string> CommandBuySalt()
        {
            Console.WriteLine($"--这个时候我准备买盐了，线程：{Thread.CurrentThread.ManagedThreadId}");
            string result = await Task.Run(() => {
                Console.WriteLine($"--屁颠屁颠去买盐，线程：{Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);
                return "盐买回来了，顺便我买了一包烟";
            });
            Console.WriteLine($"--{result}，线程：{Thread.CurrentThread.ManagedThreadId}");

            return result;
        }
    }
}
