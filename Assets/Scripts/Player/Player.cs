using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Debug")]
    [SerializeField] int hp;

    void Start()
    {
        hp = PlayerPrefs.GetInt("hp");
    }

    // for test
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            hp = PlayerPrefs.GetInt("hp");
            PlayerPrefs.SetInt("hp", hp - 1);
            Debug.Log($"Player HP: {hp - 1}");
        }
    }
    // for test

    public float GetHP()
    {
        return PlayerPrefs.GetInt("hp");
    }
}
