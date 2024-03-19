using UnityEngine;
using UnityEngine.UI;

public class EnableOnScrollToBottom : MonoBehaviour
{
    public ScrollRect scrollRect;
    public GameObject[] objectsToEnable;

    private bool bottomReached = false;

    private void Start()
    {
        //scrollRect = GetComponent<ScrollRect>();
        if (scrollRect == null)
        {
            Debug.LogError("EnableOnScrollToBottom script must be placed within a ScrollRect's Content GameObject.");
            enabled = false;
            return;
        }

        scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
    }

    private void OnScrollValueChanged(Vector2 normalizedPosition)
    {
        if (!bottomReached && normalizedPosition.y <= 0)
        {
            bottomReached = true;
            EnableObjects();
        }
    }

    private void EnableObjects()
    {
        foreach (var obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
    }
}
