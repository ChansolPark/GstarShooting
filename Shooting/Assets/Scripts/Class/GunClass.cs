using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunClass : IGun
{
    protected float damage;
    protected float rate;
    protected float rate_Count;
    protected float rebound;

    protected float cur_BulletNum;
    protected float max_BulletNum;

    protected float reload_Time;
    protected float reload_Count;

    protected float rayDistance;

    protected GunClass(float damage, float rate, float rebound,
                float max_BulletNum, float reload_Time)
    {
        this.damage = damage;
        this.rate = rate;
        this.rate_Count = 0;
        this.rebound = rebound;

        this.cur_BulletNum = max_BulletNum;
        this.max_BulletNum = max_BulletNum;

        this.reload_Time = reload_Time;
        this.reload_Count = 0;

        this.rayDistance = 1000;
    }
    // 상속받은 클래스들은 둘중 하나 선택
    public virtual void Menual_Shoot(Ray shoot_Ray){}
    public virtual void Auto_Shoot(Ray shoot_Ray){}

    public virtual void Gun_Fire(Ray shoot_Ray)
    {
        if (reload_Count == 0 && rate_Count == 0)
        {
            Shoot_Ray(shoot_Ray);

            Debug.Log(cur_BulletNum);
            cur_BulletNum--;

            if (Check_Reload())
            {
                Reloading();
            }
            else { rate_Count = rate; }
        }
    }
    public void Gun_Update()
    {
        if (reload_Count > 0)
        {
            Debug.Log("reloading" + reload_Count);
            reload_Count -= Time.deltaTime;
            if (reload_Count <= 0)
            {
                Debug.Log("reload Complete");
                reload_Count = 0;
                cur_BulletNum = max_BulletNum;
            }
        }

        if (rate_Count > 0)
        {
            rate_Count -= Time.deltaTime;

            if (rate_Count <= 0)
                rate_Count = 0;
        }
    }

    public void Reloading()
    {
        if(cur_BulletNum < max_BulletNum)
            reload_Count = reload_Time;
    }

    protected bool Check_Reload()
    {
        // 현재 총알이 0보다 작거나
        // 플레이어가 재장전 버튼을 눌렀을 경우
        if (cur_BulletNum <= 0)
        {
            return true;
        }
        return false;
    }

    protected virtual void Shoot_Ray(Ray shoot_Ray)
    {
        if (cur_BulletNum > 0 && rate_Count == 0 && reload_Count == 0)
        {
            RaycastHit hit;
            // 총알에 맞는 레이어만 충돌 체크함
            int layerMask = 1 << LayerMask.NameToLayer("Can_Hit_By_Bullet");

            if (Physics.Raycast(shoot_Ray, out hit, rayDistance, layerMask))
            {

                IHP hit_HP = hit.transform.GetComponent<IHP>();

                if (hit_HP != null)
                    hit_HP.SetDamage(this.damage);
            }
        }
    }
}
