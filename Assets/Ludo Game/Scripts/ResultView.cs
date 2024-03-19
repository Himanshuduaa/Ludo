using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ResultView : MonoBehaviour
{
    public static ResultView Instance;

    [SerializeField]
    private Transform itemPreafb;

    [SerializeField]
    private Transform container;
    private List<Transform> resultData = new List<Transform>();
    public bool resultDisplayed {get;set;}
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        resultDisplayed = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void open(List<Result> resultDatas)
    {
        if (UIManager.Instance.Panels["GameWindow"].activeInHierarchy)
            UIManager.Instance.whatToShow("Result", false);
        if(resultDatas.Count>0)
        {
            updateResult(resultDatas);
        }
    }
    public void updateResult(List<Result> resultDatas)
    {
        foreach (var item in resultData)
        {
            Destroy(item.gameObject);
        }
        resultData.Clear();
        if (resultDisplayed==false)
        {
            resultDisplayed = true;
            for(int i=0;i<resultDatas.Count;i++)
            {
                Result result = resultDatas.Find(spot => spot.position == (i + 1).ToString());
                Transform itemTransform = Instantiate(itemPreafb, container);
                itemTransform.gameObject.SetActive(true);
                GameObject playerName = GameUtil.findGameObject(itemTransform, "Profile");
                TextMeshProUGUI rankText = GameUtil.findGameObject(itemTransform, "Rank").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI amountText = GameUtil.findGameObject(itemTransform, "Winnings").GetComponent<TextMeshProUGUI>();
                Image bg = GameUtil.findGameObject(itemTransform, "Profile").transform.Find("BG").GetComponent<Image>();//.findGameObject(itemTransform, "BG").GetComponent<Image>();
                PlayerController player = LudoManager.Instance.players.Find(spot => spot.PlayerID == result.uid);
                bg.gameObject.GetComponent<Image>().sprite = UIManager.Instance.PlayerBackgrounds[player.myColour];
                playerName.gameObject.transform.Find("DP").GetComponent<Image>().sprite = UIManager.Instance.getProfileSprite(result.avatar);
                playerName.GetComponentInChildren<TextMeshProUGUI>().text = result.name;
                Debug.Log("rankText=" + rankText);
                rankText.text = result.position;
                amountText.text = "₹" + result.winning_amount;
                resultData.Add(itemTransform);
            }
        }
        
    }
    public void setBoardRanking()
    {
        if (LudoManager.Instance.matchDataManager.MatchData.result.Count != 0)
        {
            for (int i = 0; i < LudoManager.Instance.matchDataManager.MatchData.result.Count; i++)
            {
                PlayerController player = LudoManager.Instance.players.Find(spot => spot.PlayerID == LudoManager.Instance.matchDataManager.MatchData.result[i].uid);
                player.userDetails.Rank.text = LudoManager.Instance.matchDataManager.MatchData.result[i].position;
                for (int j = 0; j < player.myPawns.Count; j++)
                {
                    player.myPawns[j].gameObject.SetActive(false);
                }
                player.pawnState(false);
            }
        }
    }
}
