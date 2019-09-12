using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class Flic : MonoBehaviour
{





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            if (collision.gameObject.GetComponent<UserControl>() != null)
            {
                collision.gameObject.GetComponent<UserControl>().Respawn();
            }
        }
    }
}
