using UnityEngine;
using UnityEngine.UI;

public class SetVersion : MonoBehaviour
{
    public Text element;

    private void Start()
    {
        element.text = Application.version;
    }
}
