using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHp = 100f;
    public float iSeconds = 2f;

    [Header("Debug")]
    [SerializeField] float hp;
    [SerializeField] float iSecondsCount;

    void Start()
    {
        hp = maxHp;
    }

    // for test
    void Update()
    {
        iSecondsCount = Mathf.Max(iSecondsCount - Time.deltaTime, 0);
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
            Debug.Log($"Player HP: {hp}");
        }
    }
    // for test

    public float GetHP()
    {
        return hp;
    }

    public void TakeDamage(float damage) {
        if (hp <= 0 || iSecondsCount > 0) return;
        hp = Mathf.Max(hp - damage, 0);
        iSecondsCount = iSeconds;
        if (hp <= 0) GameManager.I.PlayerDied();
    }
}
