using System.Collections;
using System.Collections.Generic;
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
    public PathDraw lineRenderer;

    // Map size in terms of hexes
    public const int width = 20;
    public const int height = 20;
    
    // Hextiles is an array containing all gameobjects at index [x,y]
    public Hextile[,] hexTiles = new Hextile[width,height];

    // Each tile is a hexPrefab
    public Hextile hexPrefab;

    // Offset values
    private float xoff = 0.8f;
    private float zoff = 0.46f;

    // Spawn
    private int master_count = 0;


    // ** TEMPORARY VARIABLES **
    public Raycasthandler rayhandler;
    private Vector2Int currentHex;


    // Start by generating tiles and making a randomized map-config
    void Start()
    {
        generateTiles();
        randomizeHexmap(500, 3);

        currentHex = new Vector2Int(3, 0); // TODO: Remove me and make me based on character tile pos.
        lineRenderer.addNodeToPath(hexTiles[currentHex.x, currentHex.y]);
    }

    // Update is called once per frame
    void Update()
    {
        //hexTiles[4, 0].spinTile();
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
        for (int i = 0; i < iterations; i++)
        {
            int randX = Random.Range(0, width);
            int randY = Random.Range(0, height);
            int randR = Random.Range(0, continuity);
            int randT = Random.Range(0, 4);
            string effect;

            if (randT == 1)
                effect = "typeDessert";
            else if (randT == 2)
                effect = "typeWater";
            else if (randT == 3)
                effect = "typeWoods";
            else
                effect = "typeGrass";

            affectRadius(randX, randY, randR, effect);
        }
    }

    /// Applies a provided effect to a tile for given coordinates if within bounds
    private void applyEffect(int x, int y, string effect)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
            hexTiles[x,y].affectTile(effect);
    }

    /// Checks direction of input to see what tile the path should be drawn to
    public void drawDirection(Vector2 input)
    {
        Vector2Int moveDir = new Vector2Int(0, 0);
        if (input.x > 0)
            moveDir.x = 1;
        else if (input.x < 0)
            moveDir.x = -1;
        if (input.y > 0)
            moveDir.y = 1;
        else if (input.y < 0)
            moveDir.y = -1;
        drawNode(moveDir);
    }

    public Vector3 getSpawnPosition(bool master)
    {
        Vector3 pos;
        if(master)
        {
            pos = hexTiles[master_count + 1, 0].getPosition();
            ++master_count;
        }
        else
        {
            // else return non master position
        }
        return pos;
    }

    /// Draw to identified tile and update currentHex if command is within bounds of the map
    private void drawNode(Vector2Int direction)
    {
        if(currentHex.x + direction.x < width && currentHex.x + direction.x >= 0
            && currentHex.y + direction.y < height && currentHex.y + direction.y >= 0)
        {
            lineRenderer.addNodeToPath(hexTiles[currentHex.x + direction.x, currentHex.y + direction.y]);
            currentHex.x += direction.x;
            currentHex.y += direction.y;
        }
    }

    /// Generates all tiles and places them in the array.
    private void generateTiles()
    {
        float zSwitch = zoff;
        float xSwitch = xoff;
        Hextile instantiated;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                // Instantiate and add to array
                instantiated = Instantiate(hexPrefab, new Vector3(x * xoff, 0, 2 * zoff * z + zSwitch), Quaternion.identity);
                instantiated.transform.localScale = Vector3.one;
                instantiated.transform.parent = transform;
                hexTiles[x, z] = instantiated;
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
    private void affectRadius(int xCord, int yCord, int radius, string effect)
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
                        applyEffect(xCord + xi, yCord + yi, effect);
                    }
                    symmetric = false;
                }
                else
                {
                    int correction = 0;
                    if (xCord % 2 != 0)
                        correction = 1;
                    if (radius == 1) // If radius is 1 top-center tile must be made implicitly
                        applyEffect(xCord + xi, yCord + 1 - correction, effect);
                    for (int yi = -range; yi <= range; yi++)
                    {
                        applyEffect(xCord + xi, yCord + yi - correction, effect);
                        if (yi > 0)
                        {
                            applyEffect(xCord + xi, yCord + yi + 1 - correction, effect);
                        }
                    }
                    symmetric = true;
                }
            }
        }
        //Radius = 0, just affect 1 tile
        else if(radius == 0)
            applyEffect(xCord, yCord, effect);
    }
}
