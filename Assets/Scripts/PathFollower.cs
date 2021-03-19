using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                character.CurrentTile.RemoveEffect();
            }
                
            GetNextPoint();
        }
    }
    /// <summary>
    /// Get the next point on the path
    /// </summary>
    private void GetNextPoint() {

        //If we are not at the end 
        if (index < path.Count) {
            startTarget = transform.position;
            pathTarget = path[index].Position;

            journeyLength = Vector3.Distance(startTarget, pathTarget);

            startTime = Time.time;
            character.CurrentTile = path[index];
            index++;
        }
        else { //The end of the path has been reached
            ReachedEnd();
        }
    }
    /// <summary>
    /// Raised when the character has reached end of its path
    /// </summary>
    private void ReachedEnd()
    {
        moving = false;
        index = 0;
        transform.eulerAngles = transform.forward;
        pathDrawer.ClearPath();
        PathCreator.isBusy = false; //Lets another character create its path
    }
}
