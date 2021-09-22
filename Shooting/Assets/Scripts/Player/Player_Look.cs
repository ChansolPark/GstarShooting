using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Look : MonoBehaviour
{
    private float mouse_X;
    private float mouse_Y;

    // 상하로 바라볼때 제한을 두기위한 변수

    public Transform player_Head;
    public Transform player_Camera;

    public float mouse_Sensitivity;

    private float rotation_Vertical;


    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        mouse_X = Input.GetAxis("Mouse X") * mouse_Sensitivity * Time.deltaTime;
        mouse_Y = Input.GetAxis("Mouse Y") * mouse_Sensitivity * Time.deltaTime;


        rotation_Vertical -= mouse_Y;
        rotation_Vertical = Mathf.Clamp(rotation_Vertical, -90f, 90f);

        this.transform.Rotate(Vector3.up * mouse_X);

        player_Camera.localRotation = Quaternion.Euler(rotation_Vertical, 0, 0);
    }
}
