using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance 
    {
        get { return FindObjectOfType<GameManager>(); }

    }


    public WebSocketsConnection webSockets;
    public GameObject playerPrefab;

    private Player _player; //who owns the device
    private Game _game; //current game, all the game data
    private Dictionary<int, GameObject> _playerGameObjects = new Dictionary<int, GameObject>(); 



    ////////////////////////
    // MonoBehaviour Events
    ////////////////////////

    void Start() 
    {

        // Create a Player
        int playerId = Random.Range(0, 1000000); //assigns a random number, can happen that 2 users can have the same number, but chances are very low. still a stupid way to create an unique ID, but works for now
        Vector3 playerPosition = new Vector3(Random.Range(0,100),0,0); // 0, 0, 0 for now.
        _player = new Player(playerId, playerPosition.x, playerPosition.y, playerPosition.z);

        // Create a Game
        _game = new Game(); //empty game 
        _game.players.Add(_player); //1 player added to the list of players

        // Instantiate the Player's GameObject
        GameObject playerGameObject = Instantiate(playerPrefab, _player.position, Quaternion.identity);

        // Store GameObject reference in dictionary
        _playerGameObjects.Add(_player.id, playerGameObject);

        // Establish server connection
        webSockets.ConnectToServer(); //from the websocketsconnection script

    }


    void Update() 
    {

    }


    void OnApplicationQuit() 
    {
        webSockets.DisconnectFromServer();
    }



    ////////////////////////
    // User Input Events
    ////////////////////////

    public void DidReceiveMoveInput(Vector3 newPosition) 
    {
        // Update the player's position based on input
        _player.position = newPosition;
        _playerGameObjects[_player.id].transform.position = newPosition;

        // Send a PlayerMovedPackage
        webSockets.SendPlayerMovedPackage(_player);
    }


    public void DidReceiveShotInput() 
    {
        // Trigger shot for player
        TriggerPlayerShot(_player);

        // Send PlayerShotPackage
        webSockets.SendPlayerShotPackage(_player);
    }



    ////////////////////////
    // Game Management
    ////////////////////////

    private void UpdateGame(Game game) 
    {

        // Check for any players that are new or left
        List<Player> newPlayers = new List<Player>();
        List<Player> missingPlayers = new List<Player>();

        // Check for new players and add them to the newPlayers list
        foreach(Player p in game.players) 
        {
            bool isAlreadyInGame = false;
            foreach(Player pl in _game.players) 
            {
                if (p.id == pl.id) 
                {
                    isAlreadyInGame = true;
                }
            }
            if (!isAlreadyInGame) 
            {
                newPlayers.Add(p);
            }

        }


        // Check for missing players and add them to the missingPlayers list
        foreach(Player pl in _game.players) 
        {
            bool isStillInGame = false;
            foreach(Player p in game.players) 
            {
                if (pl.id == p.id) 
                {
                    isStillInGame = true;
                }
            }
            if (!isStillInGame) 
            {
                missingPlayers.Add(pl);
            }

        }


        // Then create a GameObject for new player & update the playerGameObjects dictionary & _game Game accordingly 
        foreach(Player newPlayer in newPlayers) 
        {
            GameObject newPlayerGameObject = Instantiate(playerPrefab, newPlayer.position, Quaternion.identity);
            _playerGameObjects.Add(newPlayer.id, newPlayerGameObject);
            _game.players.Add(newPlayer);
        }


        // Remove players that have left and their gameObjects from the game
        foreach(Player missingPlayer in missingPlayers) 
        {
            Destroy(_playerGameObjects[missingPlayer.id]); ////////////////////////////////////////////////?????
            _playerGameObjects.Remove(missingPlayer.id);
            _game.players.RemoveAll(p => p.id == missingPlayer.id);
        }


    }


    private void UpdatePlayerPosition(Player player) 
    {
        // Update player's position
        _playerGameObjects[player.id].transform.position = player.position;
    }


    private void TriggerPlayerShot(Player player) 
    {
        // Trigger a shot for this specific player
        _playerGameObjects[player.id].GetComponent<Gun>().Shoot();
    }



    ////////////////////////
    // Server Events
    ////////////////////////


    //connection is open, now you get to decide what happens next. doesnt do any game decisions, just notifies its ready to go
    public void OnServerConnectionOpened() 
    { 
        webSockets.SendPlayerJoinedPackage(_player);
    }


    public void OnServerConnectionClosed() 
    {
        // Try to re-establish a server connection
    }


    public void DidReceiveGameUpdatePackage(GameUpdatePackage package) 
    {
        UpdateGame(package.game); //////////////////////////////////////////??????????????????
    }


    public void DidReceivePlayerJoinedPackage(PlayerJoinedPackage package) 
    {
        // Add player to Game _game
        _game.players.Add(package.player);
        
        // Send out a GameUpdate package with updated _game
        webSockets.SendGameUpdatePackage(_game);
    }


    public void DidReceivePlayerLeftPackage(PlayerLeftPackage package) 
    {
        // Remove player from Game _game
        _game.players.RemoveAll(player => player.id == package.player.id);
        
        // Send out a GameUpdate package with updated _game
        webSockets.SendGameUpdatePackage(_game);
    }


    public void DidReceivePlayerMovedPackage(PlayerMovedPackage package) 
    {
        UpdatePlayerPosition(package.player);
    }


    public void DidReceivePlayerShotPackage(PlayerShotPackage package) 
    {
        TriggerPlayerShot(package.player);
    }



}
