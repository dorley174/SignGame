using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("mage");
    }

    void Update()
    {
        if (player.GetComponent<Player>().GetHP() == 0f)
        {
            SceneManager.LoadScene("LevelCave000");
            DataContainer.checkpointIndex = 0;
        }
    }
}
