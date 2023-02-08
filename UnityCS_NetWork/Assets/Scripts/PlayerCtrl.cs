using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerCtrl : MonoBehaviourPunCallbacks
{
    private GameObject OVRcamera;
    [SerializeField]
    GameObject Panel;
    public int hp;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            this.gameObject.layer = 6;
            this.transform.GetChild(0).gameObject.layer = 6;
        }
        else
        {
            Panel.SetActive(true);
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
        else
        {
            Panel.transform.LookAt(OVRcamera.transform.position);
        }
	}
    void damege()
    {
        this.hp--;
    }
}
