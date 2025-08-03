using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    private AudioClipHolder clipHolder;
    private void Start()
    {
        clipHolder = GetComponent<AudioClipHolder>();
        AudioController.Instance.PlayMusic(clipHolder.musicClips[0], true, 1f);
    }
    public void ToMainMenu()
    {
        AudioController.Instance.StopMusic();
        SceneManager.LoadScene("Menu");
    }
}
