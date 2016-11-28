﻿using UnityEngine;

public class Weapon_Machine : Player_Weapon_Sr
{
    void Awake()
    {
        base.Initialize();
    }

    void FixedUpdate()
    {
        base.ReducedRecoil();
    }

    protected override void Shoot()
    {
        ammo--;
        temp.Set(transform.up.x + Random.Range(-tempF, tempF), transform.up.y, transform.up.z);
        tempF += tempF;
        tempF = Mathf.Clamp(tempF, minBulletRage, maxBulletRage);
        //Send shoot all other player
        Message_Sr.Shot_Sr shot = new Message_Sr.Shot_Sr();
        shot.index = shooterIndex;
        shot.type = type;
        shot.vect = new Vector3[1];
        shot.vect[0] = temp;
        shot.thisPlayer = false;
        Networking_Server.SendAllOtherPlayer(Networking_msgType_Sr.Fire, shot, shooterIndex);
        //Send shoot me
        shot.thisPlayer = true;
        shot.ammo = ammo;
        Networking_Server.SendThisPlayer(Networking_msgType_Sr.Fire, shot, shooterIndex);

        if (Physics.Raycast(transform.position, temp.normalized, out hit, shotDistace, layerZombie, QueryTriggerInteraction.Ignore))
        {
            Player_MovePlayer zombie = hit.transform.GetComponent<Player_MovePlayer>();
            if (zombie != null)
            {
                zombie.ZombieDamage(damage, shooterIndex);
            }
        }
        if (ammo > 0)
        {
            if (shotTime > 0)
            {
                Invoke("Shoot", shotTime);
            }
        }
        else
        {
            StartReload();
        }
    }
}
