using TMPro;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField] private string bonusName;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (bonusName == "coin")
            {
                int coins = PlayerPrefs.GetInt("coins");
                PlayerPrefs.SetInt("coins", coins + 1);
                // coinCounter.text = (coins + 1).ToString();
                Destroy(gameObject);
            }
        }
    }
}
