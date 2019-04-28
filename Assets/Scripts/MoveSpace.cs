using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpace : MonoBehaviour
{
    public bool hasTurnedOn;
    public bool hasSuggestedMove;
    public bool hasPickup;
    public int wineLoss;
    public SpaceType moveSpaceType;

    [SerializeField]
    Camera camera;

    [SerializeField]
    Material mat0;

    [SerializeField]
    Material mat1;

    [SerializeField]
    GameObject backTrackPrefab;

    public LevelViewController levelController;

    public int posX, posY;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindObjectOfType<Camera>();
        gameObject.layer = 8; // set layer to "Pickable"
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        
        LayerMask mask = LayerMask.GetMask("Pickable");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            Transform objectHit = hit.transform;

            // Do something with the object that was hit by the raycast.
            if (objectHit == transform)
            {
                hasTurnedOn = true;
                bool isCardinal = false;
                int fromX = GameObject.FindObjectOfType<PlayerController>().posX;
                int fromY = GameObject.FindObjectOfType<PlayerController>().posY;
                int xdist = Mathf.Abs(fromX - posX);
                int ydist = Mathf.Abs(fromY - posY);
                if ((xdist <= 1) && (ydist <= 1) && (xdist + ydist == 1) && !GameObject.FindObjectOfType<CardControl>().Blocked(fromX, fromY, posX, posY))
                {
                    isCardinal = true;
                    Transform playerSpace = GameObject.FindObjectOfType<PlayerController>().GetCurrentSpaceTransform();
                    playerSpace.GetComponent<MoveSpace>().wineLoss = 1;
                }
               if (Input.GetMouseButtonDown(0) && (hasSuggestedMove || isCardinal))
                {
                    //instantiate a backtrack fire hazard in current space
                    Transform playerSpace = GameObject.FindObjectOfType<PlayerController>().GetCurrentSpaceTransform();
                   
                    Debug.Log("yo " + posX + " "+ posY);
                    //then move
                    GameObject.FindObjectOfType<PlayerController>().MoveToSpace(transform, playerSpace);
                    GameObject.FindObjectOfType<LevelViewController>().ClearSuggestedMoves();
                }
            }else if (hasTurnedOn)
            {
                hasTurnedOn = false;
            }
        }
        else
        {
            if (hasTurnedOn)
            {
                hasTurnedOn = false;
            }
        }

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (hasTurnedOn || hasSuggestedMove)
        {
            renderer.material = mat1;
        } else
        {
            renderer.material = mat0;
        }

    }
}
