using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP
{
    public float maxHP;
    public float hp;

    public HP(float _hp)
    {
        maxHP = _hp;
        hp = _hp;
    }

    public void Set_Heal(float _heal)
    {
        hp += _heal;

        if(hp > maxHP)
        {
            hp = maxHP;
        }
    }

    public bool Set_Damege(float _damage)
    {
        hp -= _damage;
        if(hp <= 0)
        {
            return false;
        }
        return true;
    }
}
