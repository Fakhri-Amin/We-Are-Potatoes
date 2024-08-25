using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using static Gley.MobileAds.Events;
using Gley.MobileAds;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private float minLoadingDuration;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text loadingProgressText;
    [SerializeField] private TMP_Text loadingStatusText;
    [SerializeField][Range(0, 1)] private float loadingProgress2 = 0;

    private bool isClicked;

    private void Awake()
    {
        Gley.MobileAds.API.Initialize();
        if (Data.Get<UserData>().CreatedCharacterIDs.Count == 0)
        {
            StartCoroutine(IFakeLoadSceneAsync("CharacterCreation"));
        }
        else
        {
            StartCoroutine(IFakeLoadSceneAsync("MainArea"));
        }

        // ProgressLoadScene();
    }
    
    private void ProgressLoadScene()
    {
        float currentTaskStartTime = 0f;

        bool isData3Loaded = false;

        StartCoroutine(
            IProgressLoadSceneAsync(
                new List<LoadTask>() {
                    new LoadTask(
                        "Loading data 1...",
                        () => {
                            currentTaskStartTime = Time.realtimeSinceStartup;
                        },
                        () => LoadData1(currentTaskStartTime)
                    ),
                    new LoadTask(
                        "Loading data 2...",
                        () => loadingProgress2
                    ),
                    new LoadTask(
                        "Loading from Google Play Game...",
                        () => {
                            LoadGooglePlayData1(
                                data => {
                                    isData3Loaded = true;
                                    // Data.Get<CurrencyData>().LoadFromJSON(data);
                                }
                            );
                        },
                        () => isData3Loaded ? 1f : 0f
                    ),
                    new LoadTask(
                        "Waiting for click...",
                        () => isClicked ? 1f : 0f
                    ),
                },
                "SceneName"
            )
            );
    }

    private float LoadData1(float startTime)
    {
        return (Time.realtimeSinceStartup - startTime) / 10f;
    }

    [Button]
    private void SetClick()
    {
        isClicked = true;
    }

    private IEnumerator IFakeLoadSceneAsync(string sceneName)
    {
        yield return 0;
        yield return 0;

        var sceneLoad = SceneManager.LoadSceneAsync(sceneName);
        sceneLoad.allowSceneActivation = false;

        float loadingStartTime = Time.realtimeSinceStartup;

        while (sceneLoad.isDone || Time.realtimeSinceStartup - loadingStartTime < minLoadingDuration)
        {
            float progress = (Time.realtimeSinceStartup - loadingStartTime) / minLoadingDuration;
            slider.value = progress;
            loadingProgressText.text = ((int)(progress * 100)).ToString() + "%";

            loadingStatusText.text = progress < 0.3f ? "Loading game data..." : progress < 0.6f ? "Loading map..." : "Activating scene...";

            yield return 0;
        }

        sceneLoad.allowSceneActivation = true;
    }

    private IEnumerator IProgressLoadSceneAsync(List<LoadTask> tasks, string sceneName)
    {
        yield return 0;
        yield return 0;

        float progress = 0;
        float taskProgressRange = 1 / (float)(tasks.Count + 1);
        int taskIndex = 0;

        tasks[taskIndex].OnTaskStarted?.Invoke();

        while (tasks.Count > taskIndex)
        {
            float taskProgress = tasks[taskIndex].OnTaskProgressing() * taskProgressRange;
            progress = taskProgressRange * taskIndex + taskProgress;

            slider.value = progress;
            loadingProgressText.text = ((int)(progress * 100)).ToString() + "%";

            loadingStatusText.text = tasks[taskIndex].TaskName;

            if (taskProgress >= taskProgressRange)
            {
                taskIndex++;
                if (taskIndex < tasks.Count) tasks[taskIndex].OnTaskStarted?.Invoke();
            }

            yield return 0;
        }

        var sceneLoad = SceneManager.LoadSceneAsync(sceneName);
        sceneLoad.allowSceneActivation = false;

        float loadingStartTime = Time.realtimeSinceStartup;

        while (sceneLoad.isDone || Time.realtimeSinceStartup - loadingStartTime < minLoadingDuration)
        {
            float loadSceneProgress = (Time.realtimeSinceStartup - loadingStartTime) / minLoadingDuration;

            loadSceneProgress *= taskProgressRange;
            progress = taskProgressRange * taskIndex + loadSceneProgress;
            slider.value = progress;

            loadingProgressText.text = ((int)(progress * 100)).ToString() + "%";

            loadingStatusText.text = "Loading scene...";

            yield return 0;
        }

        sceneLoad.allowSceneActivation = true;
    }

    private void LoadGooglePlayData1(System.Action<string> onDataLoaded)
    {
        StartCoroutine(ISimulateLoadingData(onDataLoaded));
    }

    private IEnumerator ISimulateLoadingData(System.Action<string> onDataLoaded)
    {
        yield return new WaitForSeconds(5);

        onDataLoaded?.Invoke("Data loaded");
    }
}

public class LoadTask
{
    public string TaskName;
    public System.Action OnTaskStarted;
    public System.Func<float> OnTaskProgressing;

    public LoadTask()
    {

    }

    public LoadTask(string taskName, System.Func<float> onProgressing)
    {
        TaskName = taskName;
        OnTaskProgressing = onProgressing;
    }

    public LoadTask(string taskName, System.Action onStarted, System.Func<float> onProgressing)
    {
        TaskName = taskName;
        OnTaskStarted = onStarted;
        OnTaskProgressing = onProgressing;
    }
}