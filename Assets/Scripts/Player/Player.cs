using UnityEngine;

public class Player : MonoBehaviour
{
    public float iSeconds = 2f;

    [Header("Debug")]
    [SerializeField] private int hp;
    [SerializeField] private int maxHP = 10;

    void Start()
    {
        hp = maxHP;
    }

    // for test
    void Update()
    {
        iSecondsCount = Mathf.Max(iSecondsCount - Time.deltaTime, 0);
        if (Input.GetKeyDown(KeyCode.T))
        {
            hp--;
            Debug.Log($"Player HP: {hp}");
        }
    }
    // for test

    public int GetHP()
    {
        return hp;
    }

    public void IncreaseHP(int plusHP)
    {
        hp += plusHP;
        if (hp > maxHP)
            hp = maxHP;
    }

    public void IncreaseHPToFull()
    {
        hp = maxHP;
    }
}
