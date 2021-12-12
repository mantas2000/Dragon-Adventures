using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private Slider hpSlider;

	public void SetHUD(FighterInfo fighterInfo)
	{
		nameText.text = fighterInfo.fighterName;
		hpSlider.maxValue = fighterInfo.maxHP;
		hpSlider.value = fighterInfo.currentHP;
	}

	public void SetHP(int hp)
	{
		hpSlider.value = hp;
	}

}
