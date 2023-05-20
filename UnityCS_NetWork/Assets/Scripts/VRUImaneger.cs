using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VRUImaneger : MonoBehaviour
{
    //制作担当　田上
    //タイトル画面のUI関連クラス
    [Header ("ボタンと素材")]
    [SerializeField] Button SinglePlayerModeButton;
    [SerializeField] Button MaltiPlayerModeButton;
    [SerializeField] Button ExplainButton;
    [SerializeField] GameObject ExplainPanel;
    [SerializeField] AudioClip selected_SE;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        //audio関連の取得とボタンのイベント待機開始
        audioSource = this.GetComponent<AudioSource>();
        SinglePlayerModeButton.onClick.AddListener(() => SinglePlayerModeEntry());
        MaltiPlayerModeButton.onClick.AddListener(() => MaltiPlayerModeEntry());
    }
    void ButtonSelectedAction()
    {
        //何かしらボタンが押されたらマッチ関連のボタンは押せないようにする
        SinglePlayerModeButton.gameObject.SetActive(false);
        MaltiPlayerModeButton.gameObject.SetActive(false);
        audioSource.PlayOneShot(selected_SE);
    } 
    //ソロモードとマルチモードそれぞれのエントリー用関数
    void SinglePlayerModeEntry()
    {
        ButtonSelectedAction();
        this.GetComponent<MatchMakeManeger>().SoloMode();
    }
    void MaltiPlayerModeEntry()
    {
        ButtonSelectedAction();
        this.GetComponent<MatchMakeManeger>().StartConect();
    }
    // Update is called once per frame
    void Update()
    {
        //操作説明ボタン系の処理
        ExplainButton.onClick.AddListener( ( ) => MoveExplain( ) );
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            ExplainPanel.SetActive(false);
        }
    }
    //操作説明パネルをOnにする
    void MoveExplain( ) {
        ExplainPanel.SetActive(true);
    }
}
