using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : GunClass
{
    public Pistol(float damage, float rate, float rebound,
                float max_BulletNum, float reload_Time)
        : base(damage, rate, rebound,
                max_BulletNum, reload_Time)
    { }
    public override void Menual_Shoot(Ray shoot_Ray)
    {
        base.Gun_Fire(shoot_Ray);
    }
}
