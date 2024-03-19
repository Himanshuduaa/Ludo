using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance;
    public GameObject chatPanel;
    public GameObject gameBoard;
    public Transform LudoGameCenter;
    public Transform LudoGameUpper;
    public bool chat;
    public Button Chatbutton;
    public static Action ActionChat;
    public bool ChatClicked;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //chatPanel.gameObject.SetActive(false);
        chat = false;
        ActionChat += openChat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void openChat()
    {
        ChatClicked = true;
        Chatbutton.interactable = false;
        if (!LudoManager.Instance.movingPawn)
        {
            StartCoroutine(openChatBox(0.1f));
        }
    }
    IEnumerator openChatBox(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if(ChatClicked == true)
        {
            chatPanel.gameObject.SetActive(true);
            if (chat == false)
            {
                Debug.Log("If");
                GameSocketManager.instance.chatManager.getAllMessages();
                chatPanel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.4f);
                yield return new WaitForSeconds(0.4f);
                gameBoard.transform.DOMove(LudoGameUpper.position, 0.3f);
                chat = true;
            }
            else
            {
                Debug.Log("Else");
                chat = false;
                chatPanel.transform.DOScale(new Vector3(0f, 0f, 0f), 0.2f);
                yield return new WaitForSeconds(0.3f);
                gameBoard.transform.DOMove(LudoGameCenter.position, 0.3f);
            }
            ChatClicked = false;
            Chatbutton.interactable = true;
        }
    }
    public void closeChatBox()
    {
        Chatbutton.interactable = true;
        ChatClicked = false;
        chat = false;
        chatPanel.transform.localScale=new Vector3(0f, 0f, 0f);
        //gameBoard.transform.localPosition=LudoGameCenter.position;
    }
}
