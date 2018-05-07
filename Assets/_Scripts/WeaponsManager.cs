using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class WeaponsManager : MonoBehaviour {
	public List<Weapon> SelectedWeapons;
	public List<Weapon> AvailableWeapons;

	public List<WeaponSelectionItem> WeaponSlots;
	

	public static WeaponsManager instance;

	public GameObject AvailableWindow;
	public Transform gridParent;
	public GameObject weaponItem;
	public int ActiveSlot=1;
	public Sprite Shield, Speed, Freeze, Health, Minishots, Heatseeker;


	private void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OpenLoadOut(int slot)
	{
		ActiveSlot = slot;
        ActiveSlot = Mathf.Clamp(ActiveSlot, 1, 3);
        transform.parent.GetComponent<RectTransform>().DOLocalMoveY(1770, 0.75f);


        return;
		AvailableWindow.SetActive(true);
        DestroyChild(gridParent.gameObject);
        foreach (Weapon wpn in AvailableWeapons)
		{
			GameObject Go = Instantiate(weaponItem, gridParent);
			WeaponSelectionItem itm = Go.GetComponent<WeaponSelectionItem>();
			itm.currentWeapon = wpn;
			itm.Assign();
			
		}


	}

	public void AddWeapon(Weapon wpn)
	{
        
        AvailableWindow.SetActive(false);
        
		SelectedWeapons.Add(wpn);
		AvailableWeapons.Remove(wpn);

		if (SelectedWeapons.Count < 4)
		{
			WeaponSlots[SelectedWeapons.Count - 1].currentWeapon = wpn;
			WeaponSlots[SelectedWeapons.Count - 1].Assign();

		}
		else
		{
            AvailableWeapons.Add(WeaponSlots[ActiveSlot - 1].currentWeapon);
            SelectedWeapons.Remove(WeaponSlots[ActiveSlot - 1].currentWeapon);
            WeaponSlots[ActiveSlot - 1].currentWeapon = wpn;
			WeaponSlots[ActiveSlot - 1].Assign();
		}

        transform.parent.GetComponent<RectTransform>().DOLocalMoveY(770, 0.75f);
    }

	void DestroyChild(GameObject Parent)
	{
		foreach(Transform child in Parent.transform)
		{
			Destroy(child.gameObject);
		}

	}

    public Sprite GetWeaponImage(string weaponID)
    {

        switch (weaponID)
        {

            case "Speed":
                return Speed;

                

            case "Shield":
                return Shield;
                

            case "Health":
                return Health;
                

            case "Heatseeker":
                return Heatseeker;
                

            case "3Shots":
                return Minishots;
                

            case "Freeze":
                return Freeze;
                

            case "Mine":
                return Freeze;
                
            default:
                return Freeze;
        }
    }

	[System.Serializable]
	public class Weapon
	{
		public string Name, Cost, CooldownTime;
        public Color powerColor;
        public Sprite icon, btnIcon;
	}
}
