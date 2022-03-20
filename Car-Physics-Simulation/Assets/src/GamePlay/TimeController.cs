using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour {
    public float TimeScale;

    private void Update() {
        Time.timeScale = TimeScale;
        Time.fixedDeltaTime = Time.deltaTime * TimeScale;
    }
}
