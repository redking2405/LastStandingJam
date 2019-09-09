using System;
using UnityEngine;
using Rewired;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (Character2D))]
    public class UserControl : MonoBehaviour
    {
        private Character2D m_Character;
        public Player player;

        private void Awake()
        {
            player = ReInput.players.GetPlayer(0);
            m_Character = GetComponent<Character2D>();
        }

        private void Update()
        {
        }

        private void FixedUpdate()
        {
            // Read the inputs.
            Vector2 h = new Vector2(player.GetAxis("Move Horizontal"), player.GetAxis("Move Vertical"));

            // Pass all parameters to the character control script.
            //m_Character.Move(h.normalized,);
        }
    }
}
