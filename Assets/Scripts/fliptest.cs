using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class fliptest : MonoBehaviour
{
    public InputMaster controls;
    bool spin = false;
    int angleCount = 0;

    void Awake()
    {
        controls = new InputMaster();
        controls.Player.Tilespin.performed += ctx => SetSpin();
    }

    void SetSpin()
    {
        spin = true;
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
        if(spin)
        {
            angleCount++;
            transform.localEulerAngles = new Vector3(-angleCount, 0, 0);
            if(angleCount == 90)
            {
                spin = false;
                angleCount = 0;
            }
        }

    }
}
