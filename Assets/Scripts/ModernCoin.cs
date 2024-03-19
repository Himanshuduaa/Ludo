using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModernCoin : MonoBehaviour
{
    public TextMeshProUGUI coins;
    public int totalCoin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setCoins(/*int coin*/)
    {
        coins.text= totalCoin.ToString();
    }
}
