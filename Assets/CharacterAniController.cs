using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Testar att trigga dödsanimationen på hilda...
public class CharacterAniController : MonoBehaviour
{
    //public animationKey;
    public Animator anim; 

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("1"))
        {
            anim.Play("Die");
        }
        /*
        if(Input.GetKeyDown("1"))
        {
            anim.Play("Armature_H|Die_Hilda");
        }

        if (Input.GetKeyDown("2"))
        {
            anim.Play("Armature_H|Run_hilda");
        }

        if (Input.GetKeyDown("3"))
        {
            anim.Play("Armature_H|NarutoRun_Hilda");
        }
        */
    }
}
