using UnityEngine;
using UnityEngine.UI;

public class ScrollViewController : MonoBehaviour
{
    public ScrollRect scrollView;
    public int itemsPerPage = 10;

    private int totalItems;
    private int totalPages;
    private int currentPage = 1;

    private float lastNormalizedPosition;

    void Start()
    {
        // Assuming you have a method to get the total number of items
        totalItems = GetTotalItems();

        // Calculate total pages based on the total number of items and items per page
        totalPages = Mathf.CeilToInt((float)totalItems / itemsPerPage);

        // Subscribe to scroll event
        scrollView.onValueChanged.AddListener(OnScrollValueChanged);
    }

    void OnScrollValueChanged(Vector2 value)
    {
        // Calculate current normalized position
        float currentNormalizedPosition = scrollView.verticalNormalizedPosition;

        // If scrolling down and passing every 10 items
        if (currentNormalizedPosition < lastNormalizedPosition)
        {
            int newItemIndex = Mathf.FloorToInt(currentNormalizedPosition * totalItems);
            int newPage = Mathf.CeilToInt((float)newItemIndex / itemsPerPage);

            if (newPage > currentPage && newItemIndex % itemsPerPage == 0)
            {
                currentPage = newPage;
                Debug.Log("Passed page: " + currentPage);
            }
        }
        // If scrolling up and passing every 10 items
        else if (currentNormalizedPosition > lastNormalizedPosition)
        {
            int newItemIndex = Mathf.CeilToInt(currentNormalizedPosition * totalItems);
            int newPage = Mathf.CeilToInt((float)newItemIndex / itemsPerPage);

            if (newPage < currentPage && newItemIndex % itemsPerPage == 0)
            {
                currentPage = newPage;
                Debug.Log("Passed page: " + currentPage);
            }
        }

        lastNormalizedPosition = currentNormalizedPosition;
    }

    // Dummy method to get the total number of items, replace with your own logic
    int GetTotalItems()
    {
        // Return the total number of items
        return 100; // For example, 100 items
    }
}
