using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Bunker.Game
{
    public class Boss_2_Body : MonoBehaviour
    {
        GameObject charge;
        GameObject shoot;
        SpriteRenderer Lazer_1;
        SpriteRenderer Lazer_2;
        public Transform Emiter_1;
        public Transform Emiter_2;
        // Start is called before the first frame update
        private void Awake()
        {
            charge = transform.Find("Charge").gameObject;
            shoot = transform.Find("Shoot").gameObject;
            Lazer_1 = transform.Find("Lazer/Emiter_1/L1").GetComponent<SpriteRenderer>();
            Emiter_1 = transform.Find("Lazer/Emiter_1");
            Lazer_2 = transform.Find("Lazer/Emiter_2/L2").GetComponent<SpriteRenderer>();
            Emiter_2 = transform.Find("Lazer/Emiter_2");
            OnIdle();
        }

        // Update is called once per frame
        void Update()
        {

        }
        //
        public void OnIdle()
        {
            charge.SetActive(false);
            shoot.SetActive(false);
            Lazer_1.size = Vector2.zero;
            Lazer_2.size = Vector2.zero;

        }
        public void OnCharge()
        {
            charge.SetActive(true);
            shoot.SetActive(false);
            Lazer_1.size = Vector2.zero;
            Lazer_2.size = Vector2.zero;
        }
        public void OnShoot()
        {
            charge.SetActive(false);
            shoot.SetActive(true);
            //Lazer_1.size = Vector2.one;
            //Lazer_2.size = Vector2.one;
        }
        public void SetLazer_1_Height(float h)
        {
            Lazer_1.size = new Vector2(1, h);
        }
        public void SetLazer_2_Height(float h)
        {
            Lazer_2.size = new Vector2(1, h);
        }
    }
}
