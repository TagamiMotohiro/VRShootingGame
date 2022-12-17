using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shieldCtrl : MonoBehaviour
{
    //private void OnTriggerEnter(Collider other)
    //{
        
    //}
    //private void OnCollisionEnter(Collision collision)
    //{
        
    //}
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            return;
        }
        Debug.Log("íeÇ™èÇÇ…ìñÇΩÇ¡ÇΩ");
        Destroy(other.gameObject);
    }
}
