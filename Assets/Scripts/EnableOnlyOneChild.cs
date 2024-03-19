using UnityEngine;

public class EnableOnlyOneChild : MonoBehaviour
{
    private void Start()
    {
        // Disable all child GameObjects by default
        //SetAllChildrenActive(false);
    }
    private void OnEnable()
    {
        // Disable all child GameObjects by default
        SetAllChildrenActive(false);
    }
   
    public void ActivateThisChild()
    {
        // Disable all child GameObjects
        SetAllChildrenActive(false);

        // Enable this child GameObject
        gameObject.SetActive(true);
    }

    private void SetAllChildrenActive(bool active)
    {
        foreach (Transform child in transform.parent)
        {
            if (child != transform) // Exclude the current child
            {
                child.gameObject.SetActive(active);
            }
        }
    }
}
