using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using System.IO;
using System.Text;

public class OldWebSocket : MonoBehaviour
{
    private WebSocket _webSocket;
    private string _serverUrl = "ws://arapp.uber.space:43540/nodejs-server"; // REPLACE [username] & [port] with yours
    private int _serverErrorCode;

    async void Start()
    {
        _webSocket = new WebSocket(_serverUrl);

        _webSocket.OnOpen += OnOpen;
        _webSocket.OnMessage += OnMessage;
        _webSocket.OnClose += OnClose;
        _webSocket.OnError += OnError;

        await _webSocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        _webSocket.DispatchMessageQueue();
#endif
    }


    // Event Handlers

    private void OnOpen()
    {
        print("Connection opened");
    }

    private void OnMessage(byte[] incomingBytes)
    {
        print("Message received");
    }

    private void OnClose(WebSocketCloseCode closeCode)
    {
        print($"Connection closed: {closeCode}");
    }

    private void OnError(string errorMessage)
    {
        print($"Connection error: {errorMessage}");
    }

    private async void OnApplicationQuit()
    {
        await _webSocket.Close();
    }
}
