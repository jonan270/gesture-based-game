using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hilda : Character
{
    public Hilda()
    {
        Element = "earth";
        Health = 100;
        isAlive = true;
        Name = "Hilda";
        attackValue = 15;

    }
    
    public void compareElement(string enemyElement)
    {
        if (enemyElement == "fire")
        {
            //
        }
    }

}
