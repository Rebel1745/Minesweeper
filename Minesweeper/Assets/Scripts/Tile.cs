using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Tile : MonoBehaviour {

    private void Start()
    {
        text = this.GetComponentInChildren<Text>();
    }

    public Color[] NumberColours;

    public int x;
    public int y;
    public bool isMined = false;
    public bool isFlagged = false;
    public bool isRevealed = false;
    public int surroundingMines = 0;
    public Tile[] neighbours;

    Text text;

    public void SetText()
    {
        Debug.Log(surroundingMines);
        isRevealed = true;

        gameObject.GetComponent<Button>().interactable = false;

        if (isFlagged)
        {
            text.text = "F";
        }
        if (isMined)
        {
            text.text = "M";
        }
        /*if(surroundingMines == 0)
        {
            text.text = "";
        }
        else
        {*/
            text.text = surroundingMines.ToString();
            //text.color = NumberColours[surroundingMines];
        //}
    }

    public IEnumerator Reveal()
    {
        SetText();

        if(surroundingMines == 0)
        {
            foreach(Tile t in neighbours)
            {
                if (!t.isRevealed && !isFlagged)
                    StartCoroutine(t.Reveal());
            }
        }
        yield return null;
    }

    public void FlagTile()
    {
        if (isFlagged)
        {
            text.text = "";
        }
        else
        {
            text.text = "F";
        }
        isFlagged = !isFlagged;
    }
}
