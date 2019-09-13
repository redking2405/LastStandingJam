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
        [SerializeField] private int playerID;
        public float spawnCooldown = 1;
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
            if (player.GetButtonDown("CloneVert") && currentTimeCooldown <= 0)
            {
                GameManager.Instance.InstantiateClone(transform.position,GetComponent<Character2D>().m_FacingRight, 1);
                ResetSpawnCooldown();
            }
            if (player.GetButtonDown("CloneJaune") && currentTimeCooldown <= 0)
            {
                GameManager.Instance.InstantiateClone(transform.position, GetComponent<Character2D>().m_FacingRight, 3);
                ResetSpawnCooldown();
            }
            if (player.GetButtonDown("CloneBleu") && currentTimeCooldown <= 0)
            {
                GameManager.Instance.InstantiateClone(transform.position, GetComponent<Character2D>().m_FacingRight, 0);
                ResetSpawnCooldown();
            }
            if (player.GetButtonDown("CloneRouge") && currentTimeCooldown <= 0)
            {
                GameManager.Instance.InstantiateClone(transform.position, GetComponent<Character2D>().m_FacingRight, 2);
                ResetSpawnCooldown();
            }
        }


        public void Vibrate()
        {

            int motorIndex=1;

            player.SetVibration(motorIndex, vibrationIntensity, vibrationTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "Hunter")
            {
                Vibrate();
            }
        }

        public void Respawn()
        {
            int ranIndex = Mathf.RoundToInt(Random.Range(0, AIBehaviour.kageBunshin.Count));

            transform.position = AIBehaviour.kageBunshin[ranIndex].transform.position;
            int color = AIBehaviour.kageBunshin[ranIndex].GetComponent<Character2D>().currentColor;
            ChangeColor(color);
            Destroy(AIBehaviour.kageBunshin[ranIndex]);

        }
        void ResetSpawnCooldown()
        {
            currentTimeCooldown = spawnCooldown;
        }
        public void ChangeColor(int i)
        {
            m_Character.SetColor(i);
        }
    }
}
