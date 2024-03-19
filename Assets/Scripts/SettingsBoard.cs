using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsBoard : MonoBehaviour
{
    public LeanToggle Sound;
    public LeanToggle Music;
    private bool isOn;
    public GameObject settingsPage;
    // Start is called before the first frame update
    void Start()
    {
        isOn = false;
        settingsPage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setSoundAndMusic()
    {
        if(Sound.On)
        {
            LudoManager.Instance.gameInfo.gameSettings.sound = true;
        }
        else
        {
            LudoManager.Instance.gameInfo.gameSettings.sound = false;
        }
        if (Music.On)
        {
            LudoManager.Instance.gameInfo.gameSettings.music = true;
        }
        else
        {
            LudoManager.Instance.gameInfo.gameSettings.music = false;
        }
    }
    public void OnOffSettings()
    {
        if(!isOn)
        {
            settingsPage.SetActive(true);
            isOn = true;
        }
        else
        {
            settingsPage.SetActive(false);
            isOn = false;
        }
    }
}
