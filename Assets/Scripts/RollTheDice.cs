using DG.Tweening;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RollTheDice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    private float scaleFactor = 1.2f;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(rollTheDice);
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Enlarge the button when pointer enters
        if (this.gameObject.GetComponent<Button>().interactable==true)
        {
            transform.DOScale(originalScale * scaleFactor, 0.1f);
        }

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // Return the button to its original size when pointer exits
        if (this.gameObject.GetComponent<Button>().interactable == true)
        {
            transform.DOScale(originalScale, 0.1f);
        }
    }
    public void makeDefaultDiceSize()
    {
        transform.DOScale(originalScale, 0.1f);
    }
    public void rollTheDice()
    {
        if(GameSocketManager.instance.connected==true)
        {
            if (this.gameObject.GetComponent<Button>().interactable == false)
            {
                transform.DOScale(originalScale, 0.1f);
            }
            DiceData diceData = new DiceData();
            diceData.match_id = LudoManager.Instance.matchDataManager.MatchData._id;
            diceData.uid = LudoManager.Instance.gameInfo.playerID;
            Player Player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.uid == LudoManager.Instance.gameInfo.playerID);
            diceData.player_number = Player.player_number;
            diceData.mode = LudoManager.Instance.matchDataManager.MatchData.mode;
            string json = JsonConvert.SerializeObject(diceData);
            this.gameObject.GetComponent<Button>().interactable = false;
            GameSocketManager.instance.DiceHit(json);
        }
    }
}
public struct DiceData
{
    public string match_id;
    public string uid;
    public string mode;
    public int player_number;
}
public class DicePlayed
{
    public int player_number;
    public Dictionary<string, int> moving_pown;
    public int dice_value;
    public string action_type;
}
