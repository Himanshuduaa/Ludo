using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D cursorTexture; // Set this in the inspector to your custom cursor texture

    void Start()
    {
        // Set the custom cursor texture
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }
}