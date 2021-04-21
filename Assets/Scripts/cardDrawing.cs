using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardDrawing : MonoBehaviour
{
    [SerializeField] ParticleSystem ps;

    public void OnPickup()
    {
        ps.Play();
    }

    public void OnDropCharacter()
    {
        ps.Stop();

    }
}
