using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class TaxiManager : MonoBehaviourPunCallbacks
{


    public float counterTimerofEvent=0f;
    public float nextEventTime = 10f;
    public bool isInEvent = false;
    public bool SingleEvent = false;

    //for spot generation
    public GameObject SpotCube;
    public GameObject PersonForTaxi;
   

    public Transform[] SpotPoints;
    public int StartSpotNumber;
    public int FinalPointNumber;
    int temp;
    private int tipAmount;
    GameObject go;
    GameObject goperson;
    public Text TipMoneyText;
    public GameObject TipGO;
    public int money = 500;

    
    public PhotonView photonView;
    private GameObject MyCar;

    public Text moneyText;
    void Start()
    {
        KnowMyCar();
        
        //TipGO = GameObject.Find("DownCounterMission");
        //TipMoneyText = GameObject.Find("DownCounterMoney").GetComponent<Text>();
        TipGO.SetActive(false);
    }

    void KnowMyCar()
    {
        GameObject[] AllPlayerCars = GameObject.FindGameObjectsWithTag("Player");
        bool found = false;

        foreach (GameObject go in AllPlayerCars)
        {
            photonView = go.GetComponent<PhotonView>();
            if (photonView.IsMine && found == false)
            {
                MyCar = go;
                found = true;
            }
        }
        photonView = MyCar.GetComponent<PhotonView>();
    }
    void Update()
    {
        counterTimerofEvent -= Time.deltaTime;
        if (counterTimerofEvent < 0 && !isInEvent && !SingleEvent&&photonView.IsMine)
        {
            counterTimerofEvent = 5;
            //event happen
            FindNextSpot();
        }
    }

    void FindNextSpot()
    {
       temp = Random.Range(0, SpotPoints.Length);
        StartSpotNumber = temp;
        FindFinalSpot();
        FinalPointNumber = temp;
        CreateStartBoxSpot();
    }

    void CreateStartBoxSpot()
    {
        go = Instantiate(SpotCube, SpotPoints[StartSpotNumber].position, SpotPoints[StartSpotNumber].rotation);
        go.transform.SetParent(SpotPoints[StartSpotNumber]);
        goperson = Instantiate(PersonForTaxi, SpotPoints[StartSpotNumber].position, SpotPoints[StartSpotNumber].rotation);
        SingleEvent = true;
    }

    public void ReachedToStartPoint()
    {
        TipGO.SetActive(true);
        Destroy(goperson);
        Destroy(go);
        isInEvent = true;
        CreateFinishSpot();
        
    }

    void CreateFinishSpot()
    {
        
        go = Instantiate(SpotCube, SpotPoints[FinalPointNumber].position, SpotPoints[FinalPointNumber].rotation);
        go.transform.SetParent(SpotPoints[FinalPointNumber]);

        //tip start reducing

        TipMoneyText.text = " ";
        StartCoroutine(ReduceTipAmmount());
    }

    IEnumerator ReduceTipAmmount()
    {
        float distance = Vector3.Distance(SpotPoints[FinalPointNumber].position , SpotPoints[StartSpotNumber].position);
        tipAmount = Random.Range(10, 20);
        print(distance);
        tipAmount *=Mathf.RoundToInt( distance);
        while (tipAmount > 0 && isInEvent)
        {
            yield return new WaitForSeconds(1f);
            tipAmount -= 2;
            TipMoneyText.text = "$" + tipAmount.ToString();
        }

    }

    public void ReachedToFinishPoint()
    {
        Destroy(go);
        isInEvent = false;
        SingleEvent = false;
        goperson = Instantiate(PersonForTaxi, SpotPoints[FinalPointNumber].position, SpotPoints[FinalPointNumber].rotation);
        Destroy(goperson, 20f);
        GameObject.Find("Countertext").GetComponent<Text>().text = "+$" + tipAmount.ToString();
        StartCoroutine(waitTOwash());
        
        money += tipAmount;
        GameObject.Find("MoneyText").GetComponent<Text>().text = " $ "+money.ToString();
        TipGO.SetActive(false);
    }

    IEnumerator waitTOwash()
    {
        yield return new WaitForSeconds(1);
        GameObject.Find("Countertext").GetComponent<Text>().text = " ";
    }
    void FindFinalSpot()
    {
        temp = Random.Range(0, SpotPoints.Length);

        if (StartSpotNumber == temp)
        {
            FindFinalSpot();
        }
       
    }

    public void UpdateMoney()
    {
        GameObject.Find("MoneyText").GetComponent<Text>().text = " $ " + money.ToString();
    }
}
