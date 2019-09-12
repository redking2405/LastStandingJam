using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kid : MonoBehaviour
{

    public int score;
    public GameObject Blood;
    public GameObject Weapon;
    public float chanceOfWeapon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Death()
    {
        Instantiate(Blood, transform.position, Quaternion.identity);
        float ran = Random.Range(0, 1f);

        if (ran > chanceOfWeapon)
        {
            Instantiate(Weapon, transform.position, Quaternion.identity);
        }


        Destroy(gameObject);
    }
}
