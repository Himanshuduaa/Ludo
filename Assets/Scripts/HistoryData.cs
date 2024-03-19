using BestHTTP.JSON;
using BestHTTP.SocketIO;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HistDataa : MonoBehaviour
{
    private string baseurl = "http://192.168.0.173:5000/ludo/api/history/";
    public string mainURL="/ludo/api/history/";
    public History history = new History();
    public GameObject historyPrefab;
    public Transform HistoryContainer;
    public List<GameObject> historyList = new List<GameObject>();

    void Start()
    {
        mainURL = GameSocketManager.instance.address + mainURL;
    }

    private string authToken = "UNITY";

    IEnumerator PostRequest()
    {
        UnityWebRequest request = new UnityWebRequest(mainURL, "POST");
        request.SetRequestHeader("Content-Type", "application/json");
        //request.SetRequestHeader("auth", GameSocketManager.instance.authToken);
        request.SetRequestHeader("auth", LudoManager.Instance.gameInfo.operatorToken);
        DownloadHandlerBuffer dH = new DownloadHandlerBuffer();
        request.downloadHandler = dH;

        request.method = "POST";

        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Response: " + request.result.ToString());
            Debug.Log("Response: " + request.downloadHandler.text);
            history = JsonConvert.DeserializeObject<History>(request.downloadHandler.text);
            showHistory();
        }
    }
    private void showHistory()
    {
        foreach (var item in historyList)
        {
            Destroy(item);
        }
        historyList.Clear();
        for (int i=0;i<history.data.Count;i++)
        {
            GameObject data = GameObject.Instantiate(historyPrefab, HistoryContainer);
            data.GetComponent<MyHistory>().entry.text = history.data[i].result.bet_amount;
            if(history.data[i].result.position=="1")
            {
                data.GetComponent<MyHistory>().status.text ="STATUS : WIN";
            }
            else
            {
                data.GetComponent<MyHistory>().status.text = "STATUS : LOSE";
            }
            data.GetComponent<MyHistory>().prizepool.text = history.data[i].result.winning_amount;
            data.GetComponent<MyHistory>().mode.text = history.data[i].mode+" Mode";
            data.GetComponent<MyHistory>().noOfPlayers.text = history.data[i].player_count +" Players Mode";
            historyList.Add(data);
        }
    }

    public void createURL(/*string id*/)
    {
        mainURL = "";
        mainURL = GameSocketManager.instance.address+"ludo/api/history/" + LudoManager.Instance.gameInfo.playerID + "/results";
        Debug.Log(mainURL);
        StartCoroutine(PostRequest());

        //StartCoroutine(GetJsonFromUrl());
    }
    IEnumerator GetJsonFromUrl()
    {
        Debug.Log("Getting history");
        UnityWebRequest www = UnityWebRequest.PostWwwForm(mainURL,"");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error+ www.downloadHandler.text);
        }
        else
        {
            // Show results as text
            Debug.Log("Showing history");

            Debug.Log(www.downloadHandler.text);
            history=JsonConvert.DeserializeObject<History>(www.downloadHandler.text);
        }
    }
}

public class PlayerHistory
{
    public string uid { get; set; }
    public string name { get; set; }
    public int avatar { get; set; }
    public string position { get; set; }
    public string winning_amount { get; set; }
    public string bet_amount { get; set; }
    public string _id { get; set; }
}

public class historydata
{
    public int player_count { get; set; }
    public string mode { get; set; }
    public int win_with_token { get; set; }
    public PlayerHistory result { get; set; }
}

public class History
{
    public List<historydata> data { get; set; }
    public string msg { get; set; }
}
