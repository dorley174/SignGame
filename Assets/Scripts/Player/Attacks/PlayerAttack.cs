using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public string enemyTag = "Enemy";
    
    public float timeAvailableForCombo = 1f;

    private SpellCast spell; // реализация разных видов заклинаний

    private List<string> actualCombo = new List<string>();
    private float timer;
    private bool inputForTimer = false;
    private bool timerIsOn = false;


    private void Awake()
    {
        spell = GetComponent<SpellCast>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("1 was pressed");
            HandleInput("Fire");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("2 was pressed");
            HandleInput("Water");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("3 was pressed");
            HandleInput("Earth");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("4 was pressed");
            HandleInput("Air");
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("5 was pressed");
            HandleInput("Venom");
        }

        //CheckMouseClickOnEnemy();
    }

    private void HandleInput(string symb)
    {
        inputForTimer = true;
        if (timerIsOn == false)
        {
            timerIsOn = true;
            StartCoroutine(ComboTimer(timeAvailableForCombo));
        }
        actualCombo.Add(symb);   
    }

    private IEnumerator ComboTimer(float timeAvailableForCombo)
    {
        while (timer < timeAvailableForCombo)
        {
            if (inputForTimer)
            {
                timer = 0f;
                inputForTimer = false;
                Debug.Log("Timer started!");
            }
            else
            {
                Debug.Log($"timer: {timer}");
                timer += 0.1f;
            }
            yield return new WaitForSeconds(0.1f);
        }

        MakeAttack(actualCombo);
        Debug.Log("Timer is out!");
        actualCombo.Clear();
        timer = 0f;
        timerIsOn = false;
    }

    private void MakeAttack(List<string> combo)
    {
        spell.HandleSpell(combo);
        Debug.Log($"Использована комбинация: {string.Join("+", combo)}");
    }
}