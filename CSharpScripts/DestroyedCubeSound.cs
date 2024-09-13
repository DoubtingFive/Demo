using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedCubeSound : MonoBehaviour
{
    AudioSource explodeSound;
    private void Awake()
    {
        explodeSound = GetComponent<AudioSource>();
        explodeSound.pitch = Random.Range(0.5f,1.5f);
        explodeSound.Play();
    }
}
