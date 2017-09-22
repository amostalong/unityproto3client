using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using Google.Protobuf;
using System.IO;

public class SocketInstance
{
	public Socket socket;
	public const int buffSize = 256;
	public byte[] buff = new byte[buffSize];
}

public class Client : SingleObject<Client> {

	byte[] receiveBuff = new byte[512];
	Dictionary<IPEndPoint, SocketInstance> socketDic = new Dictionary<IPEndPoint, SocketInstance> ();

	SocketInstance socketInstance;

	public void CreateConnection(string ip, int port)
	{
		IPEndPoint end_point = new IPEndPoint (IPAddress.Parse (ip), port);

		if (!socketDic.ContainsKey (end_point)) {

			SocketInstance socketInstance = new SocketInstance();

			socketInstance.socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			socketInstance.socket.BeginConnect (end_point, OnConnectCallback, socketInstance);

			socketDic [end_point] = socketInstance;
		}

	}

	void OnConnectCallback(IAsyncResult result)
	{
		if (result.IsCompleted)
		{
			Debug.Log("青鑫， Socket Connect Finish");

			var s = result.AsyncState as SocketInstance;

			s.socket.EndConnect(result);

			s.socket.BeginReceive(s.buff, 0, s.buff.Length, 0, OnReceiveCallback, s);
		}
		else
		{
			Debug.Log("Qx:???");
		}
	}

	void OnReceiveCallback(IAsyncResult result)
	{

		var s = result.AsyncState as SocketInstance;

		int dataLength = s.socket.EndReceive(result);

		Debug.Log("receive data length: " + dataLength);

		if (dataLength > 0)
		{
			//Debug.Log("Qx: get: " + (Encoding.ASCII.GetString(s.buff, 0, dataLength)));

			using(var stream = new MemoryStream())
			{
				stream.Read(s.buff, 0 ,dataLength);

				Byte[] dst = new Byte[dataLength];

			 	ByteArray.Copy(s.buff, 0, dst, 0, dataLength);
				
				foreach(var  b in dst){
					Debug.Log("byte: {0:G}");
				}

				var data = Tproto.Test.Parser.ParseFrom(stream);

				Debug.Log(data.Label);
				Debug.Log(data.Type);
			}
		}
	}
}