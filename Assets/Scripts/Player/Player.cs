using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHp = 100f;

    [Header("Debug")]
    [SerializeField] float hp;

    void Start()
    {
        hp = maxHp;
    }

    // for test
    void Update()
    {
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
        if (hp <= 0) return;
        hp = Mathf.Max(hp - damage, 0);
        if (hp <= 0) GameManager.I.PlayerDied();
    }
}
