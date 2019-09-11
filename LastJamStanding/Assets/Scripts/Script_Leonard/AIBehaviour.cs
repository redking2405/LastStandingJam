using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof(Character2D))]
    public class AIBehaviour : MonoBehaviour
    {
        private Character2D m_NPC;
        public static List<AIBehaviour> kageBunshin = new List<AIBehaviour>();
        // Randomizers
        public bool isHit;
        float r_movestop, r_speed;
        float minMovestop = 0.1f; float maxMovestop = 5;
        float minSpeed = 0.15f; float maxSpeed = 1f;
        Vector2 r_orientation;
        bool obstacleReached = false;
        bool moving = false;

        // Player reference
        [SerializeField]
        private Character2D character;
        
        private void Awake()
        {
            m_NPC = GetComponent<Character2D>();
            kageBunshin.Add(this);
        }

        void Start()
        {
            isHit = false;
            r_movestop = Random.Range(minMovestop, maxMovestop); // the cooldown between two moves
            r_speed = Random.Range(minSpeed, maxSpeed);    // the speed when moving the next time
        }

        Vector2 collisionPoint, collisionNorm;//Debug
        void Update()
        {
            if (m_NPC.isHit)
            {
                Death();
            }
            if (r_movestop <= 0) // Resets random values (cooldown/speed/orientation)
            {
                if (!obstacleReached)
                    SetNextMoveState(Random.Range(0, 360));
                else {
                    SetNextMoveState(0);
                    obstacleReached = false;
                }
            }
            else // Moves until the cooldown resets
            {
                r_movestop -= Time.deltaTime;
            }
            Debug.DrawRay(collisionPoint, collisionNorm, Color.red, 2f);
        }

        private void FixedUpdate()
        {
            if (moving)
                m_NPC.Move(r_speed, r_orientation);
        }

        private void OnCollisionEnter2D(Collision2D collision) // Collision detections (walls)
        {
            if (collision.collider.CompareTag("Wall"))
            {
                obstacleReached = true;
                SetRotationRangeUponEncounter(collision.GetContact(0).normal);
                collisionPoint = collision.GetContact(0).point;
                collisionNorm = collision.GetContact(0).normal;
            }
        }

        void SetNextMoveState(float orientation)
        {
            if ((!moving && !obstacleReached) || (moving && obstacleReached)) // the speed/orientation when moving the next time
            {
                r_speed = Random.Range(minSpeed, maxSpeed);
                r_orientation = RotateVector(Vector2.right, orientation);
            }
            moving = !moving;
            r_movestop = Random.Range(minMovestop, maxMovestop); // the cooldown between two moves OR the move duration
        }

        void SetRotationRangeUponEncounter(Vector2 colliderNorm)
        {
            float colliderNormAngle = (Vector2.SignedAngle(Vector2.right, colliderNorm) + 360) % 360;
            SetNextMoveState(Random.Range(colliderNormAngle - 45, (colliderNormAngle + 45) % 360));
        }

        public Vector2 RotateVector(Vector2 v, float angle)
        {
            float radian = angle * Mathf.Deg2Rad;
            float _x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
            float _y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
            return new Vector2(_x, _y);
        }

        public void Death()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            kageBunshin.Remove(this);
        }
    }
}
