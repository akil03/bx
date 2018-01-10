using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingText : MonoBehaviour
{


    public Text textObject;
    public float waitTime;
    public string textString;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Anim());
    }

    // Update is called once per frame
    void Update()
    {

    }


    public IEnumerator Anim()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            textObject.text = textString + "";
            yield return new WaitForSeconds(waitTime);
            textObject.text = textString + ".";
            yield return new WaitForSeconds(waitTime);
            textObject.text = textString + "..";
            yield return new WaitForSeconds(waitTime);
            textObject.text = textString + "...";
        }
    }

}
