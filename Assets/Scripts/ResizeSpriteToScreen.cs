using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeSpriteToScreen : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Camera mainCamera;

    void Start()
    {
        //spriteRenderer = GetComponent < SpriteRenderer>();
        mainCamera = Camera.main;

        // Call the function to resize the sprite based on the screen size
        ResizeSprite();
    }

    void Update()
    {
        ResizeSprite();

        FollowSprite();
    }

    void ResizeSprite()
    {
        if (spriteRenderer == null || mainCamera == null)
        {
            Debug.LogError("SpriteRenderer or Camera not found.");
            return;
        }

        // Get the size of the sprite
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        // Calculate the screen height and width in world coordinates
        float screenHeight = mainCamera.orthographicSize * 2.0f;
        float screenWidth = screenHeight * Screen.width / Screen.height;

        // Calculate the new scale for the sprite to match the screen size
        float newScaleX = screenWidth / spriteSize.x;
        float newScaleY = screenHeight / spriteSize.y;

        // Use the minimum scale to maintain the aspect ratio
        float newScale = Mathf.Min(newScaleX, newScaleY);

        // Apply the new scale to the sprite
        transform.localScale = new Vector3(newScale, newScale, 1f);
    }

    void FollowSprite()
    {
        if (spriteRenderer == null || mainCamera == null)
        {
            return;
        }

        // Get the position of the sprite
        Vector3 spritePosition = transform.position;

        // Set the camera's position to match the sprite's position
        mainCamera.transform.position = new Vector3(spritePosition.x, spritePosition.y, mainCamera.transform.position.z);
    }
}
