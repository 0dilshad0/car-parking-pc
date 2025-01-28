using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class wheelControl : MonoBehaviour
{
    [Header("Wheel Colliders")]
    [SerializeField] WheelCollider FR;
    [SerializeField] WheelCollider FL;
    [SerializeField] WheelCollider BR;
    [SerializeField] WheelCollider BL;

    [Header("wheel transform")]
    [SerializeField] Transform transformFR;
    [SerializeField] Transform transformFL;
    [SerializeField] Transform transformBR;
    [SerializeField] Transform transformBL;

    [SerializeField] Transform steering;

    [Header("particle Systems")]
    [SerializeField] TrailRenderer LeftTrail;
    [SerializeField] TrailRenderer rightTrail;

    [SerializeField] ParticleSystem LeftSmoke;
    [SerializeField] ParticleSystem RightSmoke;


    [SerializeField] Rigidbody rb;

  

    [Header ("Lighting")]
    [SerializeField] GameObject breakLight;
    [SerializeField] GameObject headLight;
    [SerializeField] GameObject tailLight;
    private bool IsLight = false;


    [SerializeField] AudioSource breakAudio;
    [SerializeField] AudioSource runingAudio;
    
    [Header ("driving settings")]
    public float accelaration = 1500f;
    public float breakforce = 3000f;
    public float Maxturn = 15f;
    public float downforce = 50f;
    public float TopSeed = 70f;
    public float currentSpeed = 0f;

    private float currentAccelaration = 0f;
    private float currentBreakforce = 0f;
    private float currentTurn = 0f;

    [SerializeField] GameObject overScreen;
    public float centerOfMass = -0.5f;
    private car_ui Gear;

    
    private void Start()
    {
      
        Gear = GetComponent<car_ui>();
        overScreen.SetActive(false);
        Time.timeScale = 1f;
    }
    private void FixedUpdate()
    {
       

        rb.centerOfMass = new Vector3(0, centerOfMass, 0);

        //move
        //currentAccelaration = Mathf.Clamp( accelaration * Input.GetAxis("Vertical"),-accelaration,accelaration);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if(Gear.gear.value==0)
            {
                currentAccelaration = -accelaration;
               
            }
            else
            {
                currentAccelaration = accelaration;
             
            }

        }
        else
        {
            currentAccelaration = 0;
        }
        if (rb.velocity.magnitude * 3.6f < TopSeed)
        {
            FR.motorTorque = currentAccelaration;
            FL.motorTorque = currentAccelaration;
            BL.motorTorque = currentAccelaration;
            BR.motorTorque = currentAccelaration;
        }


        //speed

        currentSpeed = Mathf.PI * FL.radius * FL.rpm * 60 / 1000;


        //break
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            currentBreakforce = breakforce;
            breakLight.SetActive(true);
            if (currentAccelaration !=0)
            {
                breakTrails(true);
            }    
          
           
        }
        else
        {

            breakLight.SetActive(false);
            currentBreakforce = 0f;
            breakTrails(false);
        }
        FR.brakeTorque = currentBreakforce;
        FL.brakeTorque = currentBreakforce;
        BR.brakeTorque = currentBreakforce;
        BL.brakeTorque = currentBreakforce;


        //side
        currentTurn = Maxturn * Input.GetAxis("Horizontal");
        FR.steerAngle = currentTurn;
        FL.steerAngle = currentTurn;

        //steering
        steering.localRotation = Quaternion.Euler(0, 0, -currentTurn * 5);

        //consroll transform
        updateWheel(FL, transformFL);
        updateWheel(FR, transformFR);
        updateWheel(BL, transformBL);
        updateWheel(BR, transformBR);


        //applaydownforce

        rb.AddForce(-transform.up * downforce * rb.velocity.magnitude);


        //audio
        
            audioUpdate();
        
      

        //Head Light

        if(Input.GetKeyDown(KeyCode.L))
        {
            if(IsLight)
            {
                headLight.SetActive(false);
                tailLight.SetActive(false);
                Debug.Log("light On");
                IsLight = false;
            }
            else
            {
                headLight.SetActive(true);
                tailLight.SetActive(true);
                Debug.Log("light Off");
                IsLight = true;
            }
        }
    }

     void updateWheel(WheelCollider col,Transform trans)
    {
        Vector3 position;
        Quaternion rotation;

        col.GetWorldPose(out position, out rotation);
        trans.position = position;
        trans.rotation = rotation;

    }

    void breakTrails(bool enable)
    {
        if(LeftTrail && rightTrail)
        {
            LeftTrail.emitting = enable;
            rightTrail.emitting = enable;
        }
       

        if(enable)
        {


            LeftSmoke.Play();
            RightSmoke.Play();
            breakAudio.volume = 0.1f;
        }
        else
        {
           
         
            LeftSmoke.Stop();
            RightSmoke.Stop();
            breakAudio.volume = 0f;
        }
     
       
    }
    void audioUpdate()
    {
        float speed = rb.velocity.magnitude;
        runingAudio.pitch = Mathf.Lerp(0.8f, 5.0f, speed / TopSeed);

       
            runingAudio.volume = Mathf.Lerp(0.1f, 0.5f, speed / TopSeed);
        
       



    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("NonObstacle"))
        {
            Time.timeScale = 0f;
            runingAudio.Stop();
            breakAudio.Stop();
            Cursor.lockState = CursorLockMode.None;
            overScreen.SetActive(true);
        }
    }
}
