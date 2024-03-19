using AYellowpaper.SerializedCollections;
using DG.Tweening;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Pawnmanager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool InHouse;
    public bool AtStar;
    public int currentIndex;
    //public MyColour colour;
    public int PawnNumber;
    public PlayerController ActualParent;
    public float rotationDuration = 2f;
    public Vector3 targetRotation = new Vector3(0, 0, 360);
    public bool movable;
    private Tweener rotationTween;
    public bool wasOnStar;
    private Vector3 originalScale;
    private float scaleFactor = 1.2f;
    public bool isMine;
    //public int pawnPoints;


    // Start is called before the first frame updat
    void Start()
    {
        InHouse = true;
        AtStar = false;
        this.gameObject.GetComponent<Button>().onClick.AddListener(MoveMe);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Enlarge the button when pointer enters
        if(movable && isMine && !LudoManager.Instance.homeIndexes.Contains(currentIndex))
        {
            originalScale = transform.localScale;
            transform.DOScale(originalScale * scaleFactor, 0.1f);
            Debug.LogWarning(this.gameObject.transform.parent.name);
        }
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // Return the button to its original size when pointer exits
        if (movable && isMine && !LudoManager.Instance.homeIndexes.Contains(currentIndex))
        {
            transform.DOScale(originalScale, 0.1f);
            Debug.LogWarning(this.gameObject.transform.parent.name);
        }
        //transform.localScale = originalScale;
    }
    public void setMyScale(Vector3 vect)
    {
        GetComponent<RectTransform>().localScale = vect;
    }
    public void MoveMe()// sending data to server to move the pawn
    {
        if(GameSocketManager.instance.connected)
        {
            //if (movable)
            //{
            //    transform.DOScale(originalScale, 0.1f);
            //}
            this.gameObject.GetComponent<Button>().interactable = false;
            for (int i = 0; i < LudoManager.Instance.players.Count; i++)
            {
                for(int j=0;j< LudoManager.Instance.players[i].myPawns.Count; j++)
                {
                    LudoManager.Instance.players[i].myPawns[j].GetComponent<Button>().interactable = false;
                    isMine = false;
                }
            }
            MovingPawnDetails movingpawn;
            movingpawn.pawn_number = PawnNumber;
            movingpawn.lobby_id = LudoManager.Instance.matchDataManager.MatchData.lobby_id;
            string json = JsonConvert.SerializeObject(movingpawn);
            Debug.Log("Pawn Data Sending to server " + json);
            LudoManager.Instance.diceManager.diceState(false);
            GameSocketManager.instance.movePawn(json);
        }
    }
    public void highlight() // Highlight aur not, according to the movable state
    {
        this.gameObject.GetComponent<Animator>().enabled = true;
        if(LudoManager.Instance.homeIndexes.Contains(currentIndex))
        {
            if(!movable)
            {
                this.gameObject.GetComponent<RectTransform>().DOScale(new Vector3(1,1,1), 0.5f);
                this.gameObject.GetComponent<Animator>().SetBool("Movable", false);
                this.gameObject.GetComponent<Image>().color = Color.white;
            }
            else
            {
                this.gameObject.GetComponent<RectTransform>().DOScale(new Vector3(1,1,1), 0.5f);
                this.gameObject.GetComponent<Animator>().SetBool("Movable", true);
            }
        }
        else
        {
            if (movable==false)
            {
                this.gameObject.GetComponent<RectTransform>().DOScale(new Vector3(1,1,1), 0.5f);
                this.gameObject.GetComponent<Animator>().SetBool("Movable", false);
                this.gameObject.GetComponent<Image>().color = Color.white;
            }
            else
            {
                this.gameObject.GetComponent<RectTransform>().DOScale(new Vector3(1,1,1), 0.5f);
                this.gameObject.GetComponent<Animator>().SetBool("Movable", true);
            }

        }
    }
    private void setPawnParent()
    {
        if (transform.parent.gameObject.name != "Inside")
        {
            gameObject.transform.SetParent(UIManager.Instance.MovingPawnParent.transform);
            transform.localScale = new Vector3(2, 2, 2);
            //transform.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 73.33f);
        }
    }
}

public struct MovingPawnDetails
{
    public int pawn_number;
    public string lobby_id;
}

public struct PawnMovementData
{
    public int pawn_number { get; set; }
    public List<int> movements { get; set; }
    public int player_number { get; set; }
    public PawnPosition pawnPosition { get; set; }
}
public class PawnPosition
{
    public List<Player> players { get; set; }
}

