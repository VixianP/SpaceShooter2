using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerUps
{
    public GameObject Projectile;
    public int PowerUpDuration;
    public PowerUps(GameObject Projectile, int PowerUpDuration)
    {
        this.Projectile = Projectile;
        this.PowerUpDuration = PowerUpDuration;
    }
}
public class PowerUpScript : MonoBehaviour
{
    
    public List<PowerUps> PowerUp = new List<PowerUps>();
    [SerializeField]
    int PowerUpMoveDownSpeed = 2;
    private void Update()
    {
        MoveDown();
    }
    void MoveDown()
    {
        transform.Translate(0, -PowerUpMoveDownSpeed * Time.deltaTime, 0);
    }
}
