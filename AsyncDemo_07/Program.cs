using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncDemo_07
{
    /*模拟买盐，多个CancellationTokenSource取消异步任务，以及注册取消后的回调委托方法*/
    class Program
    {
        static void Main(string[] args)
        {
            CookDinner_MultiCancelBuySalt();
            Console.ReadKey();
        }

        public static void CookDinner_MultiCancelBuySalt()
        {
            Console.WriteLine($"老婆开始做饭了，线程： {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"哎呀，没盐了");

            CancellationTokenSource source1 = new CancellationTokenSource();        //因为存在而取消
            CancellationTokenSource source2 = new CancellationTokenSource();        //因为放弃而取消

            //CreateLinkedTokenSource([CancellationToken[]])：创建一个将在指定的数组中任何源标记处于取消状态时，处于取消状态的CancellationTokenSource
            CancellationTokenSource source = CancellationTokenSource.CreateLinkedTokenSource(source1.Token, source2.Token);

            //注册取消时候的回调委托
            source1.Token.Register(()=> {
                Console.WriteLine($"这是因为---家里有盐---所以取消，线程： {Thread.CurrentThread.ManagedThreadId}");
            });
            source2.Token.Register((state) => {
                Console.WriteLine($"这是因为---{state}---所以取消，线程： {Thread.CurrentThread.ManagedThreadId}");
            },"不做了出去吃");
            source.Token.Register((state) => {
                Console.WriteLine($"这是因为---{state}---所以取消，线程： {Thread.CurrentThread.ManagedThreadId}");
            }, "没理由");

            Task<string> task = CommandBuySalt_MultiCancelBuySalt(source.Token);
            Console.WriteLine($"---等等好像不用买了---，线程： {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(100);
            string[] results = new string[] { "家里的盐", "不做了出去吃吧", "没理由" };
            Random r = new Random();
            switch(r.Next(1,4))
            {
                case 1:
                    source1.Cancel();
                    //source1.CancelAfter(3000);              指定多少毫秒之后才调用取消的回调方法
                    Console.WriteLine($"随机值为 1 ，既然有盐我就继续炒菜{results[0]}，线程： {Thread.CurrentThread.ManagedThreadId}");
                    break;
                case 2:
                    source2.Cancel();
                    Console.WriteLine($"随机值为 2 ， 我们出去吃不用买了{results[1]}，线程： {Thread.CurrentThread.ManagedThreadId}");
                    break;
                case 3:
                    source.Cancel();
                    Console.WriteLine($"随机值为 3 ，没理由就是不用买了{results[2]}，线程： {Thread.CurrentThread.ManagedThreadId}");
                    break;
            }
            Console.WriteLine($"最终的任务状态是： {task.Status} ,已完成： {task.IsCompleted},已取消： {task.IsCanceled},已失败：{task.IsFaulted}");
        }

        //通知我去买盐（又告诉我各种理由）
        public static async Task<string> CommandBuySalt_MultiCancelBuySalt(CancellationToken Token)
        {
            Console.WriteLine($"这时候我准备去买盐，线程： {Thread.CurrentThread.ManagedThreadId}");

            //已开始执行的任务不能被取消
            string result = await Task.Run(() =>
            {
                Console.WriteLine($"屁颠屁颠的去买盐，线程： {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);
            }, Token).ContinueWith((t)=> {
                Console.WriteLine($"盐已经买好了，线程： {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);

                return "盐已经买回来了，顺便买一包烟！";
            },Token);
            Console.WriteLine($"---{result}---，线程： {Thread.CurrentThread.ManagedThreadId}");
            return result;
        }
    }
}
