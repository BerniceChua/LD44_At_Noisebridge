using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main_control : MonoBehaviour {
    
    // NOTE(Peter): This class just acts as main() in a C++ program
    // it lets us avoid using Awake and Start (and if we really want, we can avoid Update
    // this way as well).
    //
    // This has the benefit of making it so we can visually trace a path of execution without
    // relying on Unity as a black box.
    
    public Transform StaticParent;

    [SerializeField] private GameLoopManager m_gameLoopManager;

    void Awake ()
    {
        //// Create the Level, spawn characters
        //generate_level LevelGenerator = FindObjectOfType<generate_level>();
        //LevelGenerator.GenerateLevel(StaticParent);

        //StartCoroutine(m_gameLoopManager.GameLoop());
    }
    
}
