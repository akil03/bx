
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeChecker : MonoBehaviour {
	public UpgradeItem item;
    public WeaponsManager.Weapon selectedweapon;
    public Image WeaponImg;
	public Text heading, prefix, cost, upgradeValue,descriptionTxt,energyTxt,cooldownTxt;
	public Button upgradebutton,useBtn;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void CheckBuy()
	{
        


        if (item.isWeapon)
        {
            selectedweapon = WeaponsManager.instance.AvailableWeapons.Find(i => i.Name == item.playerprefsTag);

            if (selectedweapon != null)
                useBtn.interactable = true;
            else
            {
                selectedweapon = WeaponsManager.instance.SelectedWeapons.Find(i => i.Name == item.playerprefsTag);
                useBtn.interactable = false;
            }
                

            energyTxt.text = "Energy Cost : " + item.energyCostTxt.text;
            cooldownTxt.text = "Cooldown : " + item.cooldownTxt.text;

            WeaponImg.sprite = WeaponsManager.instance.GetWeaponImage(item.playerprefsTag);
            WeaponImg.color = selectedweapon.powerColor;
        }
        else
        {


        }


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

    public void Use()
    {

        WeaponsManager.instance.AddWeapon(selectedweapon);
    }

	public void Buy()
	{
		item.Purchase();

	}
}
