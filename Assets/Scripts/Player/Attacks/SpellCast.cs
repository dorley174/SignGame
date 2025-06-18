using UnityEngine;
using System.Collections.Generic;

public class SpellCast : MonoBehaviour
{
    public Transform startPos;
    public GameObject shootSprite;
    public GameObject coilSprite;

    public float shootRange = 10f;
    public float CoilRange = 4f;

    void Update()
    {

    }

    public void HandleSpell(List<string> combo)
    {
        if (combo[0] == "Fire")
        {
            if (combo.Count == 1)
            {
                CoilSpell(1);
            }
            else if (combo.Count == 2)
            {
                if (combo[1] == "Fire")
                {
                    CoilSpell(2);
                }
            }
            else if (combo.Count == 3)
            {
                if (combo[1] == "Fire")
                {
                    if (combo[2] == "Fire")
                    {
                        CoilSpell(3);
                    }
                }
            }
        }
        else if (combo[0] == "Earth")
        {
            ShootingSpell(new Color(0.25f, 0.25f, 0.25f), "Knockback");
        }
        else if (combo[0] == "Venom")
        {
            ShootingSpell(new Color(0f, 1f, 0f), "Poison");
        }
    }

    private void ShootingSpell(Color colorOfShoot, string tagForSpell)
    {
        GameObject obj = Instantiate(shootSprite, startPos.position, Quaternion.identity);
        obj.GetComponent<SpriteRenderer>().color = colorOfShoot;
        obj.tag = tagForSpell;
    }

    private void CoilSpell(int range)
    {
        Vector3 distance = new Vector3(CoilRange*range, 0, 0);
        Instantiate(coilSprite, startPos.position + distance, Quaternion.identity);
    }

    private void SelfSpell()
    {
        
    }
}
