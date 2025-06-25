using UnityEngine;

public class UIHPController : MonoBehaviour
{
    [SerializeField] private GameObject[] heartSlots;
    private int hp;

    void Start()
    {
        hp = PlayerPrefs.GetInt("hp");
        // for test
        PlayerPrefs.SetInt("hp", 10);
        UpdateHearts();
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("hp") != hp)
        {
            hp = PlayerPrefs.GetInt("hp");
            UpdateHearts();
        }
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < heartSlots.Length; i++)
        {
            GameObject full = heartSlots[i].transform.Find("fullHeart").gameObject;
            GameObject half = heartSlots[i].transform.Find("halfHeart").gameObject;

            int heartHp = i * 2;

            if (hp >= heartHp + 2)
            {
                full.SetActive(true);
                half.SetActive(false);
            }
            else if (hp == heartHp + 1)
            {
                full.SetActive(false);
                half.SetActive(true);
            }
            else
            {
                full.SetActive(false);
                half.SetActive(false);
            }
        }
    }
}
