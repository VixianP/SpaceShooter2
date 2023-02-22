using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUpNotification : MonoBehaviour
{

    float _delay = 4;
    float _timer;

    bool _counting;
    [SerializeField]
    bool _adding;

    public int OverAllEXPGain;


    Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _timer = Time.time + _delay;
    }

    // Update is called once per frame
    void Update()
    {
        NotifyExpire();

        if(_adding == true)
        {
            gameObject.GetComponent<TextMeshProUGUI>().text = "+ " + OverAllEXPGain;
        }
    }

    void NotifyExpire()
    {
        if(_counting == true)
        {
            if(Time.time > _timer)
            {
                _animator.SetBool("Notify", false);
                _counting = false;
                OverAllEXPGain = 0;
            }
        }
    }

    public void StartNotification()
    {
        _animator.SetBool("Notify", true);
        _timer = Time.time + _delay;
        _counting = true;
    }
}
