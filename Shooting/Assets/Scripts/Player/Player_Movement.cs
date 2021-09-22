using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    // ----------------------------------------------------------- 추가 할 기능 
    // 슬라이딩 이동중일때만 사용가능하도록 (velocity값으로)
    // lerp 값 정규화 할지 고민해보기.

    // ------------------------------------------------------------ 게임 로직 구현 변수
    public CharacterController controller;

    public Transform player_Head;

    // 움직임 관련 변수
    private float move_X;
    private float move_Y;

    // 동작여부 변수
    private bool b_IsCrouch;
    private bool b_IsRunning;
    private bool b_IsSliding;

    // 각 움직임 별 속도
    private float crouch_Speed = 2;
    private float walk_Speed = 5;
    private float run_Speed = 7;
    private float sliding_Speed = 8;

    // 현재 이동속도
    private float move_Speed = 5;

    // 슬라이딩 방향
    private Vector3 sliding_Forward;

    // 앉기 낮음 정도 (사용할땐 -crouch..) 
    private float crouch_Height = 0.5f;

    // 서있을때 높이 (앉은 후 일어서기 위함)
    private float stand_Height;

    // 점프 높이
    private float jump_Height = 1f;
    // 앉은 상태에서의 점프 높이
    private float crouch_Jump_Height = 1.5f;

    // 모든 동작이 끝났는지 검사 값
    private float finish_Checker = 0.01f;


    // -------------------------------------------------- 물리 적용 변수

    // 중력 값
    private float gravity = 9.8f;

    // 중력 가속도를 담을 변수
    private Vector3 gravity_Velocity;

    // 중력 적용 및 땅을 밟고 있는지 검사
    public Transform ground_Checker;
    public LayerMask ground_Mask;
    private float ground_Distance = 0.4f;

    private bool b_IsGround = false;

    // 플레이어 이동치를 담을 변수 abs(전 좌표 - 현좌표)
    private Vector3 move_Velocity;

    // 전 좌표를 저장할 변수
    private Vector3 pre_Position = Vector3.negativeInfinity;



    // Update is called once per frame
    void Update()
    {
        // 땅을 밟고 있는지 
        if (b_IsGround)
        {

            // 점프시 b_isGround가 false가 됨으로 동작이 불가능해짐
            if (Input.GetKeyDown(KeyCode.Space) && !b_IsSliding)
            {
                Jump();
            }

            // 달리기 (앉은 상태가 아닌지, 슬라이딩 중이 아닌지)
            if (Input.GetKey(KeyCode.LeftShift) && !b_IsCrouch && !b_IsSliding)
            {
                Enable_Run();
            }
            // shift키에서 손을 땠고 (달리던 중이라면)
            else if (b_IsRunning)
            {
                Disable_Run();
            }

            // 달리기중 앉기 버튼이 눌릴 경우
            // 슬라이딩 (움직이고 있을때만)
            if (b_IsRunning && Input.GetKeyDown(KeyCode.LeftControl))
            {
                if(move_Velocity != Vector3.zero)
                    Init_Slide();
            }

            // 앉기 (슬라이딩 중이 아닐때만)
            if (Input.GetKey(KeyCode.LeftControl) && !b_IsSliding)
            {
                Enable_Crouch();
            }
            // 앉은 상태이고 슬라이딩 중이 아닐떄만 일어섬
            else if (b_IsCrouch && !b_IsSliding)
            {
                Disable_Crouch();
            }
        }
        if (b_IsSliding)
        {
            Sliding();
        }
        // 슬라이딩 중이 아니라면 
        else
        {
            // 전 좌표 저장
            pre_Position = this.transform.position;

            // 이동
            Move();

            // 전 좌표와 현 좌표를 velocity에 대입
            SetVelocity(pre_Position, this.transform.position);
        }
    }

    private void Move()
    {
        move_X = Input.GetAxis("Horizontal") * move_Speed * Time.deltaTime;
        move_Y = Input.GetAxis("Vertical") * move_Speed * Time.deltaTime;

        Vector3 move_Vec = this.transform.right * move_X + this.transform.forward * move_Y;
        controller.Move(move_Vec);
    }

    // 물리 구현 (점프, 중력 등)
    private void FixedUpdate()
    {
        b_IsGround = Physics.CheckSphere(ground_Checker.position, ground_Distance, ground_Mask);

        // 한번 떨어진 후에 땅에 착지했을 경우 값 초기화
        if (b_IsGround && gravity_Velocity.y < 0)
        {
            // 땅에 다 떨어지기 전에 체크될수 있으므로 나머지도 다 떨어지도록
            gravity_Velocity.y = -2f;
        }

        // 중력 값 계산
        gravity_Velocity.y += -gravity * Time.deltaTime;

        controller.Move(gravity_Velocity * Time.deltaTime);
    }


    private void SetVelocity(Vector3 prePosition, Vector3 curPosition)
    {
        move_Velocity = prePosition - curPosition;
        // 부호 제거해서 대입
        move_Velocity = new Vector3(Mathf.Abs(move_Velocity.x), Mathf.Abs(move_Velocity.y), Mathf.Abs(move_Velocity.z));
    }

    // 점프 함수
    private void Jump()
    {
        // 앉아있을때면 더 높이
        if (b_IsCrouch)
        {
            // 점프 물리학 공식
            gravity_Velocity.y = Mathf.Sqrt(crouch_Jump_Height * -2 * -gravity);
        }
        else
        {
            // 점프 물리학 공식
            gravity_Velocity.y = Mathf.Sqrt(jump_Height * -2 * -gravity);
        }
    }

    // 달리기 함수
    private void Enable_Run()
    {
        if (!b_IsRunning)
        {
            b_IsRunning = true;
        }
        move_Speed = Mathf.Lerp(move_Speed, run_Speed, 0.1f);
    }

    private void Disable_Run()
    {
        // 달리기 멈춤이 완료되면
        if (move_Speed - walk_Speed < finish_Checker)
        {
            b_IsRunning = false;
            move_Speed = walk_Speed;
        }

        // 현재 빨리진 속도를 walkSpeed를 향해 lerp
        move_Speed = Mathf.Lerp(move_Speed, walk_Speed, 0.2f);
    }


    // 앉기 함수
    private void Enable_Crouch()
    {
        if (!b_IsCrouch)
        {
            b_IsCrouch = true;
            stand_Height = player_Head.transform.localPosition.y;
            move_Speed = crouch_Speed;
        }

        // 카메라 내려감
        player_Head.transform.localPosition = Vector3.Lerp(player_Head.transform.localPosition,
                                                         new Vector3(player_Head.transform.localPosition.x, -crouch_Height, player_Head.transform.localPosition.z), 0.3f);
    }
    private void Disable_Crouch()
    {
        // 일어슴이 완료 되면
        if (stand_Height - player_Head.transform.localPosition.y < finish_Checker)
        {
            // 앉음 여부 false
            b_IsCrouch = false;

            // lerp로 인해 값들이 조금 부족해서 완성 값 대입
            move_Speed = walk_Speed;
            player_Head.transform.localPosition = new Vector3(player_Head.transform.localPosition.x, stand_Height, player_Head.transform.localPosition.z);
        }

        // 일어서기.
        player_Head.transform.localPosition = Vector3.Lerp(player_Head.transform.localPosition, new Vector3(player_Head.transform.localPosition.x, stand_Height, player_Head.transform.localPosition.z), 0.1f);
    }
    private void Init_Slide()
    {
        b_IsSliding = true;
        b_IsRunning = false;

        sliding_Forward = this.transform.forward;

        stand_Height = player_Head.transform.localPosition.y;
        move_Speed = sliding_Speed;
    }
    private void Sliding()
    {
        // 소숫점 계산으로 늘어지는것을 막기위해 finish_checker에 + 값 해줌
        if (move_Speed - crouch_Speed < finish_Checker + 1f)
        {
            b_IsSliding = false;
            move_Speed = crouch_Speed;

            // 앉음을 true로 줘서 자연스럽게 일어나는 연출하도록 (플레이어가 lCtrl키를 계속 누르고 있다면 앉아있기도 해야 함)
            b_IsCrouch = true;
        }
        // 속도 서서히 감소
        move_Speed = Mathf.Lerp(move_Speed, crouch_Speed, 0.03f);

        // 카메라 내려감
        player_Head.transform.localPosition = Vector3.Lerp(player_Head.transform.localPosition,
                                                     new Vector3(player_Head.transform.localPosition.x, -crouch_Height, player_Head.transform.localPosition.z), 0.1f);

        // 앞으로 이동
        Vector3 move_Vec = sliding_Forward * move_Speed * Time.deltaTime;
        controller.Move(move_Vec);
    }
}
