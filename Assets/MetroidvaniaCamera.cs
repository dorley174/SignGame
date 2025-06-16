using UnityEngine;

public class MetroidvaniaCamera : MonoBehaviour
{
    [Header("Camera Follow Settings")]
    [SerializeField] private Transform player; // Ссылка на трансформ игрока
    [SerializeField] private float smoothTime = 0.125f; // Время сглаживания движения камеры
    [SerializeField] private Vector3 offset; // Смещение камеры относительно игрока
    [SerializeField] private float lookAheadDistance = 2f; // Дистанция предугадывания движения
    [SerializeField] private float lookAheadSpeed = 0.1f; // Скорость предугадывания

    [Header("Camera Bounds")]
    [SerializeField] private bool useBounds = true; // Использовать ли границы карты
    [SerializeField] private Vector2 minBounds; // Минимальные координаты камеры
    [SerializeField] private Vector2 maxBounds; // Максимальные координаты камеры

    [Header("Zoom Settings")]
    [SerializeField] private float defaultZoom = 5f; // Стандартный размер ортографической камеры
    [SerializeField] private float zoomSpeed = 2f; // Скорость изменения масштаба
    [SerializeField] private float minZoom = 3f; // Минимальный масштаб
    [SerializeField] private float maxZoom = 7f; // Максимальный масштаб

    private Camera cam;
    private Vector3 lookAheadOffset;
    private Vector3 targetPosition;

    void Start()
    {
        cam = GetComponent<Camera>();

        // Проверка на наличие игрока
        if (player == null)
        {
            Debug.LogError("Player Transform is not assigned!");
            enabled = false; // Отключаем скрипт, если игрок не назначен
        }

        // Инициализируем позицию камеры
        targetPosition = player.position + offset;
        transform.position = targetPosition;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // Целевая позиция камеры
        targetPosition = player.position + offset + lookAheadOffset;

        // Плавное движение камеры с использованием Lerp
        Vector3 desiredPosition = Vector3.Lerp(transform.position, targetPosition, smoothTime * Time.fixedDeltaTime * 60f);

        // Сохраняем Z координату камеры
        desiredPosition.z = transform.position.z;

        // Применяем позицию
        transform.position = desiredPosition;

        // Ограничение камеры в пределах карты
        if (useBounds)
        {
            float camHeight = cam.orthographicSize;
            float camWidth = camHeight * cam.aspect;

            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBounds.x + camWidth, maxBounds.x - camWidth);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, minBounds.y + camHeight, maxBounds.y - camHeight);
            transform.position = clampedPosition;
        }

        // Обновление предугадывания
        UpdateLookAhead();
        // Обновление масштаба
        UpdateZoom();
    }

    private void UpdateLookAhead()
    {
        // Получаем направление движения игрока
        Vector3 playerVelocity = player.GetComponent<Rigidbody2D>().linearVelocity;
        Vector3 targetLookAhead = Vector3.zero;

        if (playerVelocity.magnitude > 0.1f)
        {
            targetLookAhead = new Vector3(playerVelocity.x, playerVelocity.y, 0).normalized * lookAheadDistance;
        }

        // Плавное изменение предугадывания
        lookAheadOffset = Vector3.Lerp(lookAheadOffset, targetLookAhead, lookAheadSpeed * Time.fixedDeltaTime);
    }

    private void UpdateZoom()
    {
        // Пример: увеличение масштаба при высокой скорости игрока
        float playerSpeed = player.GetComponent<Rigidbody2D>().linearVelocity.magnitude;
        float targetZoom = defaultZoom + (playerSpeed * 0.1f); // Масштаб зависит от скорости
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

        // Плавное изменение масштаба
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSpeed * Time.fixedDeltaTime);
    }

    // Метод для динамического изменения границ
    public void SetCameraBounds(Vector2 newMinBounds, Vector2 newMaxBounds)
    {
        minBounds = newMinBounds;
        maxBounds = newMaxBounds;
    }

    // Метод для мгновенного перемещения камеры
    public void SnapToPlayer()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z) + offset;
            targetPosition = transform.position; // Синхронизируем целевую позицию
        }
    }
}