using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerPanelUI : MonoBehaviour {
	public Text LevelNo, PlayerName, RemainingLives, HP, FillRatio, EnergyTxt;
	public RectTransform FillPanel,HealthPanel,EnergyPanel;
	public Image ColorPanel,Shape;
	public float lerpFillSpeed,lerpHPSpeed,lerpEnergy;
	public float fillamount,hpRatio;
	public Snake SelectedSnake;
    public RawImage AvatarTex;
	float lerpFill,lerpHP;
    public WeaponButton[] WB;
    public GameObject[] LivesImg;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		UpdateDetails ();
	}


	public void Init(){
		PlayerName.text = SelectedSnake.name;
		HP.text = SelectedSnake.currentHP + "/" + SelectedSnake.maxHP;
		ColorPanel.color = SelectedSnake.spriteColor;
		Shape.sprite = SelectedSnake.collectedPieceSprite;
		Shape.color = SelectedSnake.spriteColor;

		lerpHP = SelectedSnake.currentHP;
		lerpHP = 0;

        if (AvatarTex && SelectedSnake.AvatarCam)
            AvatarTex.texture = SelectedSnake.AvatarCam.targetTexture;

        if (WB.Length > 0)
        {
            //foreach (WeaponButton btn in WB)
            //    btn.AssignWeapon();

            for (int i = 0; i < WeaponsManager.instance.SelectedWeapons.Count; i++)
            {
                WB[i].weaponType = WeaponsManager.instance.SelectedWeapons[i].Name;
                WB[i].EnergyCost = int.Parse(WeaponsManager.instance.SelectedWeapons[i].Cost);
                WB[i].cooldownTime = int.Parse(WeaponsManager.instance.SelectedWeapons[i].CooldownTime);
                WB[i].AssignWeapon();
            }

            //for(int i = 0; i < 3; i++)
            //{
            //    WB[i].weaponType = WeaponsManager.instance.SelectedWeapons[i];  
            //    WB[i].AssignWeapon();
            //}
        }

    }

	public void UpdateDetails(){
		if (!SelectedSnake)
			return;

        //		if(!SelectedSnake.isBot)
        //			RemainingLives.text = "Lives : " + (SelectedSnake.Lives);
        //		else
        //			RemainingLives.text = "Lives : " + (ObliusGameManager.instance.EnemyLives-1);

        //RemainingLives.text = "Lives : " + SelectedSnake.Lives.ToString();
        UpdateLives();

        fillamount = ((float)SelectedSnake.ownedGroundPieces.Count / (900 - 60 - 28 - 28)) * 100;

        //if(SelectedSnake.playerID==1)
        //    AccountDetails.instance.Save(mostAreaCovered:fillamount);

		FillRatio.text = fillamount.ToString ("0.00") + " %";
		HP.text = SelectedSnake.currentHP + "/" + SelectedSnake.maxHP;

		lerpFill = Mathf.MoveTowards (lerpFill, fillamount, Time.deltaTime * lerpFillSpeed);
		lerpHP = Mathf.MoveTowards (lerpHP, SelectedSnake.currentHP, Time.deltaTime * lerpHPSpeed);
        lerpEnergy = Mathf.MoveTowards(lerpEnergy, SelectedSnake.energy, Time.deltaTime * lerpFillSpeed);


        FillPanel.localScale = new Vector3 (lerpFill / 100, 1, 1);
		HealthPanel.localScale = new Vector3 (lerpHP / (float)SelectedSnake.maxHP, 1, 1);

        if (EnergyPanel)
        {
            EnergyPanel.localScale = new Vector3(lerpEnergy / 10, 1, 1);
            EnergyTxt.text = Mathf.FloorToInt(SelectedSnake.energy).ToString();
            //EnergyTxt.text = (SelectedSnake.energy).ToString("0.00") + "/10";
        }
            


    }


    void UpdateLives()
    {
        foreach (GameObject Go in LivesImg)
            Go.SetActive(false);

        for (int i = 0; i < SelectedSnake.Lives; i++)
            LivesImg[i].SetActive(true);

    }
}
