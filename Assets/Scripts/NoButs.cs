using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class NoButs : MonoBehaviour
{
    string answer = "...but?";

    // Start is called before the first frame update
    void Start()
    {
        StraightAnswer();
    }

    void StraightAnswer()
    {
        answer = "Yes";
        //answer = "...umm?";
    }

}
