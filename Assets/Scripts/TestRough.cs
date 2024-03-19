using UnityEngine;
using System.Collections;

public class TestRough : MonoBehaviour
{
    public float timer = 0f;
    public bool isTimerRunning = false;

    [ContextMenu("Do StartTimer")]
    // Start the timer coroutine
    public void StartTimer()
    {
        if (!isTimerRunning)
        {
            isTimerRunning = true;
            StartCoroutine(UpdateTimer());
        }
    }

    [ContextMenu("Do StopTimer")]

    // Stop the timer coroutine
    public void StopTimer()
    {
        if (isTimerRunning)
        {
            isTimerRunning = false;
            StopCoroutine(UpdateTimer());
        }
    }

    // Coroutine to update the timer
    private IEnumerator UpdateTimer()
    {
        while (isTimerRunning)
        {
            timer += Time.deltaTime; // Increment the timer by the time since the last frame
            yield return null; // Wait for the next frame
        }
    }

    [ContextMenu("Do GetTimerValue")]

    // Get the current timer value
    public float GetTimerValue()
    {
        return timer;
    }
}
