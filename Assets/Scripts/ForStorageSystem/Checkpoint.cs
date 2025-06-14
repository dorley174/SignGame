using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private int coins;
    
    void Start()
    {
    }

    // Method for checkpoint activation
    void Activation()
    {
        PlayerPrefs.Save();
        int totalCoins = PlayerPrefs.GetInt("coins");
        PlayerPrefs.SetInt("coins", totalCoins + coins);
    }
}
