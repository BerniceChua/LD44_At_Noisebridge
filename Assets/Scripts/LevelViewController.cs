using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelViewController : MonoBehaviour
{
    float mouseSpeedX, mouseSpeedY;

    [SerializeField]
    Transform levelTransform;

    [SerializeField]
    Camera mainCamera;

    bool leftClicking, rightClicking;

    // Start is called before the first frame update
    void Start()
    {
        leftClicking = false;
        rightClicking = false;
    }

    // Update is called once per frame
    void Update()
    {
        leftClicking = Input.GetMouseButton(0);
        rightClicking = Input.GetMouseButton(1);
        if (leftClicking)
        {
            Debug.Log("LeftClick");
            mouseSpeedX = Input.GetAxis("Mouse X");
            levelTransform.eulerAngles += new Vector3(0f, mouseSpeedX, 0f);
        }
        if (rightClicking)
        {
            mouseSpeedY = Input.GetAxis("Mouse Y");
            mainCamera.orthographicSize -= mouseSpeedY / 2f;
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 2.5f, 5.0f);
        }
    }
}
