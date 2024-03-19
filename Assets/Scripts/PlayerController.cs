using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Experimental.GraphView.GraphView;

public enum Colour
{
    Red,Blue,Green,Yellow
}
public enum Number
{
    One,Two,Three,Four
}
public class PlayerController : MonoBehaviour
{
    public Turn myColour;
    public PlayerPosition position;
    public Number number;
    public Image playerBG;
    public string PlayerID;
    public bool MyTurn;
    public List<Pawnmanager> myPawns;
    public List<SpotManager> Path = new List<SpotManager>();
    public int TurnNumber;
    public TextMeshProUGUI PlayernameTMP;
    public TextMeshProUGUI PoistionNumber;
    public TextMeshProUGUI WinningNumber;
    //public string Playername;
    public DateTime ServerTime;
    public UserDetails userDetails;
    public List<MissTurn> MissTurn=new List<MissTurn>();
    public Sprite Ball;
    public Sprite Pawn;
    public Action SetSprite;

    //internal void EndTurn()
    //{
    //    //throw new NotImplementedException();
    //    MyTurn = false;
    //    for(int i=0;i<myPawns.Count; i++)
    //    {
    //        myPawns[i].GetComponent<Button>().interactable = false;
    //    }
    //}
   
    private void setSprite()
    {
        for(int i=0;i<myPawns.Count;i++)
        {
            if (LudoManager.Instance.homeIndexes.Contains(myPawns[i].currentIndex))
            {
                myPawns[i].GetComponent<Image>().sprite = Ball;
            }
            else
            {
                myPawns[i].GetComponent<Image>().sprite = Pawn;
            }
        }
    }
    public void cutThePawn(PawnCut pawnCut)
    {
        StartCoroutine(cutThePawnEnum(pawnCut));
    }
    IEnumerator cutThePawnEnum(PawnCut pawnCut)
    {
        for (int i = 0; i < LudoManager.Instance.matchDataManager.MatchData.players.Count; i++)
        {
            if (LudoManager.Instance.matchDataManager.MatchData.players[i].uid == PlayerID)
            {
                if (LudoManager.Instance.matchDataManager.MatchData.mode != "Modern")
                    LudoManager.Instance.matchDataManager.MatchData.players[i].pown_position[pawnCut.player_number.ToString()] = 0;
                else
                    LudoManager.Instance.matchDataManager.MatchData.players[i].pown_position[pawnCut.player_number.ToString()] = pawnCut.goTo;

            }
        }
        //MOVING THE PAWN TO HOME INDEX
        Pawnmanager pawn = myPawns.Find(spot => spot.PawnNumber == pawnCut.pownDie);
        string pawnPlace = "";
        if (LudoManager.Instance.matchDataManager.MatchData.mode != "Modern")
            pawnPlace = pawnCut.player_number.ToString() + "00" + pawnCut.pownDie.ToString();
        else
            pawnPlace = pawnCut.goTo.ToString();
        SpotsMap spots = LudoManager.Instance.spotsMap.Find(spot => spot.index == int.Parse(pawnPlace));
        pawn.transform.DOMove(spots.transform.position, 0.5f);

        Player playerdata = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.uid == PlayerID);
        if (LudoManager.Instance.matchDataManager.MatchData.mode != "Modern")
            playerdata.pown_position[pawnCut.player_number.ToString()] = int.Parse(pawnCut.player_number.ToString() + "00" + pawnCut.pownDie.ToString());
        else
        {
            playerdata.pown_position[pawnCut.player_number.ToString()] = pawnCut.goTo;
        }
        AudioManager.instance.PlayAudio("PawnCut");
        yield return new WaitForSeconds(0.5f);
        pawn.transform.SetParent(spots.transform, true);
        pawn.transform.localScale = new Vector3(1, 1, 1); ;
        pawnCut.pownDie = 0;

        for (int i = 0; i < LudoManager.Instance.spotsMap.Count; i++)
        {
            LudoManager.Instance.spotsMap[i].setPlayerOnSpot();
        }
        pawn.currentIndex = int.Parse(pawnPlace);
        SetSprite.Invoke();
        pawn.highlight();
    }
    public void setPlayerID(string id)
    {
        if(id!=null)
        PlayerID = id;
    }
    public void setSpriteAction()
    {
        if (SetSprite == null)
            SetSprite += setSprite;
    }
    private void Awake()
    {
        if(SetSprite == null)
            SetSprite += setSprite;
    }
    // Start is called before the first frame update
    void Start()
    {
        if(SetSprite == null)
            SetSprite += setSprite;
        MyTurn = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void turnOffPawnVisibility()
    {
        for(int i=0;i<myPawns.Count; i++)
        {
            myPawns[i].gameObject.SetActive(false);
        }
    }
    public void normalizePawn() // Stop the blinking animation, make the pawns non interactable and stop highlighting
    {
        for (int j = 0; j < myPawns.Count; j++)
        {
            myPawns[j].GetComponent<Button>().interactable = false;
            myPawns[j].movable = false;
            myPawns[j].highlight();
            myPawns[j].GetComponent<UnityEngine.UI.Image>().raycastTarget = false;
        }
        playerBG.GetComponent<Animator>().SetBool("Blink", false);
    }
    public void movePawn(PawnMovementData pawnMovement) // Move the pawn animation
    {
        if(this.gameObject!=null)
            StartCoroutine(moveThePawnEnum(pawnMovement));
    }
    
    IEnumerator moveThePawnEnum(PawnMovementData pawnMovement) // Move the pawn animation
    {
        Pawnmanager pawnmanager = myPawns.Find(spot => spot.PawnNumber == pawnMovement.pawn_number);
        if(pawnmanager==null)
        {
            Debug.LogError("Error");
        }
        LudoManager.Instance.movingPawn = true;
        
        for (int i = 0; i < pawnMovement.movements.Count; i++)
        {
            SpotsMap spot = LudoManager.Instance.spotsMap.Find(spot => spot.index == pawnMovement.movements[i]);
            setPawnParent(pawnmanager);
            pawnmanager.transform.DOMove(spot.transform.position, 0.3f);
            pawnmanager.transform.localScale = new Vector3(1, 1, 1);
            
            AudioManager.instance.PlayAudio("Pawn move");
            yield return new WaitForSeconds(0.15f);
            if (i == pawnMovement.movements.Count - 1 && spot.star == true)
            {
                pawnmanager.transform.SetParent(spot.transform, true);
                pawnmanager.wasOnStar = true;
            }
            else if (i == pawnMovement.movements.Count - 1)
            {
                pawnmanager.transform.SetParent(spot.transform, true);
                pawnmanager.wasOnStar = false;
            }
            pawnmanager.currentIndex = pawnMovement.movements[i];
            pawnmanager.highlight();
            spot.setPlayerOnSpot();
            //LudoManager.Instance.setPawnsScale();
            if(i==pawnMovement.movements.Count -1)
            {
                if(LudoManager.Instance.SafeZoneIndex.Contains(pawnmanager.currentIndex))
                {
                    AudioManager.instance.PlayAudio("safe token");
                }
                if (LudoManager.Instance.homeIndexes.Contains(pawnmanager.currentIndex))
                {
                    AudioManager.instance.PlayAudio("goal");
                }
            }

            //if(PlayerID==LudoManager.Instance.playerID)
            spot.CheckWin();
        }
        if(LudoManager.Instance.matchDataManager.MatchData.mode=="Modern") // Set Coins in Modern Mode before Moving
            LudoManager.Instance.setCoinsBeforeMoving(pawnMovement);

        for (int i = 0; i < LudoManager.Instance.players.Count; i++)
        {
            for(int k=0;k< LudoManager.Instance.players[i].myPawns.Count;k++)
            {
                LudoManager.Instance.players[i].myPawns[k].movable = false;
                LudoManager.Instance.players[i].myPawns[k].highlight();
            }
            //LudoManager.Instance.players[i].
            for (int j = 0; j < LudoManager.Instance.players[i].myPawns.Count; j++)
            {
                //UnityEngine.Debug.Log("players[i].myPlayers[j].currentIndex=" + LudoManager.Instance.players[i].myPawns[j].currentIndex);
                SpotsMap spot = LudoManager.Instance.spotsMap.Find(spot => spot.index == LudoManager.Instance.players[i].myPawns[j].currentIndex);

                if (spot == null)
                { continue; }

                LudoManager.Instance.players[i].myPawns[j].transform.SetParent(spot.transform, true);
                spot.setPlayerOnSpot();
            }
        }
        for (int i = 0; i < LudoManager.Instance.spotsMap.Count; i++)
        {
            LudoManager.Instance.spotsMap[i].setPlayerOnSpot();
        }
        //LudoManager.Instance.setPawnsScale();

        if (LudoManager.Instance.pawnCut.pownDie != 0)
        {
            LudoManager.Instance.CutThePawn();
        }
        LudoManager.Instance.pawnCut.pownDie = 0;
        LudoManager.Instance.movingPawn = false;
        if (AnimationManager.instance.ChatClicked)
        {
            Debug.LogError("Invoking get all");
            AnimationManager.ActionChat.Invoke();
        }
        SetSprite.Invoke();
    }
    private void setTotalPoints()
    {

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
    public void pawnState(bool state)
    {
        for (int j = 0; j < myPawns.Count; j++)
        {
            myPawns[j].gameObject.SetActive(state);
        }
    }
    public void playerExitted() // Other player exits from the board
    {
        Debug.Log("Removing the player in process...............");
        for(int i=0;i<myPawns.Count;i++)
        {
            //Destroy(myPawns[i]);
            myPawns[i].transform.SetParent(transform.parent);
            myPawns[i].gameObject.SetActive(false);
        }
        //userDetails.PlayerAvatar.sprite= null;
        userDetails.PlayerAvatar.color = Color.clear;// = null;
        userDetails.userName.text = "";
        userDetails.modernCoin.gameObject.SetActive(false);
        foreach (Transform child in userDetails.MissingTurn)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("Done Removing");
    }
}
