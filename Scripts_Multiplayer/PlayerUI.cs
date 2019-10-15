using UnityEngine;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    RectTransform healthBarFill;

    private Player player;
    private PlayerMotor controller;

    public void SetPlayer(Player _player)
    {
        player = _player;
        controller = player.GetComponent<PlayerMotor>();
    }

    void Start()
    {
        Debug.Log("Leave room is enable!!!!!!");
        PauseMenu.IsOn = false;
        //if(PauseMenu.IsOn == false)
        //{
        //    pauseMenu.SetActive(false);
        //}
    }

    void Update () 
    {
        SetHealthAmount(player.GetHealthPct());

        if(Input.GetKeyDown(KeyCode.Q))
        {
            TogglePauseMenu();
        }
		
	}

  
    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.IsOn = pauseMenu.activeSelf;
    }

    void SetHealthAmount(float _amount)
    {
        healthBarFill.localScale = new Vector3(_amount,1f, 1f);
    }
}
