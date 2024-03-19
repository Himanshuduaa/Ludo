using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAccountManager : MonoBehaviour
{
    public static PlayerAccountManager Instance { get; private set; }
    public Sprite Avatar;
    public List<GameObject> PlayerPicture;
    public List<TextMeshProUGUI> WalletBalance=new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> userName=new List<TextMeshProUGUI>();
    public int avatarIndex;
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
        //UpdateAccountBalance();
        UpdateAvatar();
    }
    public void UpdateAvatar()
    {
        //LudoManager.Instance.MyAvatarIndex = 0;
        for (int i = 0; i < PlayerPicture.Count; i++)
        {
            PlayerPicture[i].GetComponent<Image>().sprite = UIManager.Instance.avatars[LudoManager.Instance.gameInfo.avatar].sprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void updatedDetails()
    {
        for (int i = 0; i < PlayerPicture.Count; i++)
        {
            PlayerPicture[i].GetComponent<Image>().sprite = Avatar;
        }
    }
    public void UpdateAccountBalance(string balance)
    {
        for(int i = 0;i < WalletBalance.Count;i++)
        {
            WalletBalance[i].text = ((int)Math.Round(decimal.Parse(balance))).ToString();
        }
    }
    public void UpdateUserName(string username)
    {
        for (int i = 0; i < userName.Count; i++)
        {
            userName[i].text = username;
        }
    }
}
