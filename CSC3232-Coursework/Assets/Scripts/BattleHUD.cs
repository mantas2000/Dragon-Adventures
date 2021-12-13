using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private TextMeshProUGUI hpIndicator;
	[SerializeField] private Slider hpSlider;

	public void SetHUD(FighterInfo fighterInfo)
	{
		// Setup fighter HUD display
		nameText.text = fighterInfo.fighterName;
		hpSlider.maxValue = fighterInfo.maxHP;
		hpSlider.value = fighterInfo.currentHP;
		hpIndicator.text = fighterInfo.currentHP.ToString();
	}

	public void SetHP(int hp)
	{
		// Update HP slider
		hpSlider.value = hp;
		
		// Update HP indicator
		hpIndicator.text = hp.ToString();
	}

}
