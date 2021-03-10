using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Raycasthandler : MonoBehaviour
{
    public InputMaster controls;
    public Vector2 controllerPosition;

    //TODO: Shoould use UIElement interaction system from steamVR?
    Ray ray;
    RaycastHit hit;

    // Awake activates controls and listens for actions listed in InputMaster
    void Awake()
    {
        controls = new InputMaster();
        //controls.Player.Tilespin.performed += ctx => SetSpin();
    }

    public RaycastHit getRayHit()
    {
        controllerPosition = controls.Player.Rayposition.ReadValue<Vector2>();
        ray = Camera.main.ScreenPointToRay(controllerPosition);
        if (Physics.Raycast(ray, out hit, 10000.0f))
        {
            return hit;
        }
        else
            return hit;
    }

    // Enable and disable controls when necessary
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void PrintName(GameObject go)
    {
        Debug.Log(go.name);
    }
}

