using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName="Level Specification")]
public class level_specification : ScriptableObject {
    
    public GameObject Prefab_Alien;
    public GameObject Prefab_Robot;
    
    public GameObject[] Prefab_SpawnerContainers;
    
    public GameObject Prefab_CatapultLeft;
    public GameObject Prefab_CatapultRight;
    public GameObject Prefab_Inventory;
    
	public GameObject Prefab_Fence;
    
    public GameObject Prefab_BlockBorder;
    
    public Vector3 Background_RootPosition;
}
