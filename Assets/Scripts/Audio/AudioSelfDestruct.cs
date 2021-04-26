using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSelfDestruct : MonoBehaviour
{
    public AudioSource Source;

    private void Update()
    {
        if (!Source.isPlaying)
            Destroy(gameObject);
    }

}
