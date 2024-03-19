using BestHTTP.JSON;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class SocketTurnManager : MonoBehaviour
{
    public TurnPlay turnPlay;
    public string turnofplayer;
    public string ServerDataOfplayer;
    public bool myTurn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayTurn(string json) // Play the turn, called from who's turn
    {
        TurnPlay turnPlay = JsonConvert.DeserializeObject<TurnPlay>(json);
        LudoManager.Instance.timerPlayer.resetValues();
        LudoManager.Instance.turnManager.turnPlay = turnPlay; // Access the first (and only) item in the list
        LudoManager.Instance.timerPlayer.currentTime = turnPlay.current_time;
        LudoManager.Instance.timerPlayer.startup_time = turnPlay.current_time;
        LudoManager.Instance.timerPlayer.timeUpTime = turnPlay.timeup_time;
        LudoManager.Instance.timerPlayer.actionType = turnPlay.action_type;
        //LudoManager.Instance.timerPlayer.timer_value = turnPlay.timer_value;

        PlayerController PlayerController = LudoManager.Instance.players.Find(spot => spot.TurnNumber == turnPlay.player_number);
        turnofplayer = PlayerController.userDetails.userName.text + " with turn " + PlayerController.TurnNumber+ " Colour "+ PlayerController.myColour+ " With ID " + PlayerController.PlayerID;
        LudoManager.Instance.diceManager.DicePlayed.player_number = PlayerController.TurnNumber;
        ServerDataOfplayer = turnPlay.player_number.ToString() ;
        for (int i = 0; i < LudoManager.Instance.players.Count; i++)
        {
            LudoManager.Instance.players[i].playerBG.GetComponent<Animator>().SetBool("Blink", false);
            for (int j = 0; j < LudoManager.Instance.players[i].myPawns.Count; j++)
            {
                LudoManager.Instance.players[i].myPawns[j].GetComponent<UnityEngine.UI.Image>().raycastTarget = false;
            }
        }
        myTurn = false;

        for (int i = 0; i < LudoManager.Instance.players.Count; i++)
        {
            if (LudoManager.Instance.players[i].PlayerID == PlayerController.PlayerID)
            {
                LudoManager.Instance.players[i].playerBG.GetComponent<Animator>().SetBool("Blink", true);
                if(PlayerController.PlayerID==LudoManager.Instance.gameInfo.playerID)
                {
                    myTurn = true;
                }
            }
            else
            {
                LudoManager.Instance.players[i].playerBG.GetComponent<Animator>().SetBool("Blink", false);
            }
        }
        
        for (int i = 0; i < PlayerController.myPawns.Count; i++)
        {
            PlayerController.myPawns[i].GetComponent<UnityEngine.UI.Image>().raycastTarget = true;
        }

        if (PlayerController.PlayerID == LudoManager.Instance.gameInfo.playerID && turnPlay.action_type == "dice_run" && myTurn)
        {
            LudoManager.Instance.diceManager.diceState(true);
        }
        else
        {
            Debug.Log("Turning off dice");
            LudoManager.Instance.diceManager.diceState(false);
        }
        if (LudoManager.Instance.timerPlayer.actionType == "dice_run" || LudoManager.Instance.timerPlayer.actionType == "pawn_run")
        {
            LudoManager.Instance.timerPlayer.isTimerRunning = true;
            LudoManager.Instance.timerPlayer.StopTimer();
            LudoManager.Instance.timerPlayer.ShowTime();
        }
    }
    public void PlayTurnWithoutTime()
    {
        LudoManager.Instance.timerPlayer.actionType = turnPlay.action_type;
        Player player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.uid == LudoManager.Instance.gameInfo.playerID);
        PlayerController PlayerController = LudoManager.Instance.players.Find(spot => spot.PlayerID == player.uid);
        if(player.has_turn)
        {
            for (int i = 0; i < LudoManager.Instance.players.Count; i++)
            {
                if (LudoManager.Instance.players[i].PlayerID == PlayerController.PlayerID)
                {
                    LudoManager.Instance.players[i].playerBG.GetComponent<Animator>().SetBool("Blink", true);
                }
                else
                {
                    LudoManager.Instance.players[i].playerBG.GetComponent<Animator>().SetBool("Blink", false);
                }
            }
        }
        for(int i=0;i<LudoManager.Instance.players.Count;i++)
        {
            for(int j=0;j< LudoManager.Instance.players[i].myPawns.Count;j++)
            {
                LudoManager.Instance.players[i].myPawns[j].GetComponent<UnityEngine.UI.Image>().raycastTarget=false;
            }
        }
        
        for (int i=0;i< PlayerController.myPawns.Count;i++)
        {
            PlayerController.myPawns[i].GetComponent<UnityEngine.UI.Image>().raycastTarget = true;
        }
    }
    public int GetNextTurn(int currentTurn)  // Getting the turn for next player
    {
        if (LudoManager.Instance.MaxPlayers == 4) // For 4 player game
        {
            currentTurn = currentTurn + 1;
            if (currentTurn > 4)
            {
                currentTurn = 1;
            }
            LudoManager.Instance.whichturn = currentTurn;
            return currentTurn;
        }
        else if (LudoManager.Instance.MaxPlayers == 2) // For 2 player game
        {
            if (currentTurn == 3)
            {
                currentTurn = 1;
            }
            else
            {
                currentTurn = 3;
            }
            LudoManager.Instance.whichturn = currentTurn;
            return currentTurn;
        }
        else
        {
            currentTurn = currentTurn + 1;
            if (currentTurn > 3)
            {
                currentTurn = 1;
            }
            LudoManager.Instance.whichturn = currentTurn;
            return currentTurn;
        }
    }
}
[System.Serializable]
public struct TurnPlay
{
    public int player_number;
    public string current_time;
    public string timeup_time;
    public string action_type;
}
[System.Serializable]
public struct DiceHit
{
    public int uid;
}
public struct PawnCut
{
    public int player_number;
    public int pownDie;
    public int goTo;
}

public class ExitGame
{
    public string lobby_id;
    public string uid;
}
public class MatchExit
{
    public bool isExit;
    public string message;
}
public class ExittedPlayerID
{
    public string player_id;
    public string rank;
}