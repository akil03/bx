using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WeaponButton : MonoBehaviour {
	public Image CoolDownImage,selectedWeapon;
	public int EnergyCost,cooldownTime;
	public string weaponType;
	public Sprite[] WeaponImgs;
	public Snake SelectedSnake;
	public Text costTxt, cdTxt;
	float useTime;
	float timeDiff;

    public Ease easetype;
    public Vector3 minScale, maxScale;
    public float duration;

    Sequence seq;

    private void OnEnable()
    {
        //weaponType
    }


    // Use this for initialization
    void Start () {
        seq = DOTween.Sequence();
        //GetComponent<UIButton>().StopNormalStateAnimations();

    }
	
	// Update is called once per frame
	void Update () {

		if (!SelectedSnake)
			return;
		
		if (SelectedSnake.energy < EnergyCost || CoolDownImage.IsActive())
		{
			if (CoolDownImage.IsActive())
			{
				timeDiff = Time.time-useTime;

				cdTxt.text = Mathf.FloorToInt(cooldownTime- timeDiff).ToString()+"S";
			}
            if (isAnimating)
                EndAnim();
			GetComponent<Button>().interactable = false;
		}
		else
		{
            if (!isAnimating)
                StartAnim();
            GetComponent<Button>().interactable = true;
			
		}
			



	}

	public void AssignWeapon()
	{
		SelectedSnake = GUIManager.instance.inGameGUI.PlayerPanel[0].SelectedSnake;
		switch (weaponType)
		{

			case "Speed":
				selectedWeapon.sprite = GUIManager.instance.inGameGUI.Speed;
				//EnergyCost = 4;
				//cooldownTime = 10;
				break;

			case "Shield":
				selectedWeapon.sprite = GUIManager.instance.inGameGUI.Shield;
				//EnergyCost = 3;
				//cooldownTime = 10;
				break;

			case "Health":
				selectedWeapon.sprite = GUIManager.instance.inGameGUI.Health;
				//EnergyCost = 3;
				//cooldownTime = 10;
				break;

			case "Heatseeker":
				selectedWeapon.sprite = GUIManager.instance.inGameGUI.Heatseeker;
				//EnergyCost = 9;
				//cooldownTime = 30;
				break;

			case "3Shots":
				selectedWeapon.sprite = GUIManager.instance.inGameGUI.Minishots;
				//EnergyCost = 5;
				//cooldownTime = 20;
				break;

			case "Freeze":
				selectedWeapon.sprite = GUIManager.instance.inGameGUI.Freeze;
				//EnergyCost = 3;
				//cooldownTime = 10;
				break;

			case "Mine":
				selectedWeapon.sprite = GUIManager.instance.inGameGUI.Freeze;
				//EnergyCost = 3;
				//cooldownTime = 10;
				break;


		}
		costTxt.text = EnergyCost.ToString();
	}

	public void OnClick()
	{
        EndAnim();

        SelectedSnake.energy -= EnergyCost; 

		switch (weaponType)
		{

			case "Speed":
				SelectedSnake.ActivateSpeed();
				break;

			case "Shield":
				SelectedSnake.ActivateShield();
				break;

			case "Health":
				SelectedSnake.UseHealth();
				break;

			case "Heatseeker":
				SelectedSnake.ActivateMissile();
				break;

			case "3Shots":
				SelectedSnake.ActivateBlasters();
				break;

			case "Freeze":
				SelectedSnake.FireFreeze();
				break;

			case "Mine":
				SelectedSnake.DropMine();
				break;


		}
		
		CoolDownImage.gameObject.SetActive(true);
		CoolDownImage.fillAmount = 1;

		CoolDownImage.DOFillAmount(0, cooldownTime).SetEase(Ease.Linear).OnComplete(()=>
		{
			CoolDownImage.gameObject.SetActive(false);
		});
		useTime = Time.time;

	}

    bool isAnimating;
    void StartAnim()
    {
        isAnimating = true;
        maxScale = new Vector3(1.05f, 1.05f, 1.05f);
        seq.Append(
         GetComponent<RectTransform>().DOScale(minScale, duration).SetEase(easetype).OnComplete(() =>
         {
             GetComponent<RectTransform>().DOScale(maxScale, duration).SetEase(easetype).OnComplete(() => {
                 StartAnim();
             });
         }));
    }


    void EndAnim()
    {
        isAnimating = false;
        maxScale = minScale;
        seq.Kill();
        GetComponent<RectTransform>().localScale = minScale;
        
    }

	public enum WeaponType
	{
		Speed, Shield, Health, Heatseeker, MiniShots, Freeze, Mine
	}
}
