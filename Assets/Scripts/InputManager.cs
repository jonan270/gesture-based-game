using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputMaster controls;
    //private PathDraw lineRenderer;

    [SerializeField]
    private Hexmap map;
    [SerializeField]
    private PathFollower follower;
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
        controls.Player.DrawPath.performed += ctx => map.drawDirection(ctx.ReadValue<Vector2>());
        controls.Player.EndTurn.performed += ctx => endTurn();
    }

    void endTurn()
    {
        //Debug.Log("Ended.");
        //follower.moveBetween();
        follower.moving = true;
    }
}
