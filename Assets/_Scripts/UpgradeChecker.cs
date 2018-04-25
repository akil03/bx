
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeChecker : MonoBehaviour {
	public UpgradeItem item;
	public Text heading, prefix, cost, upgradeValue,descriptionTxt;
	public Button upgradebutton;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void CheckBuy()
	{
		heading.text = item.upgradeName+ " (Level "+(item.upgradeLevel+1)+")";
		prefix.text = item.prefix+" : "+ item.currentValue;
		upgradeValue.text = "+" + item.UpgradeValues[item.upgradeLevel].ToString();
		cost.text = item.UpgradeCosts[item.upgradeLevel].ToString();
        string[] split = item.description.Split('\\');
        descriptionTxt.text = split[0] + "\n" + split[1];


        if (item.UpgradeCosts[item.upgradeLevel] > AccountDetails.instance.playerData.Gold)
		{
		   upgradebutton.interactable = false;
		}
		else
		{
		  //  cost.color = Color.gray;
			upgradebutton.interactable = true;
		}
		
	}

	public void Buy()
	{
		item.Purchase();

	}
}
