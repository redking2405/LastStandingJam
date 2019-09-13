using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kid : MonoBehaviour
{
    public int score;
    public GameObject Blood;
    public float chanceOfWeapon;

    public void Death()
    {
        Instantiate(Blood, transform.position, Quaternion.identity);
        float ran = Random.Range(0, 1f);

        if (ran > chanceOfWeapon)
        {
            GameManager.Instance.InstantiateWeapon(transform.position);
        }

        Destroy(gameObject);
    }
}
