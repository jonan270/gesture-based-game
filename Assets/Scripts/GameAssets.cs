
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lets you drag references to this scripts prefab and acces it anywhere in code

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;

    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _i;
        }
    }

    public Sprite blood_red_bar;
    // Start is called before the first frame update

}

