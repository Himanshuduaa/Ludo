using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CopyGameCode : MonoBehaviour
{
    public TextMeshProUGUI gamecode;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CopyTextToClipboard()
    {
        // Ensure there is text to copy
        if (gamecode.text !="")
        {
            // Get the text from the Text component
            string text = gamecode.text;

            // Copy the text to the clipboard
            GUIUtility.systemCopyBuffer = text;

            Debug.Log("Text copied to clipboard: " + text);
        }
        else
        {
            Debug.LogWarning("Text component not assigned.");
        }
    }
    public void copyRefLink()
    {
        if (UIManager.Instance.referenceLink.text != "")
        {
            // Get the text from the Text component
            string text = UIManager.Instance.referenceLink.text;

            // Copy the text to the clipboard
            GUIUtility.systemCopyBuffer = text;

            Debug.Log("Text copied to clipboard: " + text);
        }
        else
        {
            Debug.LogWarning("Text component not assigned.");
        }
    }
    public void copyRefCode()
    {
        if (UIManager.Instance.referenceCode.text != "")
        {
            // Get the text from the Text component
            string text = UIManager.Instance.referenceCode.text;

            // Copy the text to the clipboard
            GUIUtility.systemCopyBuffer = text;

            Debug.Log("Text copied to clipboard: " + text);
        }
        else
        {
            Debug.LogWarning("Text component not assigned.");
        }
    }
}
