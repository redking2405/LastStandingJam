using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    // Randomizers
    float r_movestop, r_speed, r_orientation;

    // Player reference
    [SerializeField]
    private Transform p_transform;

    void Start()
    {
        r_movestop = Random.Range(0, 5); // the cooldown between moves
        r_speed = Random.Range(1, 3);    // the speed when moving the next time
        r_orientation = p_transform.rotation.eulerAngles.z;  // the character rotation
    }

    // Update is called once per frame
    void Update()
    {
        if (r_movestop == 0)
        {

        }
    }
}
