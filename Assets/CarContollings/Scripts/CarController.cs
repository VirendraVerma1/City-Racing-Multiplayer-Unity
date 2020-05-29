using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CarController : MonoBehaviourPunCallbacks
{
    public WheelCollider WheelFL;
    public WheelCollider WheelFR;
    public WheelCollider WheelBL;
    public WheelCollider WheelBR;

    public GameObject FL;
    public GameObject FR;
    public GameObject BL;
    public GameObject BR;


    public float topSpeed = 250f;
    public float maxTorque = 200f;
    public float maxSteerAngle = 45f;
    public float currentSpeed;
    private float maxBrakeTorque;

    private float Forward;
    private float Turn;
    private float Brake;

    private Rigidbody rb;
    public Vector3 CenterOfMass = new Vector3(0f, -0.1f, 0f);


    public Joystick joystickhor;
    public Joystick joystickver;

    public GameObject Car1;
    public GameObject Car2;
    public GameObject Car3;
    private bool changeCar = false;
    // Use this for initialization
    void Start()
    {
        saveload.Load();
        topSpeed = 200;
        maxTorque = 50f;
        maxSteerAngle = 20f;
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = CenterOfMass;

        joystickhor = GameObject.Find("PlayerStatCanvas").transform.Find("JoystickPannel").transform.Find("Fixed Joystick").GetComponent<Joystick>();
        joystickver = GameObject.Find("PlayerStatCanvas").transform.Find("JoystickPannel").transform.Find("Fixed Joystick (1)").GetComponent<Joystick>();
        if (photonView.IsMine)
            photonView.RPC("ChangeCar", RpcTarget.AllBuffered);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Forward = joystickver.Vertical;
        Turn = joystickhor.Horizontal;
        Brake = Input.GetAxis("Jump");

        WheelFR.steerAngle = maxSteerAngle * Turn;
        WheelFL.steerAngle = maxSteerAngle * Turn;

        currentSpeed = 2 * 22 / 7 * WheelBL.radius * WheelBL.rpm * 60 / 1000;//calculation in kmph

        if (currentSpeed < topSpeed)
        {
            WheelBL.motorTorque = maxTorque * Forward;
            WheelBR.motorTorque = maxTorque * Forward;
        }

        WheelBL.brakeTorque = maxBrakeTorque * Brake;
        WheelBR.brakeTorque = maxBrakeTorque * Brake;
        WheelFL.brakeTorque = maxBrakeTorque * Brake;
        WheelFR.brakeTorque = maxBrakeTorque * Brake;

    }

    void Update()
    {
        Quaternion flq;
        Vector3 flv;

        WheelFL.GetWorldPose(out flv, out flq);
        FL.transform.position = flv;
        FL.transform.rotation = flq;
        

        Quaternion Blq;
        Vector3 Blv;

        WheelBL.GetWorldPose(out Blv, out Blq);
        BL.transform.position = Blv;
        BL.transform.rotation = Blq;


        Quaternion frq;
        Vector3 frv;

        WheelFR.GetWorldPose(out frv, out frq);
        FR.transform.position = frv;
        FR.transform.rotation = frq;


        Quaternion Brq;
        Vector3 Brv;

        WheelBR.GetWorldPose(out Brv, out Brq);
        BR.transform.position = Brv;
        BR.transform.rotation = Brq;

        if (photonView.IsMine)
            photonView.RPC("ChangeCar", RpcTarget.AllBuffered);

    }

    [PunRPC]
    public void ChangeCar()
    {
        if (!changeCar)
        {
            if (saveload.carid == 1&&!Car1.active)
            {
                Car1.SetActive(true);
                Car2.SetActive(false);
                Car3.SetActive(false);
               
            }
            else if (saveload.carid == 2&&!Car2.active)
            {
                Car1.SetActive(false);
                Car2.SetActive(true);
                Car3.SetActive(false);
                
            }
            else if (saveload.carid == 3&&!Car3.active)
            {
                Car1.SetActive(false);
                Car2.SetActive(false);
                Car3.SetActive(true);
            }
        }
    }
}
