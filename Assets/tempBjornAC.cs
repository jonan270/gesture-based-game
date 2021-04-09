using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// temp chráracters controller till bjorn
public class tempBjornAC : MonoBehaviour
{
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("4"))
        {
            anim.Play("Die_Bjorn");
        }

        if (Input.GetKeyDown("5"))
        {
            anim.Play("Run_Bjorn");
        }

 
    }
}
