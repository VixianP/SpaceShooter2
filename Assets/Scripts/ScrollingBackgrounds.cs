using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackgrounds : MonoBehaviour
{
    [SerializeField]
    private int MovementSpeed =5;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, -MovementSpeed * Time.deltaTime,0);
        if(transform.position.y < -70)
        {
            transform.position = new Vector2(0, 70);
        }
    }
}
