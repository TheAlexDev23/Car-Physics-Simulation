using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainCarSimulationScript : MonoBehaviour {
    private Rigidbody cmp_rb;
    public void Start() {
        cmp_rb = GetComponent<Rigidbody>();
        
        /*Air density (rho) is 1.29 kg/m3 (0.0801 lb-mass/ft3), 
        frontal area is approx. 2.2 m2 (20 sq. feet), 
        Coefficient of friction depends on the shape of the car and determined via wind tunnel tests.  
        Approximate value for a Corvette: 0.30
        */
        cnst_Cdrag = 0.4257f; //0.5 * 0.30 * 2.2 * 1.29.  
        cnst_Crr   = 12.8f;   //30 * Cdrag

        GetWheels();

        Time.fixedDeltaTime /= 5;
    }

    private GameObject go_WheelsParentGameObject;
    private List<GameObject> go_wheelsGameObjects = new List<GameObject>();
 
    private void GetWheels() {
        go_WheelsParentGameObject = GameObject.Find("Wheels");
        foreach (Transform wheel in go_WheelsParentGameObject.transform) {
            go_wheelsGameObjects.Add(wheel.gameObject);
        }
    }

    public void FixedUpdate() {
        SimulatePhyisics();
    }

    private Vector3 Ftraction; //Traction generated by wheels rolling on the surface
    private Vector3 Fdrag; //Air resistance
    private Vector3 Frr; //Rolling resistance
    private Vector3 Fbraking; //Braking force
    private Vector3 Flong; //The total longtitudinal force
    public float var_EngineForce;
    private float _var_EngineForce;
    private bool var_isBraking;
    private float cnst_Cdrag; //Air resistance constant
    private float cnst_Crr; //Rolling resisteance constant
    public float cnst_Cbraking; //Braking constant

    private void SimulatePhyisics() {
        var u = transform.forward; 
        var velocity = cmp_rb.velocity;

        //Calculation of forces
        _var_EngineForce = var_EngineForce * Input.GetAxis("Vertical");

        Ftraction = _var_EngineForce * u;
        Fdrag     = -cnst_Cdrag * velocity * velocity.magnitude; 
        Frr       = -cnst_Crr * velocity;

        if(var_isBraking)
            Fbraking  = -u * cnst_Cbraking * Mathf.Clamp(cmp_rb.velocity.magnitude, 0, 1);
        else 
            Fbraking = Vector3.zero;

        Flong = Ftraction + Fdrag + Frr + Fbraking;

        if(Input.GetAxis("Horizontal") != 0) cmp_rb.AddForce(go_wheelsGameObjects[3].transform.forward * _var_EngineForce + Frr);
        cmp_rb.AddForce(Flong);
    }

    private void SimulateCarRotation() {
        var angleMultiplier = 0;
        if (Input.GetAxis("Horizontal") < 0) angleMultiplier = -1;
        if (Input.GetAxis("Horizontal") > 0) angleMultiplier = 1;

        var _angle = Vector3.Angle(transform.forward, go_wheelsGameObjects[3].transform.forward) * angleMultiplier;
        
        var rotationVector = new Vector3(0, _angle, 0);

        float speedRotationMultiplier = 0;

        if (cmp_rb.velocity.magnitude > 3) speedRotationMultiplier = 1.5f;
        if (cmp_rb.velocity.magnitude > 45) speedRotationMultiplier = 1f;
        if (cmp_rb.velocity.magnitude > 90) speedRotationMultiplier = .5f;

        transform.Rotate(rotationVector * Time.deltaTime * speedRotationMultiplier);
    }

    private void UpdateWheelAngle() {
        go_WheelsParentGameObject.transform.position = transform.position;
        go_WheelsParentGameObject.transform.rotation = transform.rotation;

        var Horizontal = Mathf.Clamp(Input.GetAxis("Horizontal"), -0.85f, 0.85f);

        var angle = 90 - Mathf.Acos(Horizontal) * Mathf.Rad2Deg;

        if((int)angle == 0) angle = 0;

        var wheelRotationVector = new Vector3(0, angle, 0);

        go_wheelsGameObjects[2].transform.localEulerAngles = go_wheelsGameObjects[3].transform.localEulerAngles = wheelRotationVector;
    }

    private void UpdateDebugInfo() {
        var text = GameObject.Find("Canvas").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        text.text = 
        "Traction: " + Ftraction + "\n" +
        "Drag: " + Fdrag + "\n" +
        "Rolling Resistance: " + Frr + "\n" + 
        "Braking: " + Fbraking + "\n" +
        "Longitudinal: " + Flong + "\n" +
        "Engine force (N): " + _var_EngineForce + "\n" +
        "Is Braking (T/F): " + var_isBraking + "\n" +
        "Horizontal: " + Input.GetAxis("Horizontal").ToString("0.00") + "\n" +
        "Vertical: " + Input.GetAxis("Vertical").ToString("0.00") + "\n" +
        "Velocity: " + cmp_rb.velocity + "\n" +
        "Speed: " + cmp_rb.velocity.magnitude.ToString("000");
    }
   
    public void Update() {
        UpdateWheelAngle();
        SimulateCarRotation();
        UpdateDebugInfo();
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            var_isBraking = true;
            //var_EngineForce -= cnst_Cbraking;
        }
        if (Input.GetKeyUp(KeyCode.Space)) {
            var_isBraking = false;
            //var_EngineForce += cnst_Cbraking;
        }
    }
}
