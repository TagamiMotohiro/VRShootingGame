using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChange : MonoBehaviour
{
    PlayerGun gun_Script;
    PlayerGun.GUN_STATE state; 
    [SerializeField]
    TMPro.TextMeshProUGUI state_Text;
    [SerializeField]
    Transform camerapos;
    // Start is called before the first frame update
    void Start()
    {
        gun_Script = GetComponent<PlayerGun>();
    }
    // Update is called once per frame
    void Update()
    {
        TextLookAt();
        //state_Text.gameObject.transform.LookAt(camerapos);
        state = gun_Script.state;
        switch (state)
        {
            case PlayerGun.GUN_STATE.RAPID:
                state_Text.text = "RAPID";
                break;
            case PlayerGun.GUN_STATE.SHOTGUN:
                state_Text.text = "SHOTGUN";
                break;
        }
    }
    void TextLookAt()
    {
        state_Text.transform.LookAt(camerapos);
    }
}
