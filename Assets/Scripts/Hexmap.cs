using Photon.Pun;
using UnityEngine;
/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * 
 * Hexmap generates and keeps track of all generated map tiles.
 * It tells each tile to perform certain via function calls to
 * each specific tile.
 * 
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */


//[ExecuteInEditMode] // DEBUG: Toggle this to display tiles in editor

public class Hexmap : MonoBehaviour
{
    private InputMaster controls;
    //public PathDraw lineRenderer;

    // Map size in terms of hexes
    public const int width = 20;
    public const int height = 20;
    
    // Hextiles is an array containing all gameobjects at index [x,y]
    public Hextile[,] hexTiles = new Hextile[width,height];

    // Each tile is a hexPrefab
    public Hextile hexPrefab;

    // Offset values
    private const float xoff = 0.8f;
    private const float zoff = 0.46f;

    // Spawn
    private int master_count = 0;

    PhotonView photonView;

    // Start by generating tiles and making a randomized map-config
    void Awake()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView == null)
            Debug.LogError("Missing photonView component");
        generateTiles();
        randomizeHexmap(500, 3);
    }

    /// <summary>
    /// Utilizes affectRadius() to generate a random hexmap
    /// according to provided parameters.
    /// 
    /// More iterations generate a more varied map and higher
    /// continuity makes larger areas of tiletypes.
    /// 
    /// For best result use a large number of iterations and
    /// a continuity that is smaller than 8.
    /// 
    /// </summary>
    /// <param name="iterations"></param>
    /// <param name="continuity"></param>
    public void randomizeHexmap(int iterations, int continuity)
    {
        Debug.Log("Randomizing map");
        for (int i = 0; i < iterations; i++)
        {
            int randX = Random.Range(0, width);
            int randY = Random.Range(0, height);
            int randR = Random.Range(0, continuity);
            int randT = Random.Range(0, 4);
            ElementState element;

            if (randT == 1)
                element = ElementState.Fire;
            else if (randT == 2)
                element = ElementState.Water;
            else if (randT == 3)
                element = ElementState.Earth;
            else
                element = ElementState.Wind;

            affectRadius(randX, randY, randR, element);
        }

        //synka nya mapen med andra 
        SynchronizeNetworkMap();
    }
    /// <summary>
    /// Synchronize all tiles for both players so they have identical map layout 
    /// </summary>
    public void SynchronizeNetworkMap()
    {
        Debug.LogError("Synchronizing map over network");
        //synka nya mapen med andra 
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //TODO: if lobby function, removed buffered target
                AreaEffect areaEffect = hexTiles[x, y].areaEffect;
                photonView.RPC("RPC_UpdateTile", RpcTarget.Others, x, y, hexTiles[x, y].tileType, areaEffect.isActivated
                    ,areaEffect.TrapElement, areaEffect.healthModifier, hexTiles[x, y].isOccupied);
            }
        }
    }

    [PunRPC]
    void RPC_UpdateTile(int x, int y, ElementState tileElement, bool isTrapActive, ElementState trapElement, int trapModifier, bool isCharActive)
    {
        hexTiles[x, y].Synchronize(tileElement, isTrapActive, trapElement, trapModifier, isCharActive);
    }

    /// <summary>
    /// Syncs occupation status over network
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="isOccupied"></param>
    /// <param name="character"></param>
    public void SetOccupation(int x, int y, bool isOccupied, Character character)
    {
        if(isOccupied)
        {
            hexTiles[x, y].SetOccupant(character);
        }
        else
        {
            hexTiles[x, y].RemoveOccupant();
        }
        photonView.RPC("RPC_UpdateTile", RpcTarget.Others, x, y, hexTiles[x, y].tileType, hexTiles[x, y].areaEffect.isActivated
                    , hexTiles[x, y].areaEffect.TrapElement, hexTiles[x, y].areaEffect.healthModifier, hexTiles[x, y].isOccupied);
    }


    /// <summary>
    /// Change the element type of a single tile
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="element"></param>
    private void ChangeTileElement(int x, int y, ElementState element)
    {
        if (CheckValid(x, y))
        {
            hexTiles[x, y].makeType(element);
            photonView.RPC("RPC_UpdateTile", RpcTarget.Others, x, y, hexTiles[x, y].tileType, hexTiles[x, y].areaEffect.isActivated
            , hexTiles[x, y].areaEffect.TrapElement, hexTiles[x, y].areaEffect.healthModifier, hexTiles[x, y].isOccupied);
        }
    }


    public void ChangeEffect(int x, int y, bool setEffect, ElementState element = ElementState.None, int healthMod = 0) 
    {
        if(CheckValid(x,y)) 
        {
            if(setEffect){
                hexTiles[x, y].AddEffect(element, healthMod);
            }
            else {
                hexTiles[x,y].RemoveEffect();
            }
            AreaEffect areaEffect = hexTiles[x, y].areaEffect;
            photonView.RPC("RPC_UpdateTile", RpcTarget.Others, x, y, hexTiles[x, y].tileType, areaEffect.isActivated, areaEffect.TrapElement, areaEffect.healthModifier);

        }
    }

    /// Check if the given index is within bounds of map
    private bool CheckValid(int x, int y) 
    {
        return (x >= 0 && x < width && y >= 0 && y < height);
    }

    public Hextile GetSpawnPosition(bool master)
    {
        // retunera fr�n hosts tiles yo
        Hextile tile = null;
        if(master && CheckValid(master_count + 3, 0))
        {
            tile = hexTiles[master_count + 3, 0];
            master_count+=4;
            return hexTiles[master_count + 3, 0];
        }
        else if(!master && CheckValid(master_count + 3, height - 1))
        {
            // else return non master position
            tile = hexTiles[master_count + 3, height - 1];
            master_count += 4;
            return hexTiles[master_count + 3, height - 1];
        }
        return tile;
    }

    /// Generates all tiles and places them in the array.
    private void generateTiles()
    {
        Debug.Log("Generating tiles");
        float zSwitch = zoff;
        float xSwitch = xoff;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                // Instantiate and add to array
                Hextile instantiated = Instantiate(hexPrefab, new Vector3(x * xoff, 0, 2 * zoff * z + zSwitch), Quaternion.identity);
                instantiated.transform.localScale = Vector3.one;
                instantiated.transform.parent = transform;
                //Hextile tile = instantiated.GetComponent<Hextile>();
                hexTiles[x, z] = instantiated;
                hexTiles[x, z].SetTileIndex(new Vector2Int(x, z));
                //photonView.RPC("RPC_AddTile", RpcTarget.AllBuffered, instantiated, x, z);
            }
            // Only offsett odd rows
            if (x % 2 == 0)
            {
                zSwitch = 0.0f;
            }
            else
            {
                zSwitch = zoff;
            }
            xSwitch += xoff;
        }
    }

    // This function is completely unreadable. Good luck! Mvh Jonathan at 23:16
    //
    // Should be changed some day. Preferably before 21:00.
    // TODO: Redo the entire thing? (Atleast it works as intended I guess.)
    public void affectRadius(int xCord, int yCord, int radius, ElementState element)
    {
        if (radius >= 1)
        {
            int nAffect = 0; // Number of tiles on each side of center to affect

            // is center of row consisting of 1 or 2 tiles?
            bool symmetric;
            if (radius % 2 == 0)
            {
                symmetric = true;
                nAffect++;
            }
            else
                symmetric = false;

            // Affect a growing amount of points around centerpoints.
            for (int xi = -radius; xi <= radius; xi++)
            {
                if (xi <= 0)
                    nAffect += 1;
                else
                    nAffect -= 1;

                int range = nAffect / 2 + (radius - 1) / 2;

                if (symmetric)
                {
                    for (int yi = -range; yi <= range; yi++)
                    {
                        ChangeTileElement(xCord + xi, yCord + yi, element);
                    }
                    symmetric = false;
                }
                else
                {
                    int correction = 0;
                    if (xCord % 2 != 0)
                        correction = 1;
                    if (radius == 1) // If radius is 1 top-center tile must be made implicitly
                        ChangeTileElement(xCord + xi, yCord + 1 - correction, element);
                    for (int yi = -range; yi <= range; yi++)
                    {
                        ChangeTileElement(xCord + xi, yCord + yi - correction, element);
                        if (yi > 0)
                        {
                            ChangeTileElement(xCord + xi, yCord + yi + 1 - correction, element);
                        }
                    }
                    symmetric = true;
                }
            }
        }
        //Radius = 0, just affect 1 tile
        else if(radius == 0)
            ChangeTileElement(xCord, yCord, element);
    }
}
