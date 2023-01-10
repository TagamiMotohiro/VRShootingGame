using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class MoveTarget : Target
{
    List<GameObject> player_List;
    GameObject target;
    [SerializeField]
    float chaseSpeed;
    // Start is called before the first frame update
	void OnCollisionEnter(Collision collision)
	{
        base.OnCollisionEnter(collision);
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PhotonView>().IsMine == false) { return; }
            PhotonNetwork.Destroy(this.gameObject);
            maneger.PlusScore(-2000);
        }
        else
        if (collision.gameObject.tag == "Shield")
        {
            if (collision.gameObject.GetComponent<PhotonView>().IsMine == false) { return; }
            PhotonNetwork.Destroy(this.gameObject);
            maneger.PlusScore(200);
        }
	}
	// Update is called once per frame
	void Update()
    {
        if (target == null) { return; }
        transform.position = Vector3.MoveTowards(transform.position,target.transform.position,chaseSpeed*Time.deltaTime);
    }

    public void SetTarget(GameObject g)
    {
        target = g;
    }
}
