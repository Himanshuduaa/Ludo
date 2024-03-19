using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SaveUserData : MonoBehaviour
{
    private string mainURL;
    // Start is called before the first frame update
    void Start()
    {
        mainURL = GameSocketManager.instance.address + "/ludo/api/games/updateGameSettings";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void updateGameSettings()
    {
        Debug.Log("Updating Settings");
        StartCoroutine(PostChengeRequest("settings"));
    }
    public void updateAvatar()
    {
        Debug.Log("Updating Avatar");
        StartCoroutine(PostChengeRequest("avatar"));
    }
    IEnumerator PostChengeRequest(string changeType)
    {
        mainURL = GameSocketManager.instance.address + "ludo/api/games/updateGameSettings";
        string postData = "";
        if (changeType == "avatar")
        {
            ChangeAvatarRequest changeRequest = new ChangeAvatarRequest();
            changeRequest.uid = LudoManager.Instance.gameInfo.playerID;
            changeRequest.changeType = changeType;
            changeRequest.account = LudoManager.Instance.gameInfo.account;
            ChangeAvatar changeData =new ChangeAvatar();
            changeData.avatar = LudoManager.Instance.gameInfo.avatar;
            changeRequest.changeData = changeData;
            postData = JsonConvert.SerializeObject(changeRequest);
        }
        else
        {
            ChangeNotificationRequest changeRequest = new ChangeNotificationRequest();
            changeRequest.uid = LudoManager.Instance.gameInfo.playerID;
            changeRequest.changeType = changeType;
            changeRequest.account = LudoManager.Instance.gameInfo.account;
            ChangeSettings changeData=new ChangeSettings();
            GameSettings gameSettings = new GameSettings();
            gameSettings.notifications = UIManager.Instance.notificationValue();
            gameSettings.sound = UIManager.Instance.soundValue() ;
            gameSettings.music = UIManager.Instance.musicValue() ;
            changeData.game_settings = gameSettings;
            changeRequest.changeData = changeData;
            postData = JsonConvert.SerializeObject(changeRequest);

        }
        // Convert the data to a JSON string

        Debug.Log(postData);
        UnityWebRequest request = new UnityWebRequest(mainURL, "POST");
        request.SetRequestHeader("auth", LudoManager.Instance.operatorToken);

        request.SetRequestHeader("Content-Type", "application/json");

        // Set the request body
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(postData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Success Response: " + request.result.ToString());
        }
    }
}
public class ChangeSettings
{
    public GameSettings game_settings;
}
public class ChangeAvatar
{
    public int avatar;
}

public class ChangeAvatarRequest
{
    public string uid;
    public ChangeAvatar changeData;
    public string changeType;
    public string account;
}
public class ChangeNotificationRequest
{
    public string uid;
    public ChangeSettings changeData;
    public string account;
    public string changeType;
}
