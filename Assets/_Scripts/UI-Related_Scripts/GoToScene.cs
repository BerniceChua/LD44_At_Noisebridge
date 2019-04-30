using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
    /// <summary>
    /// UI Button to restart the level.
    /// </summary>
    public void UIButtonRestartGame() {
        SceneManager.LoadScene("Mark");
    }
}
