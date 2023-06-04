using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public PlayerMove player;
    public GameObject[] Stages;

    public Image[] UIHealth;
    public TextMeshProUGUI UIPoint;
    public TextMeshProUGUI UIStage;
    public GameObject UIRestartBtn;
    

    void Update()
    {   
        // 점수는 Update 문으로 표시
        
        UIPoint.text = (totalPoint + stagePoint).ToString();  
    }

    public void NextStage()
    {   
        //Change Stages
        if(stageIndex < Stages.Length-1) {
            Stages[stageIndex].SetActive(false);
            stageIndex++; // stageIndex 값에 따라서 스테이지 변경
            Stages[stageIndex].SetActive(true);
            PlayerReposition();

            UIStage.text = "STAGE" + (stageIndex + 1); // Index 값을 보여줄 때에는, 0부터 시작하니 1을 더해주기.
        }
        else { //Game Clear
            
            //Player Control Lock
            
            Time.timeScale = 0;
            
            //Restart Button UI
            
            Text btnNext = UIRestartBtn.GetComponentInChildren<Text>(); // InChildren : 자식 오브젝트를 불러올때 GetComponent 에 붙이는 함수
            btnNext.text = "Clear!";    
            ViewBtn();
        }
        
        
        //Calculate Point
        
        totalPoint += stagePoint;
        stagePoint = 0;
    }

    public void HealthDown()
    {
        if(health > 1) { 
            
            health--;
            UIHealth[health].color = new Color(1,0,1, 0.4f);
        }
        else {
            
            //All Health UI Off

            UIHealth[0].color = new Color(1,0,1, 0.4f);
            
            //Player Die Effet

            player.OnDie();
            
            //Result UI

            Debug.Log("죽었습니다!");
            
            //Retry Button UI
            
            Invoke("ViewBtn" , 3);

        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            
            //Player Reposition
            if (health > 1)
                PlayerReposition();
            

            //Health Down
            HealthDown();
            
        }
        else if (collision.gameObject.tag == "Border") {
            Destroy(gameObject);
        }
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(-3 , 0 , 1);
        player.VelocityZero();
    }

    void ViewBtn()
    {
        UIRestartBtn.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

}