using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CheckCar : MonoBehaviourPunCallbacks {

	private PhotonView photonView;
	private Vector3 TargetPosition;
	private Quaternion TargetRotation;

    public bool my;
    public string OwnerName;
	void Start () {
		photonView = GetComponent<PhotonView> ();
        
		if (photonView.IsMine) {
            my = true;
			GetComponent<CarController> ().enabled = true;
            OwnerName = photonView.Owner.NickName;
            FindObjectOfType<RaceOrganizerManager>().MyName = OwnerName;
            FindObjectOfType<RaceOrganizerManager>().MainHoon = my;
           
			//GetComponent<Playermanager> ().enabled = true;
		} else {
            my = false;
            FindObjectOfType<RaceOrganizerManager>().MainHoon = my;
			GetComponent<CarController> ().enabled = false;
           
		}
	}

	
}
