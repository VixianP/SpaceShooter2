using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPointUIScript : MonoBehaviour
{
    [SerializeField]
    GameObject _inputDisplay1;
    [SerializeField]
    GameObject _inputDisplay2;
    [SerializeField]
    GameObject _inputDisplay3;


    Animator _inputanim1;
    Animator _inputanim2;
    Animator _inputanim3;


    void Start()
    {
        _inputanim1 = _inputDisplay1.GetComponent<Animator>();
        _inputanim2 = _inputDisplay2.GetComponent<Animator>();
        _inputanim3 = _inputDisplay3.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        DisplayUI();

    }

    public void ShowAll()
    {
        _inputanim1.SetBool("Display", true);
        _inputanim2.SetBool("Display2", true);
        _inputanim3.SetBool("Display3", true);

        _inputanim1.SetBool("Close", false);
        _inputanim2.SetBool("Close2", false);
        _inputanim3.SetBool("Close3", false);
    }

    public void CloseAll()
    {
        _inputanim1.SetBool("Close", true);
        _inputanim2.SetBool("Close2", true);
        _inputanim3.SetBool("Close3", true);
    }

    void DisplayUI()
    {
        if (_inputanim1 != null && _inputanim2 != null && _inputanim3 != null)
        {
            if (_inputanim1.GetCurrentAnimatorStateInfo(0).IsName("Hold"))
            {
                _inputanim1.SetBool("Display", false);
            }
            if (_inputanim2.GetCurrentAnimatorStateInfo(0).IsName("Hold2"))
            {
                _inputanim2.SetBool("Display2", false);
            }
            if (_inputanim3.GetCurrentAnimatorStateInfo(0).IsName("Hold3"))
            {
                _inputanim3.SetBool("Display3", false);
            }
        }
    }
}
