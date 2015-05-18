using Microsoft.AspNet.SignalR;
using Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using WebApplication3.Hubs;

namespace WebApplication3
{
    class ProcessObs : IProcessObs
    {
        IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<perHub>();


        public override void stareI(Byte[] inputs)
        {
            hubContext.Clients.All.i0(inputs[0]);
            hubContext.Clients.All.i8(inputs[8]);
            System.Diagnostics.Debug.WriteLine("inputs:  " + inputs[0].ToString());
        }

        public override void stareO(Byte[] outputs)
        {
            hubContext.Clients.All.o0(outputs[0]);
            hubContext.Clients.All.o8(outputs[8]);
            System.Diagnostics.Debug.WriteLine("outputs:  " + outputs[0].ToString());
        }
    }



    class MainP : IProcessObs
    {
        static IProcessObs obs = new ProcessObs();
        public  static ProcessInterface process;
                

        internal static void Main()
        {
            process = new ProcessInterface(obs);
            process.Start();
            process.ModAutomat();
        }
    }
}