using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hilda : Character
{
    public Hilda()
    {
        Element = "earth";
        Health = 90;
        isAlive = true;
        Name = "Hilda";
        attackValue = 15;
    }
    
    public void compareElement(string enemyElement)
    {
        if (enemyElement == "fire")
        {
            attackValue -= 5;
        }
    }

    public void OnEnable()
    {
        card = ScriptableObject.CreateInstance<Card>();
        card.nameCharacter = "Hilda";
        card.description = "Bj�rn will go berserk and do ... extra damage" + "\n" + "Bj�rn will drink mead and ..." + "Bj�rn will do cleave and hurt multiple enemies.";


    }

}
