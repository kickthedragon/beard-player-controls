using UnityEngine;
using System.Collections;

public enum DamageTypeEnum
{
	Piercing = 0,
	Slashing = 1,
	Bludgeoning = 2,
	Fire = 3,
	Cold = 4,
	Poison = 5,
	Falling = 6,

}

public abstract class DamageType : Enumeration {
	
	public static readonly DamageType Piercing = new PiercingType();
	public static readonly DamageType Slashing = new SlashingType();
	public static readonly DamageType Bludgeoning = new BludgeoningType();
	public static readonly DamageType Fire = new FireType();
	public static readonly DamageType Cold = new ColdType();
	public static readonly DamageType Poison = new PoisonType();
	public static readonly DamageType Falling = new FallingType();
	
	public Color HudColor { get; private set; }

	public DeathType DeathType { get; private set; }

	
	private DamageType()
	{
	}
	
	private DamageType(int value, string displayName) : base(value, displayName)
	{
	}
	

	
	private class PiercingType : DamageType
	{
		
		public PiercingType() : base(0, "Piercing")
		{
			HudColor = Color.gray;
			DeathType = DeathType.NormalMob;
		}
		
	}
	
	
	private class SlashingType : DamageType
	{
		
		public SlashingType() : base(1, "Slashing")
		{
			HudColor = Color.gray;
			DeathType = DeathType.NormalMob;
		}
		
	}

	
	private class BludgeoningType : DamageType
	{
		
		public BludgeoningType() : base(2, "Bludgeoning")
		{
			HudColor = Color.gray;
			DeathType = DeathType.NormalMob;
		}

	}

	
	private class FireType : DamageType
	{
		
		public FireType() : base(3, "Fire")
		{
			HudColor = new Color(.8f, .5f, .2f);
			DeathType = DeathType.NormalMob;
		}
		
	}
	
	private class ColdType : DamageType
	{
		
		public ColdType() : base(4, "Cold")
		{
			HudColor = Color.blue;
			DeathType = DeathType.NormalMob;
		}
		
	}
	
	private class PoisonType : DamageType
	{
		
		public PoisonType() : base(5, "Poison")
		{
			HudColor = Color.green;
			DeathType = DeathType.NormalMob;
		}
		
	}

	
	private class FallingType : DamageType
	{
		
		public FallingType() : base(6, "Falling")
		{
			HudColor = Color.red;
			DeathType = DeathType.NormalMob;
		}
		
	}
	
}
