using System.Collections;
using UnityEngine;
using DatabaseControl;
using UnityEngine.SceneManagement;

public class UserAccountManager : MonoBehaviour {

    public static UserAccountManager instance;

    public string loggedInSceneName = "Lobby";
    public string loggedOutSceneName = "LoginMenu";

    public delegate void OnDataReceivedCallback(string data);

    private void Awake()
    {
        if(instance!=null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);

    }

    //These store the username and password of the player when they have logged in
    public static string playerUsername { get; protected set; }
    public static string playerPassword { get; protected set; }

    public static bool IsLoggedIn { get; protected set; }

    public void LogOut()
    {
        playerUsername = "";
        playerPassword = "";

        IsLoggedIn = false;

        SceneManager.LoadScene(loggedOutSceneName);
    }

    public void LogIn(string username, string password)
    {
        playerUsername = username;
        playerPassword = password;

        IsLoggedIn = true;

        SceneManager.LoadScene(loggedInSceneName);
    }

    IEnumerator GetData(OnDataReceivedCallback onDataReceived)
    {
        string data = "Error";

        IEnumerator e = DCF.GetUserData(playerUsername, playerPassword); // << Send request to get the player's data string. Provides the username and password
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Error")
        {
            //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
            //ResetAllUIElements();
            playerUsername = "";
            playerPassword = "";
            //loginParent.gameObject.SetActive(true);
            //loadingParent.gameObject.SetActive(false);
            //Login_ErrorText.text = "Error: Unknown Error. Please try again later.";
        }
        else
        {
            string DataRecived = response;
            data = DataRecived;

            //The player's data was retrieved. Goes back to loggedIn UI and displays the retrieved data in the InputField
            //loadingParent.gameObject.SetActive(false);
            //loggedInParent.gameObject.SetActive(true);
            //LoggedIn_DataOutputField.text = response;
        }
        if(onDataReceived!=null)
            onDataReceived.Invoke(data);
    }

    IEnumerator SetData(string data)
    {
        IEnumerator e = DCF.SetUserData(playerUsername, playerPassword, data); // << Send request to set the player's data string. Provides the username, password and new data string
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Success")
        {
            //The data string was set correctly. Goes back to LoggedIn UI
            //loadingParent.gameObject.SetActive(false);
            //loggedInParent.gameObject.SetActive(true);
        }
        else
        {
            //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
            //ResetAllUIElements();
            playerUsername = "";
            playerPassword = "";
            //loginParent.gameObject.SetActive(true);
            //loadingParent.gameObject.SetActive(false);
            //Login_ErrorText.text = "Error: Unknown Error. Please try again later.";
        }
    }


    public void GetUserData(OnDataReceivedCallback onDataReceived)
    {
        if (UserAccountManager.IsLoggedIn)
        {
            StartCoroutine(GetData(onDataReceived));
        }
    }

    public void SendUserData(string data)
    {
        if (UserAccountManager.IsLoggedIn)
        {
            StartCoroutine(SetData(data));
        }
    }

}
