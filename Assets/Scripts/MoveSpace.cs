using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpace : MonoBehaviour
{
    public bool hasTurnedOn;

    [SerializeField]
    Camera camera;

    [SerializeField]
    Material mat0;

    [SerializeField]
    Material mat1;

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
                Debug.Log(gameObject.name);
                MeshRenderer renderer = GetComponent<MeshRenderer>();
                renderer.material = mat1;
                hasTurnedOn = true;
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject.FindObjectOfType<PlayerController>().MoveToSpace(transform);
                }
            }else if (hasTurnedOn)
            {
                Debug.Log("exiting2");
                MeshRenderer renderer = GetComponent<MeshRenderer>();
                renderer.material = mat0;
                hasTurnedOn = false;
            }
        }
        else
        {
            if (hasTurnedOn)
            {
                Debug.Log("exiting");
                MeshRenderer renderer = GetComponent<MeshRenderer>();
                renderer.material = mat0;
                hasTurnedOn = false;
            }
        }
        
    }
}
