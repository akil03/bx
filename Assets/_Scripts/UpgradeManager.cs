using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    private void Awake()
    {
        instance = this;
    }


    public void UpgradeRocket()
    {
        if (AccountDetails.instance.accountDetails.scriptData.rocket < 6)
        {
            AccountDetails.instance.Save(rocket: 1);
        }
    }

    public void UpgradeHealth()
    {
        if (AccountDetails.instance.accountDetails.scriptData.health < 6)
        {
            AccountDetails.instance.Save(health: 1);
        }
    }

    public void UpgradeSpeed()
    {
        if (AccountDetails.instance.accountDetails.scriptData.speed < 6)
        {
            AccountDetails.instance.Save(speed: 1);
        }
    }

    public void UpgradeFreeze()
    {
        if (AccountDetails.instance.accountDetails.scriptData.freeze < 6)
        {
            AccountDetails.instance.Save(freeze: 1);
        }
    }

    public void UpgradeLives()
    {
        if (AccountDetails.instance.accountDetails.scriptData.lives < 6)
        {
            AccountDetails.instance.Save(lives: 1);
        }
    }

    public void UpgradeShield()
    {
        if (AccountDetails.instance.accountDetails.scriptData.shield < 6)
        {
            AccountDetails.instance.Save(shield: 1);
        }
    }
}
