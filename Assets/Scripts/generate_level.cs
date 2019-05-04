using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class generate_level : MonoBehaviour 
{
    
    public level_specification LevelSpecification;
    public List<GameObject> allAbilityButtons;

    public GameObject[] levelSpecSpacePrefabs;
    public GameObject[] levelSpecPickupPrefabs;

    public void GenerateLevel (Transform StaticParent, level_specification LevelSpecification)
    {
        float centerX = LevelSpecification.sizeX / 2f;
        float centerZ = LevelSpecification.sizeZ / 2f;

        //fill in the prefab arrays for the level spec info
        //this allows us to set the prefab arrays IN ONE PLACE, ONCE, and create new level specs quickly
        int pfIndex = 0;
        LevelSpecification.Prefab_Spaces = new GameObject[levelSpecSpacePrefabs.Length];
        foreach(GameObject spacePF in levelSpecSpacePrefabs)
        {
            LevelSpecification.Prefab_Spaces[pfIndex] = levelSpecSpacePrefabs[pfIndex];
            pfIndex += 1;
        }
        pfIndex = 0;
        LevelSpecification.Prefab_Pickups = new GameObject[levelSpecPickupPrefabs.Length];
        foreach (GameObject spacePF in levelSpecPickupPrefabs)
        {
            LevelSpecification.Prefab_Pickups[pfIndex] = levelSpecPickupPrefabs[pfIndex];
            pfIndex += 1;
        }

        int pStartX = 0, pStartY = 0;
        PlayerController pcon = GameObject.FindObjectOfType<GameLoopManager>().m_playerController;
        pcon.levelObject = StaticParent;
        pcon.levelSize = LevelSpecification.sizeX;
        GameObject.FindObjectOfType<LevelViewController>().levelTransform = StaticParent;
        CardControl cardControl = GameObject.FindObjectOfType<CardControl>();
        cardControl.levelObject = StaticParent;
        cardControl.player = pcon;
        cardControl.levelSize = LevelSpecification.sizeX;
        //populate available ability buttons for the level

        foreach(Button b in cardControl.m_button)
        {
            b.gameObject.SetActive(false);
        }
        cardControl.m_button = new Button[LevelSpecification.available_abilities.Length];
        string buttonName = "";
        for(int i = 0; i < cardControl.m_button.Length; i++)
        {
            buttonName = LevelSpecification.available_abilities[i];
            GameObject butt = allAbilityButtons.Find(x => x.name == buttonName);
            Debug.Log(butt);
            cardControl.m_button[i] = butt.GetComponent<Button>();
            cardControl.m_button[i].gameObject.SetActive(true);
        }
        cardControl.SyncButtons();

        pcon.grapeHealAmount = LevelSpecification.grapeHealAmount;
        pcon.wineHealAmount = LevelSpecification.wineHealAmount;
        pcon.backtrackDamage = LevelSpecification.backtrackDamage;
        pcon.hazardDamage = LevelSpecification.hazardDamage;
        pcon.walkDamage = LevelSpecification.walkDamage;
        pcon.playerStartWine = LevelSpecification.playerStartWine;
        pcon.playerMaxWine = LevelSpecification.playerMaxWine;
        pcon.wine = pcon.playerStartWine;

        CardControl abilityController = GameObject.FindObjectOfType<CardControl>();
        abilityController.dashLoss = LevelSpecification.dashDamage;
        abilityController.diagLoss = LevelSpecification.diagDamage;

        GameObject layoutObj = LevelSpecification.LevelLayoutPrefab;
        GameObject[,] spacesArray = CreateSpacesArray(layoutObj.transform.Find("LevelSpaces"), 
                                                        LevelSpecification.sizeX, 
                                                        LevelSpecification.sizeZ);
        GameObject[,] pickupsArray = CreatePickupsArray(layoutObj.transform.Find("LevelPickups"),
                                                        LevelSpecification.sizeX,
                                                        LevelSpecification.sizeZ);
        for (int i = 0; i < LevelSpecification.sizeX; i++)
        {
            for (int j = 0; j < LevelSpecification.sizeZ; j++)
            {

                //Instantiate Spaces

                //int to enum type conversion?
                //SpaceType typeOfSpace = LevelSpecification.data[i, j];
                RowData rowData = LevelSpecification.data.rows[i];
                int spacetype = (int)(rowData.row[j]);
                //GameObject spaceprefab = LevelSpecification.Prefab_Spaces[spacetype];
                GameObject space = null;
                if (spacesArray[i, j] != null)
                {
                    GameObject spaceprefab = spacesArray[i, j];
                    //if(spacetype == 3)
                    if (spaceprefab.name.Contains("ares"))
                    {
                        pcon.posX = i;
                        pcon.posY = j;
                        pStartX = i;
                        pStartY = j;
                    }
                    space = (GameObject)Instantiate(spaceprefab, StaticParent);
                    space.transform.localPosition = new Vector3((1f * i) - centerX, space.transform.localPosition.y, (1f * j) - centerZ);
                    //if its a moveable space, give it xy coords
                    if (space.GetComponent<MoveSpace>() != null)
                    {
                        space.GetComponent<MoveSpace>().posX = i;
                        space.GetComponent<MoveSpace>().posY = j;
                        space.GetComponent<MoveSpace>().moveSpaceType = (SpaceType)spacetype;
                    }
                }

                //Instantiate Pickups

                RowData pickupsData = LevelSpecification.pickups.rows[i];
                int pickuptype = pickupsData.row[j];

                //0 type is  "no pickup"
                if (pickupsArray[i,j] != null)
                {
                    //GameObject pickupPrefab = LevelSpecification.Prefab_Pickups[pickuptype-1];
                    GameObject pickupPrefab = pickupsArray[i, j];
                    GameObject pickup = (GameObject)Instantiate(pickupPrefab, space.transform);
                    pickup.transform.localPosition = new Vector3(0f, 1.1f, 0f);
                    if (!pickup.name.Contains("dionysus"))
                    {
                        pickup.transform.localScale = new Vector3(.5f, .5f / pickup.transform.parent.localScale.y, .5f);
                    }
                    else
                    {
                        pickup.transform.localScale = new Vector3(20f, 20f / pickup.transform.parent.localScale.y, 20f);
                    }
                    space.GetComponent<MoveSpace>().hasPickup = true;
                }
                if (pickuptype == 5)
                {
                    pStartX = space.GetComponent<MoveSpace>().posX;
                    pStartY = space.GetComponent<MoveSpace>().posY;
                }
            }
        }
        //pcon.levelObject = GameObject.Find("LevelObject").transform;
        pcon.MoveToSpace(pStartX, pStartY);
        pcon.ResetExitStatus();
        pcon.spacesArray = spacesArray;
        pcon.pickupsArray = pickupsArray;
    }
    
    GameObject[,] CreateSpacesArray(Transform levelTF, int sizeX, int sizeZ)
    {
        GameObject[,] retArray = new GameObject[sizeX, sizeZ];
        //entire array is null to start
        foreach(Transform spaceTF in levelTF)
        {
            int x = (int)spaceTF.localPosition.x;
            int z = (int)spaceTF.localPosition.z;
            retArray[x, z] = spaceTF.gameObject;
        }
        return retArray;
    }

    GameObject[,] CreatePickupsArray(Transform pickupsTF, int sizeX, int sizeZ)
    {
        GameObject[,] retArray = new GameObject[sizeX, sizeZ];
        //entire array is null to start
        foreach (Transform pickupTF in pickupsTF)
        {
            int x = (int)pickupTF.localPosition.x;
            int z = (int)pickupTF.localPosition.z;
            retArray[x, z] = pickupTF.gameObject;
        }
        return retArray;
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
