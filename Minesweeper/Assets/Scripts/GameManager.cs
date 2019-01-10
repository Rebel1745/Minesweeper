using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//
// TODO
//// Lay the mines after the user clicks and make sure there is a "safe zone" around the initial click - decrease chances of mine spawning on the edges
//// Jazz up the game over animation - spawn a particle where the mine explodes
////
//// MAYBE: Fix the button spawning locations on the canvas so it can all scale nicely (I am afraid of the UI system in Unity)
//

public class GameManager : MonoBehaviour {

    public GameOver gameOver;

    public GameObject TilePrefab;
    public int Width = 10;
    public int Height = 10;
    public int NumberOfMines = 10;
    public Transform TileParent;

    public int ChanceForEdgeSpawn = 30;

    public int currentMines;
    public float currentTime = 0;

    public bool isGameStarted = false;
    public bool isGameOver = false;

    public float tileXOffset, tileYOffset;

    private Tile[,] tiles;
    private Dictionary<Tile, GameObject> tileToGameObjectMap;
    private Dictionary<GameObject, Tile> gameObjectToTileMap;

    // GUI elements
    public Text RemainingTimeText;
    public Text RemainingMinesText;

    // Use this for initialization
    void Start ()
    {
        gameOver = GetComponent<GameOver>();
        SetupTiles();
        ConfigureTiles();
    }

    private void Update()
    {
        if (isGameStarted && !isGameOver)
        {
            currentTime += Time.deltaTime;

            if (AllTilesRevealed())
            {
                GameOver(null);
            }

            UpdateGUI();
        }        
    }

    public void StartGame(Tile tile)
    {
        LayMines(tile);
        CalculateTileNumbers();
        UpdateGUI();

        isGameStarted = true;

        StartCoroutine(tile.Reveal());
    }

    void UpdateGUI()
    {
        RemainingMinesText.text = "Mines: " + currentMines.ToString();
        RemainingTimeText.text = "Time: " + Mathf.Ceil(currentTime).ToString();
    }

    void SetupTiles()
    {
        tiles = new Tile[Width, Height];
        tileToGameObjectMap = new Dictionary<Tile, GameObject>();
        gameObjectToTileMap = new Dictionary<GameObject, Tile>();

        float tileWidth = 30f; //TilePrefab.GetComponent<BoxCollider2D>().size.x;
        float tileHeight = 30f; //TilePrefab.GetComponent<BoxCollider2D>().size.y;

        Tile tile;

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                GameObject tileGO = Instantiate(TilePrefab, new Vector3(x * tileWidth + tileXOffset, y * tileHeight + tileYOffset, 0), Quaternion.identity, TileParent);
                tile = tileGO.GetComponent<Tile>();
                tiles[x, y] = tile;
                tile.x = x;
                tile.y = y;
                tileToGameObjectMap[tile] = tileGO;
                gameObjectToTileMap[tileGO] = tile;
                tileGO.name = "Tile (" + x + ", " + y + ")";
            }
        }
    }

    void LayMines(Tile clickedTile)
    {
        int layedMines = 0;
        int currentPasses = 0;
        int randX = 0;
        int randY = 0;
        Tile tile;

        // create a list of tiles that will not be mine-able
        List<Tile> excludeList = new List<Tile>();
        
        // Add current tile to list
        excludeList.Add(clickedTile);

        // Add current tiles' neighbours
        foreach(Tile t in clickedTile.neighbours)
        {
            excludeList.Add(t);
        }

        while (layedMines < NumberOfMines)
        {
            if(currentPasses > 1000)
            {
                Debug.LogError("Too many passes");
                break;
            }

            randX = Random.Range(0, Width);
            randY = Random.Range(0, Height);

            // pass if the mine would be placed on any of the excludeList tiles
            bool inList = false;
            foreach (Tile t in excludeList)
            {

                if (randX == t.x && randY == t.y)
                {
                    inList = true;                    
                }
            }    
            if (inList)
            {
                currentPasses++;
                continue;
            }        

            // maybe pass if the mine would be on the edge bounds a decrease edge spawn rate
            if ((randX == 0 || randY == 0 || randX == Width-1 || randY == Height - 1))
            {
                // if the random number is higher than the ChanceForEdgeSpawn the pass
                int randChance = Random.Range(0, 100);
                if(randChance > ChanceForEdgeSpawn)
                {
                    currentPasses++;
                    continue;
                }                
            }

            tile = GetTileAt(randX, randY);
            if (tile.isMined)
            {
                currentPasses++;
                continue;
            }
            else
            {
                tile.isMined = true;
                layedMines++;
                currentMines++;
                currentPasses = 0;
            }
        }

        // We've started.  Add to the total number of games we've played
        PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed", 0) + 1);
    }

    void ConfigureTiles()
    {
        // calculate the neighbours of each tile
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                tiles[x,y].neighbours = GetNeighbours(tiles[x,y].x, tiles[x,y].y);
            }
        }
    }

    public Tile[] GetNeighbours(int x, int y)
    {
        List<Tile> neighbours = new List<Tile>();

        // check to see if each tile exists
        // top left
        neighbours.Add(GetTileAt(x - 1, y + 1));
        //top middle
        neighbours.Add(GetTileAt(x + 0, y + 1));
        //top right
        neighbours.Add(GetTileAt(x + 1, y + 1));
        // centre left
        neighbours.Add(GetTileAt(x - 1, y + 0));
        // centre right
        neighbours.Add(GetTileAt(x + 1, y + 0));
        // bottom left
        neighbours.Add(GetTileAt(x - 1, y - 1));
        // bottom middle
        neighbours.Add(GetTileAt(x + 0, y - 1));
        // bottom right
        neighbours.Add(GetTileAt(x + 1, y - 1));

        List<Tile> neighbours2 = new List<Tile>();

        foreach (Tile t in neighbours)
        {
            if (t != null)
            {
                neighbours2.Add(t);
            }
        }

        return neighbours2.ToArray();
    }

    void CalculateTileNumbers()
    {
        // check each tiles neighbours for mines
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                int mineCount = 0;
                foreach(Tile t in tiles[x, y].neighbours)
                {
                    if (t.isMined)
                    {
                        mineCount++;
                    }
                }
                tiles[x, y].surroundingMines = mineCount;
            }
        }
    }

    public Tile GetTileAt(int x, int y)
    {
        if (tiles == null)
        {
            Debug.LogError("Tile Array not yet instantiated");
            return null;
        }

        try
        {
            return tiles[x, y];
        }
        catch
        {
            return null;
        }
    }

    public Tile GetTileFromGameObject(GameObject tileGO)
    {
        if (gameObjectToTileMap.ContainsKey(tileGO))
        {
            return gameObjectToTileMap[tileGO];
        }

        return null;
    }

    public GameObject GetGameObjectFromTile(Tile tile)
    {
        if (tileToGameObjectMap.ContainsKey(tile))
        {
            return tileToGameObjectMap[tile];
        }

        return null;
    }

    public void GameOver(Tile minedTile)
    {
        if (AllTilesRevealed())
        {
            Debug.Log("YOU WON!!!");
            // Add to the win count!
            PlayerPrefs.SetInt("GamesWon", PlayerPrefs.GetInt("GamesWon", 0) + 1);
            if(PlayerPrefs.GetInt("BestTime", 999) < currentTime)
            {
                PlayerPrefs.SetInt("BestTime", Mathf.RoundToInt(currentTime));
                PlayerPrefs.SetString("BestTimeDate", "Sort Dates");
            }
            gameOver.Show(true);
        }
        else
        {
            foreach (Tile t in tiles)
            {
                if(t.isMined)
                    t.SetText();
            }

            gameOver.Show(false);
        }

        isGameOver = true;
    }

    public bool AllTilesRevealed()
    {
        foreach(Tile t in tiles)
        {
            if (!t.isRevealed && !t.isFlagged)
                return false;
        }

        return true;
    }
}
