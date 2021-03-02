using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        var CharacterOne = ScriptableObject.CreateInstance<Bjorn>();
        Debug.Log(CharacterOne.Health);

        var CharacterTwo = ScriptableObject.CreateInstance<Hilda>();
        Debug.Log(CharacterTwo.Health);

        Debug.Log(CharacterOne.card.description);

        CharacterOne.Attack("Berserk", CharacterTwo);
        Debug.Log("Health after attack: " + CharacterTwo.Health);

        // Add assets to the CharacterOne/CharacterTwo

        //GameObject characterModel = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //characterModel.transform.position = new Vector3(0, 0, 0);


    }

    // Update is called once per frame
    void Update()
    {
        
    }




}
