﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HookUtils
{
    public class OnHooked : MonoBehaviour
    {
        private Collider2D m_Collider2;
        private Hook hookProjectile;

        private bool hooked = false;
        private int hookedDude = -1;
        private GameObject dude;
        // Start is called before the first frame update
        void Start()
        {
            m_Collider2 = GetComponent<Collider2D>();
            m_Collider2.isTrigger = false;
            hookProjectile = GetComponent<Hook>();
        }

        // Update is called once per frame
        void Update()
        {

            if (hookProjectile != null && hooked)
            {
                switch (hookedDude)
                {
                    case -1:
                        hookProjectile.Hit();
                        hookProjectile.HitDone();
                        hookProjectile.transform.position = Quaternion.Euler(hookProjectile.GetPlayer().transform.eulerAngles) * hookProjectile.offset + hookProjectile.GetPlayer().transform.position;

                        hookProjectile.transform.rotation = hookProjectile.GetPlayer().transform.rotation;
                        hooked = false;
                        break;
                    case 0:
                        if (dude != null)
                        {
                            hookProjectile.Hit();
                            dude.GetComponent<BasePlayer>().SetControl(false);
                            dude.GetComponent<BasePlayer>().rigidBody.velocity = new Vector2();
                            dude.GetComponent<BasePlayer>().rigidBody.AddForce((hookProjectile.GetPlayer().transform.position - dude.transform.position).normalized * 12, ForceMode2D.Impulse);
                            hookedDude = 3;
                        }

                        break;
                    case 1:
                        hookProjectile.Hit();
                        hookProjectile.GetPlayer().rigidBody.AddForce((hookProjectile.transform.position - hookProjectile.GetPlayer().transform.position).normalized * 12, ForceMode2D.Impulse);
                        hookedDude = 2;
                        break;
                    case 2:
                        hookProjectile.GetPlayer().rigidBody.AddForce((hookProjectile.transform.position - hookProjectile.GetPlayer().transform.position).normalized, ForceMode2D.Impulse);
                        if ((hookProjectile.transform.position - hookProjectile.GetPlayer().transform.position).magnitude < 1)
                        {
                            hookProjectile.HitDone();
                            hookProjectile.transform.position = Quaternion.Euler(hookProjectile.GetPlayer().transform.eulerAngles) * hookProjectile.offset + hookProjectile.GetPlayer().transform.position;
                            hookProjectile.transform.rotation = hookProjectile.GetPlayer().transform.rotation;
                            hooked = false;

                        }

                        break;
                    case 3:
                        hookProjectile.GetPlayer().rigidBody.AddForce((hookProjectile.transform.position - hookProjectile.GetPlayer().transform.position).normalized, ForceMode2D.Impulse);
                        if ((hookProjectile.GetPlayer().transform.position - dude.transform.position).magnitude < 1)
                        {
                            dude.GetComponent<BasePlayer>().rigidBody.velocity = new Vector2();
                            dude.GetComponent<BasePlayer>().SetControl(true);
                            hookProjectile.HitDone();
                            hookProjectile.transform.position = Quaternion.Euler(hookProjectile.GetPlayer().transform.eulerAngles) * hookProjectile.offset + hookProjectile.GetPlayer().transform.position;
                            hookProjectile.transform.rotation = hookProjectile.GetPlayer().transform.rotation;
                            hooked = false;
                        }

                        break;
                }
            }
            if ((hookProjectile.transform.position - hookProjectile.GetPlayer().transform.position).magnitude > hookProjectile.getWeaponRange() && hookProjectile.isFired())
            {
                hookProjectile.Hit();
                hookProjectile.HitDone();
                hookProjectile.transform.position = Quaternion.Euler(hookProjectile.GetPlayer().transform.eulerAngles) * hookProjectile.offset + hookProjectile.GetPlayer().transform.position;

                hookProjectile.transform.rotation = hookProjectile.GetPlayer().transform.rotation;
                hooked = false;
            }
        }

        protected virtual void OnCollisionEnter2D(Collision2D other)
        {
            GameObject objectHit = other.gameObject;
            if (hookProjectile != null && !hooked)
            {
                hooked = true;
                hookProjectile.Hit();
                if (objectHit.tag == "Player")
                {
                    hookedDude = 0;
                    objectHit.GetComponent<BasePlayer>().Damage(hookProjectile.getDamage());
                    dude = objectHit;
                }
                else if (objectHit.tag == "Obstacle")
                {
                    hookedDude = 1;
                }
                else
                {
                    hookedDude = -1;
                }
            }
        }
    }
}
