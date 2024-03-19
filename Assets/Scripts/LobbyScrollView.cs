using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScrollView : MonoBehaviour
{
    public MyScrollView scrollRect;
    private bool hasReachedTop = false;
    private bool hasReachedBottom = false;
    private void Start()
    {
        scrollRect = GetComponent<MyScrollView>();
        // Attach the method to the onValueChanged event
        scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
    }

    private void OnScrollValueChanged(Vector2 value)
    {
        //// Check if the vertical scroll position is at the bottom
        //if (value.y <= 0f && !hasReachedBottom)
        //{
        //    // Call your function when the scroll reaches the bottom
        //    YourFunctionToCall();
        //    hasReachedBottom = true;
        //}
        //else if (value.y > 0f)
        //{
        //    hasReachedBottom = false;
        //}

        // Check if the vertical scroll position is at the top
        if (value.y >= 1f && !hasReachedTop)
        {
            // Call your function when the scroll reaches the top
            YourFunctionToCallTop();

            // Set the flag to true to ensure the function is called only once
            hasReachedTop = true;

            // Reset the flag for reaching the bottom
            hasReachedBottom = false;
        }
        // Check if the vertical scroll position is at the bottom
        else if (value.y <= 0f && !hasReachedBottom)
        {
            // Call your function when the scroll reaches the bottom
            YourFunctionToCallBottom();

            // Set the flag to true to ensure the function is called only once
            hasReachedBottom = true;

            // Reset the flag for reaching the top
            hasReachedTop = false;
        }
        // Check if the vertical scroll position is away from the top and bottom
        else if (value.y > 0f && value.y < 1f)
        {
            // Reset the flags to allow the functions to be called again
            hasReachedTop = false;
            hasReachedBottom = false;
        }

    }
    private void YourFunctionToCallTop()
    {
        // Your code here for reaching the top
        Debug.Log("ScrollView reached the top!");
    }
    private void YourFunctionToCallBottom()
    {
        // Your code here
        Debug.Log("ScrollView reached the bottom!");
    }
}
