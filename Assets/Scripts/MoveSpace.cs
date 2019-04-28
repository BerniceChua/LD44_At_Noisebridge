using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpace : MonoBehaviour
{
    public bool hasTurnedOn;
    public bool hasSuggestedMove;
    public bool hasPickup;
    public SpaceType moveSpaceType;

    [SerializeField]
    Camera camera;

    [SerializeField]
    Material mat0;

    [SerializeField]
    Material mat1;

    [SerializeField]
    GameObject backTrackPrefab;

    public int posX, posY;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;

            // Do something with the object that was hit by the raycast.
            if (objectHit == transform)
            {
                hasTurnedOn = true;
                if (Input.GetMouseButtonDown(0) && hasSuggestedMove)
                {
                    //instantiate a backtrack fire hazard in current space
                    Transform playerSpace = GameObject.FindObjectOfType<PlayerController>().GetCurrentSpaceTransform();
                    GameObject pickup = (GameObject)Instantiate(backTrackPrefab, playerSpace);
                    pickup.transform.localPosition = new Vector3(0f, 1.5f, 0f);
                    pickup.transform.localScale = new Vector3(.5f, .5f / pickup.transform.parent.localScale.y, .5f);
                    playerSpace.GetComponent<MoveSpace>().hasPickup = true;
                    Debug.Log("yo " + posX + " "+ posY);
                    //then move
                    GameObject.FindObjectOfType<PlayerController>().MoveToSpace(transform);
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
