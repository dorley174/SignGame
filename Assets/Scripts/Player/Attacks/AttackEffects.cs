using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class AttackEffect : MonoBehaviour
{
    public float burnDuration = 3f;
    public float poisonDuration = 5f;
    public float knockbackForce = 200f;

    public void StartBurning(GameObject enemy)
    {
        StartCoroutine(BurnEnemy(enemy));
    }

    public void StartPoisoning(GameObject enemy)
    {
        StartCoroutine(PoisonEnemy(enemy));
    }

    public void ApplyKnockback(Vector3 self_position, GameObject enemy)
    {
        // Определяем направление от объекта к врагу, чтобы отбрасывание было правильным
        Vector3 direction = enemy.transform.position - self_position;
        Rigidbody2D physic = enemy.GetComponent<Rigidbody2D>();
        direction.Normalize(); 

        // Применяем отбрасывание
        physic.AddForce(direction * knockbackForce);
        
        Debug.Log($"Объект {enemy.name} отброшен на {knockbackForce} единиц");
    }

    private IEnumerator BurnEnemy(GameObject enemy)
    {
        float timer = 0f;
        Vector3 position_change = new Vector3(0, 0, 0);
        Color enemyColor = enemy.GetComponent<SpriteRenderer>().color;

        while (timer < burnDuration)
        {
            int damage = Random.Range(1, 4);
            enemy.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0f, 0f);
            Debug.Log($"Объект {enemy.name} горит. Получен урон {damage}.");

            // Колбасим врага по X
            float pushForce = 0.25f * damage;
            if (Random.Range(1, 3) > 1)
            {
                enemy.transform.position += new Vector3(pushForce, 0, 0);
                position_change += new Vector3(pushForce, 0, 0);
            }
            else
            {
                enemy.transform.position -= new Vector3(pushForce, 0, 0);
                position_change -= new Vector3(pushForce, 0, 0);
            }

            timer += 1f;
            yield return new WaitForSeconds(1f);
        }

        // Возвращаем в исходное положение
        enemy.GetComponent<SpriteRenderer>().color = enemyColor;
        enemy.transform.position -= position_change;
    }

    private IEnumerator PoisonEnemy(GameObject enemy)
    {
        float timer = 0f;
        Vector3 position_change = new Vector3(0, 0, 0);
        Color enemyColor = enemy.GetComponent<SpriteRenderer>().color;

        while (timer < poisonDuration)
        {
            int damage = Random.Range(1, 4);
            enemy.GetComponent<SpriteRenderer>().color = new Color(0f, 0.5f, 0f);
            Debug.Log($"Объект {enemy.name} отравлен. Получен урон {damage}.");

            // Колбасим врага по Y
            float pushForce = 0.2f * damage;
            if (Random.Range(1, 3) > 1)
            {
                enemy.transform.position += new Vector3(pushForce, 0, 0);
                position_change += new Vector3(pushForce, 0, 0);
            }
            else
            {
                enemy.transform.position -= new Vector3(pushForce, 0, 0);
                position_change -= new Vector3(pushForce, 0, 0);
            }

            timer += 1f;
            yield return new WaitForSeconds(1f);
        }

        // Возвращаем в исходное положение
        enemy.GetComponent<SpriteRenderer>().color = enemyColor;
        enemy.transform.position -= position_change;
    }
}