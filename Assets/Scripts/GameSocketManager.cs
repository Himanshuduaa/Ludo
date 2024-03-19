using System;
using System.Collections.Generic;
//using SocketIOClient;
//using SocketIOClient.Newtonsoft.Json;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.Linq;
using System.Reflection;
using System.Drawing.Drawing2D;
using BestHTTP.SocketIO3;
using BestHTTP.SocketIO3.Transports;
using BestHTTP.SocketIO3.Events;
using BestHTTP.JSON;
//using System.Net.Sockets;
using System.Net;
using Unity.VisualScripting;
using AYellowpaper.SerializedCollections;

public class GameSocketManager : MonoBehaviour
{
    public static GameSocketManager instance;
    public List<TheLobby> lobbyDataList = new List<TheLobby>();
    public SocketManager Manager;
    [SerializeField]
    public string address = "";
    public string mainIP = "";
    public bool main;
    private Socket rootSocket;
    public Socket ludoSocket;
    public ChatManager chatManager;
    private bool gettingLobby=false;
    public string authToken;
    //public string encodedAuthToken;
    public bool connected;
    public bool unAuthorized;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        address= "http://"+LudoManager.Instance.gameInfo.address+":5000/";
    }
    private void Update()
    {
        if(ludoSocket!=null && ludoSocket.IsOpen)
        {
            connected = true;
        }
        else
        {
            connected=false;
            loading();
        }
    }
    public void startTheLudoGame()
    {
        if (ludoSocket == null)
        {
            loading();
            Debug.Log("Connecting to ludo socket...");

            SocketOptions options = new SocketOptions();
            options.ConnectWith = TransportTypes.WebSocket;
            options.Auth = (manager, socket) => new { AppToken = "te9777gbh@#3343bhh34$h2!n44bh@" };
            options.AdditionalQueryParams = new PlatformSupport.Collections.ObjectModel.ObservableDictionary<string, string>();
            options.AdditionalQueryParams.Add("token", authToken);
            options.AdditionalQueryParams.Add("gameCode", "ludo");
            options.AutoConnect = false;
            options.Timeout = TimeSpan.FromSeconds(1);
            options.ReconnectionDelay = TimeSpan.FromSeconds(1);
            //options.ReconnectionAttempts = 0;
            //options.Reconnection = false; ;
            // Create the Socket.IO manager
            if (main == true)
            {
                address = mainIP;//"http://prod-5-8b0f8f5708d6e40f.elb.ap-south-1.amazonaws.com:5000";
            }
            Manager = new SocketManager(new Uri(this.address), options);
            rootSocket = Manager.Socket;
            ludoSocket = Manager.GetSocket("/ludo");
            Manager.Open();
            ludoSocket.On<ConnectResponse>(SocketIOEventTypes.Connect, OnConnected);
            ludoSocket.On<string>(SocketIOEventTypes.Disconnect, (arg1) => OnDisConnected(arg1));
            ludoSocket.On<string>("receiveLobby", (arg1) => receivedLobby(arg1)); //"get-lobby"
            ludoSocket.On<string>("lobby-created", (arg1) => createLobby(arg1)); //"create-lobby"
            ludoSocket.On<string>("status", (arg1) => checkStatus(arg1));//------------------------------------Called when connected with socket
            ludoSocket.On<string>("status-game", (arg1) => checkGameStatus(arg1));// "game-status"
            ludoSocket.On<string>("lobby-join", (arg1) => JoiningTheLobby(arg1));//"join-lobby"
            ludoSocket.On<JoinedPlayerInformation>("OtherJoinedLobby", (arg1) => otherPlayerJoinedLobby(arg1));//------------------------------------Called by server when any player joins lobby
            ludoSocket.On<string>("match-start", (arg1) => startTheMatch(arg1));//------------------------------------Called by server when match starts
            ludoSocket.On<string>("whos-turn", (arg1) => WhosTurn(arg1));//------------------------------------Called by server to inform about the turn
            ludoSocket.On<string>("after-dice-run", (arg1) => DiceReport(arg1));//"dice-run"
            ludoSocket.On<string>("pawn-move", (arg1) => moveThePawn(arg1));//"move-pawn"
            ludoSocket.On<string>("pawncut", (arg1) => pawnCut(arg1));//------------------------------------Called by server to inform if any pawn gets cut
            ludoSocket.On<string>("lobby-exited", (arg1) => playerExittedLobby(arg1)); //"exit-lobby"
            ludoSocket.On<string>("watcher-join", (arg1) => WatcherJoin(arg1));//"join-watcher"
            ludoSocket.On<string>("turn-missed", (arg1) => TurnMissed(arg1));//------------------------------------Called by server if any player missed the turn
            ludoSocket.On<string>("result", (arg1) => result(arg1));//------------------------------------Called by server to show the result
            ludoSocket.On<string>("result-position", (arg1) => resultPosition(arg1));
            ludoSocket.On<string>("error", (arg1) => error(arg1));//------------------------------------Called by server in case of error
            ludoSocket.On("on-main-menu", () => onMainMenu()); //"main-menu"
            ludoSocket.On<string>("player-exit", (arg1) => PlayerExited(arg1));//"exit-game"------------------------------Data for other players
            ludoSocket.On<string>("match-exit", (arg1) => MatchExited(arg1));//------------------------------confirmation for exit to the player who exitted
            ludoSocket.On<string>("onBetAmounts", (arg1) => getBetAmounts(arg1));
            ludoSocket.On<string>("setPlayerDetails", (arg1) => onPlayerDetails(arg1));
            ludoSocket.On<string>("onGameRequest", (arg1) => onGameRequest(arg1));
            ludoSocket.On<string>("lobby-rejoin", (arg1) => lobbyRejoin(arg1)); //"rejoin-lobby"
        }
    } 
    private void PlayerExited(string json)
    {
        Debug.Log("Player Exitted  "+json);
        ExittedPlayerID playerexit = JsonConvert.DeserializeObject<ExittedPlayerID>(json);
        LudoManager.Instance.otherPlayerExitted(playerexit);
    }
    private void MatchExited(string json) // Player exits the match,response is coming to the player who has exitted
    {
        Debug.Log("Match Exitted  "+json);
        MatchExit matchexit = JsonConvert.DeserializeObject<MatchExit>(json);
        LudoManager.Instance.MatchExitted(matchexit);
    }
    private void result(string json)
    {
        Debug.Log("Result:"+ json);
        List<Result> result = JsonConvert.DeserializeObject<List<Result>>(json);
        LudoManager.Instance.matchDataManager.MatchData.result = result;
        LudoManager.Instance.WhoIsWinner();
    }
    private void resultPosition(string json)
    {
        Debug.Log("Result:" + json);
        List<Result> result = JsonConvert.DeserializeObject<List<Result>>(json);
        LudoManager.Instance.matchDataManager.MatchData.result = result;
        LudoManager.Instance.DisplayResultOnBoard();
    }
    private void error(string json)
    {
        Debug.Log("Result:" + json);
        Error result = JsonConvert.DeserializeObject<Error>(json);

        UIManager.Instance.ErrorDisplay(result);
    }
    private void onMainMenu()
    {
        Debug.Log("ONMAINMENU");
        LudoManager.Instance.restart();
    }
    private void onPlayerDetails(string json) // Getting the player details from server
    {
        Debug.Log("player details are "+ json);
        USERDetails details= JsonConvert.DeserializeObject<USERDetails>(json);
        if(details!=null)
        {
            LudoManager.Instance.gameInfo.setPlayerInfo(details);
        }
    }
    private void onGameRequest(string json) // Game request from server
    {
        GameReq details = JsonConvert.DeserializeObject<GameReq>(json);
        if(LudoManager.Instance.gameInfo.playerID != details.uid)
        {
            UIManager.Instance.Panels["GameRequests"].GetComponent<GameRequest>().setGameRequest(details);
        }
    }
    private void lobbyRejoin(string json) // Rejoining details from the server when the player rejoins after ending the match
    {
        Debug.Log("Rejoining details sent by server is " + json);
        LudoManager.Instance.matchDataManager.MatchData =null;
        LudoManager.Instance.lobbymanager.PlayersInLobby.Clear();
        UIManager.Instance.Panels["Result"].SetActive(false);
        JoiningTheLobby(json);
    }
    private void forbidden(string json)
    {
        Debug.Log("Rejoining details sent by server is " + json);
        LudoManager.Instance.matchDataManager.MatchData = null;
        LudoManager.Instance.lobbymanager.PlayersInLobby.Clear();
        UIManager.Instance.Panels["Result"].SetActive(false);
        JoiningTheLobby(json);
    }
    private void TurnMissed(string json) // Server response for player turn missed.
    {
        Debug.Log("Turn Missed ==> " + json);
        TurnMissed missed = JsonConvert.DeserializeObject<TurnMissed>(json);
        for (int i = 0; i < LudoManager.Instance.matchDataManager.MatchData.players.Count; i++)
        {
            if (LudoManager.Instance.matchDataManager.MatchData.players[i].uid == missed.uid)
            {
                Player player = LudoManager.Instance.matchDataManager.MatchData.players[i];
                player.missed_turn = missed.missed_turn;
                LudoManager.Instance.matchDataManager.MatchData.players[i] = player;
                LudoManager.Instance.setMissingTurns(player.uid, int.Parse(LudoManager.Instance.matchDataManager.MatchData.max_missing), player.missed_turn);
            }
        }
        LudoManager.Instance.missed=missed;
    }
    private void WatcherJoin(string json) // called by "watcher-join", when watcher joins the game
    {
        Debug.Log("Joining the watcher to game "+json);
        LudoManager.Instance.joinTheWatcher(json);
    }
    private void moveThePawn(string json)
    {
        Debug.Log("Pawn Move Data is " + json);
        if (UIManager.Instance.Panels["GameWindow"].activeInHierarchy)// Checking if player is actually inside the game
            LudoManager.Instance.moveThePawn(json);
    }
    private void pawnCut(string json) // "pawncut" Response coming from server whenever any pawn gets cut
    {
        Debug.Log("Pawn Cut data is " + json);
        if (UIManager.Instance.Panels["GameWindow"].activeInHierarchy)// Checking if player is actually inside the game
            LudoManager.Instance.pawnCutData(json);
    }
    private void WhosTurn(string turnPlay)
    {
        StartCoroutine(whosTurnIenum(turnPlay));
    }
    IEnumerator whosTurnIenum(string json) // To know who's turn is this
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Whos Turn Called="+json);
        if (UIManager.Instance.Panels["GameWindow"].activeInHierarchy)// Checking if player is actually inside the game
        {
            LudoManager.Instance.turnManager.PlayTurn(json);
        }
    }
    private void DiceReport(string json) // "after-dice-run" response
    {
        Debug.Log("DICE DATA IS "+json);
        if (UIManager.Instance.Panels["GameWindow"].activeInHierarchy)// Checking if player is actually inside the game
            LudoManager.Instance.playTheDice(json);
       // LudoManager.Instance.WhoIsWinner();
    }
    private void startTheMatch(string json) // "match-start" is given by server when player comes inside match when the game is started from first time 
    {
        Debug.Log("Starting the match  "+json);
        LudoManager.Instance.startTheMatch(json);
        joinRoom();
    }
    private void joinRoom()
    {
        Debug.LogWarning("joinRoom (chat socket) ==>" + LudoManager.Instance.matchDataManager.MatchData.match_id);
        chatManager.chatSocket.Emit("joinRoom",LudoManager.Instance.matchDataManager.MatchData.match_id);
    }
    private void otherPlayerJoinedLobby(JoinedPlayerInformation playersInLobby)
    {
        LudoManager.Instance.otherPlayerJoinedLobby(playersInLobby); // When any other player joins the lobby
    }
    private void playerExittedLobby(string playerExitted) // When Player Exits the lobby, called from "lobby-exited"
    {
        Debug.Log("Player Exitted " + playerExitted);
        LudoManager.Instance.playerExitted(playerExitted);
    }
    private void checkStatus(string json) // When game reloads again, to know where the player was, "status" is the response
    {
        UnityEngine.Debug.Log("data of status received is " + json);
        if (json != string.Empty && json != "" && json != null)
            LudoManager.Instance.checkStatus(json,false);
        else if (json == string.Empty || json == "" || json==null)
        {
            UIManager.Instance.HomePage.SetActive(true);
        }
    }
    private void checkGameStatus(string json) //"status-game" Response to know the current state of the game
    {
        UnityEngine.Debug.Log ("Status-game is "+json);
        if(json!="false")
            LudoManager.Instance.checkGameStatus(json, false);
    }
    private void JoiningTheLobby(string json) //"lobby-join" when player joins any lobby
    {
        UnityEngine.Debug.Log (json);
        LudoManager.Instance.JoiningTheLobby(json);
    }
    public void getBet() // Getting the bets data from server
    {
        GETData getData = new GETData();
        getData.uid = LudoManager.Instance.gameInfo.playerID;
        string json = JsonConvert.SerializeObject(getData);
        Debug.LogWarning("betAmounts==> " + json);
        ludoSocket.Emit("betAmounts", json);
    }
    private void getBetAmounts(string bets) // Received bets data from server on "onBetAmounts"
    {
        Debug.Log("Bets are " + bets);
        BetData betdata= JsonConvert.DeserializeObject<BetData>(bets);
        UIManager.Instance.Panels["AmountSelection"].GetComponent<AmountSelection>().setBets(betdata);
    }
    public void ClearLobbyContent() // Clear the lobby
    {
        foreach (var item in LudoManager.Instance.lobbymanager.lobbyList)
        {
            Destroy(item);
        }
        LudoManager.Instance.lobbymanager.lobbyList.Clear();
    }
    public void exitTheGame() // Exitting the game and telling server to come on main menu
    {
        Exitting exitting = new Exitting();
        exitting.uid = LudoManager.Instance.gameInfo.playerID;
        exitting.lobby_id = LudoManager.Instance.lobbymanager.activeLobby.lobbyID;
        string json = JsonConvert.SerializeObject(exitting);
        Debug.LogWarning("main-menu ==>" + json);
        ludoSocket.Emit("main-menu", json);
    }
    public void ExitLobby()
    {
        if (connected)
        {
            Debug.Log("Trying to exit lobby");
            playerExitLobby playerExitLobby = new playerExitLobby();
            playerExitLobby.uid = LudoManager.Instance.gameInfo.playerID;
            playerExitLobby.lobby_id = LudoManager.Instance.lobbymanager.activeLobby.lobbyID;
            string json = JsonConvert.SerializeObject(playerExitLobby);
            LudoManager.Instance.lobbymanager.PlayersInLobby.Clear();
            //LudoManager.Instance.exitLobbies();
            LudoManager.Instance.lobbymanager.exitLobbies();
            Debug.LogWarning("exit-lobby ==>" + json);
            ludoSocket.Emit("exit-lobby", json);
            gettingLobby = false;
        }
    }
    private void OnApplicationQuit()
    {
        Debug.Log("Windows application is closing.");
        ExitLobby();
        // Add your cleanup or save logic here.
    }
    public void exitGame(string json)
    {
        Debug.LogWarning("exit-game ==>" + json);
        ludoSocket.Emit("exit-game", json);
    }
    public void movePawn(string json) // Sending data from client to move the pawn
    {
        if (connected)
        {
            Debug.LogWarning("move-pawn ==>" + json);
            ludoSocket.Emit("move-pawn", json);
        }
    }

    private void createLobby(string stringList)
    {
        Debug.Log("Saving ID " + stringList);
        LudoManager.Instance.lobbymanager.createLobby(stringList);
    }
   
    private void receivedLobby(string response)
    {
        Debug.Log ("Data for lobby is "+response.ToString());
        LudoManager.Instance.receivedLobby(response);
        //gettingLobby = false;

    }
    public void receiveLobby()
    {
        if (connected)
        {
            ludoSocket.Emit("reciceve-last-upadte-lobby");
        }
    }

    [ContextMenu("CallStatus")]

    public void playerStatus() // Know the status of the Ludo, whether player was in game, lobby or nowhere
    {
        //Debug.Log("Inside Player Status");
        SendingID data = new SendingID();
        data.uid = LudoManager.Instance.gameInfo.playerID;
        string json = JsonConvert.SerializeObject(data);
        Debug.LogWarning("player-status ==> " + json);
        ludoSocket.Emit("player-status", json);

        if (LudoManager.Instance.gameInfo.uid != "")
        {
            chatManager.turnOnChat();
            UIManager.Instance.gameConnected();
            LudoManager.Instance.gameInfo.getTheData();
        }
    }
    public void GameStatus() // Caled when we switch the tab to get the active condition of the game
    {
        Debug.Log("Inside Player Status");
        if (connected && LudoManager.Instance.matchDataManager.MatchData.match_id!="")
        {
            SendingID iD = new SendingID();
            iD.uid = LudoManager.Instance.gameInfo.playerID;
            iD.match_id = LudoManager.Instance.matchDataManager.MatchData._id;
            iD.lobby_id = LudoManager.Instance.matchDataManager.MatchData.lobby_id;
            string json = JsonConvert.SerializeObject(iD);
            Debug.LogWarning("game-status ==> " + json);
            ludoSocket.Emit("game-status", json);
        }
    }
    
    [ContextMenu("GetLobby")]
    public void GetLobby()
    {
        if (connected)
        {
            GetLobby getlobby=new GetLobby();
            getlobby.uid = LudoManager.Instance.gameInfo.uid.ToString();
            getlobby.players = UIManager.Instance.Filters.filterDetails.players;
            getlobby.ep = UIManager.Instance.Filters.filterDetails.entry;
            getlobby.prizepool = UIManager.Instance.Filters.filterDetails.prizepool;
            getlobby.mode = UIManager.Instance.Filters.filterDetails.mode;
            string json=JsonConvert.SerializeObject(getlobby);
            Debug.LogWarning("get-lobby ==>" + json);
            ludoSocket.Emit("get-lobby", json);
        }
    }
    public void joinLobby(string json) // Send data from client to join a lobby
    {
        if (connected)
        {
            Debug.Log(json);
            LudoManager.Instance.watcher = false;
            Debug.LogWarning("join-lobby==> " + json);
            ludoSocket.Emit("join-lobby", json);
        }
    }
    public void DiceHit(string json)// Send data from client to run a dice
    {
        if (connected)
        {
            Debug.LogWarning("dice-run==> " + json);
            ludoSocket.Emit("dice-run", json);
        }
    }
    public void CreateLobby(string lobby)// Send data from client to create a lobby
    {

        if (connected)
        {
            Debug.LogWarning("create-lobby ==> "+lobby);
            ludoSocket.Emit("create-lobby", lobby);
        }
    }
    public void NotAuthorized()
    {
        MainThreadDispatcher.RunOnMainThread(() =>
        {
            UIManager.Instance.whatToShow("NotAuthorized", false);

        });
    }
    private void OnDisConnected(string resp)
    {
        UnityEngine.Debug.Log("Disconnected from Ludo server"+ resp);
        if(!LudoManager.Instance.gameInfo.unAuthorized)
        {
            loading();
        }
    }
    private void OnConnected(ConnectResponse resp)
    {
        MainThreadDispatcher.RunOnMainThread(() =>
        {
            UnityEngine.Debug.Log("Connected to Ludo server");
            StartCoroutine(askfordetails());
        });
    }
    IEnumerator askfordetails()
    {
        yield return new WaitForSeconds(1f);
        Debug.LogWarning(" get-player-details");
        ludoSocket.Emit("get-player-details");
    }
    public void Authorized()
    {
        MainThreadDispatcher.RunOnMainThread(() =>
        {
            //getPlayerData();
        });
    }
    public void loading()
    {
        MainThreadDispatcher.RunOnMainThread(() =>
        {
            if(!LudoManager.Instance.gameInfo.unAuthorized)
                UIManager.Instance.whatToShow("Loading", false);
        });
    }
    public void SendMessage()
    {
        GetLobby();
    }
}
public class SendingID
{
    public string uid;
    public string match_id;
    public string lobby_id;
}