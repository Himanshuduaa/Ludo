using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.Text;
using TMPro;
using System.Linq;
using Newtonsoft.Json;

public enum Turn
{
    Red, Green, Yellow,Blue
}
public enum Mode
{
    Modern,Classic
}
public enum WinningCriteria
{
    OnePawn,FourPawn
}
public enum PlayerPosition
{
    BlueBottom, RedBottom, YellowBottom, GreenBottom
}
public enum PlayerTurn
{
    One,Two,Three,Four
}
public class LudoManager : MonoBehaviour
{
    public static LudoManager Instance;
    [Header(" Serialize Fields")]
    #region privateVars
    private WinningCriteria winningCriteria;
    [SerializeField]
    private Mode mode;
    [SerializeField]
    public PlayerTurn localPlayerTurn;
    [SerializeField]
    private PlayerPosition CurrentLocalPosition;
    private Dictionary<PlayerPosition, Action> ColourStateActions = new Dictionary<PlayerPosition, Action>();
    [SerializeField]
    private TextMeshProUGUI timerText;
    public List<int> SafeZoneIndex = new List<int> { 1, 9, 14, 22, 27, 35, 40, 48 };
    [SerializeField]
    public InternetConnectionChecker internetConnectionChecker;
    [SerializeField]
    public List<HomePoints> homePoints = new List<HomePoints>();
    [SerializeField]
    private GameObject matchStartedUI;
    [SerializeField]
    private BoardMapping boardMapping;
    #endregion

    public int whichturn;
    public PlayerTimer timerPlayer;
    public List<PlayerController> players = new List<PlayerController>(); // Array of player GameObjects
    public LobbyManager lobbymanager;
    public string operatorToken;
    [Header("Room To Join Information")]
    public List<SpotsMap> spotsMap = new List<SpotsMap>();
    public MatchDataManager matchDataManager;
    public int time;
    public SocketTurnManager turnManager;
    public int MaxPlayers;
    public PawnMovementData pawnMovement;
    public bool movingPawn;
    internal PawnCut pawnCut;
    public List<int> winningIndexesLocalPlayer=new List<int>();
    public bool watcher;
    public TurnMissed missed = new TurnMissed();
    public List<LobbyDetails> nestedGamesList = new List<LobbyDetails>();
    public DiceManager diceManager;
    public List<int> homeIndexes = new List<int>();
    public ModernModeGame modernMode;
    public GameInfo gameInfo;
    public string GenerateRandomID(int length)
    {
        UnityEngine.Debug.LogError("Creating The ID");
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        StringBuilder randomId = new StringBuilder(length);

        System.Random random = new System.Random();

        for (int i = 0; i < length; i++)
        {
            randomId.Append(chars[random.Next(chars.Length)]);
        }

        return randomId.ToString();
    }
    
    public void exitThisGame() // Exitting the game and telling server to come on main menu
    {
        GameSocketManager.instance.exitTheGame();
    }
    private void Awake()
    {
        movingPawn = false;
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            UnityEngine.Debug.Log("DESTROYING LUDO");
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ColourStateActions[PlayerPosition.BlueBottom] = StateCheck;
        ColourStateActions[PlayerPosition.RedBottom] = StateCheck;
        ColourStateActions[PlayerPosition.YellowBottom] = StateCheck;
        ColourStateActions[PlayerPosition.GreenBottom] = StateCheck;
        AudioManager.instance.PlayAudio("BackgroundMusic");
        // Get the URL of the WebGL page
        string url = Application.absoluteURL;
        UnityEngine.Debug.Log("URL of Page is " + url);
        int index = url.IndexOf("?id=");
        string idString = "";
        if (index != -1)
        {
            // Extract the substring after '?id='
            idString = url.Substring(index + 4);

            Console.WriteLine("String after ?id=: " + idString);
        }
        else
        {
            Console.WriteLine("'?id=' not found in the URL.");
        }
        if(LudoManager.Instance.gameInfo.encodedAuthToken=="" && GameSocketManager.instance.authToken=="")
        {
            LudoManager.Instance.gameInfo.encodedAuthToken = idString;
            UnityEngine.Debug.Log("ID: " + idString);
            string decodedId = DecodeBase64(idString);
            GameSocketManager.instance.authToken = decodedId;
            UnityEngine.Debug.Log("Decoded id is " + decodedId);
        }
        else
        {
            string decodedId = DecodeBase64(LudoManager.Instance.gameInfo.encodedAuthToken);
            GameSocketManager.instance.authToken = decodedId;
        }
        if(GameSocketManager.instance.ludoSocket!=null&&GameSocketManager.instance.ludoSocket.IsOpen)
        {
            UnityEngine.Debug.Log("warning null");
            if(gameInfo.uid!="")
                gameInfo.getTheData();
        }
    }
    string DecodeBase64(string base64String) // Decoding the token received from URL to base 64
    {
        byte[] bytes = Convert.FromBase64String(base64String);
        return System.Text.Encoding.UTF8.GetString(bytes);
    }
    
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            KnowTheStatusOfGame();
        } 
    }
    public void playOnline()
    {
        lobbymanager.playOnline();
        getBet();
    }
    public void getBet() // Get the bets if dont have bets data otherwise display the bets data in game
    {
        if (gameInfo.Amount.Count == 0)
        {
            GameSocketManager.instance.getBet();
        }
        else
        {
            UIManager.Instance.Panels["AmountSelection"].GetComponent<AmountSelection>().Amount=LudoManager.Instance.gameInfo.Amount;
            UIManager.Instance.Panels["AmountSelection"].GetComponent<AmountSelection>().setBetsUI();
        }
    }
    public void exitGame()
    {
        ExitGame exit = new ExitGame();
        exit.uid = gameInfo.playerID;
        exit.lobby_id = matchDataManager.MatchData.lobby_id;
        if(!watcher)
        {
            string json = JsonConvert.SerializeObject(exit);
            UnityEngine.Debug.Log("Exitting game "+json);
            GameSocketManager.instance.exitGame(json);
        }
        else
        {
            string exitString = JsonConvert.SerializeObject(exit);
            Debug.LogWarning(" exit-watcher==> "+ exitString);
            GameSocketManager.instance.ludoSocket.Emit("exit-watcher",exitString);
            restart();
        }
    }
    public void MatchExitted(MatchExit matchExit) // Player exits the match,response is coming to the player who has exitted
    {
        if(matchExit.isExit)
        {
            restart();
        }
    }
    public void otherPlayerExitted(ExittedPlayerID playerexit) // Any Other Player exits the match,response is coming all the players left in the match
    {
        PlayerController playerController = players.Find(spot => spot.PlayerID == playerexit.player_id);
        if(playerexit.player_id==gameInfo.playerID)
        {
            restart();
        }
        else
        playerController.userDetails.setRank(playerexit.rank);//playerExitted();
    }
    public void pawnCutData(string json) // "pawncut" Response coming from server whenever any pawn gets cut
    {
        List<List<PawnCut>> playerDataList = JsonConvert.DeserializeObject<List<List<PawnCut>>>(json);
        if (playerDataList != null && playerDataList.Count > 0 && playerDataList.Count > 0 && playerDataList[0].Count > 0)
        {
            PawnCut playerData = playerDataList[0][0];
            pawnCut = playerData;
            int playerNumber = playerData.player_number;
            int pownDieValue = playerData.pownDie;
            Console.WriteLine($"Player Number: {playerNumber}");
            Console.WriteLine($"PownDie Value: {pownDieValue}");
        }
        else
        {
            Console.WriteLine("Invalid JSON structure.");
        }
    }
    public void joinTheWatcher(string json) //called by "watcher-join", when watcher joins the game, to show him thecurrent status of the game
    {
        MatchData match = JsonConvert.DeserializeObject<MatchData>(json);
        matchDataManager.MatchData = match;
        watcher = true;
        UIManager.Instance.whatToShow("GameWindow", false);
        lobbymanager.activeLobby.LocalPlayerColour = "Red";
        SetEverythingAccordingToMe();
        matchDataManager.setCurrentUpdate();
        for (int i = 0; i < matchDataManager.MatchData.result.Count; i++)
        {
            PlayerController player = players.Find(spot => spot.PlayerID == matchDataManager.MatchData.result[i].uid);
            player.userDetails.setRank(matchDataManager.MatchData.result[i].position);
            player.turnOffPawnVisibility();
        }
    }
    private void KnowTheStatusOfGame() // Called when player switches the tab to know the current progress of game
    {
        if ((UIManager.Instance.Panels["GameWindow"].activeInHierarchy && !UIManager.Instance.Panels["Result"].activeInHierarchy) || UIManager.Instance.Panels["Warning"].activeInHierarchy) // Checking if player is actually inside the game
        {
            if (GameSocketManager.instance.ludoSocket != null)
            {
                if (GameSocketManager.instance.ludoSocket.IsOpen)
                {
                    GameSocketManager.instance.GameStatus();
                }
            }
        }
        if(UIManager.Instance.Panels["Loading"].activeInHierarchy && GameSocketManager.instance.connected)
        {
            UnityEngine.Debug.Log("Loading but connected");
            UIManager.Instance.HomePage.SetActive(true);
            UIManager.Instance.bottomBar.SetActive(true);
        }
    }
    public void moveThePawn(string json) // For moving the pawn to specific position, called on "pawn-move" from server
    {
        PawnMovementData data = JsonConvert.DeserializeObject<PawnMovementData>(json);
        pawnMovement = data; // Access the first (and only) item in the list
        movingPawnIenum();
    }
    public void ShowMovablePawn() // To show which pawn is movable after dice run, called on "after-dice-run" from server
    {
        PlayerController playerController = players.Find(spot => spot.TurnNumber == diceManager.DicePlayed.player_number);
        matchDataManager.showMovable(playerController, diceManager.DicePlayed.moving_pown);
    }
    private void movingPawnIenum() // For moving the pawn to specific position, called on "pawn-move" from server
    {
        LudoManager.Instance.timerPlayer.StopTimer(); // Stop the ongoing Timer
        for(int i=0;i<players.Count;i++) // Highlighting the movable pawns and not highlighting the non movable
        {
            for(int j = 0; j < players[i].myPawns.Count;j++)
            {
                players[i].myPawns[j].highlight();
            }
        }
        PlayerController playercontroller = players.Find(spot => spot.TurnNumber == pawnMovement.player_number);
        playercontroller.movePawn(pawnMovement);
    }
    public void setCoinsBeforeMoving(PawnMovementData pawnMovement) // Set Coins in Modern Mode before Moving
    {
        for (int j = 0; j < pawnMovement.pawnPosition.players.Count; j++)
        {
            PlayerController playercontroller = players.Find(spot => spot.PlayerID == pawnMovement.pawnPosition.players[j].uid);
            playercontroller.userDetails.modernCoin.totalCoin = int.Parse(pawnMovement.pawnPosition.players[j].totalCoins);
            playercontroller.userDetails.modernCoin.setCoins();
        }
    }
    public void WatcherMatchExit() // Called when watcher exits the match
    {
        SendingID iD = new SendingID();
        iD.uid = LudoManager.Instance.gameInfo.playerID;
        iD.match_id = matchDataManager.MatchData.match_id;
        string json = JsonConvert.SerializeObject(iD);
        UnityEngine.Debug.LogWarning("exit-watcher ==> " + json);
        GameSocketManager.instance.ludoSocket.Emit("exit-watcher", json);
        UIManager.Instance.whatToShow("HomePage", true);

    }
    public void noInternet() // No Internet Connectivity
    {
        UnityEngine.Debug.LogError("Internet Disconnected");
        MainThreadDispatcher.RunOnMainThread(() =>
        {
            UIManager.Instance.whatToShow("Loading", false);
        });
    }
    public void SetEverythingAccordingToMe() // Setting the game Board, player local Position, Chat and other stuff
    {
        AnimationManager.instance.closeChatBox();
        UnityEngine.Debug.Log("Setting everything according to me");
        setBoardAccordingToMe();
    }
    private void deleteCache() // Deleting the ranks and resetting the board winnings
    {
        for (int i = 0; i < players.Count; i++)
        {
            if(players[i].userDetails!=null)
            players[i].userDetails.setRank("");
        }
    }
    
    private void setPlayerDP()  // Set Player Profile Picture
    {
        for(int i=0;i< matchDataManager.MatchData.players.Count;i++)
        {
            PlayerController player = players.Find(spot => spot.TurnNumber == matchDataManager.MatchData.players[i].player_number);
            if(player != null)
            {
                player.userDetails.PlayerAvatar.color = Color.white;// = null;
                player.userDetails.PlayerAvatar.sprite = UIManager.Instance.avatars[matchDataManager.MatchData.players[i].avatar].GetComponent<Image>().sprite;
                player.userDetails.PlayerAvatar.preserveAspect = true;
                player.userDetails.userName.text = matchDataManager.MatchData.players[i].userName;// = true;
            }
        }
    }
    public void setMissingTurns(string id,int max, int missed)  // Set Missing Turns of Player
    {
        PlayerController PlayerController = players.Find(spot => spot.PlayerID == id);
        if (PlayerController.MissTurn.Count > 0)
        {
            foreach (var item in PlayerController.MissTurn)
            {
                if (item != null)
                    Destroy(item.gameObject);
            }
            PlayerController.MissTurn.Clear();
        }
        for (int j = 0; j < max; j++)
        {

            GameObject MissTurn = Instantiate(UIManager.Instance.MissingTurn, PlayerController.userDetails.MissingTurn);
            MissTurn missing = MissTurn.GetComponent<MissTurn>();
            if (j < missed)
            {
                missing.GetComponent<Image>().color = Color.red;
            }
            else
            {
                missing.GetComponent<Image>().color = Color.green;
            }
            PlayerController.MissTurn.Add(MissTurn.GetComponent<MissTurn>());
        }
    }
    public void WhoIsWinner() // Show Result Window over the game
    {
        if(this.gameObject!=null)
        {
            UIManager.Instance.exitGame.interactable = false;
            diceManager.Dice.interactable = false;
            StartCoroutine(ShowWinner());
        }
    }
    IEnumerator ShowWinner()
    {
        if (UIManager.Instance.Panels["GameWindow"].activeInHierarchy)
        {
            diceManager.diceState(false);//Commented on 13th Feb
            UIManager.Instance.exitGame.interactable=false;
        }
        yield return new WaitForSeconds(2f);
        if (UIManager.Instance.Panels["GameWindow"].activeInHierarchy)
        {
            diceManager.diceState(false);//Commented on 13th Feb
            ResetAll.instance.resetAll();
            UIManager.Instance.whatToShow("Result", false);
            ResultView.Instance.open(matchDataManager.MatchData.result);
            UIManager.Instance.exitGame.interactable = true;
        }
            
    }
    public void DisplayResultOnBoard() // Display Result on Game Board
    {
        if(this.gameObject!=null)
            StartCoroutine(ShowResult());
    }
    IEnumerator ShowResult() // Set results on the ongoing game board
    {
        yield return new WaitForSeconds(0.6f);
        for (int i = 0; i < matchDataManager.MatchData.result.Count; i++)
        {
            PlayerController player = players.Find(spot => spot.PlayerID == matchDataManager.MatchData.result[i].uid);
            player.userDetails.setRank(matchDataManager.MatchData.result[i].position);
            player.turnOffPawnVisibility();
        }
    }
    public void playTheDice(string json) //Run the dice after getting "after-dice-run" response
    {
        diceManager.playTheDice(json);
    }
    public void CutThePawn() // Cut the pawn
    {
        if(this.gameObject!=null)
        {
            PlayerController player = players.Find(spot => spot.TurnNumber == pawnCut.player_number);
            player.cutThePawn(pawnCut);
        }
    }
    
    public void setBoardAccordingToMe() // Setting the game Board, player local Position, Chat and other stuff
    {
        AnimationManager.instance.closeChatBox();
        Player foundSpot = new Player();
        if (watcher == false)
        {
            foundSpot = matchDataManager.MatchData.players.Find(spot => spot.uid == gameInfo.playerID);
        }
        else
        {
            foundSpot = matchDataManager.MatchData.players[0];
        }
        for (int i = 0; i < matchDataManager.MatchData.players.Count; i++)
        {
            for (int j = 0; j < matchDataManager.MatchData.players[i].pown_position.Count; j++)
            {
                if (matchDataManager.MatchData.players[i].pown_position[(j + 1).ToString()] == 0)
                {
                    matchDataManager.MatchData.players[i].pown_position[(j + 1).ToString()] = int.Parse(matchDataManager.MatchData.players[i].player_number + "00" + (j + 1).ToString());
                }
            }
        }
        setWinningIndexes(foundSpot);
        setlocalPlayerTurn(foundSpot);
        setLocalPlayerColour();
        setChat(!watcher); // Set chat on but only players can see, not the watcher
        boardMapping.setUsers(); // Setting users profile according to their turn
        SetColours();
        setBoardAndTiles();
        if (watcher)
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].TurnNumber = matchDataManager.MatchData.players[i].player_number;
            }
        }
        boardMapping.setPawns();
        setPlayerDP();
        startModernMode();
        for (int i = 0; i < matchDataManager.MatchData.players.Count; i++)
        {
            setMissingTurns(matchDataManager.MatchData.players[i].uid, int.Parse(matchDataManager.MatchData.max_missing), matchDataManager.MatchData.players[i].missed_turn);
        }
        if (!UIManager.Instance.Panels["GameWindow"].activeInHierarchy)
        {
            UIManager.Instance.Panels["GameWindow"].SetActive(true);
        }
        ResultView.Instance.setBoardRanking();
        deleteCache();
    }
    private void setChat(bool state) // Set chat on but only players can see, not the watcher
    {
        AnimationManager.instance.Chatbutton.gameObject.SetActive(state);
        AnimationManager.instance.chatPanel.gameObject.SetActive(state);
    }
    private void setWinningIndexes(Player foundSpot) // Setting the winning indexes 
    {
        for(int i=0;i<4;i++)
        {
            int num=int.Parse(foundSpot.player_number.ToString()+"00"+(i+1).ToString());
        }
    }
    private void setLocalPlayerColour() // Set the colour chosen by the local player and the player will always be at bottom left corner
    {
        string color = "";
        if (watcher==false)
        {
            Player player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.uid == LudoManager.Instance.gameInfo.playerID);
            color = player.color;
        }
        else
        {
            Player player = LudoManager.Instance.matchDataManager.MatchData.players[0];
            color = player.color;
        }
        if (color == "Red")
        {
            SetColourState(PlayerPosition.RedBottom);
            ExecuteStateAction(PlayerPosition.RedBottom);
            return;
        }
        else if (color == "Blue")
        {
            ExecuteStateAction(PlayerPosition.BlueBottom);
            SetColourState(PlayerPosition.BlueBottom);
            return;
        }
        else if (color == "Green")
        {
            ExecuteStateAction(PlayerPosition.GreenBottom);
            SetColourState(PlayerPosition.GreenBottom);
            return;
        }
        else if (color == "Yellow")
        {
            ExecuteStateAction(PlayerPosition.YellowBottom);
            SetColourState(PlayerPosition.YellowBottom);
            return;
        }
    }
    private void setlocalPlayerTurn(Player foundSpot) // Set local players turn
    {
        if (foundSpot.player_number == 1)
        {
            localPlayerTurn = PlayerTurn.One;
            return;
        }
        else if (foundSpot.player_number == 2)
        {
            localPlayerTurn = PlayerTurn.Two;
            return;
        }
        else if (foundSpot.player_number == 3)
        {
            localPlayerTurn = PlayerTurn.Three;
            return;
        }
        else if (foundSpot.player_number == 4)
        {
            localPlayerTurn = PlayerTurn.Four;
            return;
        }
    }
    public int GetNextTurn(int currentTurn) // Getting the turn for next player
    {
        int turn=turnManager.GetNextTurn(currentTurn);
        return turn;
    }
    public void SetColours()
    {
        if(watcher==false)
        boardMapping.SetColour();
        else
        {
            MaxPlayers= matchDataManager.MatchData.players.Count;
            boardMapping.setWatcherGame();
        }
    }
    public PlayerController GetNextPlayer(PlayerController currentPlayer)
    {
        int currentIndex = players.IndexOf(currentPlayer);
        if (currentIndex == -1)
        {
            return null;
        }
        int nextIndex = (currentIndex + 1) % players.Count;
        return players[nextIndex];
    }

    public void setPawnsScale() // Set pawns scale according to their position if they are in home or outside the home
    {
        for (int i = 0; i < players.Count; i++)
        {
            for (int j = 0; j < players[i].myPawns.Count; j++)
            {
                if (homeIndexes.Contains(players[i].myPawns[j].currentIndex))
                {
                    players[i].myPawns[j].GetComponent<RectTransform>().localScale = new Vector3(2, 2, 2);
                }
                else
                {
                    players[i].myPawns[j].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                }
            }
        }
    }
    public void setBoardAndTiles()
    {
        //if(watcher==true)
        //{
        //    localPlayerTurn = PlayerTurn.One;
        //}
        boardMapping.setBoard();
    }
    private void ExecuteStateAction(PlayerPosition state)
    {
        if (ColourStateActions.TryGetValue(state, out Action action))
        {
            action?.Invoke();
        }
    }
    public void StateCheck() // Setting the colour chosen by player at left bottom of board
    {
        boardMapping.StateCheck(CurrentLocalPosition);
    }
    private void SetColourState(PlayerPosition newState)
    {
        if (CurrentLocalPosition != newState)
        {
            CurrentLocalPosition = newState;
            ExecuteStateAction(newState);
        }
    }
    public void addLobby() // Adding the lobby, first page selection
    {
        if(UIManager.Instance.ClassicMode.On)
        {
            mode = Mode.Classic;
        }
        else
        {
            mode = Mode.Modern;
        }
        if (UIManager.Instance.WinWithOneToken.On)
        {
            winningCriteria = WinningCriteria.OnePawn;
        }
        else
        {
            winningCriteria = WinningCriteria.FourPawn;
        }
    }
    public void startTheMatch(string json) // Called when the lobby gets full and the match gets start to share the match data and setting the board
    {
        matchDataManager.MatchData = new MatchData();
        MatchData matchdata = JsonConvert.DeserializeObject<MatchData>(json);
        matchDataManager.MatchData = matchdata; // Access the first (and only) item in the list
        time = matchDataManager.MatchData.match_start_time;
        MaxPlayers = matchDataManager.MatchData.players.Count;
        UIManager.Instance.whatToShow("GameWindow", false);
        timerText.text=time.ToString();
        matchStartedUI.SetActive(false);
        MainThreadDispatcher.RunOnMainThread(() =>
        {
            UIManager.Instance.whatToShow("GameWindow", false);
            matchStartedUI.SetActive(true);
        });
        SetEverythingAccordingToMe();
        if(this.gameObject!=null)
        StartCoroutine(startTimer());
        if(matchDataManager.MatchData.mode == "Modern")
            modernMode.setStartAndEnd(matchDataManager.MatchData.match_started, matchDataManager.MatchData.match_ended);
    }
    IEnumerator startTimer() // "match-start" is given by server when player comes inside match when the game is started from first time , 3-2-1-Go
    {
        if(time>0)
        {
            timerText.text = time.ToString();
            time = time - 1;
            matchStartedUI.SetActive(true);
            AudioManager.instance.PlayAudio("countdown");
            yield return new WaitForSeconds(1f);
        if(this.gameObject!=null)
            StartCoroutine(startTimer());
        }
        else
        {
            matchStartedUI.SetActive(false);
        }
    }
    public void goInsideMatch()
    {
        UIManager.Instance.whatToShow("GameWindow", false);
        for (int i=0;i< matchDataManager.MatchData.players.Count;i++)
        {
            players[i].PlayerID = matchDataManager.MatchData.players[i].uid;
        }
        PlayerController playerController = players.Find(spot => spot.PlayerID == gameInfo.playerID);
        lobbymanager.activeLobby.LocalPlayerColour = playerController.myColour.ToString();
        MaxPlayers = matchDataManager.MatchData.players.Count; 
        SetEverythingAccordingToMe();
    }
    public void otherPlayerJoinedLobby(JoinedPlayerInformation playersInLobby) // When any other player joins the lobby
    {
        if (!LudoManager.Instance.lobbymanager.PlayersInLobby.ContainsKey(playersInLobby.uid.ToString()))
        {
            JoinedPlayerInformation playersinlobby = new JoinedPlayerInformation();
            playersinlobby.color = playersInLobby.color.ToString();
            playersinlobby.uid = playersInLobby.uid.ToString();
            playersinlobby.avatar = playersInLobby.avatar;
            playersinlobby.userName = playersInLobby.userName;
            LudoManager.Instance.lobbymanager.PlayersInLobby.Add(playersInLobby.uid.ToString(), playersinlobby);
        }
        lobbymanager.updateFindingScreen();
    }
    
    public void playerExitted(string playerExitted) // When Player Exits the lobby, called from "lobby-exited"
    {
        lobbymanager.playerExitted(playerExitted);
        lobbymanager.exitLobbies();
        lobbymanager.updateFindingScreen();
        PlayerExitted exit = JsonConvert.DeserializeObject<PlayerExitted>(playerExitted);
        if (exit.uid == gameInfo.playerID)
        {
            UIManager.Instance.whatToShow("HomePage", true);
        }
    }
    public void exitLobbies()
    {
        lobbymanager.exitLobbies();
    }
    private void startModernMode() // Start the modern mode, setup UI and coins accordingly
    {
        if (matchDataManager.MatchData.mode == "Modern")
        {
            modernMode.gameObject.SetActive(true);
            for (int i = 0; i < matchDataManager.MatchData.players.Count; i++)
            {
                PlayerController player = players.Find(spot => spot.TurnNumber == matchDataManager.MatchData.players[i].player_number);
                if (player != null)
                {
                    player.userDetails.modernCoin.gameObject.SetActive(true);// = null;
                }
                player.userDetails.modernCoin.totalCoin = int.Parse(matchDataManager.MatchData.players[i].totalCoins);
                player.userDetails.modernCoin.setCoins();
            }
            modernMode.setStartAndEnd(matchDataManager.MatchData.match_started, matchDataManager.MatchData.match_ended);
        }
        else
        {
            modernMode.gameObject.SetActive(false);
            for (int i = 0; i < matchDataManager.MatchData.players.Count; i++)
            {
                PlayerController player = players.Find(spot => spot.TurnNumber == matchDataManager.MatchData.players[i].player_number);
                if (player != null)
                {
                    player.userDetails.modernCoin.gameObject.SetActive(false);
                }
            }
        }
    }
    public void checkStatus(string json, bool reload) // When game reloads again, to know where the player was, "status" is the response
    {
        TheLobby result = JsonConvert.DeserializeObject<TheLobby>(json);
        if(result.lobby_data!=null) // Taking the player inside Lobby after reloading or disconnection with the help of "status" response
        {
            lobbymanager.setLobbyFromStatus(result,json); // Taking the player inside Lobby after reloading or disconnection with the help of "status" response
        }
        if (result.match_data==null) // if matchdata is null go inside the lobby
        {
            matchDataManager.MatchData =result.match_data;
            lobbymanager.goInsideLobby(result.lobby_data.current_datetime, result.lobby_data.started_at, result.lobby_data.player_count);
            lobbymanager.updateFindingScreen();
        }
        else // if matchdata is not null, take the user to the match
        {
            UIManager.Instance.whatToShow("GameWindow", false);
            UIManager.Instance.Panels["Result"].SetActive(false);
            matchDataManager.MatchData = result.match_data;
            MaxPlayers= matchDataManager.MatchData.players.Count;
            if (result.match_data.result != null)
            {
                matchDataManager.MatchData.result = result.match_data.result;
            }
            lobbymanager.updateFindingScreen();
            SetEverythingAccordingToMe();
            DisplayResultOnBoard();
            turnManager.PlayTurnWithoutTime();
            matchDataManager.setCurrentUpdate();
            if (matchDataManager.MatchData != null && matchDataManager.MatchData.mode == "Modern")
            {
                startModernMode();
            }
            else
            {
                modernMode.gameObject.SetActive(false);
            }
            if (matchDataManager.MatchData.result.Count != 0)
            {
                resultDisplay();
            }
        }
    }
    private void resultDisplay() // Show the result on board if game is not completed and show the result panel if the game is completed
    {
        StartCoroutine(ShowResult());// Show the result on board if game is not completed
        if (matchDataManager.MatchData.result.Count == matchDataManager.MatchData.players.Count)//  show the result panel if the game is completed
        {
            WhoIsWinner();
        }
    }
    public void checkGameStatus(string json, bool reload) //"status-game" Response to know the current state of the game
    {
        GameUpdate result = JsonConvert.DeserializeObject<GameUpdate>(json);
        matchDataManager.saveGameUpdate(result);
        
        for(int i=0;i< matchDataManager.MatchData.players.Count;i++)
        {
            setMissingTurns(matchDataManager.MatchData.players[i].uid, int.Parse(matchDataManager.MatchData.max_missing), matchDataManager.MatchData.players[i].missed_turn);
        }
        SetEverythingAccordingToMe();
        if(result.current_update!=null)
        {
            matchDataManager.MatchData.current_update=result.current_update;
        }
        matchDataManager.setCurrentUpdate();
        lobbymanager.updateFindingScreen();
        if(matchDataManager.MatchData.result.Count!=0)
        {
            resultDisplay();
        }
    }
    public void JoiningTheLobby(string json) //"lobby-join" when player joins any lobby
    {
        lobbymanager.PlayersInLobby.Clear();
        lobbymanager.joiningTheLobby(json);
    }
    public void receivedLobby(string response) //"receiveLobby" Data to know the lobbies present
    {
        lobbymanager.receiveLobby(response);
    }
    public void restart() //"Restarting the game from fresh"
    {
        StartCoroutine(restartGame());
    }
    IEnumerator restartGame() //"Restarting the game from fresh"
    {
        GameSocketManager.instance.loading();
        timerPlayer.StopTimer();
        timerPlayer.isTimerRunning = false;
        matchDataManager.MatchData = new MatchData();
        GameSocketManager.instance.ludoSocket.Disconnect();
        yield return new WaitForSeconds(1f);
        Destroy(GameSocketManager.instance.gameObject);
        Destroy(ChatManager.instance.gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene("LudoLatest");
        StopAllCoroutines();
    }
    public void Quit()
    {
        Application.Quit();
    }
}