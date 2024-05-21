using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class NPC_AI : MonoBehaviour
{
    // Start is called before the first frame update
    NavMeshAgent npcNavi;
    public GameObject TargetObj;
    public Animator NPC01_ani;
    public int DetectDistance = 10;
    public float AttkDistance = .9f;
    public float NPC_HP = 100f;
    public Image NPC_HpIMG;
    static public bool NPC_isAtk = false;
    public float NPC_AtkTime;

    public Transform NPC_HpBar;
    public Transform CameraObj;
    void Start()
    {
        TargetObj = GameObject.Find("player");
        npcNavi = GetComponent<NavMeshAgent>();
        NPC01_ani = GetComponent<Animator>();
        //  NPC_HpBar = GameObject.Find("NPC_Canvas").transform;
        NPC_HpBar = transform.Find("NPC_Canvas");
        CameraObj = GameObject.Find("Main Camera").transform;
    }
    // Update is called once per frame
    void Update()
    {
        NPC_HpIMG.fillAmount = NPC_HP * 0.01f;     
        if (GameObject.Find("NPC_Canvas") != null)
            NPC_HpBar.LookAt(CameraObj.position);
        else
            NPC_HpBar = GameObject.Find("NPC_Canvas").transform;

        //float NPC_Speed = npcNavi.velocity.magnitude;   // 取得NPC的速度. 
        NPCJudge();
        
    }


    private void OnTriggerEnter(Collider Hit)
    {
        if (Hit.gameObject.tag == "weapon" && NPC_HP > 0&& playerC.Player_isAttk==true)
        {
            NPC_HP -= 50;
        }

        if (Hit.gameObject.tag== "bullet" && NPC_HP > 0 )
        {
            NPC_HP -= 100;
        }
    }

    void NPCJudge()
    {
        float dist = Vector3.Distance(transform.position, TargetObj.transform.position);
        if (dist < DetectDistance && NPC_HP > 0 && playerC.PlayHp > 0) // 偵測NPC和玩家之間的距離, 如在所設定的距離內, 開始追蹤
        {
            npcNavi.destination = TargetObj.transform.position;
            NPC01_ani.SetFloat("Blend", 1);
        }
        else // 沒有的話, 則追蹤自己 或也可以改成回到NPC原先的位置. 
        {
            npcNavi.destination = transform.position;
            NPC01_ani.SetFloat("Blend", 0);
        }
        if (dist < AttkDistance && playerC.PlayHp > 0)
        {
            NPC01_ani.SetBool("NPC_ATK", true);
        }
        else
        {
            NPC01_ani.SetBool("NPC_ATK", false);
        }
        if (NPC_HP <= 0)
        {
            NPC01_ani.SetBool("NPC_Die", true);
            Destroy(this.gameObject, 5);
        }
        NPC_AtkTime = NPC01_ani.GetFloat("NPC_AtkTtime");
        NPC_isAtk = NPC01_ani.GetFloat("NPC_AtkTtime") > 0.006 ? true : false;
    }  
}
