using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpellButton : MonoBehaviour
{
    public SpellBook sb;
    public Spell spell;
    public GameObject[] buttons;
    public Text[] textFields;
    public Sprite[] textures;
    public Image rune;

    public void selectSpell(int id)
    {
        int textFieldNumber = 0;
        spell = sb.spells[id];

        rune.sprite = textures[id];
        rune.gameObject.SetActive(true);

        textFields[textFieldNumber].text = spell.name;
        textFieldNumber++;

        textFields[textFieldNumber].text = "Mana cost: " + spell.manaCost;
        textFieldNumber++;

        if(spell.damage < 0)
            textFields[textFieldNumber].text = "Heal: " + (-1 * spell.damage);
        else
            textFields[textFieldNumber].text = "Damage: " + spell.damage;
        textFieldNumber++;

        if(spell.damageType == DamageType.Instant)
            textFields[textFieldNumber].text = "Type: " + spell.damageType;
        else if (spell.damageType == DamageType.Dot)
            textFields[textFieldNumber].text = "Type: Over time";
        else
            textFields[textFieldNumber].text = "Type: Instant + Over time";
        textFieldNumber++;

        if (spell.damageType == DamageType.Dot || spell.damageType == DamageType.InstantDot)
        {
            textFields[textFieldNumber].text = "Damage timer: " + spell.dotTimer;
            textFieldNumber++;
        }

        if (spell.aoe)
        {
            textFields[textFieldNumber].text = "AOE range: " + spell.aoeRange;
            textFieldNumber++;
        }

        if (spell.lifeTime > 0)
        {
            textFields[textFieldNumber].text = "Life time: " + spell.lifeTime + " sec";
            textFieldNumber++;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
        for (int i = 0; i < textFields.Length; i++)
        {
            textFields[i].gameObject.SetActive(true);
        }
    }

    public void Back()
    {
        if (!buttons[0].active)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].SetActive(true);
            }
            for (int i = 0; i < textFields.Length; i++)
            {
                textFields[i].text = "";
                textFields[i].gameObject.SetActive(false);
            }
            rune.gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OpenSpellBook()
    {
        gameObject.SetActive(true);
    }
}
