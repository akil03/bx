using System;
[Serializable]
public class Location
{
    public string country;
    public double latitide;
    public string city;
    public double longditute;
}
[Serializable]
public class ReservedCurrencies
{
}
[Serializable]
public class ReservedCurrency1
{
}

public class ReservedCurrency2
{
}
[Serializable]
public class ReservedCurrency3
{
}
[Serializable]
public class ReservedCurrency4
{
}
[Serializable]
public class ReservedCurrency5
{
}
[Serializable]
public class ReservedCurrency6
{
}
[Serializable]
public class AccountDetailsScriptData
{
    public string PING;
    public int MMR;
    public float mostAreaCovered;
    public int highestTrophies;
    public int totalKills;
    public int totalDeaths;
    public int totalWins;
    public int totalLoss;
    public string slot1 = "0";
    public string slot2 = "0";
    public string slot3 = "0";
    public string slot4 = "0";

    public int Gold;
    public int Gem;
    public int rocket, health, speed, freeze, lives, shield;
}
[Serializable]
public class VirtualGoods
{
}

[Serializable]
public class AccountDetailsData
{
    public int currency1;
    public int currency2;
    public int currency3;
    public int currency4;
    public int currency5;
    public int currency6;
    public string displayName;
    public ExternalIds externalIds;
    public Location location;
    public string requestId;
    public ReservedCurrencies reservedCurrencies;
    public ReservedCurrency1 reservedCurrency1;
    public ReservedCurrency2 reservedCurrency2;
    public ReservedCurrency3 reservedCurrency3;
    public ReservedCurrency4 reservedCurrency4;
    public ReservedCurrency5 reservedCurrency5;
    public ReservedCurrency6 reservedCurrency6;
    public AccountDetailsScriptData scriptData;
    public string userId;
    public string userName;
    public VirtualGoods virtualGoods;
}