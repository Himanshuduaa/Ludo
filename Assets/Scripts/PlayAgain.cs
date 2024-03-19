using Newtonsoft.Json;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayAgain : MonoBehaviour
{
    public Button myButton;
    public float delay;

    void Start()
    {
        
    }
    void OnEnable()
    {
        //// Ensure that the Button component is assigned
        //if (myButton == null)
        //{
        //    Debug.LogError("Button component not assigned!");
        //    return;
        //}
        //myButton.gameObject.SetActive(true);
        //// Start the coroutine to disable the button after 5 seconds
        //StartCoroutine(DisableButtonAfterDelayCoroutine(delay));

    }
    public void startPlayAgainTimer()
    {
        // Ensure that the Button component is assigned
        if (myButton == null)
        {
            Debug.LogError("Button component not assigned!");
            return;
        }
        myButton.gameObject.SetActive(true);
        // Start the coroutine to disable the button after 5 seconds
        StartCoroutine(DisableButtonAfterDelayCoroutine(delay));
    }
    public void rejoinLobby()
    {
        ResetAll.instance.resetAll();
        RejoinLobby rejoinLobby=new RejoinLobby();
        rejoinLobby.uid = LudoManager.Instance.gameInfo.playerID;
        rejoinLobby.lobby_id = LudoManager.Instance.matchDataManager.MatchData.lobby_id;
        rejoinLobby.avatar = LudoManager.Instance.gameInfo.avatar.ToString();
        Player player = LudoManager.Instance.matchDataManager.MatchData.players.Find(spot => spot.uid == LudoManager.Instance.gameInfo.playerID);
        string color = player.color;
        rejoinLobby.color = color;
        rejoinLobby.page = "xyz";
        rejoinLobby.userName = LudoManager.Instance.gameInfo.userName;
        string json=JsonConvert.SerializeObject(rejoinLobby);
        Debug.Log("Sending data to rejoin-lobby as "+json);
        GameSocketManager.instance.ludoSocket.Emit("rejoin-lobby",json);
        ResetPawns();
    }
    public void ResetPawns()
    {
        for(int i=0;i<LudoManager.Instance.players.Count;i++)
        {
            for(int j = 0; j < LudoManager.Instance.players[i].myPawns.Count;j++)
            {
                LudoManager.Instance.players[i].myPawns[j].transform.SetParent(LudoManager.Instance.players[i].transform, true);
            }
        }
    }
    private void OnDisable()
    {
        StopCoroutine(DisableButtonAfterDelayCoroutine(0));
        myButton.gameObject.SetActive(false);
    }
    IEnumerator DisableButtonAfterDelayCoroutine(float delaySeconds)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delaySeconds);

        // Disable the button after the delay
        //myButton.interactable = false;
        myButton.gameObject.SetActive(false);

        Debug.Log("Button disabled after " + delaySeconds + " seconds.");
    }
}
