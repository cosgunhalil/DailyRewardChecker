using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class NtpServerConnection {

    private bool isConnected;

    public NtpServerConnection()
    {
        TestConnection();
    }

    public bool GetIsConnected()
    {
        return isConnected;
    }

    private void TestConnection()
    {
        var networkReachability  = Application.internetReachability;
        if (networkReachability == NetworkReachability.NotReachable)
        {
            isConnected = false;
        }
        else
        {
            isConnected = true;
        }
    }

    public DateTime GetTime()
    {
        const string ntpServer = "pool.ntp.org";
        var ntpData = new byte[48];
        ntpData[0] = 0x1B;

        var addresses = Dns.GetHostEntry(ntpServer).AddressList;
        var ipEndPoint = new IPEndPoint(addresses[0], 123);
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        socket.Connect(ipEndPoint);
        socket.Send(ntpData);
        socket.Receive(ntpData);
        socket.Close();

        ulong intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | (ulong)ntpData[43];
        ulong fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | (ulong)ntpData[47];

        var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
        var networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);

        return networkDateTime.ToLocalTime();
    }
}
