using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject TilePrefab;
    public int Width = 10;
    public int Height = 10;
    public int NumberOfMines = 10;
    public Transform TileParent;

    public float tileXOffset, tileYOffset;

    private Tile[,] tiles;
    private Dictionary<Tile, GameObject> tileToGameObjectMap;
    private Dictionary<GameObject, Tile> gameObjectToTileMap;

    // Use this for initialization
    void Start () {
        SetupTiles();
        LayMines();
        ConfigureTiles();
        CalculateTileNumbers();
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

    void LayMines()
    {
        int currentMines = 0;
        int currentPasses = 0;
        int randX = 0;
        int randY = 0;
        Tile tile;

        while(currentMines < NumberOfMines)
        {
            if(currentPasses > 10)
            {
                Debug.LogError("Too many passes");
                break;
            }
            randX = Random.Range(0, Width-1);
            randY = Random.Range(0, Height-1);

            tile = GetTileAt(randX, randY);
            if (tile.isMined)
            {
                currentPasses++;
                continue;
            }
            else
            {
                tile.isMined = true;
                currentMines++;
                currentPasses = 0;
            }
        }
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
                //tiles[x, y].SetText();
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
}
