using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
public class TargetManeger : MonoBehaviourPunCallbacks
{
    [SerializeField]
    int spawn_Limit = 10;//ターゲットの沸き数上限
    [SerializeField]
    float late=10f;
    float cool_Time;

    //ゲームオブジェクトのTransFormを指定してターゲットが沸く範囲を指定
    [SerializeField]
    Transform rightUp_Point;
    [SerializeField]
    Transform leftUp_Point;
    [SerializeField]
    Transform rightDown_Point;
    [SerializeField]
    Transform leftDown_Point;
    [SerializeField]
    Transform Depth_Point;

    List<GameObject> targetList=new List<GameObject>();//ターゲット沸き数管理用のList

    //沸き位置指定用のランダム上限下限
    float random_x_min=0;
    float random_x_max=0;
    float random_y_min=0;
    float random_y_max=0;
    float random_z_min=0;
    float random_z_max=0;
    // Start is called before the first frame update
    void Start()
    {
        TargetInit();
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            cool_Time += Time.deltaTime;
            if (cool_Time <= late) { return; }
            if (!CheckLimit()) { return; }//沸き上限に達した場合生成は一旦待つ
            cool_Time = 0;
            float random_x = Random.Range(random_x_min,random_x_max);
            float random_y = Random.Range(random_y_min,random_y_max);
            float random_z = Random.Range(random_z_min,random_z_max);
            int Random_Num = Random.Range(0,100);
            //確率で分岐(弾を撃ってくる＆硬い紫の筒は確率低め)
            if (Random_Num >= 0 && Random_Num <= 45)
            {
                GameObject g =PhotonNetwork.Instantiate("TargetSphere", new Vector3(random_x,random_y,random_z),Quaternion.identity);
                targetList.Add(g);
            }
            if (Random_Num >= 46 && Random_Num <= 50)
            {
                GameObject g =PhotonNetwork.Instantiate("TargetCube", new Vector3(random_x, random_y, random_z), Quaternion.identity);
                targetList.Add(g);
            }
            if (Random_Num >= 51 && Random_Num <= 100)
            {
                GameObject g = PhotonNetwork.Instantiate("Pipe", new Vector3(random_x, random_y, random_z), Quaternion.identity);
                targetList.Add(g);
            }
        }
    }
    bool CheckLimit()
    {
        targetList.RemoveAll(n=>n==null);
        if (targetList.Count <= spawn_Limit)
        {
            return true;
        }
        return false;
    }
    void TargetInit()
    { 
    random_x_min = leftUp_Point.position.x;
    random_x_max = rightUp_Point.position.x;
    random_y_min = leftDown_Point.position.y;
    random_y_max = leftUp_Point.position.y;
    random_z_min = leftUp_Point.position.z;
    random_z_max = Depth_Point.position.z;
    }
}
