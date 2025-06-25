using UnityEngine;

public class ShootSpell : MonoBehaviour
{
    public float force = 200f;
    public float lifeDistance = 20f;
    public bool lookRight;
    public string effectType;

    private Rigidbody2D physic;
    private Vector3 origPos;

    private void Start()
    {
        origPos = transform.position;
        physic = GetComponent<Rigidbody2D>();
        if (lookRight)
        {
            physic.AddForce(new Vector2(force, 0));
        }
        else
        {
            physic.AddForce(new Vector2(-force, 0));
        }
    }
    void Update()
    {
        CheckIfAlive();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("Collision!");
            EffectsManager.Instance.effect.ApplyEffect(gameObject, other.gameObject, effectType);
            Destroy(gameObject);
        }
    }

    private void CheckIfAlive()
    {
        if (lookRight && transform.position.x - origPos.x >= lifeDistance)
        {
            Destroy(gameObject);
        }
        else if (lookRight == false && origPos.x - transform.position.x >= lifeDistance)
        {
            Destroy(gameObject);
        }
    }
}
