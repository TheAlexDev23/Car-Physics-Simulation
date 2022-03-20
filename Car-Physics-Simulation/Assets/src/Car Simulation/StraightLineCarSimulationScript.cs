using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightLineCarSimulationScript : MonoBehaviour {
    private Rigidbody cmp_rb;
    private void Start() {

        cmp_rb = GetComponent<Rigidbody>();
        cnst_Cdrag = 0.4257f; //0.5 * 0.30 * 2.2 * 1.29.  
        cnst_Crr   = 12.8f;   //30 * Cdrag

        Time.fixedDeltaTime /= 5;
    }

    [HideInInspector] public Vector3 Ftraction; //Traction generated by wheels rolling on the surface
    [HideInInspector] public Vector3 Fdrag; //Air resistance
    [HideInInspector] public Vector3 Frr; //Rolling resistance
    [HideInInspector] public Vector3 Fbraking; //Braking force
    [HideInInspector] public Vector3 Flong; //The total longtitudinal force
    
    [SerializeField]  private int var_rpm; //Max RPM
    [SerializeField]  private int var_accelerationRpmMultiplier; //RPM added per second when fully pressed "W" key
    [HideInInspector] public int _var_rpm; //Current RPM
    [SerializeField]  private AnimationCurve var_RpmToTorque; //Conversion curve between RPM and torque
    [HideInInspector] public float _var_EngineForce; //Curent torque force applied to the car
    [HideInInspector] public bool var_isBraking;
    [SerializeField]  private int var_turboBoostAmount; //When boosting this would be added to accelerationRpmMultiplier 
    
    private float cnst_Cdrag; //Air resistance constant
    private float cnst_Crr; //Rolling resisteance constant
    [SerializeField]
    private float cnst_Cbraking; //Braking constant

    private void SimulatePhyisics() {
        var u = transform.forward; 
        var velocity = cmp_rb.velocity;

        if (_var_rpm > 0) 
            _var_EngineForce = 1000 * var_RpmToTorque.Evaluate(_var_rpm / 1000);
        else
            _var_EngineForce = -1000 * var_RpmToTorque.Evaluate(-_var_rpm / 1000); //because our graph doesn't have results for negative values, this needs to be done

        //Calculation of forces
        Ftraction = _var_EngineForce * u; //Traction is the main force generated by the wheels
        Fdrag     = -cnst_Cdrag * velocity * velocity.magnitude;  //Drag is the air resistance
        Frr       = -cnst_Crr * velocity; //Rolling Resistance is the resistance caused by the friction of the wheels with the ground

        if(var_isBraking)
            Fbraking  = -cmp_rb.velocity / 5 * cnst_Cbraking * Mathf.Clamp(cmp_rb.velocity.magnitude, 0, 1); //if velocity is 0, then braking force would also be 0 on all axis
        else 
            Fbraking = Vector3.zero;
        
        Flong = Ftraction + Fdrag + Frr + Fbraking;

        cmp_rb.AddForce(Flong);
    }
}
