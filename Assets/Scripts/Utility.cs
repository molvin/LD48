using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    private readonly static float Divisor = 0.713f;
    private readonly static Vector3 XUnitVector = new Vector3(1f, 0, -Divisor);
    private readonly static Vector3 YUnitVector = new Vector3(-1f, 0, -Divisor);
    public static Vector3 IsoToWorld(Vector2 IsoPoint)
    {
        return (XUnitVector / 2f) * IsoPoint.x + (YUnitVector/2f) * IsoPoint.y;
    }
    public static Vector2 WorldToIso(Vector3 WorldPoint)
    {
        WorldPoint.y = 0;
        return new Vector2(Vector3.Dot(WorldPoint, XUnitVector) / Divisor, Vector3.Dot(WorldPoint, YUnitVector) / Divisor);
    }
}
