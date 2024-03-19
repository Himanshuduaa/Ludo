using Coffee.UIExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShineEffect : MonoBehaviour
{
    private float progress = 0.0f;
    private bool increasing = true;

    // Update is called once per frame
    void Update()
    {
        if (increasing)
        {
            progress += Time.deltaTime * 0.5f; // Adjust the multiplier to control the speed of movement (0.5f for example).
            if (progress >= 1.0f)
            {
                progress = 1.0f;
                increasing = false;
            }
        }
        else
        {
            progress -= Time.deltaTime * 0.5f; // Adjust the multiplier to control the speed of movement (0.5f for example).
            if (progress <= 0.0f)
            {
                progress = 0.0f;
                increasing = true;
            }
        }
        this.gameObject.GetComponent<ShinyEffectForUGUI>().location = progress;

        // You can use the 'progress' variable for various purposes in your game.
    }
}
