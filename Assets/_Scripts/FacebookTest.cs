using Facebook.Unity;
using UnityEngine;

public class FacebookTest : MonoBehaviour
{
    public FacebookActor facebookActor;

    private void OnGUI()
    {
        GUILayout.Label("FB Init:" + FB.IsInitialized.ToString());
        GUILayout.Label("FB Login:" + FB.IsLoggedIn.ToString());
        if (!FB.IsLoggedIn)
        {
            if (GUILayout.Button("Login"))
            {
                facebookActor.Login();
            }
        }
        if (FB.IsLoggedIn)
        {
            if (GUILayout.Button("Logout"))
            {
                facebookActor.Logout();
            }
        }
    }
}
