using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Leaning : MonoBehaviour
{
    // 기울이기 구현 변수

    public Transform pivot;

    public float speed = 100f;
    public float maxAngle = 20f;

    private float curAngle = 0f;

    // 기울이기 중 벽 뚫림 방지 변수 
    public Transform player_Camera;
    private float ray_Distance = 0.5f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        // lean left
        if (Input.GetKey(KeyCode.Q) && !Physics.Raycast(player_Camera.position, -player_Camera.right, ray_Distance)) 
        {
            // curAngle을 maxAngle의 값까지 speed * T 만큼 올려 대입해주는데 moveTowardsangle함수는 360도 각도 값에 알맞게 넣어주는 함수
            curAngle = Mathf.MoveTowardsAngle(curAngle, maxAngle, speed * Time.deltaTime);
        }
        // lean right
        else if (Input.GetKey(KeyCode.E) && !Physics.Raycast(player_Camera.position, player_Camera.right, ray_Distance))
        {
            curAngle = Mathf.MoveTowardsAngle(curAngle, -maxAngle, speed * Time.deltaTime);
        }
        // reset lean
        else
        {
            curAngle = Mathf.MoveTowardsAngle(curAngle, 0f, speed * Time.deltaTime);
        }

        pivot.localRotation = Quaternion.AngleAxis(curAngle, Vector3.forward);
    }
}
