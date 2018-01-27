using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeItem : MonoBehaviour {
	public Text itemName,upgradeCost;
	public List<Transform> Bars;
	public List<int> UpgradeCosts,UpgradeValues;
	public int ID,baseValue,currentValue,upgradeLevel,maxLevel;
	public string playerprefsTag,upgradeName;
	// Use this for initialization
	void Start () {
		//PlayerPrefs.DeleteKey (playerprefsTag);
		AssignValues ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void Buy(){

		upgradeLevel++;
		PlayerPrefs.SetInt (playerprefsTag, upgradeLevel);
		UpdatePanel();

	}

	public void AssignValues(){
		upgradeLevel = PlayerPrefs.GetInt (playerprefsTag);

		UpdatePanel ();


	}

	public void UpdatePanel(){
		
		currentValue = baseValue + (upgradeLevel*UpgradeValues[0]);
		switch (ID) {
		case 1:
			SnakesSpawner.instance.HealthValue = currentValue;
			break;
		case 2:
			SnakesSpawner.instance.SpeedValue = (float)currentValue/(float)20;
			break;
		case 3:
			SnakesSpawner.instance.LifeValue = currentValue;
			break;
		}

		itemName.text=upgradeName+"  ( "+currentValue+" )";

		for (int i = 0; i < upgradeLevel; i++) {
			Bars [i].GetChild (0).GetComponent <Image> ().enabled = true;
			Bars [i].GetComponentInChildren <Text> ().text = "";
		}


		if (upgradeLevel == 6) {
			GetComponent <Button> ().interactable = false;
			//Bars [upgradeLevel - 1].GetComponentInChildren <Text> ().text = "MAX";
			upgradeCost.transform.gameObject.SetActive (false);
		} else {
			Bars [upgradeLevel].GetComponentInChildren <Text> ().text = "+" + UpgradeValues [upgradeLevel].ToString ();
			upgradeCost.text = UpgradeCosts [upgradeLevel].ToString ();
		}

	}
}
