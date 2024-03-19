using AYellowpaper.SerializedCollections;
using BestHTTP.JSON;
using Lean.Gui;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class LobbyManager : MonoBehaviour
{
    public ActiveLobbyDetails activeLobby;

    public List<PlayerProfile> TwoPlayerWindow = new List<PlayerProfile>();
    public List<PlayerProfile> FourPlayerWindow = new List<PlayerProfile>();
    public List<Lobby> lobbyList = new List<Lobby>();
    [SerializedDictionary("Player ID", "Player Information")]
    public SerializedDictionary<string, JoinedPlayerInformation> PlayersInLobby = new SerializedDictionary<string, JoinedPlayerInformation>();
    //public int Players;
    [Header("Lobby Joining")]
    //public string lobbyJoined;
    
    public LobbyWindow window2;
    public LobbyWindow window4;
    public GameObject Lobby;
    

    private IEnumerator LobbyRefresh()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            // Check if the target object is active
            if (Lobby != null && Lobby.activeSelf)
            {
                // Call your function here
                getLobby();
            }
        }
    }
    private void getLobby()
    {
        GameSocketManager.instance.GetLobby();
    }
    void Start()
    {
        activeLobby.JoiningColour = "Green";
        StartCoroutine(LobbyRefresh());
    }

    public void goInsideLobby(string current_datetime, string started, int count)
    {
        if (activeLobby.NoOfPlayers == 2) //Setting upthe 2 player lobby for player
        {
            UIManager.Instance.whatToShow("Finding2Players", false);
            UIManager.Instance.WinAmountMentionedInLobby2.text = "₹ " + activeLobby.WinningPrice.ToString();
            if (activeLobby.is_private == true)
            {
                UIManager.Instance.gameCode2PlayerButton.SetActive(true);
                UIManager.Instance.gameCode2Player.text = activeLobby.gameCode;
            }
            else
            {
                UIManager.Instance.gameCode2PlayerButton.SetActive(false);
            }
            startLobbyTime(current_datetime, started, count);
        }
        else if (LudoManager.Instance.lobbymanager.activeLobby.NoOfPlayers == 4) //Setting upthe 4 player lobby for player
        {
            UIManager.Instance.WinAmountMentionedInLobby4.text = "₹ " + activeLobby.WinningPrice.ToString();
            UIManager.Instance.whatToShow("Finding4Players", false);
            if (activeLobby.is_private == true)
            {
                UIManager.Instance.gameCode4PlayerButton.SetActive(true);
                UIManager.Instance.gameCode4Player.text = activeLobby.gameCode;
            }
            else
            {
                UIManager.Instance.gameCode2PlayerButton.SetActive(false);
            }
            startLobbyTime(current_datetime, started, count);
        }
        else
        {
            UIManager.Instance.whatToShow("HomePage", true);
        }
    }
    public void createLobby(string stringList)//"lobby-created" is the response when player creates any lobby
    {
        CreateLobby createdLobbyDetails = JsonConvert.DeserializeObject<CreateLobby>(stringList);
        if (createdLobbyDetails != null)
        {
            activeLobby.lobbyID = createdLobbyDetails.lobby_id;
            activeLobby.gameCode = createdLobbyDetails.game_code;
            activeLobby.started_at = createdLobbyDetails.started_at;
            activeLobby.current_datetime = createdLobbyDetails.current_datetime;
        }
        setupLobby();
    }
    private void setupLobby() // Setting up the lobby for player with the joined players, basically the finding screen
    {
        setupLobby(LudoManager.Instance.gameInfo);
        updateFindingScreen();
    }
    public void setLobbyFromStatus(TheLobby result, string json) // Taking the player inside Lobby after reloading or disconnection with the help of "status" response
    {
        setLobbyDetails(json);
        activeLobby.is_private = result.lobby_data.is_private;
        activeLobby.gameCode = result.lobby_data.game_code;

        for (int i = 0; i < result.lobby_data.players.Count; i++)
        {
            if (!PlayersInLobby.ContainsKey(result.lobby_data.players[i].uid))
            {
                JoinedPlayerInformation JoinedPlayerInf = new JoinedPlayerInformation();
                JoinedPlayerInf.uid = result.lobby_data.players[i].uid;
                JoinedPlayerInf.color = result.lobby_data.players[i].color;
                JoinedPlayerInf.avatar = result.lobby_data.players[i].avatar;
                JoinedPlayerInf.userName = result.lobby_data.players[i].userName;
                PlayersInLobby.Add(result.lobby_data.players[i].uid, JoinedPlayerInf);
            }
        }
    }
    public void setupLobby(GameInfo gameInfo)
    {
        if (!PlayersInLobby.ContainsKey(gameInfo.playerID))
        {
            JoinedPlayerInformation playersinlobby = new JoinedPlayerInformation();
            playersinlobby.color = activeLobby.LocalPlayerColour.ToString();
            playersinlobby.uid = gameInfo.playerID.ToString();
            playersinlobby.avatar = gameInfo.avatar;
            playersinlobby.userName = gameInfo.userName;
            PlayersInLobby.Add(gameInfo.playerID, playersinlobby);
        }
        if (activeLobby.NoOfPlayers == 2)
        {
            UIManager.Instance.whatToShow("Finding2Players", false);
            UIManager.Instance.WinAmountMentionedInLobby2.text = "₹ " + activeLobby.WinningPrice.ToString();

        }
        else if (activeLobby.NoOfPlayers == 4)
        {
            UIManager.Instance.WinAmountMentionedInLobby4.text = "₹ " + activeLobby.WinningPrice.ToString();
            UIManager.Instance.whatToShow("Finding4Players", false);
        }
        startLobbyTime(activeLobby.current_datetime, activeLobby.started_at, activeLobby.NoOfPlayers);
    }
    public void updateFindingScreen()
    {
        if (LudoManager.Instance.matchDataManager.MatchData == null || LudoManager.Instance.matchDataManager.MatchData.match_id == "")
        {
            if (activeLobby.NoOfPlayers == 2)
            {
                UIManager.Instance.Panels["2PlayerWindow"].SetActive(true);
                if (activeLobby.is_private == true)
                {
                    UIManager.Instance.gameCode2PlayerButton.SetActive(true);
                    UIManager.Instance.gameCode2Player.text = activeLobby.gameCode;
                }
                else
                {
                    UIManager.Instance.gameCode2PlayerButton.SetActive(false);
                }
                set2PlayerWindow();
            }
            else if (activeLobby.NoOfPlayers == 4)
            {
                UIManager.Instance.Panels["4PlayerWindow"].SetActive(true);
                if (activeLobby.is_private == true)
                {
                    UIManager.Instance.gameCode4PlayerButton.SetActive(true);
                    UIManager.Instance.gameCode4Player.text = activeLobby.gameCode;
                }
                else
                {
                    UIManager.Instance.gameCode4PlayerButton.SetActive(false);
                }
                set4PlayerWindow();
            }
            else
            {
                UIManager.Instance.whatToShow("HomePage", true);
            }
        }
        if (LudoManager.Instance.matchDataManager.MatchData != null)
        {
            if (LudoManager.Instance.matchDataManager.MatchData.match_id != "")
            {
                UIManager.Instance.whatToShow("GameWindow", false);
            }
        }
    }
    public void set2PlayerWindow()
    {
        for (int i = 0; i < TwoPlayerWindow.Count; i++)
        {
            try
            {
                JoinedPlayerInformation element = PlayersInLobby.ElementAt(i).Value;
                TwoPlayerWindow[i].playerSearch.gameObject.SetActive(false);
                TwoPlayerWindow[i].searching = false;
                TwoPlayerWindow[i].profilePicture.sprite = UIManager.Instance.avatars[element.avatar].GetComponent<Image>().sprite;
                if (PlayersInLobby.ElementAt(i).Key == LudoManager.Instance.gameInfo.playerID)
                {
                    TwoPlayerWindow[i].nameOfPlayer.text = "You";
                }
                else
                {
                    TwoPlayerWindow[i].nameOfPlayer.text = element.userName;
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                TwoPlayerWindow[i].searching = true;
                TwoPlayerWindow[i].startRolling();
                TwoPlayerWindow[i].nameOfPlayer.text = "Finding...";
            }
        }
    }
    public void EmitExitLobby()
    {
        GameSocketManager.instance.ExitLobby();
    }
    public void set4PlayerWindow()
    {
        for (int i = 0; i < FourPlayerWindow.Count; i++)
        {
            try
            {
                // Attempt to access an element in an array or collection
                JoinedPlayerInformation element = LudoManager.Instance.lobbymanager.PlayersInLobby.ElementAt(i).Value;
                FourPlayerWindow[i].playerSearch.gameObject.SetActive(false);
                FourPlayerWindow[i].searching = false;
                FourPlayerWindow[i].profilePicture.sprite = UIManager.Instance.avatars[element.avatar].GetComponent<Image>().sprite;
                Debug.Log("Sprite player is " + FourPlayerWindow[i].profilePicture.sprite.name);
                if (PlayersInLobby.ElementAt(i).Key == LudoManager.Instance.gameInfo.playerID)
                {
                    FourPlayerWindow[i].nameOfPlayer.text = "You";
                }
                else
                {
                    FourPlayerWindow[i].nameOfPlayer.text = element.userName;
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                FourPlayerWindow[i].searching = true;
                FourPlayerWindow[i].startRolling();
                FourPlayerWindow[i].nameOfPlayer.text = "Finding...";
            }
        }
    }
    public void exitLobbies()
    {
        for (int i = 0; i < FourPlayerWindow.Count; i++)
        {
            FourPlayerWindow[i].searching = true;
            FourPlayerWindow[i].startRolling();
            FourPlayerWindow[i].nameOfPlayer.text = "Finding...";
        }
        for (int i = 0; i < TwoPlayerWindow.Count; i++)
        {
            TwoPlayerWindow[i].searching = true;
            TwoPlayerWindow[i].startRolling();
            TwoPlayerWindow[i].nameOfPlayer.text = "Finding...";
        }
    }
    public void playOnline()
    {
        for (int i = 0; i < TwoPlayerWindow.Count; i++)
        {
            TwoPlayerWindow[i].resetAll();
        }
        for (int i = 0; i < TwoPlayerWindow.Count; i++)
        {
            FourPlayerWindow[i].resetAll();
        }
        PlayersInLobby.Clear();
        activeLobby.inviteFriends = false;
    }
    public void joiningTheLobby(string json)
    {
        LobbyDetails lobbyDetailsList = JsonConvert.DeserializeObject<LobbyDetails>(json);
        if (lobbyDetailsList.is_active == true)
        {
            if (/*LudoManager.Instance.RoomJoiningFor*/ lobbyDetailsList.player_count == 2)
            {
                UIManager.Instance.TwoPlayerMode.text = lobbyDetailsList.mode;//LudoManager.Instance.lobbymanager.ModeOfGameJoining.ToString();
                UIManager.Instance.TwoPlayerMode2.text = lobbyDetailsList.win_with_token.ToString()+ " Token To Win";//LudoManager.Instance.lobbymanager.TokenToWinInJoining.ToString() + " Token To Win";
                UIManager.Instance.whatToShow("Finding2Players", false);
                UIManager.Instance.WinAmountMentionedInLobby2.text = "₹ " + lobbyDetailsList.wp.ToString();


            }
            else if (/*LudoManager.Instance.RoomJoiningFor*/ lobbyDetailsList.player_count == 4)
            {
                UIManager.Instance.FourPlayerMode.text = lobbyDetailsList.mode;//LudoManager.Instance.lobbymanager.ModeOfGameJoining.ToString();
                UIManager.Instance.FourPlayerMode2.text = lobbyDetailsList.win_with_token.ToString() + " Token To Win";//LudoManager.Instance.lobbymanager.TokenToWinInJoining.ToString() + " Token To Win";
                UIManager.Instance.WinAmountMentionedInLobby4.text = "₹ " + lobbyDetailsList.wp.ToString();
                UIManager.Instance.whatToShow("Finding4Players", false);

            }
            LobbyDetails result = lobbyDetailsList; // Access the first (and only) item in the list
            Console.WriteLine(result); // Output: 64f56811d777567dc375d12e
            activeLobby.lobbyID = result._id;
            activeLobby.EntryPrice = int.Parse(result.ep);
            activeLobby.Mode = result.mode;
            activeLobby.NoOfPlayers = result.player_count;
            activeLobby.tokensToWin = int.Parse(result.win_with_token);
            activeLobby.WinningPrice = int.Parse(result.wp);

            for (int i = 0; i < result.players.Count; i++)
            {
                if (!PlayersInLobby.ContainsKey(result.players[i].uid))
                {
                    JoinedPlayerInformation JoinedPlayerInf = new JoinedPlayerInformation();
                    JoinedPlayerInf.uid = result.players[i].uid;
                    JoinedPlayerInf.color = result.players[i].color;
                    JoinedPlayerInf.avatar = result.players[i].avatar;
                    JoinedPlayerInf.userName = result.players[i].userName;
                    //JoinedPlayerInf.id = result._id;
                    PlayersInLobby.Add(result.players[i].uid, JoinedPlayerInf);
                }
            }
            updateFindingScreen();
            startLobbyTime(lobbyDetailsList.current_datetime, lobbyDetailsList.started_at, lobbyDetailsList.player_count);
        }
        else
        {
            UnityEngine.Debug.Log("LOBBY IS FULL");
        }
    }
    public void startLobbyTime(string current,string started_at, int count)
    {
        if(count==2)
        {
            window2.StartedAt = started_at.ToString();
            window2.startTimer();
        }
        if (count == 4)
        {
            window4.StartedAt = started_at.ToString();
            window4.startTimer();
        }
    }
    public void playerExitted(string playerExitted)
    {
        //LobbyDetails result = JsonConvert.DeserializeObject<LobbyDetails>(lobbyDetailsList);
        PlayerExitted exit = JsonConvert.DeserializeObject<PlayerExitted>(playerExitted);
        //Console.WriteLine(result);
        //activeLobby.lobbyID = result._id;
        //activeLobby.EntryPrice = int.Parse(result.ep);
        //activeLobby.Mode = result.mode;
        //activeLobby.NoOfPlayers = result.player_count;
        //activeLobby.tokensToWin = int.Parse(result.win_with_token);
        //activeLobby.WinningPrice = int.Parse(result.wp);
        //PlayersInLobby.Clear();
        PlayersInLobby.Remove(exit.uid);
        //for (int i = 0; i < result.players.Count; i++)
        //{
        //    if (!PlayersInLobby.ContainsKey(result.players[i].uid))
        //    {
        //        JoinedPlayerInformation JoinedPlayerInf = new JoinedPlayerInformation();
        //        JoinedPlayerInf.uid = result.players[i].uid;
        //        JoinedPlayerInf.color = result.players[i].color;
        //        JoinedPlayerInf.avatar = result.players[i].avatar;
        //        JoinedPlayerInf.userName = result.players[i].userName;
        //        PlayersInLobby.Add(result.players[i].uid, JoinedPlayerInf);
        //    }
        //}
    }
    private string checkColour()
    {
       if(UIManager.Instance.joinRed.On==true)
        {
            return "Red";
        }
        if (UIManager.Instance.joinBlue.On == true)
        {
            return "Blue";
        }
        if (UIManager.Instance.joinGreen.On == true)
        {
            return "Green";
        }
        if (UIManager.Instance.joinYellow.On == true)
        {
            return "Yellow";
        }
        return "Green";
    }
    public void setLobbyDetails(string json)
    {
        TheLobby result = JsonConvert.DeserializeObject<TheLobby>(json);
        Console.WriteLine(result);
        activeLobby.lobbyID = result.lobby_data._id;
        activeLobby.EntryPrice = int.Parse(result.lobby_data.ep);
        activeLobby.EntryPrice = int.Parse(result.lobby_data.ep);
        activeLobby.Mode = result.lobby_data.mode;
        activeLobby.NoOfPlayers = result.lobby_data.player_count;
        activeLobby.tokensToWin = int.Parse(result.lobby_data.win_with_token);
        activeLobby.WinningPrice = int.Parse(result.lobby_data.wp);
        for (int i = 0; i < result.lobby_data.players.Count; i++)
        {
            if (!PlayersInLobby.ContainsKey(result.lobby_data.players[i].uid))
            {
                JoinedPlayerInformation JoinedPlayerInf = new JoinedPlayerInformation();
                JoinedPlayerInf.uid = result.lobby_data.players[i].uid;
                JoinedPlayerInf.color = result.lobby_data.players[i].color;
                JoinedPlayerInf.avatar = result.lobby_data.players[i].avatar;
                JoinedPlayerInf.userName = result.lobby_data.players[i].userName;
                PlayersInLobby.Add(result.lobby_data.players[i].uid, JoinedPlayerInf);
            }
        }
    }
    public void JoinALobby()
    {
        JoinedPlayerInformation join = new JoinedPlayerInformation();
        join.lobby_id = activeLobby.LobbyIDJoining;
        join.uid = LudoManager.Instance.gameInfo.playerID;
        join.color = checkColour();
        //LudoManager.Instance.matchDataManager.MatchData.color = join.color;
        join.avatar = LudoManager.Instance.gameInfo.avatar;
        join.userName = LudoManager.Instance.gameInfo.userName;
        join.is_private = false;
        LudoManager.Instance.lobbymanager.activeLobby.LocalPlayerColour = checkColour();
        if (activeLobby.MaxPlayersCount == 2)
        {
            join.page = "Finding2Players";
        }
        else if (activeLobby.MaxPlayersCount == 4)
        {
            join.page = "Finding4Players";
        }
        string json = JsonConvert.SerializeObject(join);
        UnityEngine.Debug.Log(json);
        LudoManager.Instance.lobbymanager.activeLobby.ModeOfGameJoining = activeLobby.ModeOfGameJoining;
        LudoManager.Instance.lobbymanager.activeLobby.TokenToWinInJoining = activeLobby.TokenToWinInJoining;
        //UIManager.Instance.Panels["SelectJoiningColour"].SetActive(true);
        GameSocketManager.instance.joinLobby(json);
    }

    public void openInviteFriends()
    {
        activeLobby.inviteFriends = true;
        activeLobby.is_private = true;
        UIManager.Instance.Panels["AmountSelection"].SetActive(true);
    }
    public void setJoiningColour(string colour)
    {
        activeLobby.JoiningColour = colour;
        //LudoManager.Instance.matchDataManager.MatchData.color = colour;
    }
    public void SetPlayers(int players)
    {
        activeLobby.NoOfPlayers = players;
    }
    public void SetMode(string mode)
    {
        activeLobby.Mode = mode;
        setModerModeUI();
    }
    private void setModerModeUI()
    {
        if (activeLobby.Mode == "Modern")
        {
            UIManager.Instance.WinWithFourToken.GetComponent<LeanButton>().interactable = false;
            UIManager.Instance.WinWithOneToken.GetComponent<LeanButton>().interactable = false;
            UIManager.Instance.WinWithFourToken.GetComponent<Image>().color = Color.grey;
            UIManager.Instance.WinWithOneToken.GetComponent<Image>().color = Color.grey;
        }
        else
        {
            UIManager.Instance.WinWithFourToken.GetComponent<LeanButton>().interactable = true;
            UIManager.Instance.WinWithOneToken.GetComponent<LeanButton>().interactable = true;
            UIManager.Instance.WinWithFourToken.GetComponent<Image>().color = Color.white;
            UIManager.Instance.WinWithOneToken.GetComponent<Image>().color = Color.white;
        }
    }
    public void TokensToWin(int Tokens)
    {
        activeLobby.tokensToWin = Tokens;
    }
    public void setMyColour(string colour)
    {
        activeLobby.LocalPlayerColour = colour;
    }
    public void conFirmDetails()
    {
        confirmingEntryAndWinning();
        confirmingLocalColour();
        confirmingPlayers();
        confirmingMode();
        confirmingTokensToWin();
        confirmingTimer();
        sendLobbyDetails();
        UpdateWindowFinding();
    }
    public void UpdateWindowFinding()
    {
        if(activeLobby.NoOfPlayers == 2)
        {
            UIManager.Instance.TwoPlayerMode.text = activeLobby.Mode;
            UIManager.Instance.TwoPlayerMode2.text = activeLobby.tokensToWin.ToString() + " Tokens to win";
        }
        else if (activeLobby.NoOfPlayers == 4)
        {
            UIManager.Instance.FourPlayerMode.text = activeLobby.Mode;
            UIManager.Instance.FourPlayerMode2.text = activeLobby.tokensToWin.ToString()+" Tokens to win";
        }
    }
    public void sendLobbyDetails()
    {
        LobbyDetails lobby = new LobbyDetails();
        lobby.created_by =LudoManager.Instance.gameInfo.playerID;
        lobby.ep = activeLobby.EntryPrice.ToString();
        lobby.wp = activeLobby.WinningPrice.ToString();
        lobby.mode=activeLobby.Mode;
        lobby.color=activeLobby.LocalPlayerColour;
        lobby.player_count = activeLobby.NoOfPlayers;
        lobby.time = activeLobby.Time;
        lobby.bonus_limit = 5;
        lobby.win_with_token = activeLobby.tokensToWin.ToString();
        lobby.userName = LudoManager.Instance.gameInfo.userName;
        lobby.avatar = LudoManager.Instance.gameInfo.avatar;
        lobby.is_private = activeLobby.is_private;
        lobby.account = LudoManager.Instance.gameInfo.account;
        JoinedPlayerInformation joinedplayer=new JoinedPlayerInformation();
        joinedplayer.uid=LudoManager.Instance.gameInfo.playerID.ToString();
        joinedplayer.color=activeLobby.LocalPlayerColour.ToString();
        joinedplayer.avatar=LudoManager.Instance.gameInfo.avatar;
        joinedplayer.userName = LudoManager.Instance.gameInfo.userName;
        if(activeLobby.NoOfPlayers == 2)
        {
            joinedplayer.page= "Finding2Players";
        }
        else if (activeLobby.NoOfPlayers == 4)
        {
            joinedplayer.page = "Finding4Players";
        }
        UnityEngine.Debug.Log("Created lobby data is " + lobby);
        GameSocketManager.instance.CreateLobby(Newtonsoft.Json.JsonConvert.SerializeObject(lobby));
    }
    private void confirmingPlayers()
    {
        if (UIManager.Instance.Select2Players.On)
        {
            activeLobby.NoOfPlayers = 2;
        }
        else if (UIManager.Instance.Select4Players.On)
        {
            activeLobby.NoOfPlayers = 4;
        }
    }
    private void confirmingMode()
    {
        if (UIManager.Instance.ClassicMode.On)
        {
            activeLobby.Mode = "Classic";
        }
        else if (UIManager.Instance.ClassicProSelected.On)
        {
            activeLobby.Mode = "Classic Pro";
        }
        else if (UIManager.Instance.ModernSelected.On)
        {
            activeLobby.Mode = "Modern";
        }
    }
    private void confirmingTokensToWin()
    {
        if(activeLobby.Mode !="Modern")
        {
            if (UIManager.Instance.OneTokenToWin.On)
            {
                activeLobby.tokensToWin = 1;
            }
            else if (UIManager.Instance.FourTokenToWin.On)
            {
                activeLobby.tokensToWin = 4;
            }
        }
        else
        {
            activeLobby.tokensToWin = 4;
        }
    }
    private void confirmingEntryAndWinning()
    {

    }
    private void confirmingLocalColour()
    {
        if (UIManager.Instance.Red.On)
        {
            activeLobby.LocalPlayerColour = "Red";
        }
        else if (UIManager.Instance.Blue.On)
        {
            activeLobby.LocalPlayerColour = "Blue";
        }
        else if (UIManager.Instance.Green.On)
        {
            activeLobby.LocalPlayerColour = "Green";
        }
        else if (UIManager.Instance.Yellow.On)
        {
            activeLobby.LocalPlayerColour = "Yellow";
        }
        Debug.Log("Local player colour is "+activeLobby.LocalPlayerColour); 
    }
    private void confirmingTimer()
    {
        if (UIManager.Instance.Timer2.On)
        {
            activeLobby.Time = 1;
        }
        else if (UIManager.Instance.Timer5.On)
        {
            activeLobby.Time = 5;
        }
        else if (UIManager.Instance.Timer7.On)
        {
            activeLobby.Time = 7;
        }
        else if (UIManager.Instance.Timer10.On)
        {
            activeLobby.Time = 10;
        }
        else if (UIManager.Instance.Timer15.On)
        {
            activeLobby.Time = 15;
        }
    }
    public void ClearLobbyContent()
    {
        foreach (var item in lobbyList)
        {
            //Debug.Log(item.gameObject.name);
            //if(item.gameObject.name!="LoadingDots")
            Destroy(item);
        }
        lobbyList.Clear();
    }
    public void receiveLobby(string response)
    {
        if (lobbyList != null)
        {
            ClearLobbyContent();
        }
        string json = response.ToString();
        LudoManager.Instance.nestedGamesList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LobbyDetails>>(json);

        foreach (Transform child in UIManager.Instance.lobbyTransform)
        {
            if(child.gameObject.name!="LoadingDots")
            Destroy(child.gameObject);
            lobbyList.Clear();
        }
        foreach (var game in LudoManager.Instance.nestedGamesList)
        {
            GameObject lobbbyData = Instantiate(UIManager.Instance.LobbyDataPrefab, UIManager.Instance.lobbyTransform);
            lobbbyData.gameObject.GetComponent<Lobby>().Mode.text = game.mode;
            lobbbyData.gameObject.GetComponent<Lobby>().EntryPrice.text = game.ep.ToString();
            lobbbyData.gameObject.GetComponent<Lobby>().WinningPrice.text = game.wp.ToString();
            lobbbyData.gameObject.GetComponent<Lobby>().NoOfPlayers.text = game.activePlayers.ToString();
            lobbbyData.gameObject.GetComponent<Lobby>().StartedAt = game.started_at.ToString();
            lobbbyData.gameObject.GetComponent<Lobby>().win_with_token = game.win_with_token.ToString();
            lobbbyData.gameObject.GetComponent<Lobby>().mode = game.mode.ToString();
            lobbbyData.gameObject.GetComponent<Lobby>().id = game._id;
            lobbbyData.gameObject.GetComponent<Lobby>().MaxPlayersCount = game.player_count;
            lobbbyData.gameObject.GetComponent<Lobby>().MaxPlayersCountTMP.text = game.player_count.ToString();
            lobbbyData.gameObject.GetComponent<Lobby>().active = game.is_active;
            if (game.is_active)
            {
                lobbbyData.gameObject.GetComponent<Lobby>().joinLobbyButton.interactable = true;
                lobbbyData.gameObject.GetComponent<Lobby>().joinLobbyButton.gameObject.SetActive(true);
                lobbbyData.gameObject.GetComponent<Lobby>().joinWatcherButton.gameObject.SetActive(false);
                lobbbyData.gameObject.GetComponent<Lobby>().joinWatcherButton.interactable = false;
            }
            else
            {
                lobbbyData.gameObject.GetComponent<Lobby>().joinLobbyButton.interactable = false;
                lobbbyData.gameObject.GetComponent<Lobby>().joinLobbyButton.gameObject.SetActive(false);
                lobbbyData.gameObject.GetComponent<Lobby>().joinWatcherButton.gameObject.SetActive(true);
                lobbbyData.gameObject.GetComponent<Lobby>().joinWatcherButton.interactable = true;

            }
            //foreach (var player in game.players) 
            //{
            //    JoinedPlayerInformation playerInformation = new JoinedPlayerInformation();
            //    playerInformation.color = player.color;
            //    playerInformation.avatar = player.avatar;
            //    playerInformation.uid = player.uid;
            //    playerInformation.page = player.page;
            //    playerInformation.userName = player.userName;
            //    lobbbyData.gameObject.GetComponent<Lobby>().PlayersInLobby.Add(player.uid, playerInformation);
            //}
            lobbyList.Add(lobbbyData.GetComponent<Lobby>());
        }
        //GetTheLobby();
    }
    public void clearAll()
    {
        activeLobby.Mode = "";
        activeLobby.NoOfPlayers = 0;
        activeLobby.Time = 0;
        activeLobby.tokensToWin = 0;
        activeLobby.lobbyID = "";
        activeLobby.LobbyIDJoining = "";
        activeLobby.ModeOfGameJoining = "";
        activeLobby.TokenToWinInJoining = "";
        activeLobby.MaxPlayersCount = 0;
        PlayersInLobby.Clear();
    }
    public void joinGameCode()
    {
        if(UIManager.Instance.gameCode.text!="" || UIManager.Instance.gameCode.text.Length==7)
        {
            JoinWithGameCode join = new JoinWithGameCode();
            join.uid = LudoManager.Instance.gameInfo.playerID;
            join.game_code = UIManager.Instance.gameCode.text.ToLower();
            join.is_private = true;
            join.color = "Red";
            join.avatar = LudoManager.Instance.gameInfo.avatar;
            join.userName = LudoManager.Instance.gameInfo.userName;
            string json = JsonConvert.SerializeObject(join);
            activeLobby.LocalPlayerColour = "Red";
            Debug.Log("joining "+json);
            GameSocketManager.instance.joinLobby(json);
        }
    }
}
public class CreateLobby
{
    public string lobby_id;
    public string game_code;
    public bool is_private;
    public string started_at;     
    public string current_datetime;
}
public class RejoinLobby
{
    public string lobby_id;
    public string uid;
    public string color;
    public string avatar;
    public string userName;
    public string page;
}
public class JoinWithGameCode
{
    public string uid;
    public string game_code;
    public bool is_private;
    public string color;
    public int avatar;
    public string userName;
}

