using System;
using System.Collections;
using UnityEngine;
using Rewired;
using Random = UnityEngine.Random;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (Character2D))]
    public class UserControl : Singleton<UserControl>
    {
        private Character2D m_Character;
        public Player player;
        public GameObject prefab;
        public GameObject instance;
        private GameObject attachedWeapon;
        private IEnumerator attackCoroutine = null;
        [SerializeField] private int playerID;
        public float attackCooldown = 1;
        public float currentTimeCooldown = 0;
        public float vibrationTime;
        public float vibrationIntensity;
        
        public int GetPlayerID()
        {
            return playerID;
        }

        public void SetPlayerID(int newID)
        {
            playerID = newID;
            player = ReInput.players.GetPlayer(playerID);
        }
        private void Awake()
        {
            player = ReInput.players.GetPlayer(playerID);

            m_Character = GetComponent<Character2D>();

            attachedWeapon = GetComponentInChildren<WeaponAttack>().gameObject;
        }

        private void Start()
        {
        }

        private void FixedUpdate()
        {
            // Read the inputs.
            Vector2 h = new Vector2(player.GetAxis("Move Horizontaly"), player.GetAxis("Move Verticaly"));
            if (currentTimeCooldown > 0) { currentTimeCooldown -= Time.fixedDeltaTime; }
            m_Character.Move(h.magnitude,h);
            if (player.GetButtonDown("PlayerAttack") && currentTimeCooldown <= 0)
            {
                attackCoroutine = attachedWeapon.GetComponent<WeaponAttack>().Attack(m_Character.sweepCurve);
                StartCoroutine(attackCoroutine);
                ResetAttackCooldown();
            }
            m_Character.Move(h.magnitude, h);
        }

        public void Vibrate()
        {
            int motorIndex=1;
            player.SetVibration(motorIndex, vibrationIntensity, vibrationTime);
        }

        public void Respawn()
        {
            transform.position = GameManager.Instance.respawnPoints[Random.Range(0, GameManager.Instance.respawnPoints.Length)];
        }
        void ResetAttackCooldown()
        {
            currentTimeCooldown = attackCooldown;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Weapon"))
            {
                if (collision.transform.parent == null) // exchange weapon if on the floor
                {
                    if (attackCoroutine != null)
                    {
                        StopCoroutine(attackCoroutine);
                        attackCoroutine = null;
                    }
                    // Dropping weapon on the floor
                    Vector2 weaponAnchor = attachedWeapon.transform.localPosition;
                    attachedWeapon.transform.parent = null;
                    attachedWeapon.tag = "unusable";
                    attachedWeapon.GetComponent<Collider2D>().enabled = true;

                    // Picking up the other weapon on the floor
                    attachedWeapon = collision.gameObject;
                    attachedWeapon.transform.parent = transform;
                    attachedWeapon.transform.localPosition = weaponAnchor;
                    attachedWeapon.GetComponent<Collider2D>().enabled = false;
                }
                else if (collision.transform.parent != transform) // death
                {
                    //attachedWeapon.transform.parent = null;
                    Respawn();
                    //attachedWeapon.GetComponent<Collider2D>().enabled = true;
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("unusable"))
            {
                collision.tag = "Weapon";
            }
        }
    }
}
