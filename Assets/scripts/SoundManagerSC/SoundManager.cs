using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource bumpPiecies;

    void BumpPiecie()
    {
        bumpPiecies.Play();
    }
}
