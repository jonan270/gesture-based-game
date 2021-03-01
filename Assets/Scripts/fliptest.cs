using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class fliptest : MonoBehaviour
{
    public InputMaster controls;
    int spinVel = 30;

    void Awake()
    {
        controls = new InputMaster();
        controls.Player.Tilespin.performed += ctx => Spin();
    }

    void Spin()
    {
        Debug.Log("Spin tile spin!!");
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        //controls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.A)) {
        //    transform.Rotate(Vector3.up * spinVel * Time.deltaTime);
        //}

    }
}
