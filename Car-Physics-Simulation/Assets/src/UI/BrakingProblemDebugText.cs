using UnityEngine;
using TMPro;

public class BrakingProblemDebugText : MonoBehaviour {
    public TextMeshProUGUI Guitext;

    public bool IsReacting;
    public bool IsBreaking;

    private void Start() {
        Guitext = GetComponent<TextMeshProUGUI>();
    }

    private float reactionTime;
    private float reactionDistance;
    private float stoppingTime;
    private float stoppingDistance;

    private float stoppingDistanceTotal;
    private float stoppingTimeTotal;

    private void Update() {
        var car  = GameObject.Find("Car").GetComponent<StraightLineCarSimulationScript>();
        //We're driving normally, without reacting or breaking
        if (!IsReacting && !IsBreaking)  {
            Guitext.text = 
            "RPM: " + car._var_rpm + "\n" +
            "Speed: " + car.gameObject.GetComponent<Rigidbody>().velocity.magnitude.ToString("000") + "\n" +
            "Is Braking: " + car.var_isBraking + "\n\n\n" + 
            "Velocity: " + car.gameObject.GetComponent<Rigidbody>().velocity + "\n" +
            "Braking Force: " + car.Fbraking + "\n" +
            "Time elapsed: " + Time.time.ToString("000.000");
        } else if (IsReacting) {
            //During this time the driver is reacting to the obstacle
            reactionTime += Time.deltaTime;
            reactionDistance += car.gameObject.GetComponent<Rigidbody>().velocity.magnitude * Time.deltaTime;

            Guitext.text =
            "Speed: " + car.gameObject.GetComponent<Rigidbody>().velocity.magnitude.ToString("000") + "\n" +
            "Reaction Time: " + reactionTime + "\n" +
            "ReactionDistane: " + reactionDistance;
        } else if (IsBreaking && (int)car.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 0) {
            //During this time the driver is pressing the brakes and the car still hasn't stopped moving
            car.var_isBraking = true;
            stoppingDistance += car.gameObject.GetComponent<Rigidbody>().velocity.magnitude * Time.deltaTime;
            stoppingTime += Time.deltaTime;

            stoppingDistanceTotal = reactionDistance + stoppingDistance;
            stoppingTimeTotal = reactionTime + stoppingTime;

            Guitext.text = 
            "Speed: " + car.gameObject.GetComponent<Rigidbody>().velocity.magnitude.ToString("000") + "\n\n" +
            "Reaction Time: " + reactionTime + "\n" +
            "Reaction Distance: " + reactionDistance + "\n" + 
            "Breaking Time: " + stoppingDistance + "\n" +
            "Breaking Distance: " + stoppingTime + "\n\n" +
            "Stopping Time (Reaction Time + Breaking Time): " + (float)(reactionTime + stoppingTime) + "\n" +
            "Stopping Distance (Reaction Distance + Breaking Distance): " + (float)(reactionDistance + stoppingDistance); 
        } else if (!((int)car.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 0)) {
            //If we are braking and the speed reaches 0, then means that we stopped. 
            Guitext.text = 
            "Speed: " + car.gameObject.GetComponent<Rigidbody>().velocity.magnitude.ToString("000") + "\n\n" +
            "Reaction Time: " + reactionTime + "\n" +
            "Reaction Distance: " + reactionDistance + "\n" + 
            "Breaking Time: " + stoppingTime + "\n" +
            "Breaking Distance: " + stoppingDistance + "\n\n" +
            "Stopping Time (Reaction Time + Breaking Time): " + stoppingTimeTotal + "\n" +
            "Stopping Distance (Reaction Distance + Breaking Distance): " + stoppingDistanceTotal; 
        }
    }
}
