using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardDrawing : MonoBehaviour
{
    //GameObject ob;
    // Update is called once per frame
    ParticleSystem ps;
    void Start()
    {
        //ob = GameObject.Find("HandCards(Clone)");
        ps = GetComponent<ParticleSystem>();
    }
    void Update()
    {

        //if (PlayerManager.Instance.selectedCharacter == null)
        //{
        //    //ob.GetComponent<HandCards>().setTextHand(false);
        //    ps.Stop();
            
        //}
        // else if (PlayerManager.Instance.selectedCharacter != null) {

        //    //ob.GetComponent<HandCards>().setTextHand(true);
        //    ps.Play();
                
        //}
    }

    public void OnPickup()
    {
        ps.Play();
    }

    public void OnDropCharacter()
    {
        ps.Stop();

    }
}
