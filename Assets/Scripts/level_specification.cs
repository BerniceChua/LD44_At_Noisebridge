using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName="Level Specification")]
public class level_specification : ScriptableObject {
    
    public GameObject Prefab_Space;
    public GameObject Prefab_Hazard;
    public GameObject Prefab_Wall;

    public GameObject[] Prefab_Spaces;
    public GameObject[] Prefab_Pickups;
    
    public GameObject Prefab_ExitSpace;

    public int sizeX, sizeZ;

    public SpaceType[] Spaces;

    public GameObject Prefab_AbilityInventory;
        
    public Vector3 Background_RootPosition;

    public ArrayLayout data;

    public ArrayLayout pickups;
}

