
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace AsynchronousProgramDemo
{
    class Program
    {
        /*15.2.5异步Main()方法*/
        private const string url = "http://www.cninnovation.com";
        static async Task Main()
        {
            //SynchronizedAPI();
            //AsynchronousPattern();
            //EventBasedAsyncPattern();
            await TaskBaseAsyncPatternAsync();
            Console.WriteLine("hello gamecc666!");
            Console.ReadKey();
        }

        /*15.2.4基于任务的异步模式TAP（>NETFramework4.5）
         * 4.5中对WebClient进行了更新，并提供了基于任务的异步模式
         * 该模式定义了一个带有“Async”后缀的方法，并返回一个 Task类型，由于之前已经提供了一个带Async的方法，所以这里就使用例如：DownloadStringTaskAsync()
         * async:在C#中表示包含了异步执行的函数，该函数的执行不会阻塞调用线程
         * await位于async函数中，声明了一个异步执行的入口，当程序运行时从该入口创建并进入一个异步线程环境，
         */
        private static async Task TaskBaseAsyncPatternAsync()
        {
            Console.WriteLine(nameof(TaskBaseAsyncPatternAsync));
            using (var client = new WebClient())
            {
                string content = await client.DownloadStringTaskAsync(url);
                Console.WriteLine(content.Substring(0, 100));
                Console.WriteLine();
            }
        }
        /*15.2.3基于事件的异步模式（.NetFramework2.0）
         * 该模式定义了一个带有“Async”后缀的方法；
         * 该实例依旧使用WebClient,和同步的DownloadString();WebClient提供了一个DownloadStringAsync()，当请求完成时会触发DownloadStringCompleted事件，可以检索结果并做处理
         * DownloadStringCompleted的类型是DownloadStringCompletedEventHandler（详细可参考：https://docs.microsoft.com/zh-cn/dotnet/api/system.net.downloadstringcompletedeventhandler?redirectedfrom=MSDN&view=netframework-4.8）
         */
        private static void EventBasedAsyncPattern()
        {
            Console.WriteLine(nameof(EventBasedAsyncPattern));
            using (var client = new WebClient())
            {
                client.DownloadStringCompleted += (sender, e) =>                  //这里不用匿名函数+lambda的话可以直接new DownloadStringCompletedEventHandler(callback)即可
                {                                                                 //其中callback(object sender,DownloadStringCompletedEventArgs event)
                    Console.WriteLine(e.Result.Substring(0, 100));
                };
                client.DownloadStringAsync(new Uri(url));
                Console.WriteLine();
            }
        }
        /*15.2.2异步模式（.NetFramework1.0）
         * 注意：该模式定义了BeginXXX(),EndXXX()方法，例如：如果有一个同步的DownloadString()则异步模式为：BeginDownloadString(),EndDownString()
         * 要求：BeginXXX()接受同步方法的所有输入参数；EndXXX()方法使用同步方法的所有输出参数，并按照同步方法的返回类型来返回结果
         *       BeginXXX()返回IAsyncResult类型的数据
         * 对于request.BeginGetResponse(ReadResponse, null)第二个参数的说明使用：（详细请见：https://technet.microsoft.com/zh-cn/windows/system.net.httpwebrequest.begingetresponse(v=vs.94)）
         *     RequestState myRequestState=new RequestState();
         *     myRequestState.request=request;
         *     request.BeginGetResponse(ReadResponse, myRequestState)即可；
         */
        private static void AsynchronousPattern()
        {
            Console.WriteLine(nameof(AsynchronousPattern));
            WebRequest request = WebRequest.Create(url);
            IAsyncResult result = request.BeginGetResponse(ReadResponse, null);

            void ReadResponse(IAsyncResult ar)
            {
                using (WebResponse response = request.EndGetResponse(ar))
                {
                    Stream stream = response.GetResponseStream();
                    var reader = new StreamReader(stream);
                    string content = reader.ReadToEnd();
                    Console.WriteLine(content.Substring(0, 100));
                    Console.WriteLine();                                          //只起到一个换行的大作用
                }
            }
        }
        /*15.2.1同步调用*/
        private static void SynchronizedAPI()
        {
            Console.WriteLine(nameof(SynchronizedAPI));                           //nameof=>得到元素的名字（可以避免使用反射）
            using (var client = new WebClient())
            {
                string content = client.DownloadString(url);
                Console.WriteLine(content.Substring(0, 100));
            }
            Console.WriteLine();                                                  //只起到一个换行的大作用
        }
    }
}
