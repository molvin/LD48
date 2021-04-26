using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu]
public class AudioObject : ScriptableObject
{
    public AudioClip[] Clips;
    public AudioMixerGroup MixerGroup;
    public float MinVolume = 1;
    public float MaxVolume = 1;
    public float MinPitch = 1;
    public float MaxPitch = 1;
    public bool Loop = false;
    public float SpatialBlend = 0;

    public AudioClip GetClip()
    {
        return Clips.Length == 0 ? null : Clips[Random.Range(0, Clips.Length)];
    }
}
