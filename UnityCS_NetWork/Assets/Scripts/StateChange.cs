using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChange : MonoBehaviour
{
    PlayerGun gun_Script;
    PlayerGun.GUN_STATE state; 
    [SerializeField]
    TMPro.TextMeshProUGUI state_Text;
    // Start is called before the first frame update
    void Start()
    {
        gun_Script = GetComponent<PlayerGun>();
    }
    // Update is called once per frame
    void Update()
    {
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
}
