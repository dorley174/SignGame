using TMPro;
using UnityEngine;

public class CoinTracker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinCounter;

    void Awake()
    {
        coinCounter.text = PlayerPrefs.GetInt("coins").ToString();
        Debug.Log($"{PlayerPrefs.GetInt("coins")}");
    }

    void Update()
    {
        coinCounter.text = PlayerPrefs.GetInt("coins").ToString();
        Debug.Log($"{PlayerPrefs.GetInt("coins")}");  
    }
}
