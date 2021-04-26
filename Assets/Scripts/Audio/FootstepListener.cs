using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepListener : MonoBehaviour
{
    public void OnFootstep()
    {
        Debug.Log("Footstep");
        AudioSystem.Play("Footstep");
    }
}
