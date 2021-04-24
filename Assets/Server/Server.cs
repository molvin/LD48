using GameStructure;
using Netkraft.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server
{
    static string ipAdress = "127.0.0.1";
    static int port = 13000;
    static byte[] buffer = new byte[65535];
    static MemoryStream mem = new MemoryStream();
    public static LDTimeLine RequestTimeLine()
    {
        LDTimeLine timeline = new LDTimeLine();
        try
        {
            // Create a TcpClient.
            TcpClient client = new TcpClient(ipAdress, port);

            // Send request message to server
            byte[] data = System.Text.Encoding.ASCII.GetBytes("Get me TimeLine");
            NetworkStream stream = client.GetStream();
            stream.Write(data, 0, data.Length);
            Debug.Log("Sent: request");

            // Receive the TcpServer.response.
            // Read the first batch of the TcpServer response bytes.
            int bytes = stream.Read(buffer, 0, buffer.Length);
            mem.Seek(0, SeekOrigin.Begin);
            mem.Write(buffer, 0, bytes);
            mem.Seek(0, SeekOrigin.Begin);
            timeline = WritableSystem.Read<LDTimeLine>(mem);
           
            // Close everything.
            stream.Close();
            client.Close();
        }
        catch (ArgumentNullException e){ Debug.Log("ArgumentNullException: {0}"); }
        catch (SocketException e){ Debug.Log("SocketException: {0}");}
        return timeline;
    }
    public static bool PushTimeLine(LDTimeLine TimeLine)
    {
        bool success = false;
        try
        {
            // Create a TcpClient.
            Int32 port = 13000;
            TcpClient client = new TcpClient(ipAdress, port);

            // Send request message to server
            WritableSystem.Write(mem, TimeLine);
            int pos = (int)mem.Position;
            mem.Seek(0, SeekOrigin.Begin);
            int length = mem.Read(buffer, 0, pos);

            //Send
            client.GetStream().Write(buffer, 0, length);
            success = true;

            client.GetStream().Close();
            client.Close();
        }
        catch (ArgumentNullException e) { Console.WriteLine("ArgumentNullException: {0}", e); }
        catch (SocketException e) { Console.WriteLine("SocketException: {0}", e); }
        return success;
    }
}
