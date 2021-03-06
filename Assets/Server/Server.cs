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
    static string ipAdress = "94.46.42.40";
    static int port = 22001;
    static byte[] buffer = new byte[65535];
    static MemoryStream mem = new MemoryStream();
    public LDTimeLine RequestTimeLine()
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
        catch (ArgumentNullException e){ Debug.Log($"ArgumentNullException: {e}"); }
        catch (SocketException e){ Debug.Log($"SocketException: {e}");}
        return timeline;
    }
    public bool PushTimeLine(LDTimeLineBranchRequest branch)
    {
        bool success = false;
        try
        {
            // Create a TcpClient.
            TcpClient client = new TcpClient(ipAdress, port);
            mem.Seek(0, SeekOrigin.Begin);
            // Send request message to server
            WritableSystem.Write(mem, branch);
            int pos = (int)mem.Position;
            mem.Seek(0, SeekOrigin.Begin);
            int length = mem.Read(buffer, 0, pos);

            mem.Seek(0, SeekOrigin.Begin);
            LDTimeLineBranchRequest debug_equest = WritableSystem.Read<LDTimeLineBranchRequest>(mem);


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

[Writable]
public struct LDTimeLine
{
    public LDBlock[] timeLine;
}
[Writable]
public struct LDTimeLineBranchRequest
{
    public int branchBlockIndex;
    public LDBlock[] timeLine;
}
[Writable]
public struct LDBlock
{
    public ushort level;
    public LDAttribute[] mods;
    public LDCharacter[] characters;
    public int[] branches;
}
[Writable]
public struct LDCharacter
{
    public string name;
    public byte color;
    public byte role;
    public LDAttribute[] attributes;
    public LDInputFrame[] timeLine;
}
[Writable]
public struct LDAttribute
{
    public byte type;
    public ushort value;
}
[Writable]
public struct LDInputFrame
{
    public byte action;
    public ushort cell;
}
