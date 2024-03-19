using DG.Tweening;
using Lean.Gui;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Filters : MonoBehaviour
{
    private bool open=false;
    public LeanToggle prizePoolLowest = new LeanToggle();
    public LeanToggle prizePoolHighest = new LeanToggle();
    [Space(10)]
    public LeanToggle player2 = new LeanToggle();
    public LeanToggle player4 = new LeanToggle();
    [Space(10)]
    public LeanToggle classicMode = new LeanToggle();
    public LeanToggle classicProMode = new LeanToggle();
    public LeanToggle modernMode = new LeanToggle();
    [Space(10)]
    public LeanToggle entryFeeLowest = new LeanToggle();
    public LeanToggle entryFeeHighest = new LeanToggle();

    [Space(10)]

    [SerializeField]
    public FilterDetails filterDetails = new FilterDetails();

    // Start is called before the first frame update
    void Start()
    {
        filterDetails=GetComponent<FilterDetails>();
    }
    public void setDefaultFilters()
    {
        prizePoolLowest.On = true;
        player2.On = true;
        classicMode.On = true;
        entryFeeLowest.On = true;
    }
    private void enableDisablePanel()
    {
        if (!open)
        {
            this.gameObject.transform.DOScale(new Vector3(1,1,1), 0.2f);
            open = true;
        }
        else
        {
            this.gameObject.transform.DOScale(new Vector3(0,0,0), 0.2f);
            open = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void checkTheFilters()
    {
        enableDisablePanel();
    }
    public void setPrizePool(string prizePool)
    {
        filterDetails.prizepool = prizePool;
    }
    public void setPlayer(string setPlayer)
    {
        filterDetails.players = setPlayer;
    }
    public void setMode(string setMode)
    {
        filterDetails.mode = setMode;
    }
    public void setEntry(string setEntry)
    {
        filterDetails.entry = setEntry;
    }
}