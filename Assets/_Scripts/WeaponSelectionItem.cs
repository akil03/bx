using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeaponSelectionItem : MonoBehaviour {
    public WeaponsManager.Weapon currentWeapon;
    public Image WeaponImg;
    public Text WeaponName, WeaponCost, CooldownTxt;
    public string WeaponType;
    public GameObject EmptyTxt;
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
        EmptyTxt.SetActive(false);
        WeaponName.text = currentWeapon.Name;
        WeaponCost.text = ""+currentWeapon.Cost;
        WeaponImg.color = currentWeapon.powerColor;
        WeaponCost.transform.parent.gameObject.SetActive(true);
        WeaponCost.transform.parent.GetComponent<Image>().color = currentWeapon.powerColor;
        CooldownTxt.text = "Cooldown: " + currentWeapon.CooldownTime;

        WeaponImg.sprite = WeaponsManager.instance.GetWeaponImage(currentWeapon.Name);

       
    }
}
