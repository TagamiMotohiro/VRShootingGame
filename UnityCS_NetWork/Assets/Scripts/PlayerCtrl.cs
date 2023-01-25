using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerCtrl : MonoBehaviourPunCallbacks
{
    private GameObject OVRcamera;
    public int hp;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        { 
            this.gameObject.layer = 6;
        }
        OVRcamera = GameObject.Find("OVRCameraRig");
    }

    // Update is called once per frame
    void Update()
    {
		if (photonView.IsMine)
		{
			this.transform.position = OVRcamera.transform.position;
		}
        if (hp <= 0)
        {
           
        }
	}
    void damege()
    {
        this.hp--;
    }
}
