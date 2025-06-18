using UnityEngine;
using System.Collections;

public class CoilSpray : MonoBehaviour
{
    public float duration = 1f;


    private void Start()
    {
        StartCoroutine(CreateForTime(duration));
    }

    void Update()
    {

    }

    private IEnumerator CreateForTime(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            Debug.Log("Coil timer:" + timer);
            timer += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log($"{other.gameObject.name} был подожжен!");
            EffectsManager.Instance.effect.StartBurning(other.gameObject);
        }
    }
}
