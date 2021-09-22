using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Gun : MonoBehaviour
{

    // Start is called before the first frame update

    public Transform FirePos;


    private Pistol pistol;
    private AssertRifle rifle;

    private IGun using_Gun;

    private void Awake()
    {
        pistol = new Pistol(10, 1, 0.5f, 6, 5);
        rifle = new AssertRifle(2, 0.1f, 0.1f, 30, 5);
    }
    void Start()
    {
        using_Gun = pistol;
    }

    // Update is called once per frame
    void Update()
    {
        using_Gun.Gun_Update();

        // 단발
        if(Input.GetMouseButtonDown(0))
        {
            using_Gun.Menual_Shoot(new Ray(FirePos.position, FirePos.forward));
        }

        // 자동
        if(Input.GetMouseButton(0))
        {
            using_Gun.Auto_Shoot(new Ray(FirePos.position, FirePos.forward));
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            using_Gun.Reloading();
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            using_Gun = rifle;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            using_Gun = pistol;
        }
    }
}
