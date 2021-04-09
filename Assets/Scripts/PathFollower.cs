using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

[System.Serializable]
public class UnityGameObjectEvent : UnityEvent<GameObject>
{

} 

// This is just a simple solution that should be reworked.
public class PathFollower : MonoBehaviour
{
    /// <summary>
    /// Event when character has reached the end of its path
    /// </summary>
    public UnityGameObjectEvent movingComplete;

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
    private Hexmap map;
    //private AbilityManager abilities;

    /// <summary>
    /// reference to attached character script
    /// </summary>
    private Character character;

    private void Start() {
        character = GetComponent<Character>();
        map = FindObjectOfType<Hexmap>();
        //abilities = FindObjectOfType<AbilityManager>();

        //Adds the pathCreator as a listener to this event
        movingComplete.AddListener(FindObjectOfType<PathCreator>().OnReachedEnd);

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
                character.ModifyHealth(character.CurrentTile.areaEffect.healthModifier);
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

        // If we are at the end of the path stop movement
        if (index == path.Count)
        {
            ReachedEnd();
        }

        // If we encounter an enemy along the path, deal damage and stop
        else if (path[index].isOccupied && !path[index].occupant) // If occupant is null it exists on the other players side
        {
            Character target = PlayerManager.Instance.GetCharacterAt(path[index].tileIndex.x, path[index].tileIndex.y);
            AbilityManager.ManagerInstance.DamageCharacter(target, character.CalculateAutoAttack(target));
            ReachedEnd();
        }

        // Else move
        else
        {
            //Debug.Log("Is it occupied? " + path[index].isOccupied);
            startTarget = transform.position;
            pathTarget = path[index].Position;

            journeyLength = Vector3.Distance(startTarget, pathTarget);

            startTime = Time.time;

            map.SetOccupation(character.CurrentTile.tileIndex.x, character.CurrentTile.tileIndex.y, false, character); // Old tile is no longer occupied
            character.CurrentTile = path[index];
            map.SetOccupation(character.CurrentTile.tileIndex.x, character.CurrentTile.tileIndex.y, true, character); // New tile is occupied

            index++;
        }
    }

    /// <summary>
    /// Raised when the character has reached end of its path
    /// </summary>
    private void ReachedEnd()
    {
        Debug.Log(gameObject.name + " reached end of its path");
        moving = false;
        index = 0;
        transform.eulerAngles = PhotonNetwork.IsMasterClient ? Vector3.zero : new Vector3(0, 180, 0);
        movingComplete.Invoke(gameObject);
    }
}
