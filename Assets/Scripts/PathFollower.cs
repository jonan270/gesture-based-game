using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is just a simple solution that should be reworked.
public class PathFollower : MonoBehaviour
{
    [SerializeField]
    private PathDraw pathDrawer;

    public Hextile current; // Current hextile where the pathfollower is positioned
    public bool moving; // Should the follower be moving?

    private Vector3 currentFront; // Always facing toward positive z (forward)
    private int moveCounter; // Counter used for lerping positions
    private int nodeIndex; // Indexing for lerping
    private const float stopSize = 0.01f; // How fast the gameobject moves between tiles

    void Reset()
    {
        // TODO: when it is possible to reset the linrenderer, this function should be called after a move is completed
        moveCounter = 0;
        nodeIndex = 0;
        moving = false;
        currentFront = transform.forward;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
            moveBetween();
    }

    /// <summary>
    /// Turns the pathfollower toward next
    /// </summary>
    /// <param name="next"></param>
    private void turnToNext(Hextile next)
    {
        Vector3 moveVec = next.getPosition() - transform.position; // Vector pointing from current position to the tile
        float angle = Vector3.Angle(currentFront, moveVec);

        // Check for negative angle
        Vector3 cross = Vector3.Cross(currentFront, moveVec);
        if (cross.y < 0) angle = -angle;

        transform.eulerAngles = new Vector3(0, angle, 0);
    }

    /// <summary>
    /// Moves the PathFollower across the list of tiles
    /// </summary>
    public void moveBetween()
    {
        //Temporary solution. List must be reversed
        List<Hextile> moveNodes = new List<Hextile>(pathDrawer.tilesToDraw);
        moveNodes.Reverse();
        int size = moveNodes.Count;
        if (size > 0)
        {
            if(moveCounter == 0)
                turnToNext(moveNodes[nodeIndex]);

            Vector3 currentPos = transform.position; // Current position
            Vector3 nextPos = moveNodes[nodeIndex].getPosition(); // Move here
            transform.position = Vector3.Lerp(currentPos, nextPos, moveCounter * stopSize); // Find angle to rotate
            moveCounter++;

            // Check if the movement between 2 tiles is finished. If finished, move on to next tile.
            if (moveCounter * stopSize >= 1)
            {
                moveCounter = 0;
                if (nodeIndex < size - 1)
                {
                    nodeIndex++;
                    turnToNext(moveNodes[nodeIndex]);
                }
                // Else tile is reached
                else if (nodeIndex == size - 1)
                {
                    nodeIndex++;
                    transform.eulerAngles = transform.forward;
                }
            }
            // Check if end of list is reached
            if (nodeIndex == size)
            {
                moving = false;
                currentFront = transform.forward;
                //Debug.Log(currentFront);
                current = moveNodes[nodeIndex-1];
                pathDrawer.EmptyList();
            }
        }
    }
}
