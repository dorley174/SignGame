using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHp = 100f;

    [Header("Debug")]
    [SerializeField] float hp;

    private Color originalColor;

    void Start()
    {
        hp = maxHp;
        originalColor = this.GetComponent<SpriteRenderer>().color;
    }

    void Update()
    {

    }

    public void TakeDamage(float amount)
    {
        hp -= amount;
        Debug.Log($"{name} получил {amount} урона. Осталось HP: {hp}.");
        if (hp <= 0)
        {
            Die();
        }
    }

    public float GetHP()
    {
        return hp;
    }
    public void Die()
    {
        Debug.Log($"Объект {name} был уничтожен!");
        Destroy(gameObject);
    }

    public void ReturnToOrig()
    {
        GetComponent<SpriteRenderer>().color = originalColor;
    }
}
