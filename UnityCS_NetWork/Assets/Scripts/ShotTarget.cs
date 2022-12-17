using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShotTarget : Target
{
    [SerializeField]
    bool isStart = false;
    [SerializeField]
    float late=10f;
    float coolTime;
    GameObject firePos;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            isStart = true;
        }
        base.Start();
        firePos = transform.GetChild(0).gameObject;
    }
	// Update is called once per frame
	void Update()
    {
        if (!isStart) { return; }
        if (coolTime > late)
        {
            coolTime = 0;
            if (PhotonNetwork.LocalPlayer.IsMasterClient == false) { return; }
            PhotonNetwork.Instantiate("ChaseSphere",firePos.transform.position,Quaternion.identity);
        }
        coolTime += Time.deltaTime;
    }
}
