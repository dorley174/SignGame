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
            hp -= 10f;
            Debug.Log($"Player HP: {hp}");
        }
    }
    // for test

    public float GetHP()
    {
        return hp;
    }
}
