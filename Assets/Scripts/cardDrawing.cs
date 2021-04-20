using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardDrawing : MonoBehaviour
{
    //GameObject ob;
    // Update is called once per frame
    void Start()
    {
        

    }
    void Update()
    {

        if (PlayerManager.Instance.selectedCharacter == null)
        {
            //ob.GetComponent<HandCards>().setTextHand(false);
            this.gameObject.GetComponent<ParticleSystem>().enableEmission = false;
            
        }
         else if (PlayerManager.Instance.selectedCharacter != null) {

            //ob.GetComponent<HandCards>().setTextHand(true);
            this.gameObject.GetComponent<ParticleSystem>().enableEmission = true;
                
        }
    }
}
