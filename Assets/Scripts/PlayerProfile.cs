using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfile : MonoBehaviour
{
    public Image profilePicture;
    public GameObject playerSearch;
    public Button SearchButton;
    public TextMeshProUGUI nameOfPlayer;
    public bool searching;
    Color transparentColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    //Color opaque = new Color(255f, 255f, 255f, 255f);
    // Start is called before the first frame update
    void Start()
    {
        startRolling();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void startRolling()
    {   
        if(this.gameObject.activeInHierarchy)
        StartCoroutine(startSearching());
        //if(!LudoManager.Instance.lobbymanager.inviteFriends)
        //{
        //    SearchButton.gameObject.SetActive(false);
        //}
        //else
        //{
        //    SearchButton.gameObject.SetActive(true);
        //}
    }
    IEnumerator startSearching()
    {
        if (searching == true)
        {
            yield return new WaitForSeconds(0.05f);
            playerSearch.gameObject.SetActive(true);
            int a = Random.Range(0, UIManager.Instance.BlurredPlayers.Count);
            playerSearch.gameObject.GetComponent<Image>().sprite = UIManager.Instance.BlurredPlayers[a];
            profilePicture.color = Color.clear;
            StartCoroutine(startSearching());
        }
        else
        {
            profilePicture.color = Color.white;
            playerSearch.gameObject.SetActive(false);
        }
    }
    public void resetAll()
    {
        searching = true;
        playerSearch.gameObject.SetActive(true);
        nameOfPlayer.text = "FINDING...";
        int a = Random.Range(0, UIManager.Instance.BlurredPlayers.Count);
        playerSearch.gameObject.GetComponent<Image>().sprite = UIManager.Instance.BlurredPlayers[a];
        profilePicture.color = Color.white;
    }
}
