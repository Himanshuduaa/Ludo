using AYellowpaper.SerializedCollections;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct JoinedPlayerInformation
{
    public string uid;
    public string color;
    public int avatar;
    public string lobby_id;
    public string page;
    public string userName;
    public bool is_private;
}
[Serializable]
public class Player
{
    public string uid;
    public string color;
    [SerializedDictionary("Number", "Position")]
    public SerializedDictionary<string, int> pown_position;
    public bool has_turn;
    public int missed_turn;
    public bool pown_move;
    public int player_number;
    public string totalCoins;
    public int avatar;
    public string userName;
}
[Serializable]
public class CurrentUpdate
{
    public int player_number;
    public int timer_value;
    public string current_time;
    public string turn_start_time;
    public string action_type;
    public string timeup_time;
    public Dictionary<string, int> pown_move;
    public int dice_number;
}
public class TurnMissed
{
    public string uid;
    public int missed_turn;
}
public class Exitting
{
    public string uid;
    public string lobby_id;
}
public class USERDetails
{
    public GameSettings game_settings;
    public string _id;
    public string uid;
    public int avatar;
    public string referral_link;
    public string referral_code;
    public DateTime updatedAt;
    public string userToken;
    public string userName;
    public string balance;
    public string account;
    public string operatorToken;
}
[Serializable]
public class GameSettings
{
    public bool notifications;
    public bool music;
    public bool sound;
}
