using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneNameToLoad;
    public float timeToWait;

    private bool loadingSceneNow;

    private void Start()
    {
        if (string.IsNullOrEmpty(sceneNameToLoad)) { sceneNameToLoad = SceneManager.GetActiveScene().name; timeToWait = 1; }
    }

    public void SetSceneByName(string _newScene) // set scene to load by this or the sequentially option
    {
        sceneNameToLoad = _newScene;
    }   

    public void SetSceneSequentially() // a.k.a this one
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        sceneNameToLoad = SceneManager.GetSceneAt(currentScene + 1).name;
    }

    public void SetTimeToWait(float _time) // set a time to load
    {
        timeToWait = _time;
    }

    public void LoadSetSceneNow() // call to lock in what information we have
    {
        StartCoroutine(LoadSceneOnTimer());
    }

    private IEnumerator LoadSceneOnTimer() // this runs the animations
    {
        if (!loadingSceneNow)
        {
            loadingSceneNow = true;
            yield return new WaitForSeconds(timeToWait);
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }
}
