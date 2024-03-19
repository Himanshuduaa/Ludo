using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Test : MonoBehaviour
{
    public TMP_InputField id;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setPlayerID() 
    {
        LudoManager.Instance.gameInfo.playerID = id.text;
        UIManager.Instance.StartTheGame();
    }
}
