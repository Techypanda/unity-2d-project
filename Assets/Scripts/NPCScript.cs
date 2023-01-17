using UnityEngine;

public class NPCScript : MonoBehaviour
{
    public Texture2D HoverCursor;
    void OnMouseOver() {
        Cursor.SetCursor(HoverCursor, Vector2.zero, CursorMode.Auto);
    }
    void OnMouseExit() {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); 
    }
}
