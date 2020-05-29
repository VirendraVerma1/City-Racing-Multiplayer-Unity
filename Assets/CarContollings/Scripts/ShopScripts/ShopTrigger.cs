using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShopTrigger : MonoBehaviourPunCallbacks
{

    public GameObject ShopPannel;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && col.gameObject.GetComponent<PhotonView>().IsMine)
        {
            ShopPannel.SetActive(true);
        }
    }
}
