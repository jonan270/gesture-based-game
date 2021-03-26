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

    private bool isBusy = false;


    private void Start() 
    {
        tiles = new List<Hextile>();
    }

    //Man har ritat färdigt 
    public void FinishPath(GameObject selectedCharacter) {
        selectedCharacter.GetComponent<PathFollower>().StartMoving(new List<Hextile>(tiles));
        tiles.Clear();
        isBusy = true;

    }

    public void AddTile(Hextile h)
    {
        if (isBusy || h == null)
            return;
        
        tiles.Add(h);
        pathdrawer.DrawPath(CreatePointsFromTiles());        
    }

    //Skapa en lista som PathDraw kan rita ut punkterna
    private Vector3[] CreatePointsFromTiles() {
        List<Vector3> points = new List<Vector3>();
        Vector3 offset = new Vector3(0,offsetY,0);
        foreach(Hextile tile in tiles) {
            points.Add(tile.Position + offset);
        }
        return points.ToArray();
    }

    public void OnReachedEnd(GameObject obj)
    {
        isBusy = false; //Lets another character create its path
        obj.GetComponent<Character>().CurrentState = Character.CharacterState.ActionCompleted; //TODO change to a function call instead 
        pathdrawer.ClearPath();
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
