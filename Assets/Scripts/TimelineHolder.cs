using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStructure;

public class TimelineHolder : MonoBehaviour
{
    public LDTimeLine TimeLine;
    public static TimelineHolder Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        TimeLine = Server.RequestTimeLine();
    }
}
