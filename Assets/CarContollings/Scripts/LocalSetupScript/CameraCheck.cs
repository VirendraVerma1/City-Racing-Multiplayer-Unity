using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraCheck : MonoBehaviourPunCallbacks {

	private PhotonView photonView;
	void Start () {

		photonView = GetComponent<PhotonView> ();
		if (photonView.IsMine) {
			GetComponent<CameraController> ().enabled = true;
			GetComponent<Camera> ().enabled = true;
		} else {
			GetComponent<CameraController> ().enabled = false;
			GetComponent<Camera> ().enabled = false;
		}
	}

}
