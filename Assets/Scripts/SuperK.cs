using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperK : MonoBehaviour
{
    #region Movement And Positioning
    [SerializeField]
    int MovementSpeed;

    public GameObject PlayerGameObject;
    Vector3 PlayerPosition;
    #endregion
    #region FireModes
    [SerializeField]
    GameObject[] fireMode;
    /*
     * 0 = solo firing, 1 = combined, 2 Combined ChargeShot
     */

    #endregion
    #region Health and Regeration
    [SerializeField]
    int SKHealth;
    float RegenTimer; //for health or death
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        
    }

   public void AttachToPlayer()
    {
        //if stopped
        //if right click, go to player and attach
    }
    public void DetachFromPlayer()
    {
        //if already attached, shooot out ward until distance is reached or right click it pressed again
        //if distanced traveled stop
        //if right click is pressed and distance is not traveled stop
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if collided from bottom, attach
        //if collide with laser, take damage

    }
}
