using UnityEngine;

public class SphereProperties : MonoBehaviour
{
    public Material[] silverDark, silverLight, silverMixed, goldDark, goldLight, goldMixed, crysDark, crysLight, crysMixed;
    public MeshRenderer[] sphereMeshes;
    public RandomSpin top, bottom;
    public Animator[] Holder;

    public GameObject SphereObject;

    public Camera _cam;
    // Use this for initialization
    void Start()
    {

        _cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

    }



    void Show()
    {

        SphereObject.SetActive(true);
    }


    void Hide()
    {

        SphereObject.SetActive(false);
    }


    [ContextMenu("Idle")]
    public void Idle()
    {
        top.Stop();
        bottom.Stop();

    }
    [ContextMenu("Unlock")]
    public void StartUnlock()
    {

        top.Spin();
        bottom.Spin();
    }

    [ContextMenu("ApplySilver")]
    public void ApplySilver()
    {
        sphereMeshes[0].materials = silverDark;
        sphereMeshes[1].materials = silverLight;
        sphereMeshes[2].materials = silverDark;
        sphereMeshes[3].materials = silverMixed;


        sphereMeshes[4].materials = silverDark;
        sphereMeshes[5].materials = silverLight;
        sphereMeshes[6].materials = silverDark;
        sphereMeshes[7].materials = silverMixed;

    }


    [ContextMenu("ApplyGold")]
    public void ApplyGold()
    {



        sphereMeshes[0].materials = goldDark;
        sphereMeshes[1].materials = goldLight;
        sphereMeshes[2].materials = goldDark;
        sphereMeshes[3].materials = goldMixed;


        sphereMeshes[4].materials = goldDark;
        sphereMeshes[5].materials = goldLight;
        sphereMeshes[6].materials = goldDark;
        sphereMeshes[7].materials = goldMixed;




    }
    [ContextMenu("ApplyCrystal")]
    public void ApplyCrystal()
    {
        sphereMeshes[0].materials = crysDark;
        sphereMeshes[1].materials = crysLight;
        sphereMeshes[2].materials = crysDark;
        sphereMeshes[3].materials = crysMixed;


        sphereMeshes[4].materials = crysDark;
        sphereMeshes[5].materials = crysLight;
        sphereMeshes[6].materials = crysDark;
        sphereMeshes[7].materials = crysMixed;

    }

    [ContextMenu("Animate")]
    public void OpenSphere()
    {
        _cam.enabled = true;
        Holder[0].GetComponent<Animator>().Play("OpenChest");
        Holder[1].GetComponent<Animator>().Play("OpenChest");

    }
}