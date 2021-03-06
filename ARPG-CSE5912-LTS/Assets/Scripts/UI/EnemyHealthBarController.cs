using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarController : HealthBarController
{
    public Slider lerpBar;
    protected override void Start()
    {
        base.Start();
        lerpBar.maxValue = healthBar.maxValue;
        lerpBar.value = healthBar.value;
    }
    protected override void UpdateSlider()
    {
        float lerpSpd = 10f;
        base.UpdateSlider();
        lerpBar.value = Mathf.Lerp(lerpBar.value, healthBar.value, Time.deltaTime *lerpSpd);

    }

}
//    //public Image healthBar;

//    //public float maxHealth = 100f;
//    //public float currHealth;

//    float lerpSpd;

//    // PlayerController_Script Player;  for updating current player health, not using at this point



//    private void Start()
//    {
//        // currHealth = maxHealth;

//        //stats = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Stats>();
//        //maxHealth = stats.maxHealth;

//        //stats.health = stats.maxHealth;
//        //currHealth = stats.health;
//    }

//    private void Update()
//    {
//        if (stats[StatTypes.HP] > stats[StatTypes.MaxHP]) stats[StatTypes.HP] = stats[StatTypes.MaxHP];
//        lerpSpd = 10f * Time.deltaTime;
//        HealthBarFiller();
//        colorChanger();
//        //currHealth = stats.health;

//    }

//    /* change the health bar with a lerp speed*/
//    public void HealthBarFiller()
//    {
//        //Debug.Log("Health" + stats[StatTypes.HEALTH]);
//        Debug.Log("Max Health" + stats[StatTypes.MaxHP]);
//        //need float division
//        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (float)stats[StatTypes.HP] / (float)stats[StatTypes.MaxHP], lerpSpd);
//    }
//    public void colorChanger()
//    {
//        //need fload division
//        Color healthC = Color.Lerp(Color.red, Color.green, (float)stats[StatTypes.HP] / (float)stats[StatTypes.MaxHP]);
//        healthBar.color = healthC;

//    }
//    //public void HitDamage(float damageRate)
//    //{
//    //    if(stats[StatTypes.HEALTH] > 0)
//    //    {
//    //        stats[StatTypes.HEALTH] -= damageRate;
//    //    }
//    //}
//    //public void healing(float healingRate)
//    //{
//    //    if (stats[StatTypes.HEALTH] < 100)
//    //    {

//    //        stats[StatTypes.HEALTH] += healingRate;
//    //        if (stats[StatTypes.HEALTH] > 100)
//    //        {
//    //            stats[StatTypes.HEALTH] = 100;
//    //        }
//    //    }
//    //}
//}
