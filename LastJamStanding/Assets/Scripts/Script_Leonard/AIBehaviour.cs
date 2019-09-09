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
            r_movestop = Random.Range(1, 5); // the cooldown between two moves
            r_speed = Random.Range(1, 3);    // the speed when moving the next time
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

            ContactFilter2D wallContactFilter2D = new ContactFilter2D();
            ContactFilter2D charaContactFilter2D = new ContactFilter2D();
            wallContactFilter2D.SetLayerMask(LayerMask.NameToLayer("Wall"));
            charaContactFilter2D.SetLayerMask(LayerMask.NameToLayer("Character"));
            List<Collider2D> colliderResults = new List<Collider2D>();

            // Collision detections (walls having priority over other characters)
            if (Physics2D.OverlapCircle(transform.position, 1.5f, wallContactFilter2D, colliderResults) > 0) // Touching a wall (static walls only, going to it)
            {
                SetRotationRangeUponEncounter(colliderResults);
            }
            else if (Physics2D.OverlapPoint(transform.position + transform.right * 1.5f, charaContactFilter2D, colliderResults) > 0) // Touching obstacles in front of it (base vector is pointing to the right (rot 0)
            {
                SetRotationRangeUponEncounter(colliderResults);
            }
        }

        private void FixedUpdate()
        {
            if (moving)
                m_NPC.Move(r_speed, r_orientation);
        }

        void SetNextMoveState(float orientation)
        {
            moving = !moving;
            r_movestop = Random.Range(0.5f, 5);
            r_speed = Random.Range(1, 3);
        }

        void SetRotationRangeUponEncounter(List<Collider2D>_colliderResults)
        {
            Vector2 obstacleToCharacterNormal = Vector2.Perpendicular(_colliderResults[0].transform.position - transform.position);
            float minimumAngle = Vector2.Angle(Vector2.right, obstacleToCharacterNormal);
            SetNextMoveState(Random.Range(minimumAngle, (minimumAngle + 180) % 360));
        }
    }
}
