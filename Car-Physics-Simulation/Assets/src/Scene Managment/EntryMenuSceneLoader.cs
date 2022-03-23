using UnityEngine;
using UnityEngine.SceneManagement;

public class EntryMenuSceneLoader : MonoBehaviour {
    public void LoadFreeDrive() {
        SceneManager.LoadScene("__FreeDrive");
        SceneManager.UnloadSceneAsync("__EntryScene");
    }

    public void LoadBrakingProblem() {
        SceneManager.LoadScene("__SceneManager_BrakingProblem");
        SceneManager.UnloadSceneAsync("__EntryScene");
    }
}
