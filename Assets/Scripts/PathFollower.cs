using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
// This is just a simple solution that should be reworked.
public class PathFollower : MonoBehaviour
{
    

    private int index = 0; //index of the current node

    private bool moving = false; // Should the follower be moving?
    /// <summary>
    ///  How fast the gameobject moves between tiles
    /// </summary>
    [SerializeField]
    private float speed = 0.5f;
    private float startTime;
    private float journeyLength;
    /// <summary>
    /// The path to follow
    /// </summary>
    private List<Hextile> path;
    private Vector3 pathTarget;
    private Vector3 startTarget;

    /// <summary>
    /// Drawer object in scene
    /// </summary>
    private PathDraw pathDrawer;
    /// <summary>
    /// reference to attached character script
    /// </summary>
    private Character character;

    private void Start() {
        character = GetComponent<Character>();
        pathDrawer = FindObjectOfType<PathDraw>();
    }
    /// <summary>
    /// Begin movement of the character
    /// </summary>
    /// <param name="points">Path to follow</param>
    public void StartMoving(List<Hextile> points) {
        if(points.Count > 0) {
            path = points;
            moving = true;
            GetNextPoint();
        }
    }
    
    void Update()
    {
        if (moving)
            MoveBetweenPoints();
    }
    /// <summary>
    /// Moves the character between two points on the path
    /// </summary>
    private void MoveBetweenPoints() {
        transform.LookAt(pathTarget);
        float distanceCovered = (Time.time - startTime) * speed;
        float fraction = distanceCovered / journeyLength;
        transform.position = Vector3.Lerp(startTarget, pathTarget, fraction);

        //If we are close enough to the target move towards next
        if(Vector3.Distance(transform.position, pathTarget) <= 0.01)
        {
            // Check for traps and effects
            if (character.CurrentTile.areaEffect.isActivated)
            {
                character.ModifyHealth(character.CurrentTile.areaEffect.ApplyEffect(character));
                var index = character.CurrentTile.tileIndex;
                FindObjectOfType<Hexmap>().ChangeEffect(index.x, index.y, false);
            }
            GetNextPoint();
        }
    }
    /// <summary>
    /// Get the next point on the path
    /// </summary>
    private void GetNextPoint() {
        //bool available = index + 1 < path.Count && !path[index + 1].isOccupied; // Tile is not occupied and end is not reached
        //bool stop = index == path.Count || path[index].isOccupied;
        //Debug.Log("Stop? " + stop);

        // If we are at the end of the path stop movement
        if (index == path.Count)
        {
            ReachedEnd();
        }

        // If we encounter an enemy along the path, deal damage and stop
        else if (path[index].isOccupied)
        {
            // Do damage.
            Debug.Log("KARATE");
            ReachedEnd();
        }

        // Else move
        else
        { 
            startTarget = transform.position;
            pathTarget = path[index].Position;

            journeyLength = Vector3.Distance(startTarget, pathTarget);

            startTime = Time.time;
            character.CurrentTile.RemoveOccupant(); // Old tile is no longer occupied
            character.CurrentTile = path[index];
            character.CurrentTile.SetOccupant(character); // New tile is occupied

            index++;
        }
    }
    /// <summary>
    /// Raised when the character has reached end of its path
    /// </summary>
    private void ReachedEnd()
    {
        moving = false;
        index = 0;
        transform.eulerAngles = PhotonNetwork.IsMasterClient ? Vector3.zero : new Vector3(0, 180, 0);
        pathDrawer.ClearPath();
        PathCreator.isBusy = false; //Lets another character create its path
    }
}
