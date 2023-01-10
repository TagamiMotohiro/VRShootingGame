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
    
    // Start is called before the first frame update
    void Start()
    {
        SinglePlayerModeButton.onClick.AddListener(() => SinglePlayerModeEntry());
        MaltiPlayerModeButton.onClick.AddListener(() => MaltiPlayerModeEntry());
    }
    void ButtonSelectedAction()
    {
        SinglePlayerModeButton.gameObject.SetActive(false);
        MaltiPlayerModeButton.gameObject.SetActive(false);
    }
    void SinglePlayerModeEntry()
    {
        ButtonSelectedAction();
        this.GetComponent<MathMakeManeger>().SoloMode();
    }
    void MaltiPlayerModeEntry()
    {
        ButtonSelectedAction();
        this.GetComponent<MathMakeManeger>().StartConect();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
