using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VRUImaneger : MonoBehaviour
{
    [SerializeField] Button SinglePlayerModeButton;
    [SerializeField] Button MaltiPlayerModeButton;
    [SerializeField] Button Explain;
    [SerializeField] GameObject ExplainPanel;
    [SerializeField] AudioClip selected_SE;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        SinglePlayerModeButton.onClick.AddListener(() => SinglePlayerModeEntry());
        MaltiPlayerModeButton.onClick.AddListener(() => MaltiPlayerModeEntry());
    }
    void ButtonSelectedAction()
    {
        SinglePlayerModeButton.gameObject.SetActive(false);
        MaltiPlayerModeButton.gameObject.SetActive(false);
        audioSource.PlayOneShot(selected_SE);
    }
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
    void MoveExplain( ) {
        ExplainPanel.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        Explain.onClick.AddListener( ( ) => MoveExplain( ) );
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            ExplainPanel.SetActive(false);
        }
    }
}
