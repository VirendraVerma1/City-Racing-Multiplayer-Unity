using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SpotTrigger : MonoBehaviourPunCallbacks
{

    private bool Entered=false;
    
    void Start()
    {
       
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player"&&col.gameObject.GetComponent<PhotonView>().IsMine)
        {
           
            Entered = true;
            StartCoroutine(WaitFor3SecondsOnSpot());
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player" && col.gameObject.GetComponent<PhotonView>().IsMine)
        {
            GameObject.Find("Countertext").GetComponent<Text>().text = " ";
            Entered = false;
        }
    }

    IEnumerator WaitFor3SecondsOnSpot()
    {
        int num=3;
        while (num >= 0)
        {
            yield return new WaitForSeconds(1f);
            GameObject.Find("Countertext").GetComponent<Text>().text = num.ToString();
            num -= 1;
        }
        if (Entered)
        {
            //print("Still on path");
            GameObject.Find("Countertext").GetComponent<Text>().text = " ";
            bool tempGO = GameObject.Find("NetworkManager").gameObject.GetComponent<TaxiManager>().isInEvent;
            if (tempGO == true)
            {
                GameObject.Find("NetworkManager").gameObject.GetComponent<TaxiManager>().ReachedToFinishPoint();
                
            }
            else
            {
                
                GameObject.Find("NetworkManager").gameObject.GetComponent<TaxiManager>().ReachedToStartPoint();
            }
        }
        else
        {
            //print("Left");
            GameObject.Find("Countertext").GetComponent<Text>().text = "!!!";
            yield return new WaitForSeconds(1f);
            GameObject.Find("Countertext").GetComponent<Text>().text = " ";
        }
    }
}
