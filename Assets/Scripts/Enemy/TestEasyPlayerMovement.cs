using UnityEngine;

public class TestEasyPlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float v = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        Vector3 direction = new Vector3(h, v, 0).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }
}
