using UnityEngine;
using System;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "NewPlayerInfo", menuName = "ScriptableObjects/PlayerInfo", order = 1)]
public class GameInfo : ScriptableObject
{
    [SerializeField]
    public GameSettings gameSettings;
    public string playerID;
    public string uid;
    public int avatar;
    public string referralLink;
    public string referralCode;
    public DateTime updatedAt;
    public string userToken;
    public string userName;
    public string balance;
    public string account;
    public string operatorToken;
    public string address;
    public SerializedDictionary<int, int> Amount = new SerializedDictionary<int, int>();
    public bool unAuthorized;
    public string encodedAuthToken;

    public void setPlayerInfo(USERDetails details)
    {
        unAuthorized = false;
        if (details != null /*&& LudoManager.Instance.playerInfo.uid == ""*/)
        {
            Debug.Log("Setting player details everywhere");   
            GameSocketManager.instance.chatManager.turnOnChat();
            UIManager.Instance.gameConnected();
            gameSettings=details.game_settings;
            playerID = details.uid;
            uid = details.uid;
            avatar = details.avatar;
            referralCode = details.referral_code;
            referralLink = details.referral_link;
            updatedAt = details.updatedAt;
            userToken = details.userToken;
            userName = details.userName;
            balance = details.balance;
            account=details.account;
            operatorToken=details.operatorToken;
            LudoManager.Instance.gameInfo.playerID = uid;
            UIManager.Instance.setGameSettingsUI(gameSettings);
            LudoManager.Instance.getBet();
        }
        getTheData();
        LudoManager.Instance.matchDataManager.MatchData.ResetValues();
        GameSocketManager.instance.playerStatus();
    }
    public void getTheData()
    {
        if(uid!="")
        {
            Debug.Log("Setting user profile");
            LudoManager.Instance.gameInfo.playerID = uid;
            LudoManager.Instance.operatorToken = operatorToken;
            PlayerAccountManager.Instance.UpdateUserName(userName);
            PlayerAccountManager.Instance.UpdateAccountBalance(balance);
            PlayerAccountManager.Instance.UpdateAvatar();
            if(gameSettings!=null)
            UIManager.Instance.setGameSettingsUI(gameSettings);
            UIManager.Instance.referenceCode.text = referralCode.ToUpper();
            UIManager.Instance.referenceLink.text = referralLink;
            UIManager.Instance.HomePage.SetActive(true);
            UIManager.Instance.bottomBar.SetActive(true);
        }
    }
    [ContextMenu("DeleteData")]
    public void DeleteData()
    {
        GameSettings gameSettingss=new GameSettings();
        gameSettingss.notifications = true;
        gameSettingss.sound = true;
        gameSettingss.music=true;
        gameSettings = gameSettingss;
        playerID = "";
        uid = "";
        avatar = 0;
        referralCode = "";
        referralLink = "";
        updatedAt = new DateTime() ;
        userToken = "";
        userName = "";
        balance = "";
        account = "";
        operatorToken = "";
        unAuthorized = false;
        Amount.Clear();
        encodedAuthToken = "";
    }
}
