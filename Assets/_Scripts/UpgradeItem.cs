using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeItem : MonoBehaviour
{
    public Text itemName, upgradeCost;
    public List<Transform> Bars;
    public List<int> UpgradeCosts, UpgradeValues;
    public int ID, baseValue, currentValue, upgradeLevel, maxLevel;
    public string playerprefsTag, upgradeName;
    public UpgradeType type;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Buy()
    {
        switch (type)
        {
            case UpgradeType.rocket:
                AccountDetails.instance.Save(rocket: 1);
                break;
            case UpgradeType.minishots:
                AccountDetails.instance.Save(minishots: 1);
                break;
            case UpgradeType.heal:
                AccountDetails.instance.Save(heal: 1);
                break;
            case UpgradeType.speed:
                AccountDetails.instance.Save(speed: 1);
                break;
            case UpgradeType.freeze:
                AccountDetails.instance.Save(freeze: 1);
                break;
            case UpgradeType.shield:
                AccountDetails.instance.Save(shield: 1);
                break;
            case UpgradeType.lives:
                AccountDetails.instance.Save(lives: 1);
                break;
            case UpgradeType.health:
                AccountDetails.instance.Save(health: 1);
                break;
            case UpgradeType.movespeed:
                AccountDetails.instance.Save(movespeed: 1);
                break;
            default:
                break;
        }
        AssignValues();

    }

    public void AssignValues()
    {
        switch (type)
        {
            case UpgradeType.rocket:
                upgradeLevel = AccountDetails.instance.accountDetails.scriptData.rocket;
                break;
            case UpgradeType.minishots:
                upgradeLevel = AccountDetails.instance.accountDetails.scriptData.minishots;
                break;
            case UpgradeType.heal:
                upgradeLevel = AccountDetails.instance.accountDetails.scriptData.heal;
                break;
            case UpgradeType.speed:
                upgradeLevel = AccountDetails.instance.accountDetails.scriptData.speed;
                break;
            case UpgradeType.freeze:
                upgradeLevel = AccountDetails.instance.accountDetails.scriptData.freeze;
                break;
            case UpgradeType.shield:
                upgradeLevel = AccountDetails.instance.accountDetails.scriptData.shield;
                break;
            case UpgradeType.lives:
                upgradeLevel = AccountDetails.instance.accountDetails.scriptData.lives;
                break;
            case UpgradeType.health:
                upgradeLevel = AccountDetails.instance.accountDetails.scriptData.health;
                break;
            case UpgradeType.movespeed:
                upgradeLevel = AccountDetails.instance.accountDetails.scriptData.movespeed;
                break;
            default:
                break;
        }

        UpdatePanel();


    }

    public void UpdatePanel()
    {

        currentValue = baseValue + (upgradeLevel * UpgradeValues[0]);
        switch (ID)
        {
            case 1:
                SnakesSpawner.instance.HealthValue = currentValue;
                break;
            case 2:
                SnakesSpawner.instance.SpeedValue = (float)currentValue / (float)25;
                break;
            case 3:
                SnakesSpawner.instance.LifeValue = currentValue;
                break;
        }

        itemName.text = upgradeName + "  ( " + currentValue + " )";

        for (int i = 0; i < upgradeLevel; i++)
        {
            Bars[i].GetChild(0).GetComponent<Image>().enabled = true;
            Bars[i].GetComponentInChildren<Text>().text = "";
        }


        if (upgradeLevel == 6)
        {
            GetComponent<Button>().interactable = false;
            //Bars [upgradeLevel - 1].GetComponentInChildren <Text> ().text = "MAX";
            upgradeCost.transform.gameObject.SetActive(false);
        }
        else
        {
            Bars[upgradeLevel].GetComponentInChildren<Text>().text = "+" + UpgradeValues[upgradeLevel].ToString();
            upgradeCost.text = UpgradeCosts[upgradeLevel].ToString();
        }

    }
}


public enum UpgradeType
{
    rocket, minishots, heal, speed, freeze, shield, lives, health, movespeed
}