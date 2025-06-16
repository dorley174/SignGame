using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bonus : MonoBehaviour
{
    [SerializeField] private string bonusName;
    [SerializeField] private TextMeshProUGUI coinCount;

    void Awake()
    {
        if (coinCount != null)
        {
            coinCount.text = PlayerPrefs.GetInt("coins").ToString();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            if (bonusName == "coin")
            {
                int coins = PlayerPrefs.GetInt("coins");
                PlayerPrefs.SetInt("coins", coins + 1);
                Debug.Log(coins + 1);
                if (coinCount != null)
                {
                    coinCount.text = (coins + 1).ToString();
                }
                Destroy(gameObject);
            }
        }
    }
}
