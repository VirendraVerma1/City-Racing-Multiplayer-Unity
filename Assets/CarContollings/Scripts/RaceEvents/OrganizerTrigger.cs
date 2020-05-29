using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OrganizerTrigger : MonoBehaviourPunCallbacks
{
   public GameObject BetPannel;
    void Start()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player"&&col.gameObject.GetComponent<PhotonView>().IsMine)
        {
            BetPannel.SetActive(true);
        }
    }
}
