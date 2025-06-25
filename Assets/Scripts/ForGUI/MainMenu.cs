using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string startSceneName = "LevelCave000";

    public void StartGame()
    {
        DataContainer.checkpointIndex = 0;
        SceneManager.LoadScene(startSceneName);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}