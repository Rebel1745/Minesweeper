using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TileButton : MonoBehaviour, IPointerClickHandler
{

    public Color OriginalColour;
    public Color HoverColor;

    public int x;
    public int y;
    public bool isMined = false;
    public bool isFlagged = false;
    public int surroundingMines = 0;
    public Tile[] neighbours;

    public void SetText()
    {
        Text text = this.GetComponentInChildren<Text>();
        if (isFlagged)
        {
            text.text = "F";
        }
        if (isMined)
        {
            text.text = "M";
        }
        else
        {
            text.text = surroundingMines.ToString();
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log(x);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(eventData.button);
        if (eventData.button == PointerEventData.InputButton.Left)
        {

        }
        else if (eventData.button == PointerEventData.InputButton.Right)
            isFlagged = !isFlagged;
    }
}
