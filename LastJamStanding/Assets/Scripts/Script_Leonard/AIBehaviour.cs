using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof(Character2D))]
    public class AIBehaviour : MonoBehaviour
    {
        private Character2D m_NPC;

        // Randomizers
        float r_movestop, r_speed, r_orientation;

        // Player reference
        [SerializeField]
        private Character2D character;
        
        private void Awake()
        {
            m_NPC = GetComponent<Character2D>();
        }

        void Start()
        {
            r_movestop = Random.Range(1, 5); // the cooldown between moves
            r_speed = Random.Range(1, 3);    // the speed when moving the next time
        }

        // Update is called once per frame
        void Update()
        {
            if (r_movestop == 0)
            {
                r_movestop = Random.Range(1, 5);
                r_speed = Random.Range(1, 3);
                r_orientation = Random.Range(0, 360);
            }
            else
            {
                m_NPC.Move(r_speed, r_orientation);
                r_movestop -= Time.deltaTime;
            }
        }
    }
}
