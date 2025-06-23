using UnityEngine;
using System.Collections.Generic;

public class SpellCast : MonoBehaviour
{
    public Transform startPos;
    public GameObject shootSprite;
    public GameObject coilSprite;

    private bool lookRight;

    void Update()
    {
        lookRight = gameObject.GetComponent<PlayerAttack>().lookRight;
    }

    public void HandleSpell(List<string> combo)
    {
        if (combo.Count == 1)
        {
            if (combo[0] == "Fire1")
            {
                ShootingSpell(new Color(1f, 0.6f, 0f));
                // Шутспелл с уроном без эффекта
            }
            else if (combo[0] == "Fire2")
            {
                SelfSpell(3, "SpeedBoost");
                // Ускорение
            }
            else if (combo[0] == "Fire3")
            {
                ShootingSpell(new Color(1f, 0.6f, 0f), "Burn");
                // Шутспелл с поджогом врага
            }
            else if (combo[0] == "Earth1")
            {
                ShootingSpell(new Color(0.25f, 0.25f, 0.25f), "Knockback");
                // Шутспелл с отталкиванием (тестирование)
            }
            else if (combo[0] == "Venom1")
            {
                ShootingSpell(new Color(0f, 1f, 0f), "Poison");
                // Шутспелл с отравлением (тестирование)
            }
            else
            {
                Debug.Log("Spell not learned.");
            }
        }
        else if (combo.Count == 2)
        {
            if (combo[0] == "Fire1" && combo[1] == "Fire2")
            {
                SelfSpell(3, "ExtraDamage");
                // Следующее заклинание дополнительно внесёт 3 урона
            }
            else if (combo[0] == "Fire1" && combo[1] == "Fire3")
            {
                CoilSpell(1, "Burn");
                // Ожог в области
            }
            else if (combo[0] == "Fire2" && combo[1] == "Fire3")
            {
                ShootingSpell(new Color(1f, 0.6f, 0f), "BurnLong");
                // Шутспелл с долгим поджогом врага
            }
            else
            {
                Debug.Log("Spell not learned.");
            }
        }
        else if (combo.Count == 3)
        {
            if (combo[0] == "Fire1" && combo[1] == "Fire2" && combo[2] == "Fire3")
            {
                ShootingSpell(new Color(1f, 0.6f, 0f), "PercentDamage");
                // Шутспелл с процентным уроном от хп врага
            }
        }
}

    private void ShootingSpell(Color colorOfShoot, string effectType="No effect")
    {
        GameObject obj = Instantiate(shootSprite, startPos.position, Quaternion.identity);
        obj.GetComponent<ShootSpell>().lookRight = lookRight;
        obj.GetComponent<ShootSpell>().effectType = effectType;
        obj.GetComponent<SpriteRenderer>().color = colorOfShoot;
    }

    private void CoilSpell(int range, string effectType="No effect")
    {
        GameObject obj = Instantiate(coilSprite, startPos.position, Quaternion.identity);
        obj.GetComponent<CoilSpell>().lookRight = lookRight;
        obj.GetComponent<CoilSpell>().range = range;
        obj.GetComponent<CoilSpell>().effectType = effectType;
    }

    private void SelfSpell(float amount, string effectType)
    {
        EffectsManager.Instance.effect.ApplyEffect(gameObject, gameObject, effectType, amount);
    }
}
