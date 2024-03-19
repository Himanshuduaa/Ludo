using UnityEngine;

public class EnableControl : MonoBehaviour
{
    private void Start()
    {
        // Disable this GameObject by default
        //gameObject.SetActive(false);
        // Disable all GameObjects with the EnableControl script
        
    }
    //private void OnEnable()
    //{
    //    EnableControl[] enableControls = FindObjectsOfType<EnableControl>();
    //    foreach (EnableControl enableControl in enableControls)
    //    {
    //        enableControl.gameObject.SetActive(false);
    //    }

    //    // Enable this GameObject
    //    gameObject.SetActive(true);
    //}
    public void EnableThisObject()
    {
        // Disable all GameObjects with the EnableControl script
        EnableControl[] enableControls = FindObjectsOfType<EnableControl>();
        foreach (EnableControl enableControl in enableControls)
        {
            enableControl.gameObject.SetActive(false);
        }

        // Enable this GameObject
        gameObject.SetActive(true);
    }
}
