using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource backgroundMusic;
    void Start()
    {
        backgroundMusic.Play();
    }
}
