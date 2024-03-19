using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePicture : MonoBehaviour
{
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        // Get the index of the button based on its position in the hierarchy
        index = GetHierarchyIndex(this.gameObject.transform);

        // Print the index to the Unity console
        Debug.Log("Hierarchy Index of Button: " + index);
        this.gameObject.GetComponent<Button>().onClick.AddListener (changePicture);
    }
    private int GetHierarchyIndex(Transform transform)
    {
        return transform.GetSiblingIndex();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void changePicture()
    {
        for (int i=0; i < PlayerAccountManager.Instance.PlayerPicture.Count;i++)
        {
            PlayerAccountManager.Instance.PlayerPicture[i].GetComponent<Image>().sprite=this.gameObject.GetComponent<Image>().sprite;
        }
        PlayerAccountManager.Instance.Avatar = this.gameObject.GetComponent<Image>().sprite;
        PlayerAccountManager.Instance.avatarIndex = index;
        LudoManager.Instance.gameInfo.avatar= index;
    }
    
}
