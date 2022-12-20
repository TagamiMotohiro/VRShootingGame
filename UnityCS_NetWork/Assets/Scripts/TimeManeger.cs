using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class TimeManeger : MonoBehaviourPunCallbacks
{
    [SerializeField]
    TMPro.TextMeshProUGUI TimeText;
    int startTime;
    // Start is called before the first frame update
    void Start()
    {
       
    }
	public override void OnJoinedRoom()
	{
	   startTime = PhotonNetwork.ServerTimestamp;
	}
	// Update is called once per frame
	void Update()
    {
        
        int time =unchecked(PhotonNetwork.ServerTimestamp-startTime);
        string timestr = time.ToString();
        int time_digits = timestr.Length;
        int timeMin = time / 60000;
        int timesec = time / 1000;
        if (timesec >= 60)
        {
            timesec -= 60;
        }
        TimeText.text = timeMin.ToString("D2")+":"+timesec.ToString("D2")+":"+timestr.Substring(time_digits-3);
    }
}
