using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BrakingProblemSceneManager : MonoBehaviour {
    [SerializeField] private string UiScene; 
    [SerializeField] private string GameScene;
    [SerializeField] private string MapPartsScene;

    private AsyncOperation UiSceneLoadingOperation;
    private AsyncOperation LightingSceneLoadingOperation;
    private AsyncOperation GameSceneLoadingOperation;
    void Start() {
        GameSceneLoadingOperation     = SceneManager.LoadSceneAsync(GameScene, LoadSceneMode.Additive);
        UiSceneLoadingOperation       = SceneManager.LoadSceneAsync(UiScene, LoadSceneMode.Additive);
        LightingSceneLoadingOperation = SceneManager.LoadSceneAsync(MapPartsScene, LoadSceneMode.Additive);

        progressSlider = GameObject.Find("Loading Scene Canvas").transform.Find("SceneLoadingProgressBar").GetComponent<Slider>();
    }

    private float loadingProgress;

    private Slider progressSlider;
    void Update() {
        loadingProgress = (UiSceneLoadingOperation.progress + LightingSceneLoadingOperation.progress + GameSceneLoadingOperation.progress) / 3f;
        progressSlider.value = loadingProgress;
        if (UiSceneLoadingOperation.isDone && LightingSceneLoadingOperation.isDone && GameSceneLoadingOperation.isDone) {
            SceneManager.UnloadSceneAsync("__SceneManager_FreeDrive");
        }
    }
}
