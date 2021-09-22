using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour, IHP
{
    HP player_HP;

    void Awake()
    {
        player_HP = new HP(100);
    }

    public void SetDamage(float damage)
    {
        if (!player_HP.Set_Damege(damage))
        {
            Dead();
        }
    }
    public void SetHeal(float heal)
    {
        player_HP.Set_Heal(heal);
    }

    private void Dead()
    {

    }
}
