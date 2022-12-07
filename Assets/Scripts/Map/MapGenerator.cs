using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    // Map
    [SerializeField] private Tilemap tileMap;
    public int mapWidth;
    public int mapHeight;
    [SerializeField] private float magnification;
    private int offsetX;
    private int offsetY;
    List<List<int>> noiseGrid = new List<List<int>>();
    private List<int> neighbors = new List<int>();

    // Sprite
    public Texture2D tex;
    private List<List<Sprite>> sprite = new List<List<Sprite>>();
    private int spriteHeight = 48;
    private int tileHeight = 16;
    private int spriteColumns = 7;
    private int spriteRows = 3;
    private int spriteSheets;
    private int tileChunk = 32;
    private int thisChunk = 0;

    public static bool generateChunk = false;

    private void Awake()
    {
        spriteSheets = tex.height / spriteHeight;
        for (int i = 0; i < spriteSheets; i++)
        {
            sprite.Add(new List<Sprite>());
            for (int x = 0; x < spriteColumns; x++)
            {
                for (int y = 0; y < spriteRows; y++)
                {
                    sprite[i].Add(SliceSprite(i,x,y));
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (generateChunk)
        {
            ClearMap();
            GenerateMap();
            generateChunk = false;
        }    
    }

    private void OnEnable()
    {
        offsetX = Random.Range(1, 10000);
        offsetY = Random.Range(1, 10000);
        GenerateNoiseGrid();
        GenerateMap();
    }

    #region Build Map Array
    void GenerateNoiseGrid()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            noiseGrid.Add(new List<int>());
            for (int y = 0; y < mapHeight; y++)
            {
                noiseGrid[x].Add(getIdUsingPerlin(x, y));
            }
        }
    }

    int getIdUsingPerlin(int x, int y)
    {
        float rawPerlin = Mathf.PerlinNoise((x - offsetX) / magnification,(y - offsetY) / magnification);
        float clampPerlin = Mathf.Clamp(rawPerlin, 0.0f, 2.0f);
        float scalePerlin = clampPerlin * (sprite.Count + 1);
        if (scalePerlin > sprite.Count)
        {
            scalePerlin = sprite.Count;
        }
        return Mathf.FloorToInt(scalePerlin);
    }
    #endregion

    #region Build Tile Map
    void GenerateMap()
    {

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = (thisChunk * tileChunk); y < ((thisChunk + 1) * tileChunk); y++)
            {
                int thisTile = noiseGrid[x][y]; 
                neighbors = GetNeighbors(thisTile, x, y);
                SetTile(x, y, neighbors, thisTile);
            }
        }
        thisChunk++;
        tileMap.CompressBounds();
    }

    List<int> GetNeighbors(int tileID, int x, int y)
    {
        neighbors.Clear();

        if (x > 0 && y > 0)
        {
            neighbors.Add(noiseGrid[x - 1][y - 1]); // 1!
        }
        else
        {
            neighbors.Add(tileID);
        }

        if (x > 0)
        {
            neighbors.Add(noiseGrid[x - 1][y]); // 2
        }
        else
        {
            neighbors.Add(tileID);
        }

        if (x > 0 && y < mapHeight - 1)
        {
            neighbors.Add(noiseGrid[x - 1][y + 1]); // 3!
        }
        else
        {
            neighbors.Add(tileID);
        }

        if (y > 0)
        {
            neighbors.Add(noiseGrid[x][y - 1]); // 4
        }
        else
        {
            neighbors.Add(tileID);
        }

        neighbors.Add(tileID); // 5

        if (y < mapHeight - 1)
        {
            neighbors.Add(noiseGrid[x][y + 1]); // 6
        }
        else
        {
            neighbors.Add(tileID);
        }

        if (x < mapWidth - 1 && y > 0)
        {
            neighbors.Add(noiseGrid[x + 1][y - 1]); // 7!
        }
        else
        {
            neighbors.Add(tileID);
        }

        if (x < mapWidth - 1)
        {
            neighbors.Add(noiseGrid[x + 1][y]); // 8
        }
        else
        {
            neighbors.Add(tileID);
        }

        if (x < mapWidth - 1 && y < mapHeight - 1)
        {
            neighbors.Add(noiseGrid[x + 1][y + 1]); // 9!
        }
        else
        {
            neighbors.Add(tileID);
        }

        return neighbors;
    }
    void SetTile(int x, int y, List<int> neighbors, int thisTile)
    {
        x = x * 3;
        y = y * 3;
        if (thisTile == sprite.Count)
        { // Top Level All Fill
            SetTile(new Vector3Int(x, y, 0), sprite[sprite.Count - 1][11]); // 0
            SetTile(new Vector3Int(x, y + 1, 0), sprite[sprite.Count - 1][11]); // 1
            SetTile(new Vector3Int(x, y + 2, 0), sprite[sprite.Count - 1][11]); // 2
            SetTile(new Vector3Int(x + 1, y, 0), sprite[sprite.Count - 1][11]); // 3
            SetTile(new Vector3Int(x + 1, y + 1, 0), sprite[sprite.Count - 1][11]); // 4
            SetTile(new Vector3Int(x + 1, y + 2, 0), sprite[sprite.Count - 1][11]); // 5
            SetTile(new Vector3Int(x + 2, y, 0), sprite[sprite.Count - 1][11]); // 6
            SetTile(new Vector3Int(x + 2, y + 1, 0), sprite[sprite.Count - 1][11]); // 7
            SetTile(new Vector3Int(x + 2, y + 2, 0), sprite[sprite.Count - 1][11]); // 9
        }
        else
        {
            SetTile(new Vector3Int(x, y, 0), GetTile(0, neighbors, sprite[thisTile])); // 0
            SetTile(new Vector3Int(x, y + 1, 0), GetTile(1, neighbors, sprite[thisTile])); // 1
            SetTile(new Vector3Int(x, y + 2, 0), GetTile(2, neighbors, sprite[thisTile])); // 2
            SetTile(new Vector3Int(x + 1, y, 0), GetTile(3, neighbors, sprite[thisTile])); // 3
            SetTile(new Vector3Int(x + 1, y + 1, 0), GetTile(4, neighbors, sprite[thisTile])); // 4
            SetTile(new Vector3Int(x + 1, y + 2, 0), GetTile(5, neighbors, sprite[thisTile])); // 5
            SetTile(new Vector3Int(x + 2, y, 0), GetTile(6, neighbors, sprite[thisTile])); // 6
            SetTile(new Vector3Int(x + 2, y + 1, 0), GetTile(7, neighbors, sprite[thisTile])); // 7
            SetTile(new Vector3Int(x + 2, y + 2, 0), GetTile(8, neighbors, sprite[thisTile])); // 8
        }
    }

    Sprite GetTile(int pos, List<int> neighbors, List<Sprite> tileBase)
    {
        int thisTile = neighbors[4];
        string thisTrip = "";
        switch (pos)
        {
            case 0: // !
                if (neighbors[0] - thisTile == 2)
                {
                    return tileBase[20];
                }
                thisTrip = GetTrips(neighbors, thisTile, 3, 0, 1);
                if (thisTrip == "001" || thisTrip == "011") return tileBase[1]; // Orthogonal
                if (thisTrip == "010") return tileBase[13]; // Outer Corner
                if (thisTrip == "100" || thisTrip == "110") return tileBase[3]; // Orthogonal
                if (thisTrip == "111" || thisTrip == "101") return tileBase[0]; // Inner Corner
                break;
            case 1:
                if (neighbors[1] - thisTile == 1)
                {
                    return tileBase[1];
                }
                break;
            case 2: // !
                if (neighbors[2] - thisTile == 2)
                {
                    return tileBase[19];
                }
                thisTrip = GetTrips(neighbors, thisTile, 1, 2, 5);
                if (thisTrip == "001" || thisTrip == "011") return tileBase[5]; // Orthogonal
                if (thisTrip == "010") return tileBase[12]; // Outer Corner
                if (thisTrip == "100" || thisTrip == "110") return tileBase[1]; // Orthogonal
                if (thisTrip == "111" || thisTrip == "101") return tileBase[2]; // Inner Corner
                break;
            case 3:
                if (neighbors[3] - thisTile == 1)
                {
                    return tileBase[3];
                }
                break;
            case 4:
                return tileBase[14];
            case 5:
                if (neighbors[5] - thisTile == 1)
                {
                    return tileBase[5];
                }
                break;
            case 6: // !
                if (neighbors[6] - thisTile == 2)
                {
                    return tileBase[17];
                }
                thisTrip = GetTrips(neighbors, thisTile, 7, 6, 3);
                if (thisTrip == "001" || thisTrip == "011") return tileBase[3]; // Orthogonal
                if (thisTrip == "010") return tileBase[10]; // Outer Corner
                if (thisTrip == "100" || thisTrip == "110") return tileBase[7]; // Orthogonal
                if (thisTrip == "111" || thisTrip == "101") return tileBase[6]; // Inner Corner
                break;
            case 7:
                if (neighbors[7] - thisTile == 1)
                {
                    return tileBase[7];
                }
                break;
            case 8: // !
                if (neighbors[8] - thisTile == 2)
                {
                    return tileBase[16];
                }
                thisTrip = GetTrips(neighbors, thisTile, 5, 8, 7);
                if (thisTrip == "001" || thisTrip == "011") return tileBase[7]; // Orthogonal
                if (thisTrip == "010") return tileBase[9]; // Outer Corner
                if (thisTrip == "100" || thisTrip == "110") return tileBase[5]; // Orthogonal
                if (thisTrip == "111" || thisTrip == "101") return tileBase[8]; // Inner Corner
                break;


        }
        return tileBase[14];
    }

    void SetTile(Vector3Int pos, Sprite sprite)
    {
        CustomTileBase tile = (CustomTileBase)ScriptableObject.CreateInstance(typeof(CustomTileBase));
        tile.sprite = sprite;
        tileMap.SetTile(pos, tile);
    }
    #endregion

    private string GetTrips(List<int> neighbors, int thisTile, int first, int second, int third)
    {
        return Mathf.Clamp((neighbors[first] - thisTile), 0, 1).ToString() + Mathf.Clamp((neighbors[second] - thisTile), 0, 1).ToString() + Mathf.Clamp((neighbors[third] - thisTile), 0, 1).ToString();
    }

    private Sprite SliceSprite(int i, int x, int y)
    {
        int startX = (x * tileHeight);
        int startY = (i * spriteHeight) + (y * tileHeight);
        return Sprite.Create(tex, new Rect(startX, startY, tileHeight, tileHeight), new Vector2(0.5f, 0.5f), 16f);
    }

    public class CustomTileBase : TileBase
    {
        public Sprite sprite;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = sprite;
        }
    }
    private void ClearMap()
    {

    }
}