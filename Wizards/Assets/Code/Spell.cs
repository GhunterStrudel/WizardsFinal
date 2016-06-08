using UnityEngine;
using System.Collections;

public enum CastType
{
	Target, Direction, Channel, Uproot, Summon
};

public enum DamageType
{
	Instant, Dot, InstantDot, Shield
};

public enum ElementType
{
	Fire, Air, Ice, Earth, Dark, Light, None
}

[System.Serializable]
public class Spell 
{
	public int spellID;
	public string name;
	public int manaCost;
	public float damage;
	public ElementType elementType;
	public CastType castType;
	public DamageType damageType;
	public float dotTimer;
	public bool aoe;
	public float aoeRange;
	public bool bouncing;
	public bool cluster;
	public Color baseColor;
	public Texture effectTexture;
	public float lifeTime;
    public bool unlocked;
}

