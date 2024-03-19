using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPage : MonoBehaviour
{
    public Button reload;
    [SerializeField]
    private float timer = 15;
    [SerializeField]
    private bool reloadActivate=false;
    private void OnEnable()
    {
        Debug.Log("OnEnable Loading");
        reload.gameObject.SetActive(false);
        timer = 30;
        reloadActivate = true;
        StartCoroutine(ActivateReload());
    }
    private void OnDisable()
    {
        Debug.Log("OnDisable Loading");
        StopCoroutine(ActivateReload());
        reloadActivate = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator ActivateReload()
    {
        if(reloadActivate)
        {
            if (timer >=1)
            {
                //Debug.Log("Loading");
                yield return new WaitForSeconds(1f);
                timer = timer - 1;
                StartCoroutine(ActivateReload());
            }
            else
            {
                Debug.Log("Reoading button activated");
                string currentUrl = Application.absoluteURL;
                Application.OpenURL(currentUrl);
                //reload.gameObject.SetActive(true);
            }
        }
    }
}
