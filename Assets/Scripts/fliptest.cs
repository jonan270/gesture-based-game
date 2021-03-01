using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fliptest : MonoBehaviour
{
    int spinVel = 30;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.up * spinVel * Time.deltaTime);
        }
    }
}
