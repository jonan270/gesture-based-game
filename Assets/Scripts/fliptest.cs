using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class fliptest : MonoBehaviour
{
    public InputMaster controls;
    public Vector2 position;

    bool spin = false;
    int angleCount = 0;

    //TODO: Shoould use UIElement interaction system from steamVR?
    Ray ray;
    RaycastHit hit;

    void Awake()
    {
        controls = new InputMaster();
        controls.Player.Tilespin.performed += ctx => SetSpin();
    }

    void SetSpin()
    {
        position = controls.Player.Rayposition.ReadValue<Vector2>();
        ray = Camera.main.ScreenPointToRay(position);


        if (Physics.Raycast(ray, out hit, 10000.0f))
        {
            //Debug.Log(hit.distance);
            //hit.transform.gameObject
        }

        //Debug.Log(position.x);
        if(Physics.Raycast(ray, out hit, 10000.0f))
        {
            spin = true;
        }
    }

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

        //Debug.Log(ray);


        if(spin)
        {
            angleCount++;
            hit.transform.localEulerAngles = new Vector3(-angleCount, 0, 0);
            if(angleCount == 360)
            {
                spin = false;
                angleCount = 0;
            }
        }

    }

    private void PrintName(GameObject go)
    {
        Debug.Log(go.name);
    }
}
