using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHp = 100f;

    [Header("Debug")]
    [SerializeField] float hp;
    // for test
    private float timer = 0f;
    // for test

    void Start()
    {
        hp = maxHp;
    }

    // for test
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            timer = 0f;
            hp -= 1f;
        }
    }
    // for test

    public float GetHP()
    {
        return hp;
    }
}
