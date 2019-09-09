using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (Character2D))]
    public class UserControl : MonoBehaviour
    {
        private Character2D m_Character;

        private void Awake()
        {
            m_Character = GetComponent<Character2D>();
        }


        private void Update()
        {
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            //float h = // REWIRED. GetAxis("Horizontal");
            // Pass all parameters to the character control script.
             // m_Character.Move(h);
        }
    }
}
