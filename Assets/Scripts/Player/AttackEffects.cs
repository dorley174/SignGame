using UnityEngine;
using System.Collections;

public class AttackEffect : MonoBehaviour
{
    public void StartBurning(GameObject enemy, float duration)
    {
        StartCoroutine(BurnEnemy(enemy, duration));
    }

    public void StartPoisoning(GameObject enemy, float duration)
    {
        StartCoroutine(PoisonEnemy(enemy, duration));
    }

    public void ApplyKnockback(Vector3 player_position, GameObject enemy, float knockbackForce)
    {
        // Определяем направление от игрока к врагу, чтобы отбрасывание было правильным
        Vector3 direction = enemy.transform.position - player_position;
        direction.Normalize(); 

        // Применяем отбрасывание
        enemy.transform.position += direction * knockbackForce;
        
        Debug.Log($"Объект {enemy.name} отброшен на {knockbackForce} единиц");
    }

    private IEnumerator BurnEnemy(GameObject enemy, float duration)
    {
        float timer = 0f;
        Vector3 position_change = new Vector3(0, 0, 0);

        while (timer < duration)
        {
            int damage = Random.Range(1, 4);
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

        // Возвращаем на исходную позицию
        enemy.transform.position -= position_change;
    }

    private IEnumerator PoisonEnemy(GameObject enemy, float duration)
    {
        float timer = 0f;
        Vector3 position_change = new Vector3(0, 0, 0);

        while (timer < duration)
        {
            int damage = Random.Range(1, 4);
            Debug.Log($"Объект {enemy.name} отравлен. Получен урон {damage}.");

            // Колбасим врага по X
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

        // Возвращаем на исходную позицию
        enemy.transform.position -= position_change;
    }
}