using UnityEngine;

public class Player : MonoBehaviour
{

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
    }
}
