using UnityEngine;
using UnityEngine.UI;

public class IntervalSlider : MonoBehaviour
{
    //public Text intervalLabel; // Reference to the Text element displaying interval labels
    public int[] intervals = { 5, 10, 15, 20 }; // Array of intervals

    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        // Find the nearest interval to the current slider value
        int nearestInterval = FindNearestInterval(value);

        // Update the slider value to the nearest interval
        slider.value = nearestInterval;

        // Update the interval label
        //intervalLabel.text = nearestInterval.ToString();
    }

    private int FindNearestInterval(float value)
    {
        int nearest = intervals[0];
        float minDifference = Mathf.Abs(intervals[0] - value);

        foreach (int interval in intervals)
        {
            float difference = Mathf.Abs(interval - value);
            if (difference < minDifference)
            {
                minDifference = difference;
                nearest = interval;
            }
        }

        return nearest;
    }
}
