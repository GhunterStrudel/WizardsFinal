using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {
    public float startingHealth = 100f;
    public float startingMana = 100f;
    public float manaRegenRate = 1f;
    public float failSpellManaCost = 5f;
    public float failSpellHealthThreshold = 50f;
    public float failSpellHealthCost = 5f;
    public Slider healthSlider;
    public Slider manaSlider;

    public float currenthealth;
    float currentMana;

    // Use this for initialization
    void Start () {
        currenthealth = startingHealth;
        currentMana = startingMana;
    }
	
	// Update is called once per frame
	void Update () {
        if (currentMana < startingMana)
        {
            currentMana += manaRegenRate * Time.deltaTime;
           manaSlider.value = currentMana;
        }

        healthSlider.value = currenthealth;
	}

    public void getHit(float damage)
    {
        currenthealth -= damage;

        if (currenthealth > startingHealth)
            currenthealth = startingHealth;

        if(currenthealth <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public bool canCast(float manaCost)
    {
        if (manaCost <= currentMana)
        {
            currentMana -= manaCost;
            manaSlider.value = currentMana;
            return true;
        }
        else
            return false;
    }

    public void failSpell(float score)
    {
        currentMana -= failSpellManaCost;
        manaSlider.value = currentMana;

        if(score <= failSpellHealthThreshold)
        {
            getHit(failSpellHealthCost);
        }
    }
}
