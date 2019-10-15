using UnityEngine;
using UnityEngine.UI;

public class UserAccountLobby : MonoBehaviour {

    public Text usernameText;

    private void Start()
    {
        if(UserAccountManager.IsLoggedIn)
            usernameText.text = "Logged In As: " + UserAccountManager.playerUsername;
    }
    public void LogOut()
    {
        if(UserAccountManager.IsLoggedIn)
            UserAccountManager.instance.LogOut();
    }

}
