using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Player player;
    [SerializeField] private float respawnDelay = 3f;

    public static GameManager I;

    void Awake() {
        if (I != null) return;
        I = this;
    }

    void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    public void PlayerDied() {
        Invoke(nameof(RespawnPlayer), respawnDelay);
    }

    private void RespawnPlayer()
    {
        SceneManager.LoadScene("LevelCave000");
    }
}
