using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    public static CoroutineRunner Instance;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this; 
    }
}
