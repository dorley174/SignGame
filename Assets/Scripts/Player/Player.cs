using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHp = 100f;

    [Header("Debug")]
    [SerializeField] float hp;

    void Start() {
        hp = maxHp;
    }
}
