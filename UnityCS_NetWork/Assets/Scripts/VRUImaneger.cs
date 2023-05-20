using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VRUImaneger : MonoBehaviour
{
    //����S���@�c��
    //�^�C�g����ʂ�UI�֘A�N���X
    [Header ("�{�^���Ƒf��")]
    [SerializeField] Button SinglePlayerModeButton;
    [SerializeField] Button MaltiPlayerModeButton;
    [SerializeField] Button ExplainButton;
    [SerializeField] GameObject ExplainPanel;
    [SerializeField] AudioClip selected_SE;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        //audio�֘A�̎擾�ƃ{�^���̃C�x���g�ҋ@�J�n
        audioSource = this.GetComponent<AudioSource>();
        SinglePlayerModeButton.onClick.AddListener(() => SinglePlayerModeEntry());
        MaltiPlayerModeButton.onClick.AddListener(() => MaltiPlayerModeEntry());
    }
    void ButtonSelectedAction()
    {
        //��������{�^���������ꂽ��}�b�`�֘A�̃{�^���͉����Ȃ��悤�ɂ���
        SinglePlayerModeButton.gameObject.SetActive(false);
        MaltiPlayerModeButton.gameObject.SetActive(false);
        audioSource.PlayOneShot(selected_SE);
    } 
    //�\�����[�h�ƃ}���`���[�h���ꂼ��̃G���g���[�p�֐�
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
        //��������{�^���n�̏���
        ExplainButton.onClick.AddListener( ( ) => MoveExplain( ) );
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            ExplainPanel.SetActive(false);
        }
    }
    //��������p�l����On�ɂ���
    void MoveExplain( ) {
        ExplainPanel.SetActive(true);
    }
}
