using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public enum SpaceType
{
    NonSpace, // 0
    Basic,  // 1
    Hazard, // 2
    Start,  // 3
    Exit,   // 4
    WallLeft, // 5
    WallRight,  // 6
    WallTop,    // 7
    WallBottom, // 8
    WallTopLeft, // 9
    WallTopRight,
    WallBottomLeft,
    WallBottomRight // 12
}
