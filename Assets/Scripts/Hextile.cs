using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * 
 * Hextile contains basic information about a specific tile and provides
 * functions for modifying that specific tile
 * 
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
public class Hextile : MonoBehaviourPun
{
    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     * 
     * tileType contains information of what element the tile belongs to.
     * Enables checks such as " if (hexTile[0,0].tileType == ElementState.Fire) "
     * 
     * tileType can be: "grass", "dessert", "water" or "woods".
     * 
     * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

    /// <summary>
    /// Element type of this tile
    /// </summary>
    public ElementState tileType;
    /// <summary>
    /// Area effect on this tile, can be a trap or health potion for example
    /// </summary>
    public AreaEffect areaEffect;

    /// <summary>
    /// Returns true if this tile is occupied by a character
    /// </summary>
    public bool isOccupied;
    /// <summary>
    /// reference the occupant on this tile
    /// </summary>
    public Character occupant;

    /// <summary>
    /// trap prefab to use
    /// </summary>
    public GameObject trapPrefab;


    /// <summary>
    /// Should the tile be rotating
    /// </summary>
    [SerializeField] private bool spin;

    // The angle of the tile during rotation.
    private int angleCount = -180;

    [Header("Materials")]
    // Materials for the tilebase of different types
    [SerializeField] private Material matgrass;
    [SerializeField] private Material matdessert;
    [SerializeField] private Material matwater;
    [SerializeField] private Material matwoods;

    [Header("Type Graphics")]
    [SerializeField] private GameObject tile;
    [SerializeField] private GameObject forest;
    [SerializeField] private GameObject desert;
    [SerializeField] private GameObject water;
    [SerializeField] private GameObject grass;

    /// <summary>
    /// index [x,y] of this tile in the map
    /// </summary>
    public Vector2Int tileIndex = new Vector2Int(-1, -1);

    // Awake runs before start
    void Awake()
    {
        randomizeType(); //Set to random
    }

    // Update checks what needs to be done to the tile in each frame
    private void Update()
    {
        if (spin)
            rotateHex();
    }

    /// <summary>
    /// Synchronizes a tile over network, called from hexmap.cs
    /// </summary>
    /// <param name="tileElement">new state</param>
    /// <param name="isTrapActive">new state</param>
    /// <param name="trapElement">new state</param>
    /// <param name="trapModifier">new state</param>
    /// <param name="isCharActive">new state</param>
    public void Synchronize(ElementState tileElement, bool isTrapActive, ElementState trapElement, float trapModifier, bool isCharActive)
    {
        if(tileType != tileElement)
        {
            makeType(tileElement);
        }

        //Synchronize traps 
        if (!areaEffect.isActivated && isTrapActive)
            AddEffect(trapElement, trapModifier);
        else if (areaEffect.isActivated && !isTrapActive)
            RemoveEffect();

        if (isCharActive)
            isOccupied = true;
        else
            RemoveOccupant();
    }

    /// Spin until 1 rotation has been completed TODO: Make rotateHex timebased instead of framebased.
    private void rotateHex()
    {
        angleCount++;
        transform.localEulerAngles = new Vector3(angleCount, 0, 0);
        if (angleCount == 0)
        {
            spin = false;
            angleCount = -180;
        }
    }

    /// Tells update to initiate spinning state
    public void spinTile()
    {
        transform.localEulerAngles = new Vector3(-180, 0, 0);
        spin = true;
    }

    /// <summary>
    /// Adds an effect to this tile
    /// </summary>
    public void AddEffect(ElementState state, float healthMod)
    {
        spinTile();
        trapPrefab.SetActive(true);
        areaEffect.SetEffect(state, healthMod);
    }

    /// <summary>
    /// Clears the effect from this tile
    /// </summary>
    public void RemoveEffect()
    {
        spinTile();
        trapPrefab.SetActive(false);
        areaEffect.Remove();
    }


    /// <summary>
    /// Sets a specific character as occupant to a tile
    /// </summary>
    /// <param name="character"></param>
    public void SetOccupant(Character character)
    {
        isOccupied = true;
        occupant = character;
    }

    /// <summary>
    /// Removes occupant and sets isOccupied to false
    /// </summary>
    public void RemoveOccupant()
    {
        isOccupied = false;
        occupant = null;
    }
    /// <summary>
    /// Randomize element type of this tile
    /// </summary>
    private void randomizeType()
    {
        int randT = Random.Range(0, 4);
        // string type;
        ElementState state;
        if (randT == 1)
            state = ElementState.Earth;
        else if (randT == 2)
            state = ElementState.Fire;
        else if (randT == 3)
            state = ElementState.Water;
        else
            state = ElementState.Wind;

        makeType(state);
    }

    /// <summary>
    /// Creates a element type on this tile and activates correct material and graphics
    /// </summary>
    /// <param name="type">type of element to set this tile to</param>
    public void makeType(ElementState type)
    {
        switch(type)
        {
            case ElementState.Wind:
                {
                    tileType = ElementState.Wind;
                    tile.GetComponent<MeshRenderer>().material = matgrass;
                    ResetTile();
                    grass.SetActive(true);
                    break;
                }
            case ElementState.Fire:
                {
                    tileType = ElementState.Fire;
                    tile.GetComponent<MeshRenderer>().material = matdessert;
                    ResetTile();
                    desert.SetActive(true);
                    break;
                }
            case ElementState.Water:
                {
                    tileType = ElementState.Water;
                    tile.GetComponent<MeshRenderer>().material = matwater;
                    ResetTile();
                    water.SetActive(true);
                    break;
                }
            case ElementState.Earth:
                {
                    tileType = ElementState.Earth;
                    tile.GetComponent<MeshRenderer>().material = matwoods;
                    ResetTile();
                    forest.SetActive(true);
                    break;
                }
            default:
                {
                    tileType = ElementState.None;
                    ResetTile();
                    tile.GetComponent<MeshRenderer>().material = matgrass;
                    break;
                }
        }
        spinTile();
    }

    /// <summary>
    /// Turns of all graphics on this tile to its original state
    /// </summary>
    private void ResetTile() 
    {
        forest.SetActive(false);
        water.SetActive(false);
        grass.SetActive(false);
        desert.SetActive(false);
        trapPrefab.SetActive(false);

    }
    /// <summary>
    /// Get the position of a tile
    /// </summary>
    public Vector3 Position {
        get { return transform.position; }
    }
    /// <summary>
    /// Sets the index of this tile
    /// </summary>
    /// <param name="index"></param>
    public void SetTileIndex(Vector2Int index)
    {
        tileIndex = index;
    }
}
