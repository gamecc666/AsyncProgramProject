using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncDemo_06
{
    /* 模拟买盐的基础上，如果发现其实家里还有盐，又告诉我不用买了(取消异步执行操作)
     * 需要使用到的只是点：
     *    1：System.Threading.CancellationTokenSourc =>
     *    2:System.Threading.Tasks.CancellationToken =>
     */
    class Program
    {
        static void Main()
        {
            CookDinner_CancelBuysalt();
            Console.ReadKey();
        }

        public static void CookDinner_CancelBuysalt()
        {
            Console.WriteLine($"老婆开始做饭了，线程id: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("哎呀,没盐了！");
            CancellationTokenSource soure = new CancellationTokenSource();
            Task<string> task = CommandBuySalt_CancelBuySalt(soure.Token);
            Console.WriteLine($"不管他继续炒菜，线程：{Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(100);

            string result = "家里的盐";
            if(!string.IsNullOrEmpty(result))
            {
                soure.Cancel();
                Console.WriteLine($"家里还有盐不用买了，线程:{Thread.CurrentThread.ManagedThreadId}");
            }
            else
            {
                //如果已经取消就不能在获得结果了（否则抛出TaskCanceledWxception的异常）
                //你都已经不要我买了，我拿什么给你
                result = task.Result;
            }
            soure.Dispose();
            Console.WriteLine($"既然家里有盐我就继续炒菜{result}，线程:{Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"老婆把饭做好了，线程:{Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"最终的任务是 {task.Status}, 已完成 {task.IsCompleted},已取消 {task.IsCanceled},已失败 {task.IsFaulted}，线程:{Thread.CurrentThread.ManagedThreadId}");
        }
        public static async Task<string> CommandBuySalt_CancelBuySalt(CancellationToken token)
        {
            Console.WriteLine($"这时我准备去买盐了，线程id: {Thread.CurrentThread.ManagedThreadId}");

            //已经开始的任务不能取消
            string result = await Task.Run(() =>
            {
                Console.WriteLine($"屁颠屁颠的去买盐，线id：{Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);
            }, token).ContinueWith((t)=> {//如果没有取消的话就继续执行；ContinueWith:当任务完成或者返回一个值的时候继续进行下一个
                Console.WriteLine($"盐已经买好，线程：{Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);
                return "盐已经买回来了，顺便我还买了一包烟";
            },token);
            Console.WriteLine($"{result},线程：{Thread.CurrentThread.ManagedThreadId}");

            return result;
        }
    }
}
