using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PathCreator : MonoBehaviour
{
    /// <summary>
    /// Drawer object in scene
    /// </summary>
    [SerializeField] private PathDraw pathdrawer;
    
    [SerializeField]
    private float offsetY = 0.5f;
    private List<Hextile> tiles;


    public UnityEvent actionTaken;
    public UnityEvent FinishCreatingPath;

    private bool isBusy = false;


    private void Start() 
    {
        tiles = new List<Hextile>();
    }

    /// <summary>
    /// Call when player has finished drawing
    /// </summary>
    /// <param name="selectedCharacter"></param>
    public void FinishPath(GameObject selectedCharacter) {
        FinishCreatingPath.Invoke();
        selectedCharacter.GetComponent<PathFollower>().StartMoving(new List<Hextile>(tiles));
        tiles.Clear();
        isBusy = true;
        PlayerManager.Instance.OnPlayerStateChanged(PlayerState.characterWalking);
    }

    /// <summary>
    /// Adds a new tile to the list
    /// </summary>
    /// <param name="h"></param>
    public void AddTile(Hextile h)
    {
        if (isBusy || h == null)
            return;
        
        tiles.Add(h);
        //pathdrawer.DrawPath(CreatePointsFromTiles());        
    }

    /// <summary>
    /// Skapa en lista som PathDraw kan rita ut punkterna
    /// </summary>
    /// <returns></returns>
    private Vector3[] CreatePointsFromTiles() {
        List<Vector3> points = new List<Vector3>();
        Vector3 offset = new Vector3(0,offsetY,0);
        foreach(Hextile tile in tiles) {
            points.Add(tile.Position + offset);
        }
        return points.ToArray();
    }
    /// <summary>
    /// Allows the player to draw a new path for another character
    /// </summary>
    /// <param name="obj"></param>
    public void OnReachedEnd(GameObject obj)
    {
        Debug.Log(obj.name + " recieved path complete");
        isBusy = false;
        //pathdrawer.ClearPath();
        actionTaken.Invoke();
    }

    // //Skicka listan till en pathFollower som sedan vandrar iväg
    // private List<Hextile> CreatePathForFollower() {
    //      List<Hextile> points = new List<Hextile>();
    //     // foreach(Hextile tile in tiles) {
    //     //     points.Add(tile.getPosition());
    //     // }
    //     tiles.Clear(); //töm listan på tiles
    //     //pathdrawer.ClearPath(); //rita ingenting
    //     //return result to follower
    //     return tiles;
    // } 
}
