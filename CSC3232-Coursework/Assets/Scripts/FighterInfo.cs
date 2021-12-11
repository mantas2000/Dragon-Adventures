using UnityEngine;

public class FighterInfo : MonoBehaviour
{

	public string fighterName;
	public int damage;
	public int maxHP;
	public int currentHP;

	public bool TakeDamage(int amount)
	{
		// Take damage
		currentHP -= amount;

		return currentHP <= 0;
	}

	public void Heal(int amount)
	{
		// Increase HP
		currentHP += amount;
		
		// Ensure current HP doesn't exceed limits
		if (currentHP > maxHP) currentHP = maxHP;
	}

}
