using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Character2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f; // The fastest the player can travel in the x axis.
        [SerializeField] private LayerMask m_WhatIsGround; // A mask determining what is ground to the character
        
        const float k_DetectionRadius = .2f; // Radius of the overlap circle to determine if [close to an obstacle? (leonard's change)]
        private Animator m_Anim;            // Reference to the player's animator component.
        [SerializeField] private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.
        public Vector2 pos;
        public Color[] color = { Color.blue, Color.green, Color.red, Color.yellow };
        public Color currentColor;
        private void Awake()
        {
            // Setting up references.
            //m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();

            pos = Vector2.zero;
        }
        public Color SetColor(Color color)
        {
            GetComponent<SpriteRenderer>().color = color;
            return currentColor = color;
        }
        private void FixedUpdate()
        {
            // Set the vertical animation
            //m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
        }


        public void Move(float move, Vector2 direction)
        {
            // The Speed animator parameter is set to the absolute value of the horizontal input.
            //m_Anim.SetFloat("Speed", Mathf.Abs(move));
            // Move the character
            m_Rigidbody2D.velocity = new Vector2(pos.x * move * m_MaxSpeed, pos.y * move * m_MaxSpeed);
            print(0);
            
        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}