using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class MatchDataManager : MonoBehaviour
{
    public MatchData MatchData;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void setCurrentUpdate() // Setting the current update of game from status, status-game and for watcher-join, if the game is going on
    {
        if (MatchData.current_update != null)
        {
            LudoManager.Instance.timerPlayer.isTimerRunning = true;
            LudoManager.Instance.timerPlayer.StopTimer();
            LudoManager.Instance.timerPlayer.resetValues();
            LudoManager.Instance.timerPlayer.currentTime = MatchData.current_update.current_time;
            LudoManager.Instance.timerPlayer.startup_time = MatchData.current_update.turn_start_time;
            LudoManager.Instance.timerPlayer.timeUpTime = MatchData.current_update.timeup_time;
            LudoManager.Instance.timerPlayer.actionType = MatchData.current_update.action_type;
            LudoManager.Instance.timerPlayer.isTimerRunning = true;
            LudoManager.Instance.timerPlayer.ShowTime();
            PlayerController PlayerController = LudoManager.Instance.players.Find(spot => spot.TurnNumber == MatchData.current_update.player_number);
            //showWhosTurn();
            whosTurn(PlayerController);
            if (PlayerController.PlayerID == LudoManager.Instance.gameInfo.playerID && MatchData.current_update.action_type == "dice_run")
            {
                Debug.Log("Last dice " + MatchData.current_update.dice_number);
                LudoManager.Instance.diceManager.diceState(true);
            }
            else
            {
                LudoManager.Instance.diceManager.diceState(false);
            }
            if (MatchData.current_update.action_type == "pawn_run")
            {
                LudoManager.Instance.diceManager.setDiceNumber(MatchData.current_update.dice_number);
                if (MatchData.current_update.pown_move!=null)
                {
                    showMovable(PlayerController, MatchData.current_update.pown_move);
                }
                LudoManager.Instance.diceManager.diceState(false);
            }
        }
    }
    public void whosTurn(PlayerController PlayerController) // Whos player turn blinking enabled
    {
        for (int i = 0; i < LudoManager.Instance.players.Count; i++)
        {
            LudoManager.Instance.players[i].normalizePawn();
        }
        for (int i = 0; i < LudoManager.Instance.players.Count; i++)
        {
            LudoManager.Instance.players[i].playerBG.GetComponent<Animator>().enabled = true;
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
    public void showMovable(PlayerController PlayerController, Dictionary<string, int> moving_pown) // highlight the movable pawn of the player
    {
        for (int i = 0; i < LudoManager.Instance.players.Count; i++)
        {
            LudoManager.Instance.players[i].normalizePawn();
        }
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
        for (int i = 0; i < PlayerController.myPawns.Count; i++)
        {
            PlayerController.myPawns[i].GetComponent<UnityEngine.UI.Image>().raycastTarget = true;
        }
        if (moving_pown.Count > 0)
        {
            for (int i = 0; i < moving_pown.Count; i++)
            {
                Pawnmanager pawn = PlayerController.myPawns.Find(spot => spot.GetComponent<Pawnmanager>().PawnNumber == int.Parse(moving_pown.ElementAt(i).Key));
                pawn.movable = true;
                setPawnParent(pawn);
                pawn.highlight();
                if (PlayerController.PlayerID == LudoManager.Instance.gameInfo.playerID)
                {
                    pawn.GetComponent<Button>().interactable = true;
                    pawn.isMine = true;
                }
            }
        }
        for (int i = 0; i < LudoManager.Instance.spotsMap.Count; i++)
        {
            LudoManager.Instance.spotsMap[i].setPlayerOnSpot();
        }
    }
    public void saveGameUpdate(GameUpdate result)
    {
        MatchData.result = result.result;
        MatchData.match_started = result.match_started;
        MatchData.match_ended = result.match_ended;
        MatchData.current_update = result.current_update;
        //MatchData.players=
        for (int i = 0; i < result.players.Count; i++)
        {
            for (int j = 0; j < MatchData.players.Count; j++)
            {
                if (MatchData.players[j].uid == result.players[i].uid)
                {
                    for (int k = 0; k < result.players[j].pown_position.Count; k++)
                    {
                        MatchData.players[j].pown_position[MatchData.players[j].pown_position.ElementAt(k).Key] = result.players[j].pown_position.ElementAt(k).Value;//, result.players[j].pown_position.ElementAt(k).Value)
                    }
                    MatchData.players[j].totalCoins = result.players[i].totalCoins.ToString();
                    MatchData.players[j].missed_turn = result.players[i].missed_turn;
                }
                if (MatchData.mode == "Modern")
                {
                    PlayerController player = LudoManager.Instance.players.Find(spot => spot.PlayerID == MatchData.players[i].uid);
                    player.userDetails.modernCoin.totalCoin = int.Parse(MatchData.players[i].totalCoins);
                    player.userDetails.modernCoin.setCoins();
                }
            }
        }
        updatePlayerMissedTurn(result);
        for (int i = 0; i < MatchData.result.Count; i++)
        {
            PlayerController player = LudoManager.Instance.players.Find(spot => spot.PlayerID == MatchData.result[i].uid);
            player.userDetails.setRank(MatchData.result[i].position);
            player.turnOffPawnVisibility();
        }
        for (int i = 0; i < LudoManager.Instance.players.Count; i++)
        {
            for (int j = 0; j < LudoManager.Instance.players[i].myPawns.Count; j++)
            {
                LudoManager.Instance.players[i].myPawns[j].GetComponent<Image>().color = Color.white;
            }
        }
        if (MatchData.mode == "Modern")
        {
            LudoManager.Instance.modernMode.setStartAndEnd(result.match_started, result.match_ended);
        }
    }
    public void updatePlayerMissedTurn(GameUpdate result)
    {
        if (result.players != null)
        {
            for (int i = 0; i < result.players.Count; i++)
            {
                for (int j = 0; j < MatchData.players.Count; j++)
                {
                    if (MatchData.players[j].uid == result.players[i].uid)
                    {
                        Player playerToUpdate = MatchData.players[j];  // Retrieve the player from the list
                        playerToUpdate.missed_turn = result.players[i].missed_turn;  // Modify the property
                        MatchData.players[j] = playerToUpdate;
                    }
                }
            }
        }
    }
    private void setPawnParent(Pawnmanager pawn)
    {
        if (pawn.transform.parent.gameObject.name != "Inside")
        {
            pawn.gameObject.transform.SetParent(UIManager.Instance.MovingPawnParent.transform);
            pawn.transform.localScale = new Vector3(2, 2, 2);
            pawn.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 73.33f);
        }
    }
}
