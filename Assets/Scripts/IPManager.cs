using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class IPManager : MonoBehaviour
{
    public GameInfo gameInfo;
    public TMP_InputField address;
    // Start is called before the first frame update
    void Start()
    {
        if(gameInfo.address!="")
        address.text = gameInfo.address;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void submit()
    {
        if(address.text!="")
        {
            gameInfo.address=address.text;
            UnityEngine.SceneManagement.SceneManager.LoadScene("LudoLatest");

        }
    }
}
