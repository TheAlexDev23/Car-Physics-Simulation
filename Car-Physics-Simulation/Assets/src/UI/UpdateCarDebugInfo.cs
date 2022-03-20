using UnityEngine;
using TMPro;

public class UpdateCarDebugInfo : MonoBehaviour {
    public void Start() {
        cmp_carSimulationScript = GetComponent<MainCarSimulationScript>();
        cmp_rb = GetComponent<Rigidbody>();
    }

    public void Update() {
        UpdateDebugInfo();
    }
    
    private MainCarSimulationScript cmp_carSimulationScript;
    private Rigidbody cmp_rb;
    private void UpdateDebugInfo() {
        var text = GameObject.Find("Debug Canvas").transform.Find("Debug Text CarSimulationScript").GetComponent<TextMeshProUGUI>();

        text.text = 
        "Traction: " + cmp_carSimulationScript.Ftraction + "\n" +
        "Drag: " + cmp_carSimulationScript.Fdrag + "\n" +
        "Rolling Resistance: " + cmp_carSimulationScript.Frr + "\n" + 
        "Braking: " + cmp_carSimulationScript.Fbraking + "\n" +
        "Longitudinal: " + cmp_carSimulationScript.Flong + "\n" +
        "Engine force (N): " + cmp_carSimulationScript._var_EngineForce + "\n" +
        "Is Braking (T/F): " + cmp_carSimulationScript.var_isBraking + "\n" +
        "Horizontal: " + Input.GetAxis("Horizontal").ToString("0.00") + "\n" +
        "Vertical: " + Input.GetAxis("Vertical").ToString("0.00") + "\n" +
        "Velocity: " + cmp_rb.velocity + "\n" +
        "Speed: " + cmp_rb.velocity.magnitude.ToString("000") + "\n" +
        "RPM: " + Mathf.Abs(cmp_carSimulationScript._var_rpm);
    }
}
