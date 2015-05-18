using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading;

namespace WebApplication3.Hubs
{
    public class perHub : Hub
    {

        public perHub() {
            System.Diagnostics.Debug.WriteLine("perHub  Constructor:  " );
        }


        public void Send(string message, int val)
        {   
            if (message.Equals("s"))
                MainP.process.outputs[0] = 100;

            switch (message)
            {
                case "s": { 
                        switch(val){
                            case 0 : {
                                MainP.process.Opreste_Pompe();
                            }break;
                            default :
                                MainP.process.Opreste_Pompa((byte)val);
                                break;
                        }
                        
                    }
                    break;
                case "m": {
                        MainP.process.Toggle_Pompa((byte)val);
                    }break;
                    
                case "on_off": {
                    if (val == 1)
                        MainP.process.ModAutomat();
                    else
                        MainP.process.ModManual();
                    }
                    break;
            }
        }
    }
}