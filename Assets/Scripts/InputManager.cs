using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputMaster controls;
    //private PathDraw lineRenderer;

    [SerializeField]
    private Hexmap map;
    // [SerializeField]
    // private PathFollower follower;
    [SerializeField]
    private PathCreator creator;
    //[SerializeField]
    //private PathDraw path;

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Awake()
    {
        controls = new InputMaster();
        controls.Player.Spacebutton.performed += ctx => map.randomizeHexmap(1000, 3);
        //controls.Player.DrawPath.performed += ctx => map.drawDirection(ctx.ReadValue<Vector2>());
        controls.Player.DrawPath.performed += ctx => addShit(SelectCharacter(1));
        controls.Player.EndTurn.performed += ctx => SpawnTrap();
        controls.Player.Select1.performed += ctx => addShit(SelectCharacter(1));
        controls.Player.Select2.performed += ctx => addOtherShit(SelectCharacter(1));

    }

    private void addShit(string name) 
    {
        creator.AddTile(map.hexTiles[0,0]);
        creator.AddTile(map.hexTiles[1,1]);
        creator.AddTile(map.hexTiles[2,2]);
        creator.AddTile(map.hexTiles[3,4]);
        creator.AddTile(map.hexTiles[3,5]);
        creator.AddTile(map.hexTiles[4,4]);

        creator.FinishPath(GameObject.Find(name));

        // creator.AddTile(map.hexTiles[0,4]);
        // creator.AddTile(map.hexTiles[1,4]);
        // creator.AddTile(map.hexTiles[2,4]);
        // creator.AddTile(map.hexTiles[3,4]);
        // creator.AddTile(map.hexTiles[4,5]);
        // creator.AddTile(map.hexTiles[5,7]);

        // creator.FinishPath(GameObject.Find("Bjorn(Clone)"));
    }

    private void addOtherShit(string name) 
    {
        creator.AddTile(map.hexTiles[5,0]);
        creator.AddTile(map.hexTiles[6,1]);
        creator.AddTile(map.hexTiles[7,2]);
        creator.AddTile(map.hexTiles[8,3]);
        creator.AddTile(map.hexTiles[9,4]);
        creator.AddTile(map.hexTiles[10,5]);

        creator.FinishPath(GameObject.Find(name));
    }

    private void SpawnTrap()
    {
        //map.hexTiles[2, 2].AddEffect(ElementState.Fire, -50);
        map.ChangeEffect(2,2, true, ElementState.Fire, -50);
        Debug.Log("Trap spawned: " + map.hexTiles[2, 2].areaEffect.TrapElement + " with damage " + map.hexTiles[2, 2].areaEffect.healthModifier);
    }

    private string SelectCharacter(int num) 
    {
        switch(num)
        {
            case 1:
                return "Hilda 1(Clone)";
            case 2:
                return "Bjorn(Clone)";
            default:
                return "Hilda 1(Clone)";
        }
    }
}
