using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    GameObject character;
    GameObject charactertwo;
    // Start is called before the first frame update
    void Start()
    {

        character = new GameObject("Bjorn");

        var ch = character.AddComponent<Bjorn>();

        Bjorn b = ch.GetComponent<Bjorn>();

        Debug.Log(b.Health);

        //Debug.Log(charactertwo.Health);

        Debug.Log(b.card.description);

        //character.Attack("Berserk", charactertwo);
       // Debug.Log("Health after attack: " + charactertwo.Health);
        
        // Add assets to the CharacterOne/CharacterTwo

        //GameObject characterModel = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //characterModel.transform.position = new Vector3(0, 0, 0);


    }

    // Update is called once per frame
    void Update()
    {
        
    }




}
