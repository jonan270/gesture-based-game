using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{

    public Character bjorn;

    // Start is called before the first frame update
    void Start()
    {
        var CharacterOne = ScriptableObject.CreateInstance<Bjorn>();
        Debug.Log(CharacterOne.Health);

        var CharacterTwo = ScriptableObject.CreateInstance<Hilda>();
        Debug.Log(CharacterTwo.Health);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
