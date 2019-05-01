using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (menuName="Level Specification")]
public class level_specification : ScriptableObject {
    
    public GameObject Prefab_Space;
    public GameObject Prefab_Hazard;
    public GameObject Prefab_Wall;

    public GameObject[] Prefab_Spaces;
    public GameObject[] Prefab_Pickups;
    
    public GameObject Prefab_ExitSpace;

    public GameObject LevelLayoutPrefab;

    public int sizeX, sizeZ;

    public int grapeHealAmount;
    public int wineHealAmount;
    public int backtrackDamage;
    public int hazardDamage;
    public int walkDamage;
    public int dashDamage;
    public int diagDamage;
    public int playerStartWine;
    public int playerMaxWine;

    public SpaceType[] Spaces;

    public string[] available_abilities;

    public GameObject Prefab_AbilityInventory;
        
    public Vector3 Background_RootPosition;

    public ArrayLayout data;

    public ArrayLayout pickups;
}

