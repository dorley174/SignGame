using UnityEngine;

public class Player : MonoBehaviour
{
<<<<<<< HEAD
    public float maxHp = 100f;
    public float iSeconds = 2f;

    [Header("Debug")]
    [SerializeField] float hp;
    [SerializeField] float iSecondsCount;
=======

    [Header("Debug")]
    [SerializeField] int hp;
>>>>>>> c0147a9feb98b34abe4a3c011da90ad4966d428d

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
<<<<<<< HEAD
            TakeDamage(10);
            Debug.Log($"Player HP: {hp}");
=======
            hp = PlayerPrefs.GetInt("hp");
            PlayerPrefs.SetInt("hp", hp - 1);
            Debug.Log($"Player HP: {hp - 1}");
>>>>>>> c0147a9feb98b34abe4a3c011da90ad4966d428d
        }
    }
    // for test

    public float GetHP()
    {
        return PlayerPrefs.GetInt("hp");
    }

    public void TakeDamage(float damage) {
        if (hp <= 0 || iSecondsCount > 0) return;
        hp = Mathf.Max(hp - damage, 0);
        iSecondsCount = iSeconds;
        if (hp <= 0) GameManager.I.PlayerDied();
    }
}
