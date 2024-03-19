using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserDetails : MonoBehaviour
{
    public int Turn;
    public TextMeshProUGUI userName;
    public Turn turn;
    public Image PlayerAvatar;
    public Image RankPicture;
    public Transform MissingTurn;
    public int MissingTurns;
    public TextMeshProUGUI Rank;
    public ModernCoin modernCoin;
    public Animator award;
    public Image awardBG;
    public Image awardImg;
    //public int coins;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setRank(string rank)
    {
        if (rank != "")
        {
            int rankVal = int.Parse(rank);
            awardBG.sprite = UIManager.Instance.AwardsBG[rankVal - 1];
            awardImg.sprite = UIManager.Instance.Awards[rankVal - 1];
            award.gameObject.SetActive(true);
        }
        else
        {
            RankPicture.color = Color.clear;// = null;
            Rank.text = rank;
            award.gameObject.SetActive(false);
        }
    }
}
