using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickSound : MonoBehaviour
{
    [SerializeField]
    private string soundName="Click";
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.AddComponent<AudioSource>();
        if(this.gameObject.GetComponent<Button>()!=null )
            this.gameObject.GetComponent<Button>().onClick.AddListener(playSound);
    }
    private void playSound()
    {
        //AudioManager.instance.PlayAudio(soundName);
        if(LudoManager.Instance.gameInfo.gameSettings.sound)
            AudioManager.instance.PlayAudioOnSource(soundName,this.gameObject.GetComponent<AudioSource>());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
