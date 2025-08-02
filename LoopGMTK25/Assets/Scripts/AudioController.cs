using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// Centralized audio controller for managing music, ambient, and SFX playback.
/// Clips are provided externally; this controller does not store or preload audio.
/// Volume levels are adjustable in the Inspector before and during runtime under the AudioSource components.
public class AudioController : MonoBehaviour
{
    //Create static instance for global access
    public static AudioController Instance { get; private set; }
    //
    private bool isMuted = false;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambientSource;
    [SerializeField] private AudioSource sfxSource;

    //Store coroutines in fields to allow for easier control
    private Coroutine musicFadeCoroutine;
    private Coroutine ambientFadeCoroutine;

    private Dictionary<AudioSource, float> cachedVolumes = new();


    //Create the singleton
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    // ─────────────────────────────────────────────────────────────
    // MUTE METHOD
    // ─────────────────────────────────────────────────────────────
    public void ToggleMute()
    {
        //Get all audio sources
        var sources = GetComponents<AudioSource>();
        //Check if already muted
        if (isMuted)
        {
            foreach(var kvp in cachedVolumes)
            {
                kvp.Key.volume = kvp.Value; //UNMUTE (Set volumes back to originals)
            }
            isMuted = false;
            cachedVolumes.Clear();
            Debug.Log("Unmuting..");
        }
        else
        {
            foreach (var source in sources)//Loop through audio sources
            {
                cachedVolumes[source] = source.volume; //CACHE VOLUME

                source.volume = 0;//MUTE

                Debug.Log(source);
            }
            Debug.Log("Muting...");
            isMuted = true;
        }
        
        

    }
        

    // ─────────────────────────────────────────────────────────────
    // MUSIC METHODS
    // ─────────────────────────────────────────────────────────────

    public void PlayMusic(AudioClip clip, bool loop = true, float fadeDuration = 0f,float targetVolume = 1f)
    {
        if (musicFadeCoroutine != null) StopCoroutine(musicFadeCoroutine);

        musicSource.clip = clip;
        musicSource.loop = loop;

        if (fadeDuration > 0f)
        {
            musicSource.volume = 0f;
            musicSource.Play();
            musicFadeCoroutine = StartCoroutine(FadeIn(musicSource, targetVolume, fadeDuration, () => musicFadeCoroutine = null));
        }
        else
        {
            musicSource.Play();
        }
    }

    public void StopMusic(float fadeDuration = 0f)
    {
        if (musicFadeCoroutine != null) StopCoroutine(musicFadeCoroutine);

        if (fadeDuration > 0f)
        {
            musicFadeCoroutine = StartCoroutine(FadeOut(musicSource, fadeDuration, () => musicFadeCoroutine = null));
        }
        else
        {
            musicSource.Stop();
        }
    }

    // ─────────────────────────────────────────────────────────────
    // AMBIENT METHODS
    // ─────────────────────────────────────────────────────────────

    public void PlayAmbient(AudioClip clip, bool loop = true, float fadeDuration = 0f, float targetVolume = 1f)
    {
        if (ambientFadeCoroutine != null) StopCoroutine(ambientFadeCoroutine);

        ambientSource.clip = clip;
        ambientSource.loop = loop;

        if (fadeDuration > 0f)
        {
            ambientSource.volume = 0f;
            ambientSource.Play();
            ambientFadeCoroutine = StartCoroutine(FadeIn(ambientSource, targetVolume, fadeDuration, () => ambientFadeCoroutine = null));
        }
        else
        {
            ambientSource.Play();
        }
    }

    public void StopAmbient(float fadeDuration = 0f)
    {
        if (ambientFadeCoroutine != null) StopCoroutine(ambientFadeCoroutine); //If a coroutine is already executing then stop it

        if (fadeDuration > 0f)//Check if there is a fade duration indicated
        {
            ambientFadeCoroutine = StartCoroutine(FadeOut(ambientSource, fadeDuration, () => ambientFadeCoroutine = null));
        }
        else
        {
            ambientSource.Stop();
        }
    }

    // ─────────────────────────────────────────────────────────────
    // SFX METHOD
    // ─────────────────────────────────────────────────────────────

    public void PlaySFX(AudioClip clip, float vol = 1f)
    {
        sfxSource.PlayOneShot(clip,vol);
    }

    // ─────────────────────────────────────────────────────────────
    // FADE HELPERS (Kudos to Copilot for helping with this)
    // ─────────────────────────────────────────────────────────────

    private IEnumerator FadeIn(AudioSource source, float targetVolume, float duration, System.Action onComplete)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(0f, targetVolume, t / duration);
            yield return null;
        }
        source.volume = targetVolume;
        onComplete?.Invoke();
    }

    private IEnumerator FadeOut(AudioSource source, float duration, System.Action onComplete)
    {
        float startVol = source.volume;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(startVol, 0f, t / duration);
            yield return null;
        }
        source.Stop();
        source.volume = startVol; // Reset for next playback
        onComplete?.Invoke();
    }



    }
