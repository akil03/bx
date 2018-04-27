using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeaponSelectionItem : MonoBehaviour {
    public WeaponsManager.Weapon currentWeapon;
    public Image WeaponImg;
    public Text WeaponName, WeaponCost, CooldownTxt;
    public string WeaponType;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        WeaponsManager.instance.AddWeapon(currentWeapon);
    }

    public void Assign()
    {
        WeaponName.text = currentWeapon.Name;
        WeaponCost.text = "Energy Cost: "+currentWeapon.Cost;
        //CooldownTxt.text = "Cooldown Time: " + currentWeapon.CooldownTime;
        switch (currentWeapon.Name)
        {

            case "Speed":
                WeaponImg.sprite = WeaponsManager.instance.Speed;

                break;

            case "Shield":
                WeaponImg.sprite = WeaponsManager.instance.Shield;
                break;

            case "Health":
                WeaponImg.sprite = WeaponsManager.instance.Health;
                break;

            case "Heatseeker":
                WeaponImg.sprite = WeaponsManager.instance.Heatseeker;
                break;

            case "3Shots":
                WeaponImg.sprite = WeaponsManager.instance.Minishots;
                break;

            case "Freeze":
                WeaponImg.sprite = WeaponsManager.instance.Freeze;
                break;

            case "Mine":
                WeaponImg.sprite = WeaponsManager.instance.Freeze;
                break;
        }
    }
}
