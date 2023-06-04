using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    private float Player_JumpPower = 15.0f;
    private float Player_JumpTime;
    private bool Player_JumpReady;

    
    public float Player_MoveSpeed = 10.0f;
    private bool Player_IsGround;
    Rigidbody2D rigid;
    SpriteRenderer spr1;
    Animator anim;


    public PhysicsMaterial2D Bounce, Normal;

   
    public void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spr1 = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        PlayerMove();
        PlayerJump();

        if (rigid.velocity.y > 0 && Player_IsGround == false)
        {
            rigid.sharedMaterial = Bounce;
        }
        else
        {
            rigid.sharedMaterial = Normal;
        }
    }

    // Player Move

    // 플레이어 이동 함수 정의
    /* 
    * 키보드 입력에 따라 플레이어를 좌우로 이동시킨다.
    * 스프라이트 렌더러의 flip 속성을 활용하여 좌우 반전을 한다.
    */ 
    private void PlayerMove()
    {   
        if (Player_IsGround == false) {
        float x = Input.GetAxisRaw("Horizontal");
        Vector3 move = new Vector3(x, 0, 0) * Player_MoveSpeed * Time.deltaTime;

        // 스프린트 렌더러 flip 을 활용한 좌우반전
        if (x < 0)
        {
            spr1.flipX = true;
        }
        else if (x > 0)
        {
            spr1.flipX = false;
        }

        transform.position += move;
        }
    }

    // Player Jump

     /*
    * 점프 버튼 입력에 따라 플레이어에게 상승력을 준다.
    * 점프 버튼을 길게 누르면 점프력이 증가한다.
    * 점프 중일 때 애니메이션 상태를 변경한다.
    */
    private void PlayerJump()
    {
        if(Player_IsGround == true)
        {
            if(Input.GetButtonDown("Jump"))
            {
                Player_JumpTime = 0.0f;
                Player_JumpReady = true;
            }
            if (Input.GetButtonUp("Jump"))
            {
                if (Player_JumpTime >= 1.0f)
                {
                    rigid.velocity = new Vector2(rigid.velocity.x, Player_JumpPower * 2.0f);
                    anim.SetBool("isJumping" , true);
                }
                else if (Player_JumpTime >= 0.75f)
                {
                    rigid.velocity = new Vector2(rigid.velocity.x, Player_JumpPower * 1.75f);
                    anim.SetBool("isJumping" , true);
                }
                else if (Player_JumpTime >= 0.5f)
                {
                    rigid.velocity = new Vector2(rigid.velocity.x, Player_JumpPower * 1.5f);
                    anim.SetBool("isJumping" , true);
                }
                else if (Player_JumpTime >= 0.25f)
                {
                    rigid.velocity = new Vector2(rigid.velocity.x, Player_JumpPower * 1.25f);
                    anim.SetBool("isJumping" , true);
                }
                else
                {
                    rigid.velocity = new Vector2(rigid.velocity.x, Player_JumpPower);
                    anim.SetBool("isJumping" , true);
                }
                Player_IsGround = false;
                Player_JumpReady = false;
                anim.SetBool("isJumping" , false);
            }
            Player_JumpTime += Time.deltaTime;
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        switch(col.gameObject.tag)
        {
       
            case "Ground":
                Player_IsGround = true;
                break;
            case "DeathZone":
                Time.timeScale = 0;
                break;
            case "Goal":
                Time.timeScale = 0;
                break;
        }
    }
}
