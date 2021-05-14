using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSFX : MonoBehaviour
{
    public static ManagerSFX Instance { get; private set; }

    public AudioSource selectFX;

    public AudioSource pickupFX;
    public AudioSource dropFX;

    public AudioSource correctFX;
    public AudioSource errorFX;

    public AudioSource magicFX;

    public AudioSource runningFX;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySelectFX()
    {
        selectFX.Play();
    }

    public void PlayMagicFX()
    {
        magicFX.Play();
    }

    public void GestureStatus(bool correct)
    {
        if (correct)
            correctFX.Play();
        else
            errorFX.Play();
    }

    public void Run(bool run)
    {
        if (run)
            runningFX.Play();
        else
            runningFX.Stop();
    }

    public void PlayGrabFX(bool pickup)
    {
        if (pickup)
            pickupFX.Play();
        else
            dropFX.Play();
    }
}
