﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WinAPIDemo.ChatClient.Model;
using static WinAPIDemo.WinAPISignatures.Ws2_32;

namespace WinAPIDemo.ChatClient.Common
{
    public class ConnectionHandler
    {
        public IntPtr Connect(string ipaddr, string port)
        {
            WSAData wsaData;
            //RESULT OF MAKEWORD(2,2) MACRO -/ (512 + 2) -> 514
            //We indicate that we want to use WinSocket in version 2.2
            Int16 wVersionRequested = 0b0000_0010_0000_0010;
            int result = WSAStartup(wVersionRequested, out wsaData);
            if (result != 0)
            {
                //TextBlock.Text += "Initialization Error!\n";
                //return;
            }
            //TextBlock.Text += "Initialization Succedeed!\n";

            IntPtr _socket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
            if ((uint)_socket == INVALID_SOCKET)
            {
                //TextBlock.Text += "INVALID_SOCKET\n";
                //WSACleanup();
                //return;
            }
            //TextBlock.Text += "Socket initialized.\n";

            sockaddr_in service = new sockaddr_in();
            service.sin_addr = new in_addr();
            service.sin_family = AF_INET;
            service.sin_addr.s_addr = (uint)IPAddress.Parse(ipaddr).Address;
            service.sin_port = ushort.Parse(port);

            var iResult = connect(_socket, ref service, Marshal.SizeOf(service));

            if (iResult == SOCKET_ERROR)
                return IntPtr.Zero;

            return _socket;
        }

        public void Disconnect(Client client)
        {
            var iResult = shutdown(client.Socket, 1);
            if (iResult == SOCKET_ERROR)
            {
                //ERROR
                var err = WSAGetLastError();
            }
            closesocket(client.Socket);
            WSACleanup();
        }
    }
}