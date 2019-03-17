﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : RaycastAbility
{

    public override void Start()
    {
        base.Start();
        abCoolDown = 5;
        abCost = 20;
        gunDamage = -20;
        weaponRange = 5;
    }

    public override void Fire()
    {
        Vector2 rayOrigin = transform.position;

        //Draw a debug line which will show where our ray will eventually be
        //Debug.DrawRay(rayOrigin, fpsCam.transform.forward * weaponRange, Color.green);

        //Declare a raycast hit to store information about what our raycast has hit.
        RaycastHit hit;

        //Start our ShotEffect coroutine to turn our laser line on and off
        //StartCoroutine(ShotEffect());

        //Set the start position for our visual effect for our laser to the position of gunEnd
        //laserLine.SetPosition(0, transform.position);

        Vector2 direction = transform.right;

        //Check if our raycast has hit anything
        if (Physics.Raycast(rayOrigin, direction, out hit, weaponRange))
        {
            //Set the end position for our laser line 
            //laserLine.SetPosition(1, new Vector3(hit.point.x, hit.point.y, transform.position.z));

            Healer self = transform.GetComponent<Healer>();

            //Get a reference to a health script attached to the collider we hit
            BasePlayer target = hit.collider.GetComponent<BasePlayer>();

            //If there was a player script attached
            if (target != null && self != null)
            {
                //Call the damage function of that script, passing in our gunDamage variable
                target.Damage(gunDamage);
                self.UseMana(abCost);
            }
            Debug.Log("Heal");
            Debug.Log("Mana: " + self.CurrentMana);
            self.UseMana(abCost);
            Debug.Log("Mana: " + self.CurrentMana);

            //Check if the object we hit has a rigidbody attached
            if (hit.rigidbody != null)
            {
                //Add force to the rigidbody we hit, in the direction it was hit from
                hit.rigidbody.AddForce(-hit.normal * hitForce);
            }
        }
        else
        {
            Debug.Log("No nearby ally");
            //if we did not hit anything, set the end of the line to a position directly away from
            //laserLine.SetPosition(1, fpsCam.transform.forward * weaponRange);
            Debug.Log("No allies");
        }
    }

   // private IEnumerator ShotEffect()
    //{

        //Turn on our line renderer
        //laserLine.enabled = true;
        //Wait for .07 seconds
        //yield return shotDuration;

        //Deactivate our line renderer after waiting
        //laserLine.enabled = false;
    //}
}
