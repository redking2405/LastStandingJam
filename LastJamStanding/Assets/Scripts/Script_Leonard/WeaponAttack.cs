using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    void Start()
    {
        
    }

    public IEnumerator Attack()
    {
        float timer = 5;
        while(timer > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(90, 0, timer / 5));
            timer -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        while(timer < 5)
        {
            transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(90, 0, timer / 5));
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
