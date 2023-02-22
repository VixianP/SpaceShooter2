using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPointUIScript : MonoBehaviour
{

    float _delayExpire = 4;
    float _displayTime;

    bool _startTime;


    // Update is called once per frame
    void Update()
    {
        DisplayUI();
    }

    void DisplayUI()
    {

            if(_startTime == false )
            {
                _displayTime = Time.time + _delayExpire;
                _startTime = true;
            } 

            if(Time.time > _displayTime)
            {
                _startTime = false;
                gameObject.SetActive(false);
            }
        
    }
}
