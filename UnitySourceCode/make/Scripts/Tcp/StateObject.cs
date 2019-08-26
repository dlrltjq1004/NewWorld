// tcpClient.java by fpont 3/2000

// usage : java tcpClient <server> <port>
// default port is 1500
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class StateObject : MonoBehaviour
{
    // Client socket.
    public Socket workSocket = null;
    // Size of receive buffer.
    public const int BufferSize = 256;
    // Receive buffer.
    public byte[] buffer = new byte[BufferSize];
    // Received data string.
    public StringBuilder sb = new StringBuilder();
}

public class tcpClient
{
    ManualResetEvent receiveDone = new ManualResetEvent(false);




    [STAThread]
    public static void Main(System.String[] args)
    {

        int port = 8888;
        //		System.String server = "localhost";
        System.String server = "13.125.250.235";
        System.Net.Sockets.Socket socket = null;
        System.String lineToBeSent;
        System.IO.StreamReader input;
        System.IO.StreamWriter output;
        int ERROR = 1;

        // read arguments 
        if (args.Length == 2)
        {
            server = args[0];
            try
            {
                port = System.Int32.Parse(args[1]);
            }
            catch (System.Exception e)
            {
                System.Console.Out.WriteLine("server port = 8888 (default)");
                port = 8888;
            }
        }



        // connect to server
        try
        {

            //UPGRADE_ISSUE: Method 'java.net.Socket.getInetAddress' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javanetSocketgetInetAddress"'
            //UPGRADE_ISSUE: Method 'java.net.Socket.getPort' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javanetSocketgetPort"'
            //System.Console.Out.WriteLine("Connected with server " + socket.getInetAddress() + ":" + socket.getPort());
        }
        catch (System.Exception e)
        {
            System.Console.Out.WriteLine(e);
            System.Environment.Exit(ERROR);
        }



        try
        {
            //UPGRADE_TODO: Expected value of parameters of constructor 'java.io.BufferedReader.BufferedReader' are different in the equivalent in .NET. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1092"'
            //UPGRADE_ISSUE: 'java.lang.System.in' was converted to 'System.Console.In' which is not valid in this expression. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1109"'
            //			input = new System.IO.StreamReader(new System.IO.StreamReader(System.Console.In).BaseStream, System.Text.Encoding.UTF7);
            //			System.IO.StreamWriter temp_writer;
            //			temp_writer = new System.IO.StreamWriter((System.IO.Stream) socket.GetStream());
            //			temp_writer.AutoFlush = true;
            //			output = temp_writer;

            // get user input and transmit it to server
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();


            socket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse("13.125.250.235");
            System.Net.IPEndPoint remoteEP = new System.Net.IPEndPoint(ipAdd, 8888);

            socket.Connect(remoteEP);
            //Async Read form the server side
            Receive(socket);
            //new System.Net.Sockets.TcpClient(server, port);
            while (true)
            {
                lineToBeSent = System.Console.ReadLine();

                // stop if input line is "."
                if (lineToBeSent.Equals("."))
                {
                    socket.Close();
                    break;
                }
                //output.WriteLine(lineToBeSent);\
                //System.Net.Sockets.NetworkStream tempstream = socket.GetStream();
                socket.Send(encoding.GetBytes(lineToBeSent));
            }
            //socket.Close();



            byte[] Serbyte = new byte[30];
            //			socket.Receive(Serbyte,0,20,System.Net.Sockets.SocketFlags.None);
            //			System.Console.WriteLine("Message from server \n" + encoding.GetString(Serbyte));

            //socket.Close();


        }
        catch (System.IO.IOException e)
        {
            System.Console.Out.WriteLine(e);
        }

        try
        {

        }
        catch (System.IO.IOException e)
        {
            System.Console.Out.WriteLine(e);
        }
    }

    private static void Receive(Socket client)
    {
        try
        {
            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = client;

            // Begin receiving the data from the remote device.
            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReceiveCallback), state);

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    static string response = "";
    private static void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the state object and the client socket 
            // from the asynchronous state object.

            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;
            // Read data from the remote device.
            int bytesRead = client.EndReceive(ar);
            if (bytesRead > 0)
            {
                // There might be more data, so store the data received so far.
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                //  Get the rest of the data.
                if (state.sb.Length > 1)
                {
                    response = Encoding.ASCII.GetString(state.buffer, 0, bytesRead).ToString();
                    Console.WriteLine(response);
                }
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            else
            {
                // All the data has arrived; put it in response.
                if (state.sb.Length > 1)
                {
                    response = state.sb.ToString();
                    Console.WriteLine(response);
                }
                // Signal that all bytes have been received.

                //receiveDone.Set();

            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }


}