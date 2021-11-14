using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCursor : MonoBehaviour
{
    public void changeCursor(Texture2D newCursor) {
            CursorMode mode = CursorMode.ForceSoftware;
            Vector2 hotSpot = new Vector2(newCursor.width / 2, newCursor.height / 2);
            Cursor.SetCursor(newCursor, hotSpot, mode);
    }
    public void changeToDefaultCursor() {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); ;
    }
}
