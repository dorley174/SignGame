using UnityEngine;

public class PlayerSpellCast : MonoBehaviour
{
    [Header("Staff Settings")]
    [SerializeField] private Transform staffTip; // Точка на посохе для эффектов и стрельбы
    [SerializeField] private float staffOffset = 1f; // Расстояние нижнего конца посоха от центра персонажа
    [SerializeField] private Transform staffTransform; // Трансформ посоха
    [SerializeField] private SpriteRenderer staffSpriteRenderer; // Спрайт посоха для изменения цвета
    [SerializeField] private Vector3 idleStaffPositionLeft = new Vector3(0.335000008f, -0.555999994f, 0f); // Позиция посоха (влево)
    [SerializeField] private Vector3 idleStaffPositionRight = new Vector3(0.335000008f, -0.555999994f, 0f); // Позиция посоха (вправо)
    [SerializeField] private Vector3 idleStaffRotationLeft = new Vector3(0f, 0f, 45f); // Поворот посоха (влево)
    [SerializeField] private Vector3 idleStaffRotationRight = new Vector3(0f, 0f, -45f); // Поворот посоха (вправо)

    [Header("Spell Settings")]
    [SerializeField] private GameObject aimReticle; // Префаб прицела
    [SerializeField] private GameObject fireballPrefab; // Префаб фаербола
    [SerializeField] private float spellSpeed = 30f; // Скорость фаербола
    [SerializeField] private ParticleSystem suctionParticles; // Частицы всасывания
    [SerializeField] private float spellDuration = 3f; // Длительность заклинания
    [SerializeField] private float reticleRotationSpeed = 30f; // Скорость вращения прицела
    [SerializeField] private float timeSlowFactor = 0.5f; // Фактор замедления времени

    private float currentSpellTime;
    private bool isCasting;
    private GameObject activeReticle;
    private Vector3 targetPosition;
    private Camera mainCamera;
    private float lastHorizontalInput; // Последнее направление движения

    private void Start()
    {
        mainCamera = Camera.main;
        if (!mainCamera)
        {
            Debug.LogError("Main Camera not found! Ensure a camera is tagged as 'MainCamera'.");
        }
        if (!staffSpriteRenderer)
        {
            Debug.LogError("Staff SpriteRenderer not assigned!");
        }
        suctionParticles.Stop();
        if (aimReticle) aimReticle.SetActive(false);
        lastHorizontalInput = 0f; // По умолчанию стояние
        UpdateIdleStaff(); // Устанавливаем начальное положение посоха
    }

    private void Update()
    {
        if ((Input.GetMouseButtonDown(1) && !isCasting) || (Input.GetKeyDown(KeyCode.LeftControl) && !isCasting))
        {
            StartCasting();
        }

        if (isCasting)
        {
            UpdateCasting();
        }
        else
        {
            UpdateIdleStaff(); // Обновляем положение посоха в состоянии покоя
        }

        if ((Input.GetMouseButtonUp(1) && isCasting) || (Input.GetKeyUp(KeyCode.LeftControl) && isCasting))
        {
            CastFireball();
        }

        // Обновляем последнее направление движения
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput != 0f)
        {
            lastHorizontalInput = horizontalInput; // Сохраняем направление, если есть движение
        }
    }

    private void StartCasting()
    {
        isCasting = true;
        currentSpellTime = spellDuration;
        Time.timeScale = timeSlowFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // Показываем прицел
        targetPosition = GetMouseWorldPosition();
        activeReticle = Instantiate(aimReticle, targetPosition, Quaternion.identity);
        activeReticle.SetActive(true);

        // Запускаем частицы
        suctionParticles.Play();
    }

    private void UpdateCasting()
    {
        // Обновляем позицию прицела в реальном времени
        targetPosition = GetMouseWorldPosition();
        activeReticle.transform.position = targetPosition;
        activeReticle.transform.Rotate(0, 0, reticleRotationSpeed * Time.unscaledDeltaTime);

        // Поворачиваем и позиционируем посох
        Vector3 direction = (targetPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        staffTransform.rotation = Quaternion.Euler(0, 0, angle - 90f); // -90, чтобы верх посоха смотрел на прицел
        staffTransform.position = transform.position + direction * staffOffset;

        // Изменяем цвет посоха от черного к красному
        float t = 1f - (currentSpellTime / spellDuration); // Прогресс от 0 до 1
        staffSpriteRenderer.color = Color.Lerp(Color.black, Color.red, t);

        // Обновляем таймер
        currentSpellTime -= Time.unscaledDeltaTime;

        if (currentSpellTime <= 0)
        {
            CancelSpell();
        }
    }

    private void UpdateIdleStaff()
    {
        // Определяем позицию и поворот в зависимости от последнего направления движения
        Vector3 idlePosition = lastHorizontalInput > 0 ? idleStaffPositionRight : idleStaffPositionLeft;
        Vector3 idleRotation = lastHorizontalInput > 0 ? idleStaffRotationRight : idleStaffRotationLeft;

        staffTransform.localPosition = idlePosition;
        staffTransform.localRotation = Quaternion.Euler(idleRotation);

        // Устанавливаем черный цвет в состоянии покоя
        staffSpriteRenderer.color = Color.black;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z; // Устанавливаем z для корректного преобразования
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
        worldPos.z = 0; // Обнуляем z для 2D
        return worldPos;
    }

    private void CastFireball()
    {
        isCasting = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        // Останавливаем частицы
        suctionParticles.Stop();

        // Создаем фаербол
        GameObject fireball = Instantiate(fireballPrefab, staffTip.position, Quaternion.identity);
        Vector3 direction = (targetPosition - staffTip.position).normalized;
        fireball.GetComponent<Rigidbody2D>().linearVelocity = direction * spellSpeed; // Используем spellSpeed

        // Уничтожаем прицел
        Destroy(activeReticle);

        // Возвращаем черный цвет посоха
        staffSpriteRenderer.color = Color.black;
    }

    private void CancelSpell()
    {
        isCasting = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        // Останавливаем частицы
        suctionParticles.Stop();

        // Уничтожаем прицел
        Destroy(activeReticle);

        // Возвращаем черный цвет посоха
        staffSpriteRenderer.color = Color.black;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}