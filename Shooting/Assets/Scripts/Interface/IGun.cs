using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGun
{
    void Menual_Shoot(Ray shoot_Ray);
    void Auto_Shoot(Ray shoot_Ray);

    void Gun_Update();
    void Reloading();
}
