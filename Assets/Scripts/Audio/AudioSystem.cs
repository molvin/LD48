using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioSystem
{
    private static Dictionary<string, AudioObject> audioObjects = new Dictionary<string, AudioObject>();
    private static GameObject AudioSourcePrefab;

    public static void Play(string name, Vector3 position = default, float overwriteVolume = -1)
    {
        if(name == null || name == "")
            return;
        if(!audioObjects.ContainsKey(name))
        {
            var obj = Resources.Load($"Audio/{name}") as AudioObject;
            if(obj == null)
            {
                Debug.LogWarning("Trying to play audio object that does not exsist");
                return;
            }
            audioObjects.Add(name, obj);
        }
        if(AudioSourcePrefab == null)
        {
            var obj = Resources.Load<GameObject>("Audio/AudioSource");
            if(obj == null)
            {
                Debug.LogWarning("Failed to load audio source prefab at Resources/Audio/AudioSource");
                return;
            }
            AudioSourcePrefab = obj;
        }

        AudioObject audioObject = audioObjects[name];
        GameObject holder = Object.Instantiate(AudioSourcePrefab, position, Quaternion.identity);
        AudioSource source = holder.GetComponent<AudioSource>();

        source.clip = audioObject.GetClip();
        source.outputAudioMixerGroup = audioObject.MixerGroup;
        source.volume = overwriteVolume == -1 ? Random.Range(audioObject.MinVolume, audioObject.MaxVolume) : overwriteVolume;
        source.pitch = Random.Range(audioObject.MinPitch, audioObject.MaxPitch);
        source.spatialBlend = audioObject.SpatialBlend;
        source.loop = audioObject.Loop;

        source.Play();
        Debug.Log("Playing");
    }

}
