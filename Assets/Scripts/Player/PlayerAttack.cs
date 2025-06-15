using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public string enemyTag = "Enemy";

    // public float AnyEffectDuration = 5f; ??? общая долгота эффекта
    // но это должно быть тут, а не в эффектах, на случай, если будем улучшать
    // а пока разная долгота эффектов
    public float burnDuration = 3f;
    public float poisonDuration = 5f;
    public float knockbackForce = 2f;

    private AttackEffect receivedEffect;


    private void Awake()
    {
        receivedEffect = gameObject.AddComponent<AttackEffect>();
    }

    private void Update()
    {
        CheckMouseClickOnEnemy();
    }

    private void CheckMouseClickOnEnemy()
    {
        if (Input.GetMouseButtonDown(0)) // Левая кнопка мыши
        {
            CheckEnemyUnderCursor("Левая кнопка мыши");
            GameObject enemy = GetEnemyUnderCursor();
            if (enemy != null)
            {
                // Вызываем эффект горения из отдельного компонента
                receivedEffect.StartBurning(enemy, burnDuration);
            }
        }
        else if (Input.GetMouseButtonDown(1)) // Правая кнопка мыши
        {
            CheckEnemyUnderCursor("Правая кнопка мыши");
            GameObject enemy = GetEnemyUnderCursor();
            if (enemy != null)
            {
                // Вызываем эффект отравления из отдельного компонента
                receivedEffect.StartPoisoning(enemy, poisonDuration);
            }
        }
        else if (Input.GetMouseButtonDown(2)) // Средняя кнопка мыши
        {
            CheckEnemyUnderCursor("Средняя кнопка мыши");
            GameObject enemy = GetEnemyUnderCursor();
            if (enemy != null)
            {
                // Отталкиваем его от игрока
                receivedEffect.ApplyKnockback(transform.position, enemy, knockbackForce);
            }
        }
    }

    private GameObject GetEnemyUnderCursor()
    {
        // Как будто надо оптимизировать, ибо здесь перебираются все враги,
        // а нужен только тот, на кого мышка указала
        Vector2 cursorWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemy in enemies)
        {
            if (IsPointOverObject(cursorWorldPos, enemy))
            {
                return enemy;
            }
        }
        return null;
    }

    private void CheckEnemyUnderCursor(string buttonName)
    {
        GameObject enemy = GetEnemyUnderCursor();
        if (enemy != null)
        {
            Debug.Log($"Нажат враг с помощью: {buttonName}");
        }
    }

    private bool IsPointOverObject(Vector2 point, GameObject obj)
    {
        return Vector2.Distance(point, obj.transform.position) < 0.5f;
    }
}