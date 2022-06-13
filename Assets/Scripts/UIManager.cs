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
    bool GameOver = false;
    //to be optmized later.
    private void Update()
    {
        PlayerScoreText.text = "Score " + PlayerValues.Score;
        OnDeath();

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


}
