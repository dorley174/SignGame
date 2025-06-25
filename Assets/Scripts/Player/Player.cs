using UnityEngine;

public class Player : MonoBehaviour
{
    public float iSeconds = 2f;

    [Header("Debug")]
    [SerializeField] int hp;
    [SerializeField] float iSecondsCount;

    void Start()
    {
        hp = PlayerPrefs.GetInt("hp");
    }

    // for test
    void Update()
    {
        iSecondsCount = Mathf.Max(iSecondsCount - Time.deltaTime, 0);
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

    public void TakeDamage(float damage) {
        if (hp <= 0 || iSecondsCount > 0) return;
        hp = Mathf.Max(hp - (int)damage, 0);
        iSecondsCount = iSeconds;
        if (hp <= 0) GameManager.I.PlayerDied();
    }
}
