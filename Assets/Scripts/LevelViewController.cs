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
    double momentumX;
    double momentumY;

    // Start is called before the first frame update
    void Start()
    {
        leftClicking = false;
        rightClicking = false;
        momentumX = 0f;
        momentumY = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        leftClicking = Input.GetMouseButton(0);
        rightClicking = Input.GetMouseButton(1);
        if (leftClicking)
        {

        }
        if (rightClicking)
        {
            GameObject.FindObjectOfType<LevelViewController>().ClearSuggestedMoves();
            mouseSpeedX = Input.GetAxis("Mouse X");
            mouseSpeedY = Input.GetAxis("Mouse Y");
            momentumY -= mouseSpeedY / 8f;
            momentumX += mouseSpeedX * 4;
        }
        momentumX = momentumX * 0.9;
        levelTransform.eulerAngles += new Vector3(0f, (float)(momentumX), 0f);
        momentumY = momentumY * 0.9;
        mainCamera.orthographicSize += (float)momentumY;
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 2.5f, 5.0f);
    }

    public void ClearSuggestedMoves()
    {
        for (int i = 0; i < levelTransform.childCount; i++)
        {
            Transform tf = levelTransform.GetChild(i);
            if (tf != null)
            {
                MoveSpace space = tf.GetComponent<MoveSpace>();
                if (space != null)
                    space.hasSuggestedMove = false;
            }
        }
    }
}
