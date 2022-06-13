using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerUps
{
    public GameObject Projectile;
    public int PowerUpDuration;
    public int Damage;
    public string Category;
    public string Type;
    public string CallMethod;
    public Sprite PowerUpImage;
    //category
    //type temp or perm
    //sprite to show on sprite renderer

    public PowerUps(GameObject Projectile, int PowerUpDuration, int Damage,string Category,string Type,string CallMethod, Sprite PowerUpImage)
    {
        this.Projectile = Projectile;
        this.PowerUpDuration = PowerUpDuration;
        this.Damage = Damage;
        this.Category = Category;
        this.Type = Type;
        this.CallMethod = CallMethod;
        this.PowerUpImage = PowerUpImage;

    }
}
public class PowerUpScript : MonoBehaviour
{
    
    public List<PowerUps> PowerUp = new List<PowerUps>();
    
    public int PowerUpSelector;
    [SerializeField]
    int PowerUpMoveDownSpeed = 2;

    private void Start()
    {
        PowerUpSelector = Random.Range(0, PowerUp.Count);
        if(gameObject.GetComponent<SpriteRenderer>().sprite == null && PowerUp[PowerUpSelector].PowerUpImage != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = PowerUp[PowerUpSelector].PowerUpImage;
        }
    }
    private void Update()
    {
        MoveDown();
    }
    void MoveDown()
    {
        transform.Translate(0, -PowerUpMoveDownSpeed * Time.deltaTime, 0);
    }
}
