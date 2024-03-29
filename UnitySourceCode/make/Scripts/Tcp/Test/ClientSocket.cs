﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ClientSocket : MonoBehaviour {



	// Use this for initialization
	void Start () {
        Debug.Log("전송");
        StartClient();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("space"))
        {
          
         
        }
      
      
    }

    public static void StartClient()
    {
        // Data buffer for incoming data.  
        byte[] bytes = new byte[1024];

        // Connect to a remote device.  
        try
        {
            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress ipAddre = IPAddress.Parse("13.125.250.235");
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddre,8888);
            
       // Establish the remote endpoint for the socket.  
       // This example uses port 11000 on the local computer.  
       //   IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
       //    IPAddress ipAddress = ipHostInfo.AddressList[0];
       //    IPEndPoint remoteEP = new IPEndPoint(ipAddress, 8888);

            // Create a TCP/IP  socket.  
            //     Socket sender = new Socket(ipAddress.AddressFamily,
            //         SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint. Catch any errors.  
            try
            {
                //      sender.Connect(remoteEP);
                sender.Connect(ipEndPoint);

                Debug.Log("Soecket connected to {0}"+sender.RemoteEndPoint.ToString());

             //   //\\Console.WriteLine("Socket connected to {0}",
             //       sender.RemoteEndPoint.ToString());

                // Encode the data string into a byte array.  
                byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

                // Send the data through the socket.  
                int bytesSent = sender.Send(msg);

                // Receive the response from the remote device.  
                int bytesRec = sender.Receive(bytes);
                Console.WriteLine("Echoed test = {0}",
                    Encoding.ASCII.GetString(bytes, 0, bytesRec));

                // Release the socket.  
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

}
