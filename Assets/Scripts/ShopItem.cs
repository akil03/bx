using UnityEngine;
using System.Collections;

public class ShopItem : MonoBehaviour
{

    //THESE VALUES ARE REFERENCES FOR THE STORE

    public int price;
    //THESE VALUES ARE REFERENCES FOR THE STORE

    [HideInInspector]
    public MeshRenderer sprite;


    public Vector3 originalScale;

    [HideInInspector]
    public bool unlocked;

    public bool unlockedByDefault;

    // Use this for initialization
    void Awake()
    {

		sprite = GetComponent<MeshRenderer>();
        originalScale = transform.localScale;

    }

    void Start()
    {

        if (ShopHandler.instance.GetBoughtItemsNames().Contains(gameObject.name) || unlockedByDefault)
        {
            unlocked = true;
        }
        else
        {
            unlocked = false;
        }

    }

    // Update is called once per frame
    void Update()
    {


    }


    public void Buy()
    {
        unlocked = true;
        ShopHandler.instance.UnlockShopItem(this);
    }



}
