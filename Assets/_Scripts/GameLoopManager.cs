using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameLoopManager : MonoBehaviour {

    [SerializeField] public float m_StartDelay = 3f;             // The delay between the start of RoundStarting and RoundPlaying phases.
    [SerializeField] public float m_EndDelay = 3f;

    [SerializeField] private GameObject m_endMessagePanel;     // Reference to the overlay Panel that contains Text to display result text, etc.
    public TextMeshProUGUI m_MessageText;                  // Reference to the overlay Text to display result text, etc.

    public GameObject playerPrefab, levelObjectPrefab;
    [SerializeField] public PlayerController m_playerController;
    GameObject pconToDestroy, levelToDestroy;

    private WaitForSeconds m_StartWait;         /// Used to have a delay whilst the round starts.
    private WaitForSeconds m_EndWait;           /// Used to have a delay whilst the round or game ends.

    [SerializeField] public GameObject m_StatsPanel;

    /// <summary>
    /// MainMenu options-related UI (restart, main menu, quit)
    /// </summary>
    [Tooltip("Put MainMenuPanel here.")]
    [SerializeField] private GameObject m_menuPanel;

    [Tooltip("For scene loading, put the name of the scene for the intro")]
    [SerializeField] private string m_introScene;
    [Tooltip("For scene loading, put the name of the scene for the game level")]
    [SerializeField] private string m_gameLevelScene;

    public AudioSource EndSoundSource;
    public AudioSource Soundtrack;
    public bool PlayingEndSound;

    public Transform StaticParent;

    [SerializeField] private generate_level m_levelGenerator;
    [SerializeField] private level_specification[] m_ArrayOfLevels;


    // Use this for initialization
    void Start() {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        StartCoroutine(GameLoop());
    }

    public IEnumerator GameLoop() {

        for (int sceneCount = 0; sceneCount < m_ArrayOfLevels.Length; sceneCount++) {
           
            //Debug.Break();
            m_levelGenerator.GenerateLevel(StaticParent, m_ArrayOfLevels[sceneCount]);
            // As soon as the round begins playing let the players control the characters.
            m_playerController.Cheated = false;
            m_playerController.ReachedExit = false;
            // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
            //yield return StartCoroutine(RoundStarting());
            //Debug.Log(m_playerController);
            //Debug.Break();
            string message = CostMessage();
            m_MessageText.text = message;
            m_endMessagePanel.SetActive(true);


            while (/*!Input.GetKey(KeyCode.Return) && */!Input.GetMouseButtonDown(0)) yield return null;
            m_StatsPanel.GetComponentInChildren<TextMeshProUGUI>().text = message;
            m_StatsPanel.SetActive(true);

            // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
            m_menuPanel.SetActive(false);
            m_endMessagePanel.SetActive(false);
            // Clear the text from the screen.
            m_MessageText.text = string.Empty;
            EnablePlayerControl();

            // As soon as the round begins playing, start the countdown timer.
            while (m_playerController.ReachedExit == false && m_playerController.Cheated == false
                && m_playerController.wine > 0)
            {
                //UpdateTimer();
                // ... return on the next frame.
                yield return null;
            }
            // Stop players from moving.
            Debug.Log("disabled");
            DisablePlayerControl();
            //yield return StartCoroutine(RoundPlaying());

            // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
            // Stop players from moving.
            DisablePlayerControl();
            /// Get a message based on the scores and whether or not all the characters survived and display it.
            message = EndMessage();
            m_MessageText.text = message;
            // Wait for the specified length of time until yielding control back to the game loop.
            yield return null;
            //yield return StartCoroutine(RoundEnding());

            /// Re-enable restart/main menu/quit options when game ends.
            m_menuPanel.SetActive(true);
            PlayingEndSound = false;

            //EndSoundSource.Stop();
            //Soundtrack.Play();

            /// After 'RoundEnding()' has finished, check if player wants to play again, or go to main menu, or quit.
            /// These are for either controller buttons or keyboard shortcuts, if the players don't use the UI buttons.
            while (!Input.GetKey(KeyCode.Return))yield return null;
            if (Input.GetKey(KeyCode.Return)) {
                //if we died instead of reaching end, hold back sceneCount so we do the same scene again
                if (m_playerController.ReachedExit == false)
                {
                    sceneCount -= 1;
                }
                else
                {
                    m_StatsPanel.SetActive(false);
                }

                // Restart the level.
                pconToDestroy = m_playerController.gameObject;
                levelToDestroy = StaticParent.gameObject;

                m_playerController = GameObject.Instantiate(playerPrefab).GetComponent<PlayerController>();
                StaticParent = GameObject.Instantiate(levelObjectPrefab).transform;
                DestroyCurrentLevel();
                //SceneManager.LoadScene(m_gameLevelScene);
                
               
            } else if (Input.GetKey(KeyCode.Escape)) {
                // Go to main menu.
                SceneManager.LoadScene(m_introScene);
            } else if (Input.GetKey(KeyCode.Q)) {
                // Quit the game.
    #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }

        }

    }

    void DestroyCurrentLevel()
    {

        Destroy(pconToDestroy);
        Destroy(levelToDestroy);
    }

    private IEnumerator RoundStarting() {
        // Wait for the specified length of time until yielding control back to the game loop.
        Debug.Log(m_playerController);
        //Debug.Break();
        yield return null;
    }

    private IEnumerator RoundPlaying() {

        Debug.Log(m_playerController);

        //Debug.Break();
        /// Disable restart/main menu/quit options when game starts.
        m_menuPanel.SetActive(false);
        
        

        // Clear the text from the screen.
        m_MessageText.text = string.Empty;

        // As soon as the round begins playing, start the countdown timer.
        while (m_playerController.ReachedExit == false && m_playerController.Cheated == false) {
            //UpdateTimer();
            // ... return on the next frame.
            Debug.Log("player" + m_playerController);
            yield return null;
        }
        // Stop players from moving.
        Debug.Log("disabled");
        DisablePlayerControl();

        m_StatsPanel.SetActive(false);

        yield return StartCoroutine(RoundEnding());
    }

    private IEnumerator RoundEnding() {
        // Stop players from moving.
        DisablePlayerControl();

        /// Get a message based on the scores and whether or not all the characters survived and display it.
        string message = EndMessage();
        m_MessageText.text = message;

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return null;
    }

    private string EndMessage() {
        // By default when a round ends, and all the characters survived, show the victory message.
        string message = "";

        // If there is a casualty then change the message to reflect that.
        if (m_playerController.ReachedExit || m_playerController.Cheated) {
            message = "You've reached your goal!! \nPress Enter to Continue";
        } else {
            message = "You've ran out of wine!! \nPress Enter to Retry";
        }

        // Add some line breaks after the initial message.
        //message += "\n\n\n\n";

        m_endMessagePanel.SetActive(true);

        return message;
    }

    private string CostMessage()
    {
        CardControl ccontrol = GameObject.FindObjectOfType<CardControl>();
        string message = "COSTS!!" +
            "\nBasic Move: " + m_playerController.walkDamage +
            "\nDiagonal Move: " + ccontrol.diagLoss +
            "\nDash Move: " + ccontrol.dashLoss +
            "\n\nVALUES!!" +
            "\nChalice: " + m_playerController.wineHealAmount +
            "\nGrapes: " + m_playerController.grapeHealAmount + 
            "\n\nDAMAGE!!" + 
            "\nBackTrack: " + m_playerController.backtrackDamage +
            "\nHazards: " + m_playerController.hazardDamage;
        
    

        m_endMessagePanel.SetActive(true);

        return message;
    }
    // This function is used to turn all the player-characters back on and reset their positions and properties.
    private void ResetAllPlayers() {
        //for (int i = 0; i < m_PlayersArray.Length; i++) {
        //    m_PlayersArray[i].Reset();
        //}
    }

    private void EnablePlayerControl() {
        m_playerController.EnableControl();
    }

    private void DisablePlayerControl() {
        m_playerController.DisableControl();
    }

    /// <summary>
    /// UI Button to restart the level.
    /// </summary>
    public void UIButtonRestartGame() {
        SceneManager.LoadScene(m_gameLevelScene);
    }

    /// <summary>
    /// UI Button to go to the main menu.
    /// </summary>
    public void UIButtonGoToMainMenu() {
        SceneManager.LoadScene(m_introScene);
    }

    /// <summary>
    /// UI Button to quit the game.
    /// </summary>
    public void UIButtonQuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
