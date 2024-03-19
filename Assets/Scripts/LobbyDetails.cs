using AYellowpaper.SerializedCollections;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyDetails
{
    public string _id;
    public string ep;
    public string mode;
    public string wp;
    public int bonus_limit;
    public List<JoinedPlayerInformation> players;
    public int activePlayers;
    //public List<object> watcher;
    public int player_count ;
    public string win_with_token ;
    public string started_at;
    public string current_datetime;
    public string created_by;
    public DateTime createdAt;
    public DateTime updatedAt;
    public int __v;
    public string color;
    public int time;
    public int avatar;
    public bool is_active;
    public string userName;
    public bool is_private;
    public string game_code;
    public string account;
}
public class PlayerExitted
{
    public string uid;
}
[Serializable]
public class Result
{
    public string uid;
    public string position;
    public string winning_amount;
    //public string bet_amount;
    //public string _id;
    public int avatar { get; set; }
    public string name { get; set; }
}
public class PlayerResult
{
    public string uid { get; set; }
    public string name { get; set; }
    public int avatar { get; set; }
    public string position { get; set; }
    public string winning_amount { get; set; }
    public string bet_amount { get; set; }
    public string _id { get; set; }
}

public class ResultData
{
    public string _id { get; set; }
    public List<PlayerResult> result { get; set; }
}
public class playerExitLobby
{
    public string uid;
    public string lobby_id;
}
public class TheLobby
{
    public LobbyDetails lobby_data;
    public MatchData match_data;
}
public class GameUpdate
{
    //public string currentTurn;
    public List<Result> result;
    public List<Player> players;
    public string match_started;
    public string match_ended;
    public CurrentUpdate current_update { get; set; }
}
public class GetLobby
{
    public string players;
    public string mode;
    public string ep;
    public string prizepool;
    public string uid;
}
public class GETData
{
    public string uid;
}
public class BetData
{
    public List<int> BetAmountsArr;

    public int WinPercent;
}
[Serializable]
public class ActiveLobbyDetails
{
    public string Mode;
    public int NoOfPlayers;
    public int Time;
    public int EntryPrice;
    public int WinningPrice;
    public int tokensToWin;
    public string LocalPlayerColour;
    public string lobbyID;
    public string gameCode;
    public bool is_private;
    public string started_at;
    public string current_datetime;

    public string LobbyIDJoining;
    public string ModeOfGameJoining;
    public string TokenToWinInJoining;
    public string JoiningColour;
    public int MaxPlayersCount;
    public bool inviteFriends;
}





