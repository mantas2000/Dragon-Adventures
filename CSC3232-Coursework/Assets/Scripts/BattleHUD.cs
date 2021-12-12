using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
	[SerializeField] private Slider hpSlider;

	public void SetHUD(FighterInfo fighterInfo)
	{
		hpSlider.maxValue = fighterInfo.maxHP;
		hpSlider.value = fighterInfo.currentHP;
	}

	public void SetHP(int hp)
	{
		hpSlider.value = hp;
	}

}
