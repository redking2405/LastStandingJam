using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Character2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 1f; // The fastest the player can travel in the x axis.
        [SerializeField] private LayerMask m_WhatIsGround; // A mask determining what is ground to the character
        public bool isHit;
        const float k_DetectionRadius = .2f; // Radius of the overlap circle to determine if [close to an obstacle? (leonard's change)]
        private Animator m_Anim;            // Reference to the player's animator component.
        [SerializeField] private Rigidbody2D m_Rigidbody2D;
        public bool m_FacingRight = true;  // For determining which way the player is currently facing.
        public int currentColor = 3;
        private void Awake()
        {
            // Setting up references.
            isHit = false;
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();  
        }
        public int SetColor(int i)
        {
            switch(i)
            {
                case int n when n >= 0 && n <=3:
                    m_Anim.runtimeAnimatorController = GameManager.Instance.GetAnimatorController(i);
                    return currentColor = i;
                default:
                    m_Anim.runtimeAnimatorController = GameManager.Instance.GetAnimatorController(3);
                    return currentColor = 3;
            };

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
            // direction.Normalize();
            direction.Normalize();
            Vector2 nextVelocity = new Vector2(direction.x * move * m_MaxSpeed, direction.y * move * m_MaxSpeed);
            m_Rigidbody2D.velocity = nextVelocity;
        }

        public void Flip()
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