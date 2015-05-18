using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Process
{
    class ProcessInterface
    {

        /*
         *  Prezentare
        Alimentarea cu apa a unei retele se realizeaza dintr-un put de adancime cu ajutorul a 4 pompe.
        Pentru a mentine presiunea din retea aproape constanta, un rezervor este utilizat. Controlul
        sistemului se face cu ajutorul unui automat programabil.
        * */
        /*
         *  Descriere functionala 1
Toate cele 4 pompe au aceeasi capacitate de pompare. Pompele pot fi oprite individual prin apasarea
butoanelor S1, S2, S3 si S4. Disponibilitatea pompelor este indicata de lampile de semnalizare P1, P2,
P3 si P4. Prin apasarea butonului S0 toate pompele sunt oprite simultan. Daca presiunea indicata de
senzorul B2 scade sub presiunea setata pentru mai mult de 1s, o pompa suplimentara este pornita la
fiecare interval de 1s, pana cand valoarea corespunde cu cea setata; daca presiunea creste peste
valoarea setata, o pompa este oprita la fiecare interval de 1s, pana cand valoarea corespunde cu cea
setata.
Pentru a preveni cresterea presiunii la valori catastrofale in cazul defectarii senzorului B2, senzorul B1
confera redundanta sistemului. In cazul in care senzorul B1 este activat, semnalul de alarma trebuie sa
sune pentru 5 secunde.*/

        /*
         * • Descriere functionala 2
Pentru a evita uzura inegala a pompelor, in plus fata de descrierea functionala 1, numarul de porniri al
fiecarei pompe trebuie sa fie egal. Acest lucru este obtinut prin oprirea de fiecare data a pompei care
functioneaza de cel mai mult timp si prin pornirea celei care este oprita de cel mai mult timp.
Nota: Rata de descarcare se va seta cu ajutorul potentiometrului 1.
         * */
        public Byte[] inputs = new Byte[16];
        public Byte[] outputs = new Byte[16];
        private TcpClient client;
        private NetworkStream stream;
        IProcessObs obs;
        System.Timers.Timer t5 = new System.Timers.Timer();
        bool modAutomat = true;
        Object olock = new Object();
        bool start = false;
        enum Sensors
	    {   
            s0 = 0,
            s1 = 1,
            s2 = 2,
            s3 = 3,
            s4 = 4,
	        b1 = 6, // corect?
            b2 = 7,
            b3 = 80,
            b4 = 81
	    }
        public bool[] pompe_stare = new bool[5]; 
        Dictionary<int, int> pompe = new Dictionary<int, int>();


        public ProcessInterface(IProcessObs obs)
        {
            try
            {
                this.obs = obs;
                Int32 port = 2000;
                IPAddress localAddr = IPAddress.Parse("10.8.223.54");
                TcpListener server = new TcpListener(localAddr, port); //server for TRCV_C  ??
                server.Start();

                Console.Write("Waiting for a connection... ");
                client = server.AcceptTcpClient();
                Console.WriteLine("Connected!");
                stream = client.GetStream();
                t5.Interval = 5000;
                t5.Elapsed += timer1;
                pompe_stare[0] = false;
                pompe_stare[1] = false;
                pompe_stare[2] = false;
                pompe_stare[3] = false;
                pompe_stare[4] = false;
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

        }

        private void timer1(object sender, ElapsedEventArgs e)
        {
            DezactivareAlarma();
            t5.Stop();

        }

        public void CloseConnection()
        {
            client.Close();
        }

        public void Send()
        {
            stream.Write(outputs, 0, outputs.Length);
        }

        public void Receive()
        {
            //while(true)
            if (stream.Read(inputs, 0, inputs.Length) != 16)
                Console.WriteLine("Read Eroor !");
            else
            Console.WriteLine(String.Format("Received: {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", inputs[0],
                        inputs[1], inputs[2], inputs[3], inputs[4], inputs[5], inputs[6], inputs[7], inputs[8], inputs[9]));
        
        }

        public void Receive(Byte[] data, int length = 16)
        {
            if (stream.Read(data, 0, length) != 16)
                Console.WriteLine("Read Eroor !");
            Console.WriteLine(String.Format("Received: {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", data[0],
                        data[1], data[2], data[3], data[4], data[5], data[6], data[7], data[8], data[9]));

        }

        public void Send(Byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }

        public void Refresh()
        {
            /*
             * send all writes and receive curent state of ix.x for read
             */
                Send();
                Receive();
        }

        public void Write(byte qAddress, bool value)
        {
            /*
             * qAddress exemple :
             * for q0.0 => qAddress = 0
             * q0.1 = 1 ...
             * q8.3 = 83 ...
             */
            lock (olock)
            {
                byte a;
                if (value == true)
                { //Setting a bit
                    if (qAddress < 8)
                        outputs[0] |= Convert.ToByte(1 << qAddress);
                    else
                        if (qAddress >= 80)
                            outputs[8] |= Convert.ToByte(1 << qAddress / 8);
                }
                else
                { //Clearin a bit
                    if (qAddress < 8)
                        outputs[0] &= (Convert.ToByte(~(Convert.ToByte((byte)1) << qAddress)));
                    {
                        a = (byte)(1 << qAddress);
                        a = (byte)(~a);
                        outputs[0] &= a;
                    }
                    if (qAddress >= 80)
                        outputs[8] &= Convert.ToByte(~((byte)1 << qAddress / 8));
                }

            }

        }

        public bool Read(byte iAddress)
        {
            /*
             * iAddress exemple :
             * for i0.0 => iAddress = 0
             * i0.1 = 1 ...
             * i8.3 = 83 ...
             */
            lock (olock)
            {
                if (iAddress < 8)
                {
                    if (((inputs[0] >> Convert.ToByte(iAddress)) & 1) == 1)
                        return true;
                    else
                        return false;
                }
                else
                    if (iAddress >= 80)
                        if (((inputs[0] >> Convert.ToByte(iAddress / 8)) & 1) == 1)
                            return true;
                        else
                            return false;

                return true;
            }
        }

        public void ModAutomat()
        {
            if (modAutomat == false)
            {
                modAutomat = true;
                (new Thread(() =>
                {
                    while (modAutomat)
                    {

                        if (Read((byte)Sensors.b2) == false)
                        {
                            StartPompaDisponibila();
                            
                        }
                        else {
                            StopPompaDisponibila();
                        }
                        Thread.Sleep(1000);
                        if (Read((byte)Sensors.b1) == true) {
                            ActivareAlarma(5);
                        }

                        if (Read((byte)Sensors.s0) == true) {
                            Opreste_Pompe();
                        }
                        if (Read((byte)Sensors.s1) == true)
                        {
                            Opreste_Pompa(1);
                        }
                        if (Read((byte)Sensors.s2) == true)
                        {
                            Opreste_Pompa(2);
                        }
                        if (Read((byte)Sensors.s3) == true)
                        {
                            Opreste_Pompa(3);
                        }
                        if (Read((byte)Sensors.s4) == true)
                        {
                            Opreste_Pompa(4);
                        }

                    }

                })).Start();
            }
        }

        private void StopPompaDisponibila()
        {
            byte i = 0;
            for (i = 1; i < 5; i++)
            {
                if (pompe_stare[i] == true)
                {
                    Opreste_Pompa(i);
                    break;
                }

            }
        }

        private void StartPompaDisponibila()
        {
            byte i = 0;
            for (i = 1; i < 5; i++)
            {
                if (pompe_stare[i] == false)
                {
                    Porneste_Pompa(i);
                    break;
                }

            }
        }
        
        public void ModManual()
        {
            modAutomat = false;
        }

        public void Start()
        {
            Byte[] i = new Byte[16];
            Byte[] o = new Byte[16];
            if (start == false)
            {
                start = true;
                (new Thread(() =>
                {
                    while (start)
                    {
                        Receive(i);
                        lock (olock)
                        {
                            if (i[0] != inputs[0] || i[8] != inputs[8])
                            {
                                inputs[0] = i[0];
                                inputs[8] = i[8];
                                obs.stareI(i);
                            }
                            if (o[0] != outputs[0] || o[8] != outputs[8])
                            {
                                o[0] = outputs[0];
                                o[8] = outputs[8];
                                obs.stareO(o);
                            }           
                        }
                        Send(o);
                    }

                })).Start();
              
            }
        }


        public void Opreste_Pompa(byte pompa)
        {
            Write(pompa,false);
            pompe_stare[pompa] = false;
        }

        public void Porneste_Pompa(byte pompa)
        {
            Write(pompa, true);
            pompe_stare[pompa] = true;
        }

        public void Toggle_Pompa(byte pompa)
        {
            if (pompe_stare[pompa] == false)
                Porneste_Pompa(pompa);
            else
                Opreste_Pompa(pompa);
        }

        public void Opreste_Pompe()
        {
            Opreste_Pompa(1);
            Opreste_Pompa(2);
            Opreste_Pompa(3);
            Opreste_Pompa(4);
        }
        
        public void ActivareAlarma(byte sec)
        {
            
            if (t5.Enabled == false)
            {
                Write(5, true);
                t5.Interval = 1000*sec;
                t5.Start();
            }
        }

        public void DezactivareAlarma()
        {
            Write(5, false);
        }

        public void reset()
        {
            
            outputs[0] = 0;
            outputs[8] = 0;
            Send();
        }




    }
}
