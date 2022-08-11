using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using NativeWebSocket; // Source: https://github.com/endel/NativeWebSocket
using SimpleJSON; // Source: https://github.com/Bunny83/SimpleJSON/blob/master/SimpleJSON.cs


[Serializable]
public struct PlayerDataPackage
{
    public static string classType = "PlayerDataPackage";

    public string name;
    public int highScore;

    public PlayerDataPackage(string name, int highScore) {
      this.name = name;
      this.highScore = highScore;
    }
}


[Serializable]
public struct LevelDataPackage
{
    public static string classType = "LevelDataPackage";

    public string mapTitle;
    public int difficultyLevel;

    public LevelDataPackage(string mapTitle, int difficultyLevel) {
      this.mapTitle = mapTitle;
      this.difficultyLevel = difficultyLevel;
    }
}


public class WebSocketsConnection : MonoBehaviour
{
  private WebSocket webSocket;
  private string serverUrl = "ws://my.uber.space:43540/nodejs-server";
  private int serverErrorCode;


  //////////////////////////////////
  // MonoBehaviour Lifecycle Event Handlers
  //////////////////////////////////

  async void Start()
  {
    webSocket = new WebSocket(serverUrl);

    webSocket.OnOpen += OnOpen;
    webSocket.OnMessage += OnMessage;
    webSocket.OnClose += OnClose;
    webSocket.OnError += OnError;

    await webSocket.Connect();
  }

  void Update()
  {
    #if !UNITY_WEBGL || UNITY_EDITOR
      webSocket.DispatchMessageQueue();
    #endif
  }

  private async void OnApplicationQuit()
  {
    await webSocket.Close();
  }


  //////////////////////////////////
  // WebSockets Event Handlers
  //////////////////////////////////

  private void OnOpen() {
    print("Connection opened");
    Invoke("SendPlayerDataPackage", 0f);
  }

  private void OnMessage(byte[] inboundBytes) {
    print("Message received");
    print($"bytes: {inboundBytes}");
    string inboundString = System.Text.Encoding.UTF8.GetString(inboundBytes);
    print($"message: {inboundString}");
    
    
    if (int.TryParse(inboundString, out serverErrorCode)) {
      // If server returns an integer, it is an error
      print($"Server Error: {serverErrorCode}");
    } else {
      JSONNode json = JSON.Parse(inboundString);
      
      if (json["classType"].Value == PlayerDataPackage.classType) {
          PlayerDataPackage playerData = JsonUtility.FromJson<PlayerDataPackage>(inboundString);
          print($"Received PlayerDataPackage || name: {playerData.name}, highScore: {playerData.highScore}");
      } else if (json["classType"].Value == LevelDataPackage.classType) {
          LevelDataPackage levelData = JsonUtility.FromJson<LevelDataPackage>(inboundString);
          print($"Received LevelDataPackage || name: {levelData.mapTitle}, highScore: {levelData.difficultyLevel}");
      }
    }
  }

  private void OnClose(WebSocketCloseCode closeCode) {
    print($"Connection closed: {closeCode}");
  }

  private void OnError(string errorMessage) {
    print($"Connection error: {errorMessage}");
  }


  //////////////////////////////////
  // Helper Methods
  //////////////////////////////////

  private async void SendPlayerDataPackage() {
      string playerName = "Michael P.";
      int playerHighScore = 12400;
      PlayerDataPackage playerData = new PlayerDataPackage(playerName, playerHighScore);

      if (webSocket.State == WebSocketState.Open) {
          string json = JsonUtility.ToJson(playerData);
          byte[] bytes = Encoding.UTF8.GetBytes(json);
          await webSocket.Send(bytes);
      }
  }
}
