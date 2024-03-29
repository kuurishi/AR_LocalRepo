using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using NativeWebSocket; // Source: https://github.com/endel/NativeWebSocket
using SimpleJSON; // Source: https://github.com/Bunny83/SimpleJSON/blob/master/SimpleJSON.cs


public class WebSocketsConnection : MonoBehaviour
{


  private WebSocket webSocket;
  private string serverUrl = "ws://arapp.uber.space:43540/nodejs-server";
  private int serverErrorCode;


  //////////////////////////////////
  // MonoBehaviour Lifecycle Event Handlers
  //////////////////////////////////

  void Start()
  {
        //subscribes to the OnEvents, but doesn't connect yet. the connection opens 
        webSocket = new WebSocket(serverUrl);

        webSocket.OnOpen += OnOpen;
        webSocket.OnMessage += OnMessage;
        webSocket.OnClose += OnClose;
        webSocket.OnError += OnError;

  }



  void Update()
  {
    #if !UNITY_WEBGL || UNITY_EDITOR
      webSocket.DispatchMessageQueue(); //////////////////////////////////////////???????????
    #endif
  }


  //////////////////////////////////
  // WebSockets Event Handlers
  //////////////////////////////////

  private void OnOpen() 
  {
    print("Connection opened");
    GameManager.Instance.OnServerConnectionOpened();
  }


  //the players will receive a message if for example another player joined
  private void OnMessage(byte[] inboundBytes) 
  { 

    print("Message received"); //too vague
    string inboundString = System.Text.Encoding.UTF8.GetString(inboundBytes);
    
        Debug.Log($"<color=#5EE8A5>Incoming message: {inboundString}</color>");
    
    //checks if its a string or just an integer number. if it is just an integer, then its an error from the server (errors not implemented here. as soon as you have an error, use a diff number for diff errors. send this error code back when it occurs.)
    if (int.TryParse(inboundString, out serverErrorCode)) 
    {
      // If server returns an integer, it is an error
      print($"Server Error: {serverErrorCode}");
    } 
    else 
    {
      JSONNode json = JSON.Parse(inboundString);
      
      //a switch statement would be nice here
      if (json["packageType"].Value == "PlayerJoinedPackage") {
          // Received PlayerJoinedPackage
          PlayerJoinedPackage playerJoinedPackage = JsonUtility.FromJson<PlayerJoinedPackage>(inboundString);
          GameManager.Instance.DidReceivePlayerJoinedPackage(playerJoinedPackage);
      } else if (json["packageType"].Value == "GameUpdatePackage") {
          // Received GameUpdatePackage
          GameUpdatePackage gameUpdatePackage = JsonUtility.FromJson<GameUpdatePackage>(inboundString);
          GameManager.Instance.DidReceiveGameUpdatePackage(gameUpdatePackage); ////////////////////////////////////////????????
      } else if (json["packageType"].Value == "PlayerLeftPackage") {
          // Received PlayerLeftPackage
          PlayerLeftPackage playerLeftPackage = JsonUtility.FromJson<PlayerLeftPackage>(inboundString);
          GameManager.Instance.DidReceivePlayerLeftPackage(playerLeftPackage);
      } else if (json["packageType"].Value == "PlayerMovedPackage") {
          // Received PlayerMovedPackage
          PlayerMovedPackage playerMovedPackage = JsonUtility.FromJson<PlayerMovedPackage>(inboundString);
          GameManager.Instance.DidReceivePlayerMovedPackage(playerMovedPackage);
      } else if (json["packageType"].Value == "PlayerShotPackage") {
          // Received PlayerShotPackage
          PlayerShotPackage playerShotPackage = JsonUtility.FromJson<PlayerShotPackage>(inboundString);
          GameManager.Instance.DidReceivePlayerShotPackage(playerShotPackage);
      }

    }


  }


  private void OnClose(WebSocketCloseCode closeCode) 
  {
    print($"Connection closed: {closeCode}");
    GameManager.Instance.OnServerConnectionClosed();
  }


  private void OnError(string errorMessage) 
  {
    print($"Connection error: {errorMessage}");
  }



  //////////////////////////////////
  // Public Methods
  //////////////////////////////////

  public async void ConnectToServer() 
  {
    await webSocket.Connect(); //made a request to connect to the server and we're waiting for a response. the response is at private void OnOpen()
  }


  public async void DisconnectFromServer(Player player) 
  {
        SendPlayerLeftPackage(player);
        await webSocket.Close();

  }


  public async void SendPlayerJoinedPackage(Player player) 
  {

    PlayerJoinedPackage package = new PlayerJoinedPackage(player);

    if (webSocket.State == WebSocketState.Open) {
        string json = JsonUtility.ToJson(package);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        await webSocket.Send(bytes);
    }

  }


  public async void SendPlayerMovedPackage(Player player) 
  {

    PlayerMovedPackage package = new PlayerMovedPackage(player);

    if (webSocket.State == WebSocketState.Open) {
        string json = JsonUtility.ToJson(package);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        await webSocket.Send(bytes);
    }

  }

  public async void SendPlayerShotPackage(Player player) 
  {

    PlayerShotPackage package = new PlayerShotPackage(player);

    if (webSocket.State == WebSocketState.Open) {
        string json = JsonUtility.ToJson(package);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        await webSocket.Send(bytes);
    }

  }

    public async void SendPlayerLeftPackage(Player player)
    {

        PlayerLeftPackage package = new PlayerLeftPackage(player);

        if (webSocket.State == WebSocketState.Open)
        {
            string json = JsonUtility.ToJson(package);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            await webSocket.Send(bytes);
        }

    }

    public async void SendGameUpdatePackage(Game game) 
    {

        GameUpdatePackage package = new GameUpdatePackage(game);

        if (webSocket.State == WebSocketState.Open) 
        {
            string json = JsonUtility.ToJson(package);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            await webSocket.Send(bytes);
        }

    }




}