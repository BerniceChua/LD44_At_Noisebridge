using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName="Level Specification")]
public class level_specification : ScriptableObject {
    
    public GameObject Prefab_Space;
    public GameObject Prefab_Hazard;
    public GameObject Prefab_Wall;

    public GameObject[] Prefab_Spaces;
    
    public GameObject Prefab_ExitSpace;

    public int sizeX, sizeZ;

    public SpaceType[] Spaces;

    public GameObject Prefab_AbilityInventory;
        
    public Vector3 Background_RootPosition;

    public ArrayLayout data;
}

public enum SpaceType
{
    SpaceType_NonSpace,
    SpaceType_Basic,
    SpaceType_Hazard,
    SpaceType_Start,
    SpaceType_Exit,
    SpaceType_WallLeft,
    SpaceType_WallRight,
    SpaceType_WallTop,
    SpaceType_WallBottom,
}
