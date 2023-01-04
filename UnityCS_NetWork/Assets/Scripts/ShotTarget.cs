using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using UnityEngine.LowLevel;
using static UnityEngine.GraphicsBuffer;
using UnityChan;

public class ShotTarget : Target
{
    [SerializeField]
    bool isStart = false;
    [SerializeField]
    float late = 10f;
    float coolTime;
    GameObject firePos;
    List<GameObject> Player_List;
    GameObject LookPlayer;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            isStart = true;
        }
        Player_List = GameObject.FindGameObjectsWithTag("Player").ToList();
        LookPlayer = closestPlayer();
        base.Start();
        firePos = transform.GetChild(0).gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isStart) { return; }
        
        LookAtTransformUp();
        if (coolTime > late)
        {
            coolTime = 0;
            if (PhotonNetwork.LocalPlayer.IsMasterClient == false) { return; }
            GameObject g = PhotonNetwork.Instantiate("ChaseSphere", firePos.transform.position, Quaternion.identity);
            g.GetComponent<MoveTarget>().SetTarget(LookPlayer);
        }
        coolTime += Time.deltaTime;
    }
    GameObject closestPlayer()
    {

        GameObject clossest = null;
        float minDistance = float.MaxValue;
        foreach (GameObject g in Player_List)
        {
            float gPos = Mathf.Abs(transform.position.magnitude - g.transform.position.magnitude);
            if (gPos < minDistance)
            {
                clossest = g;
                minDistance = gPos;
            }
        }
        return clossest;
    }
    void LookAtTransformUp()
    {
        //Quaternion q = Quaternion.LookRotation(transform.up, LookPlayer.transform.position - this.transform.position); 
        //transform.rotation= q;
        this.transform.LookAt(LookPlayer.transform.position);
        this.transform.rotation = transform.rotation*Quaternion.AngleAxis(90,Vector3.right);
    }
}
