using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class StatDisplay : MonoBehaviour
{
    [SerializeField]
    TMP_Text _hP;

    [SerializeField]
    TMP_Text _speed;

    [SerializeField]
    TMP_Text _damage;

    [SerializeField]
    TMP_Text _crtChance;

    Player _playerScript;



    public void UpdateStats()
    {

        if(_playerScript == null)
        {
            _playerScript = PlayerValues.playerGameobject.GetComponent<Player>();
        }

        if (_playerScript != null)
        {
            _hP.text = _playerScript.MaxHealth.ToString();

            _speed.text = _playerScript.BaseSpeed.ToString();

            _damage.text = _playerScript._PlayerDmg.ToString();
        }

    }
 
}
