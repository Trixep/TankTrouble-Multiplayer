using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonText : MonoBehaviour
{
    public int offsetY = 2;
    public RectTransform textRect;
    Vector3 pos;

    void Start()
    {
        pos = textRect.localPosition;
        mouseExit();
    }

    public void Down()
    {
        textRect.localPosition = new Vector3(pos.x, pos.y - (float)offsetY, pos.z);
    }

    public void Up()
    {
        textRect.localPosition = pos;
    }

    public Texture2D purpleNormal;
    public Vector2 normalCursor;

    public Texture2D purplePointer;
    public Vector2 pointerCursor;

    public void mouseExit()
    {
        Cursor.SetCursor(purpleNormal, normalCursor, CursorMode.Auto);
    }
    public void mouseHover()
    {
        Cursor.SetCursor(purplePointer, pointerCursor, CursorMode.Auto);
    }
}
