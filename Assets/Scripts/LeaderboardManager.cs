using AYellowpaper.SerializedCollections;
using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    public Transform LeaderboardContentTransform;
    public GameObject LeaderboardPrefab;
    private string mainURL= "ludo/api/bets/leaderBoard";
    public LeaderBoardData leaderBoardDatas;
    public List<Leaderboard> leaderboards = new List<Leaderboard>();
    public string authToken = "UNITY";
    public TextMeshProUGUI FirstWinLose;
    public TextMeshProUGUI SecondWinLose;
    public TextMeshProUGUI ThirdWinLose;
    public Image AvatarFirst;
    public Image AvatarSecond;
    public Image AvatarThird;
    [SerializedDictionary]
    public SerializedDictionary<int, Top> TopThree = new SerializedDictionary<int, Top>();
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(PostRequest());
        mainURL = GameSocketManager.instance.address + mainURL;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void getLeaderboard()
    {
        //mainURL = GameSocketManager.instance.address + mainURL;
        StartCoroutine(PostRequest());
    }
    IEnumerator PostRequest()
    {
        // Create a class to represent the data you want to send
        RequestData requestData = new RequestData
        {
            account = LudoManager.Instance.gameInfo.account,
            gameCode = "ludo"
        };

        // Convert the data to a JSON string
        string postData = JsonConvert.SerializeObject(requestData);
        Debug.Log("Main url " + mainURL+ "Token is "+ LudoManager.Instance.operatorToken);
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
            Debug.Log("Response: " + request.result.ToString());
            Debug.Log("Response: " + request.downloadHandler.text);
            showLeaderboard(request.downloadHandler.text);
            // Assuming LeaderBoardData is a class that represents the response structure
            LeaderBoardData leaderBoardDatas = JsonConvert.DeserializeObject<LeaderBoardData>(request.downloadHandler.text);
        }
    }
    private void showLeaderboard(string json)
    {
        foreach (var item in leaderboards)
        {
            Destroy(item.gameObject);
        }
        leaderboards.Clear();
        ApiResponse response=JsonConvert.DeserializeObject<ApiResponse>(json);
        Debug.Log(response);
        for(int i=0;i<response.data.Count; i++)
        {
            if(i<=2)
            {
                TopThree[i + 1].Avatar.sprite = UIManager.Instance.avatars[response.data[i].user.avatar].sprite;
                TopThree[i + 1].WinLose.text = "Win "+response.data[i].win_count;
            }
            
            GameObject gb = Instantiate(LeaderboardPrefab, LeaderboardContentTransform);
            Leaderboard leaderboard = gb.GetComponent<Leaderboard>();
            leaderboard.rank.text = (i+1).ToString();
            leaderboard.win.text = response.data[i].win_count.ToString();
            leaderboard.name.text = response.data[i].user.name.ToString();
            //leaderboard.rank.text = (i+1).ToString();
            leaderboard.avatar.sprite = UIManager.Instance.avatars[response.data[i].user.avatar].sprite;
            leaderboards.Add(leaderboard);
        }
    }
}
[System.Serializable]
public struct Top
{
    public TextMeshProUGUI WinLose; 
    public Image Avatar;
}
public class LeaderBoardData
{

}
[System.Serializable]
public class RequestData
{
    public string account;
    public string gameCode;
}
[Serializable]
public class UserData
{
    public int match_count { get; set; }
    public int win_count { get; set; }
    public User user { get; set; }
}

[Serializable]
public class User
{
    public string name { get; set; }
    public int avatar { get; set; }
}

[Serializable]
public class ApiResponse
{
    public bool success { get; set; }
    public string message { get; set; }
    public List<UserData> data { get; set; }
}