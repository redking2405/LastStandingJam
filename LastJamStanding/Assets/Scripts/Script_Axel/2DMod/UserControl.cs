using System;
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
        private Coroutine attackCoroutine = null;
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
            attachedWeapon = GetComponentInChildren<WeaponAttack>().gameObject;
        }
        private void Awake()
        {
            player = ReInput.players.GetPlayer(playerID);

            m_Character = GetComponent<Character2D>();
        }

        private void Update()
        {
        }

        private void FixedUpdate()
        {
            // Read the inputs.
            //Vector2 h = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
            Vector2 h = new Vector2(player.GetAxis("Move Horizontaly"),player.GetAxis("Move Verticaly"));
            //print(player.GetAxis2D("Move Horizontaly", "Move Verticaly"));
            // Pass all parameters to the character control script.
            // m_Character.Move(h);
            if (currentTimeCooldown > 0) { currentTimeCooldown -= Time.fixedDeltaTime; }
            m_Character.Move(h.magnitude,h);
            if (player.GetButtonDown("PlayerAttack") && currentTimeCooldown <= 0)
            {
                attackCoroutine = attachedWeapon.GetComponent<WeaponAttack>().StartCoroutine("Attack");
                ResetAttackCooldown();
            }
        }


        public void Vibrate()
        {
            int motorIndex=1;
            player.SetVibration(motorIndex, vibrationIntensity, vibrationTime);
        }

        public void Respawn()
        {
            transform.position = new Vector2((int)Random.Range(-GameManager.Instance.screenXmax, GameManager.Instance.screenXmax), (int)Random.Range(-GameManager.Instance.screenYmax, GameManager.Instance.screenYmax));
        }
        void ResetAttackCooldown()
        {
            currentTimeCooldown = attackCooldown;
        }
        public void ChangeColor(int i)
        {
            m_Character.SetColor(i);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("weapon")) // exchange weapon
            {
                if (attackCoroutine != null)
                    StopCoroutine(attackCoroutine);
                Vector2 weaponAnchor = attachedWeapon.transform.localPosition;
                attachedWeapon.transform.parent = transform.parent;
                attachedWeapon = collision.gameObject;
                attachedWeapon.transform.parent = transform;
                attachedWeapon.transform.localPosition = weaponAnchor;
            }
        }
    }
}
