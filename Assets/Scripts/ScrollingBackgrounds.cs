using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackgrounds : MonoBehaviour
{
    [SerializeField]
    int Limit = -70;
    [SerializeField]
    int SpawnPoint = 70;
    [SerializeField]
    private int MovementSpeed =5;
    [SerializeField]
    bool IsOverHead;

    // Update is called once per frame
    void Update()
    {
        OverHeadReposition();
        transform.Translate(0, -MovementSpeed * Time.deltaTime,0);
        if(transform.position.y < Limit)
        {
            transform.position = new Vector2(0, SpawnPoint);
        }
    }
    //create spawn behind, down the road

    void OverHeadReposition()
    {
        if (IsOverHead)
        {
            if (transform.position.x == 0)
            {
                transform.position = new Vector2(Random.Range(Mathf.Max(-60, -40), Mathf.Max(60, 40)), SpawnPoint);
            }
            transform.Translate(0, -MovementSpeed * Time.deltaTime, 0);
            if (transform.position.y < Limit)
            {
                transform.position = new Vector2(Random.Range(Mathf.Max(-60,-40), Mathf.Max(60, 40)), SpawnPoint);
                return;
            }
        }
    }
}
