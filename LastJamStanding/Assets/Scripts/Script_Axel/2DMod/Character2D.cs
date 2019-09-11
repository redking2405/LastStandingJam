using System;
using UnityEngine;
using System.Collections;

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
        public void Start()
        {
            StartCoroutine("FadeIn");

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
            if(m_Rigidbody2D.velocity.magnitude > 0.1f && m_Anim.GetBool("Moving") == false)
            {
                m_Anim.SetBool("Moving", true);
            }
            else if (m_Rigidbody2D.velocity.magnitude < 0.1f && m_Anim.GetBool("Moving") == true)
            {
                m_Anim.SetBool("Moving", false);
            }
        }

        IEnumerator FadeIn()
        {
            float transitionTime = 0;
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            Color c = Color.white; 
            while (transitionTime < 1)
            {
                c = new Color(c.r, c.g, c.b, transitionTime);
                sprite.color = c;
                transitionTime += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }

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
        public void TriggerDeath()
        {
            m_Anim.SetTrigger("Death");
        }
        private void LateUpdate()
        {
            m_Anim.ResetTrigger("Death");
        }
    }
}