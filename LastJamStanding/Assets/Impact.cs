using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Impact : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public float impactLifetime = 4;
    public float impactFadeOut = 1;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, 3)];
        spriteRenderer.color = Color.clear;
        StartCoroutine("Spawn");
        StartCoroutine("Despawn");
    }
    IEnumerator Spawn()
    {
        float t = 0;
        Color c = spriteRenderer.color;
        while (t < 0.5)
        {
            c = new Color(c.r, c.g, c.b, t/0.5f);
            spriteRenderer.color = c;
            t += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator Despawn()
    {
        float t = 0;
        Color c = spriteRenderer.color;
        while (t < impactLifetime)
        {
            t += 0.2f;
            yield return new WaitForSeconds(0.2f);
        }
        t = 0;
        while (t < impactFadeOut)
        {
            c = new Color(c.r, c.g, c.b, 1 - t);
            spriteRenderer.color = c;
            t += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);

    }

}
