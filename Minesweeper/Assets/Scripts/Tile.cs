using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Tile : MonoBehaviour {
    
    Text text;
    GameManager gm;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
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

    public void SetText()
    {
        isRevealed = true;

        if (isFlagged)
        {
            text.text = "F";
        }
        else if (isMined)
        {
            text.text = "M";
        }
        else if(surroundingMines == 0)
        {
            text.text = "";
        }
        else
        {
            text.text = surroundingMines.ToString();
            text.color = NumberColours[surroundingMines];
        }

        gameObject.GetComponent<Button>().interactable = false;
    }

    public IEnumerator Reveal()
    {
        SetText();

        if (!gm.isGameOver)
        {
            if (isMined)
            {
                gm.GameOver(this);
            }

            if(surroundingMines == 0)
            {
                foreach(Tile t in neighbours)
                {
                    if (!t.isRevealed && !t.isFlagged)
                        StartCoroutine(t.Reveal());
                }
            }
        }
        yield return null;
    }

    public void RevealNeighbours()
    {
        if(isRevealed && !isFlagged && GetSurroundingFlags() == surroundingMines)
        {
            foreach (Tile t in neighbours)
            {
                if (!t.isRevealed && !t.isFlagged)
                    StartCoroutine(t.Reveal());
            }
        }
    }

    public void FlagTile()
    {
        if (isFlagged)
        {
            text.text = "";
            gm.currentMines++;
        }
        else
        {
            text.text = "F";
            gm.currentMines--;
        }
        isFlagged = !isFlagged;
    }

    int GetSurroundingFlags()
    {
        int flags = 0;

        foreach (Tile t in neighbours)
        {
            if (t.isFlagged)
                flags++;
        }

        return flags;
    }
}
