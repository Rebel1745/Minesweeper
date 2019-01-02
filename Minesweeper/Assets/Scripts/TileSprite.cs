using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSprite : MonoBehaviour {

    //public Color OriginalColour;
    //public Color HoverColor;

    public int x;
    public int y;
    public bool isMined = false;
    public bool isFlagged = false;
    public int surroundingMines = 0;
    public Tile[] neighbours;
}
