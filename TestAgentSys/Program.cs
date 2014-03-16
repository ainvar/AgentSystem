using AgentsSys;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestAgentSys
{
    class Program
    {
        static void Main(string[] args)
        {
            if (DateTime.Now.Hour > 18 && DateTime.Now.Minute > 10)
                Process.Start("shutdown", "/s /t 0");

            var crono = new Stopwatch();

            crono.Start();
            var aa = new AAgentRunner(40, 20);

            var max = 2000;
            for (var count = 1; count <= max; count++)
            {
                aa.Run(() => { LongProcessing(count.ToString()); });
            }

            aa.WaitAllAgentsCompletion();

            Console.WriteLine("Finito!!");
            crono.Stop();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(string.Format("eseguito in :{0:mm\\:ss}", crono.Elapsed));

            Console.ReadKey();
        }

        public static void LongProcessing(string unique)
        {
            Console.WriteLine(unique + "-longrunning thread id=" + Thread.CurrentThread.ManagedThreadId.ToString());
            Thread.Sleep(8000);
        }
    }
}
