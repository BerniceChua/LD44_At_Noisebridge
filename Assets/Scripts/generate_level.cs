using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generate_level : MonoBehaviour 
{
    
    public level_specification LevelSpecification;
    
    public void GenerateLevel (Transform StaticParent)
    {
        float centerX = LevelSpecification.sizeX / 2f;
        float centerZ = LevelSpecification.sizeZ / 2f;

        int pStartX = 0, pStartY = 0;
        PlayerController pcon = GameObject.FindObjectOfType<PlayerController>();

        for (int i = 0; i < LevelSpecification.sizeX; i++)
        {
            for (int j = 0; j < LevelSpecification.sizeZ; j++)
            {

                //int to enum type conversion?
                //SpaceType typeOfSpace = LevelSpecification.data[i, j];
                RowData rowData = LevelSpecification.data.rows[i];
                int spacetype = rowData.row[j];
                GameObject spaceprefab = LevelSpecification.Prefab_Spaces[spacetype];
                if(spacetype == 3)
                {
                    pcon.posX = i;
                    pcon.posY = j;
                    pStartX = i;
                    pStartY = j;
                }
                GameObject space = (GameObject)Instantiate(spaceprefab, StaticParent);
                space.transform.localPosition = new Vector3((1f * i) - centerX, space.transform.localPosition.y, (1f * j)-centerZ);
            }      
        }
        pcon.MoveToSpace(pStartX, pStartY);
    }
    
    public void InstantiateSpawnerContainer (GameObject Prefab_Container, Transform StaticParent, Transform DynamicParent)
    {
        GameObject InstanceBackground = GameObject.Instantiate(Prefab_Container);
        if (StaticParent != null)
        {
            InstanceBackground.transform.SetParent(StaticParent);
            InstanceBackground.GetComponent<environment>().Construct();
        }
        InstanceBackground.transform.position = LevelSpecification.Background_RootPosition;
        
        Transform Player = null;
        //Transform Alien = null;
        
        ability_inventory PlayerInventory = null;
        
        //spawner[] Spawns = InstanceBackground.GetComponentsInChildren<spawner>();
       /* for (int SpawnIndex = 0; SpawnIndex < Spawns.Length; SpawnIndex++)
        {
            switch (Spawns[SpawnIndex].SpawnTarget)
            {
                case spawn_target.SpawnTarget_Alien:
                {
                    GameObject InstanceAlien = Spawns[SpawnIndex].Spawn(LevelSpecification.Prefab_Alien);
                    if (DynamicParent != null)
                    {
                        InstanceAlien.transform.SetParent(DynamicParent);
                    }
                    Alien = InstanceAlien.transform;
                    Alien.gameObject.GetComponent<character_movement>().Construct(AlienInventory, 
                                                                                  MainObjectMover, 1, 2);
                }break;
                
                case spawn_target.SpawnTarget_Robot:
                {
                    GameObject InstanceRobot = Spawns[SpawnIndex].Spawn(LevelSpecification.Prefab_Robot);
                    if (DynamicParent != null)
                    {
                        InstanceRobot.transform.SetParent(DynamicParent);
                    }
                    Robot = InstanceRobot.transform;
                    Robot.gameObject.GetComponent<character_movement>().Construct(RobotInventory, 
                                                                                  MainObjectMover, 0, 1);
                }break;
                
                case spawn_target.SpawnTarget_CatapultLeft:
                {
                    GameObject InstanceCatapult = Spawns[SpawnIndex].Spawn(LevelSpecification.Prefab_CatapultLeft);
                    if (DynamicParent != null)
                    {
                        InstanceCatapult.transform.SetParent(DynamicParent);
                    }
                    InstanceCatapult.GetComponent<catapult>().Construct(Robot, Alien);
                }break;
                
                case spawn_target.SpawnTarget_CatapultRight:
                {
                    GameObject InstanceCatapult = Spawns[SpawnIndex].Spawn(LevelSpecification.Prefab_CatapultRight);
                    if (DynamicParent != null)
                    {
                        InstanceCatapult.transform.SetParent(DynamicParent);
                    }
                    InstanceCatapult.GetComponent<catapult>().Construct(Robot, Alien);
                }break;
                
                case spawn_target.SpawnTarget_Inventory:
                {
                    GameObject InstanceInventory = Spawns[SpawnIndex].Spawn(LevelSpecification.Prefab_Inventory);
                    if (DynamicParent != null)
                    {
                        InstanceInventory.transform.SetParent(DynamicParent);
                    }
                    InstanceInventory.GetComponent<block_inventory>().Construct(DynamicParent);
                    if (Spawns[SpawnIndex].transform.position.x < 0)
                    {
                        RobotInventory = InstanceInventory.GetComponent<block_inventory>();
                    }
                    else
                    {
                        AlienInventory = InstanceInventory.GetComponent<block_inventory>();
                    }
                }break;
                
				case spawn_target.SpawnTarget_Fence:
				{
					GameObject InstanceFence = Spawns[SpawnIndex].Spawn(LevelSpecification.Prefab_Fence);
					if (StaticParent != null)
					{
						InstanceFence.transform.SetParent(StaticParent);
					}
				}break;
                
                default:
                {
                    Debug.LogWarning("Unused Spawner: " + Spawns[SpawnIndex].gameObject.name +
                                     " With Target: " + Spawns[SpawnIndex].SpawnTarget);
                }break;
            }
        }
        
        if (Alien && Robot)
        {
            if (RobotInventory) 
            {
                RobotInventory.RegisterPlayer(Alien);
                RobotInventory.RegisterPlayer(Robot);
            }
            if (AlienInventory)
            {
                AlienInventory.RegisterPlayer(Alien);
                AlienInventory.RegisterPlayer(Robot);
            }
        }*/
    }
}