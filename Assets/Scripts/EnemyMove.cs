using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid; 
    Animator anim;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsulecollider;
    public int nextMove; // 행동지표를 결정할 변수

    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsulecollider = GetComponent<CapsuleCollider2D>();

        Invoke("Think" , 5); // Invoke() : 주어진 시간이 지난 뒤, 지정된 함수를 실행하는 함수
    }

   
    void FixedUpdate()
    {
        // 이동

        rigid.velocity = new Vector2(nextMove , rigid.velocity.y); // y값은 0이 되면 X
        
        // 플랫폼 체크

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0,1,0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform")); // GetMask() : 레이어 이름에 해당하는 정수값을 리턴하는 함수
        
        //RayCastHit변수의 콜라이더로 검색 확인 가능
        if (rayHit.collider == null) 
           Turn();
        
    }

    
    void Think() // 재귀 함수 : 자신을 스스로 호출하는 함수 (딜레이 없이 하면 에러 위험)
    {   // Set Next Active

        nextMove = Random.Range(-1, 2); // 최대값은 랜덤 값의 포함 X

        // Sprite Animation 

        anim.SetInteger("WalkSpeed", nextMove);

        // Flip Sprite 

        if (nextMove != 0)
        spriteRenderer.flipX = nextMove == 1;
        
        // 재귀 (Recursive)
        float nextThinkTime = Random.Range(2f,5f);
        Invoke("Think" , nextThinkTime);


    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;

        CancelInvoke();
        Invoke("Think" , 5);
    }
    
    public void OnDamaged()
    {
        //Sprite Alpha 

        spriteRenderer.color = new Color(1,1,1,0.4f);
        
        //Sprite Flip Y 

        spriteRenderer.flipY = true;
        
        //Colider Disable

        capsulecollider.enabled = false;
        
        //Die Effect Jump

        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
       
        //Destroy 

        Invoke("DeActive", 5);
    }

    void DeActive() 
    {
        gameObject.SetActive(false);
    }
    
}
