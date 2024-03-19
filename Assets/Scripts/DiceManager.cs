using DG.Tweening;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    public DicePlayed DicePlayed = new DicePlayed();
    public List<Sprite> DiceNumbers = new List<Sprite>();
    public Button Dice;
    public int state;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void diceState(bool state)
    {
        Dice.interactable = state;
    }
    IEnumerator RollTheDice() // Animation to roll the dice and showing the result
    {
        Dice.GetComponent<Animator>().enabled = true;
        Dice.animator.SetBool("Roll", true);
        AudioManager.instance.PlayAudio("dice-rolling-on-table");
        yield return new WaitForSeconds(1f);
        Dice.animator.SetBool("Roll", false);
        Dice.GetComponent<Animator>().enabled = false;
        if (DicePlayed.dice_value != 0)
            setDiceNumber(DicePlayed.dice_value);
        LudoManager.Instance.ShowMovablePawn(); // Show the pawns which can be played after running the dice
    }
    public void setDiceNumber(int stateNumber) // Set sprite of the number on dice
    {
        if(stateNumber==0) // checkingwrong response from server
        {
            Debug.LogError("Getting State of dice as " + stateNumber);
            stateNumber = 1;
        }
        else
        {
            Debug.Log("Setting dice number as "+ stateNumber);
        }
        Dice.GetComponent<Animator>().enabled = false;
        Dice.gameObject.GetComponent<Image>().sprite = DiceNumbers[stateNumber - 1];
        state = stateNumber - 1;
    }
    public void playTheDice(string json) //Run the dice after getting "after-dice-run" response
    {
        Dice.GetComponent<RollTheDice>().makeDefaultDiceSize();
        DicePlayed DicePlayed = JsonConvert.DeserializeObject<DicePlayed>(json);
        LudoManager.Instance.timerPlayer.StopTimer(); // Stop the ongoing timer
        LudoManager.Instance.diceManager.DicePlayed.dice_value = DicePlayed.dice_value; // outcome on dice
        LudoManager.Instance.diceManager.DicePlayed.player_number = DicePlayed.player_number; // Who's turn is this
        LudoManager.Instance.diceManager.DicePlayed.moving_pown = DicePlayed.moving_pown; // movable pawns data
        Player pl = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.player_number == DicePlayed.player_number);
        Player playerToUpdate = pl;  // Retrieve the player from the list
        for (int i=0;i<LudoManager.Instance.matchDataManager.MatchData.players.Count;i++)
        {
            if (LudoManager.Instance.matchDataManager.MatchData.players[i].uid==pl.uid)
            {
                LudoManager.Instance.matchDataManager.MatchData.players[i] = playerToUpdate;
            }
        }
        Player foundSpot = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.player_number == DicePlayed.player_number);
        if (foundSpot.pown_position == null)
        {
            UnityEngine.Debug.LogError("Foundspot error");
        }
        if (DicePlayed.moving_pown != null)
        {
            for (int i = 0; i < DicePlayed.moving_pown.Count; i++)
            {
                foundSpot.pown_position[DicePlayed.moving_pown.ElementAt(i).Key] = DicePlayed.moving_pown[(DicePlayed.moving_pown.ElementAt(i).Key)]; ///MOVING PAWN TO POS 9thoctober
            }
            if (DicePlayed.dice_value != 0) // Checking if server is not sending the wrong data
            {
                diceState(false); // Making the dice Non Interactable
                StartCoroutine(RollTheDice()); // Animation to roll the dice and showing the result
            }
        }
    }
}
