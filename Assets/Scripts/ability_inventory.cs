using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ability_inventory : MonoBehaviour 
{
    public ability_specification_group AbilitySpecGroup;
    
    public Transform[] InventorySlots;

    /*[System.NonSerialized]
        public interface_block[] BlockInstancesInInventory;
    [System.NonSerialized]
        public block_type[] BlockTypesInInventory;
    [System.NonSerialized]
        public int SelectedBlockIndex;
    
    [System.NonSerialized]
        public spawn_physics_block PhysicsSpawn;
    
    [System.NonSerialized]
        public Transform[] PlayerXforms;
    [System.NonSerialized]
        public int RegisteredPlayers;
    
    public void Construct (Transform DynamicParent)
    {
        SelectedBlockIndex = 0;
        
        PlayerXforms = new Transform[2];
        RegisteredPlayers = 0;
        
        BlockInstancesInInventory = new interface_block[InventorySlots.Length];
        BlockTypesInInventory = new block_type[InventorySlots.Length];
        
        for(int i = 0; i < InventorySlots.Length; i++)
        {
            SetRandomBlockAtIndex(i);
        }
        
        spawn_physics_block[] PhysicsSpawns = FindObjectsOfType<spawn_physics_block>();
        float ClosestDistance = 100000;
        int ClosestIndex = -1;
        for (int i = 0; i < PhysicsSpawns.Length; i++)
        {
            float Distance = (PhysicsSpawns[i].transform.position - this.transform.position).sqrMagnitude;
            if (Distance < ClosestDistance)
            {
                ClosestDistance = Distance;
                ClosestIndex = i;
            }
        }
        
        PhysicsSpawn = PhysicsSpawns[ClosestIndex];
        PhysicsSpawn.Construct(DynamicParent);
    }
    
    public void RegisterPlayer (Transform Player)
    {
        PlayerXforms[RegisteredPlayers++] = Player;
    }
    
    public void SetRandomBlockAtIndex (int Index)
    {
        int RandomBlock = Random.Range((int)block_type.BlockType_Invalid + 1,
                                       (int)block_type.BlockType_Count - 1);
        SetBlockAtIndex((block_type)RandomBlock, Index);
    }
    
    public void SetBlockAtIndex (block_type BlockType, int Index)
    {
        if (BlockInstancesInInventory[Index] != null)
        {
            Destroy(BlockInstancesInInventory[Index].gameObject);
            BlockTypesInInventory[Index] = block_type.BlockType_Invalid;
        }
        
        int IndexOfSpec = BlockSpecGroup.IndexOfSpecification(BlockType);
        Debug.Log("BlockType: " + BlockType + " Index: " + IndexOfSpec);
        
        block_specification BlockSpec = BlockSpecGroup.Blocks[IndexOfSpec];
        
        GameObject InterfaceBlock = GameObject.Instantiate(BlockSpec.Prefab_InterfaceBlock);
        InterfaceBlock.transform.SetParent(InventorySlots[Index], false);
        InterfaceBlock.transform.localPosition = Vector3.zero;
        
        BlockInstancesInInventory[Index] = InterfaceBlock.GetComponent<interface_block>();
        BlockTypesInInventory[Index] = BlockType;
        
        if (SelectedBlockIndex == Index)
        {
            BlockInstancesInInventory[Index].HighlightOn();
        }
        else
        {
            BlockInstancesInInventory[Index].HighlightOff();
        }
    }
    
    public void SelectNextBlock ()
    {
        if (BlockInstancesInInventory[SelectedBlockIndex])
        {
            BlockInstancesInInventory[SelectedBlockIndex].HighlightOff();
        }
        
        SelectedBlockIndex++;
        if (SelectedBlockIndex >= InventorySlots.Length) 
        {
            SelectedBlockIndex = 0;
        }
        
        BlockInstancesInInventory[SelectedBlockIndex].HighlightOn();
    }
    
    public void SpawnSelectedBlock ()
    {
        block_type SelectedType = BlockTypesInInventory[SelectedBlockIndex];
        if (SelectedType == block_type.BlockType_Invalid) { return; }
        
        int IndexOfSelected = BlockSpecGroup.IndexOfSpecification(SelectedType);
        
        GameObject Block = PhysicsSpawn.SpawnBlock(BlockSpecGroup.Blocks[IndexOfSelected].Prefab_PhysicsBlock);
        BuildObject NewBO = Block.GetComponent<BuildObject>();
        NewBO.Construct(PlayerXforms[0], PlayerXforms[1]);
        
        BlockTypesInInventory[SelectedBlockIndex] = block_type.BlockType_Invalid;
        Destroy(BlockInstancesInInventory[SelectedBlockIndex].gameObject);
        StartCoroutine(WaitToSpawnBlock(SelectedBlockIndex));
    }
    
    public WaitForSeconds WaitTime = new WaitForSeconds(15);
    public IEnumerator WaitToSpawnBlock (int BlockIndex)
    {
        yield return WaitTime;
        SetRandomBlockAtIndex(BlockIndex);
    }
    */
}
