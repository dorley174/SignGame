using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject panel;

    private void Start()
    {
        panel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Activate();
        }
    }

    public void Activate()
    {
        panel.SetActive(!panel.activeSelf);
        Time.timeScale = panel.activeSelf ? 0 : 1;
    }
}