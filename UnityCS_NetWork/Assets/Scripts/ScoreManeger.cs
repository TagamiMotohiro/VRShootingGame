using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class ScoreManeger : MonoBehaviourPunCallbacks 
{
    public static int Player1score = 0;
    public static int Player2score = 0;
    [SerializeField]
    TMPro.TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        Player1score = 0;
        Player2score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            scoreText.text = Player1score.ToString();
        }
        else
        {
            scoreText.text = Player2score.ToString();
        }
    }
    public void PlusScore(int plusScore)
    {
        var roomProps = new ExitGames.Client.Photon.Hashtable();
        if (PhotonNetwork.IsMasterClient)
        {
            Player1score += plusScore;
            roomProps["Player1Score"] = Player1score;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);
        }
        else
        {
            Player2score += plusScore;
            roomProps["Player2Score"] = Player2score;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);
        }
        if (plusScore <= 0)
        {
            scoreText.color = Color.red;
        }
        else
        {
            scoreText.color = Color.black;
        }
    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        Player1score = (int)PhotonNetwork.CurrentRoom.CustomProperties["Player1Score"];
        Player2score = (int)PhotonNetwork.CurrentRoom.CustomProperties["Player2Score"];
    }
}
