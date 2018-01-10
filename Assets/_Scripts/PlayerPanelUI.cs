using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerPanelUI : MonoBehaviour {
	public Text LevelNo, PlayerName, RemainingLives, HP, FillRatio;
	public RectTransform FillPanel,HealthPanel;
	public Image ColorPanel,Shape;
	public float lerpFillSpeed,lerpHPSpeed;
	float fillamount,hpRatio;
	public Snake SelectedSnake;

	float lerpFill,lerpHP;
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

	}

	public void UpdateDetails(){
		if (!SelectedSnake)
			return;

//		if(!SelectedSnake.isBot)
//			RemainingLives.text = "Lives : " + (SelectedSnake.Lives);
//		else
//			RemainingLives.text = "Lives : " + (ObliusGameManager.instance.EnemyLives-1);

		RemainingLives.text = "Lives : " + SelectedSnake.Lives.ToString();

		fillamount = ((float)SelectedSnake.ownedGroundPieces.Count / (900 - 60 - 28 - 28)) * 100; 

		FillRatio.text = fillamount.ToString ("0.00") + " %";
		HP.text = SelectedSnake.currentHP + "/" + SelectedSnake.maxHP;

		lerpFill = Mathf.MoveTowards (lerpFill, fillamount, Time.deltaTime * lerpFillSpeed);
		lerpHP = Mathf.MoveTowards (lerpHP, SelectedSnake.currentHP, Time.deltaTime * lerpHPSpeed);

		FillPanel.localScale = new Vector3 (lerpFill / 100, 1, 1);
		HealthPanel.localScale = new Vector3 (lerpHP / (float)SelectedSnake.maxHP, 1, 1);
	}
}
