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
    void Start()
    {
        base.Start();
        float minDistance=float.MaxValue;
        player_List = GameObject.FindGameObjectsWithTag("Player").ToList();
        foreach (GameObject g in player_List)
        {
            float gPos= Mathf.Abs(transform.position.magnitude - g.transform.position.magnitude);
            if (gPos < minDistance)
            {
                target = g;
                minDistance = gPos;
            }
        }
    }
	void OnCollisionEnter(Collision collision)
	{
        base.OnCollisionEnter(collision);
        if (collision.gameObject.tag == "Player"||collision.gameObject.tag=="Shield")
        {
            if (collision.gameObject.GetComponent<PhotonView>().IsMine == false) { return; }
            PhotonNetwork.Destroy(this.gameObject);
            maneger.PlusScore(-2000);
        }
	}
	// Update is called once per frame
	void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,target.transform.position,chaseSpeed*Time.deltaTime);
    }
}
