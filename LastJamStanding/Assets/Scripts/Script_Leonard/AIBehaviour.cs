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
        }

        void Start()
        {
            r_movestop = Random.Range(minMovestop, maxMovestop); // the cooldown between two moves
            r_speed = Random.Range(minSpeed, maxSpeed);    // the speed when moving the next time
        }

        void Update()
        {
            if (r_movestop <= 0) // Resets random values (cooldown/speed/orientation)
            {
                SetNextMoveState(Random.Range(0, 360));
            }
            else // Moves until the cooldown resets
            {
                r_movestop -= Time.deltaTime;
            }
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
                SetRotationRangeUponEncounter(collision.GetContact(0).normal);
            }
        }

        void SetNextMoveState(float orientation)
        {
            moving = !moving;
            r_movestop = Random.Range(minMovestop, maxMovestop); // the cooldown between two moves
            r_speed = Random.Range(minSpeed, maxSpeed);    // the speed when moving the next time
            r_orientation = RotateVector(Vector2.right, orientation);
        }

        void SetRotationRangeUponEncounter(Vector2 colliderNorm)
        {
            float minimumAngle = Vector2.Angle(Vector2.right, Vector2.Perpendicular(colliderNorm));
            SetNextMoveState(Random.Range(minimumAngle, (minimumAngle + 180) % 360));
        }

        public Vector2 RotateVector(Vector2 v, float angle)
        {
            float radian = angle * Mathf.Deg2Rad;
            float _x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
            float _y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
            return new Vector2(_x, _y);
        }
    }
}
