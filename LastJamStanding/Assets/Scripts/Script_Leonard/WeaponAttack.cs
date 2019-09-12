using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    float animSpeed = 4;

    public IEnumerator Attack(AnimationCurve weaponAnim)
    {
        Collider2D polyCol = GetComponent<Collider2D>();
        polyCol.enabled = true;

        float timer = 1;

        while (timer > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(45, 0, weaponAnim.Evaluate(timer)));
            timer -= Time.fixedDeltaTime * animSpeed;
            yield return new WaitForFixedUpdate();
        }
        while(timer < 1)
        {
            transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(45, -45, weaponAnim.Evaluate(timer)));
            timer += Time.fixedDeltaTime * animSpeed;
            yield return new WaitForFixedUpdate();
        }
        while (timer > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(0, -45, weaponAnim.Evaluate(timer)));
            timer -= Time.fixedDeltaTime * animSpeed;
            yield return new WaitForFixedUpdate();
        }

        polyCol.enabled = false;
    }
}
