using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "MatchData", menuName = "ScriptableObjects/MatchData", order = 1)]
public class MatchData : ScriptableObject
{
    public List<Player> players=new List<Player>();
    //public string color;
    public List<object> watcher=new List<object>();
    public bool is_active;
    public int match_start_time;
    public string match_id;
    public List<Result> result;
    public string _id;
    public string lobby_id;
    public string max_missing;
    public string match_started;
    public string match_ended;
    public string mode;
    public CurrentUpdate current_update;
    // Method to reset the values to their defaults
    [ContextMenu("DeleteData")]
    public void ResetValues()
    {
        players.Clear();
        watcher.Clear();
        is_active = false;
        match_start_time = 0;
        match_id = "";
        result.Clear();
        _id = "";
        lobby_id = "";
        max_missing = "";
        match_started = "";
        match_ended = "";
        mode = "";
        current_update= new CurrentUpdate();    
    }
}