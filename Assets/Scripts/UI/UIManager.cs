using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Text
    [SerializeField]
    private Text PlayerScoreText;
    [SerializeField]
    private Text PlayerLives;
    [SerializeField]
    private Text CurrencyText;
    #endregion

    #region Canvases
    [SerializeField]
    GameObject PlayerUserInterface;
    [SerializeField]
    GameObject GameOverScreen;
    [SerializeField]
    GameObject PauseMenu;
    #endregion

    #region External Scripts & objects

    [SerializeField]
    GameObject _statWindow;
    StatDisplay _statWindowScript;

    #endregion
    bool GameOver = false;
    //to be optmized later.

     void Start()
    {
        _statWindowScript = _statWindow.GetComponent<StatDisplay>();
    }

    private void Update()
    {
        PlayerScoreText.text = "Score " + PlayerValues.Score;
        OnDeath();
        if (Input.GetKey(KeyCode.Escape))
        {
            _statWindowScript.UpdateStats();
            PauseGame(0);
        }

    }
    void OnDeath()
    {
        if (PlayerValues.PlayerIsDead == true && GameOver == false)
        {
            PlayerUserInterface.SetActive(false);
            GameOverScreen.SetActive(true);
            GameOver = true;
        }
    }

    public void PauseGame(int timescale)
    {
            PlayerUserInterface.SetActive(false);
            PauseMenu.SetActive(true);
            Time.timeScale = timescale;
    }
}
