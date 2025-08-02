using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

//ATTACH TO ANY OBJECT THAT YOU WANT TO HAVE AUDIO
// I.E. A player character that has sound effects
public class AudioClipHolder : MonoBehaviour
{

    public List<AudioClip> musicClips;
    public List<AudioClip> ambientClips;
    public List<AudioClip> SFXClips;


    // HOW TO CALL AudioController Functions from your object with this script:
    // ─────────────────────────────────────────────────────────────
    //🔇MUTE TOGGLE
    //- AudioController.Instance.ToggleMute()

    //🎵 MUSIC CONTROL
    //- AudioController.Instance.PlayMusic(AudioClip, loop? [optional], Fade in? [optional], target volume if fading in [optional])
    //- AudioController.Instance.StopMusic(Fade in? [optional])

    //🌲 AMBIENT SOUND CONTROL
    //- AudioController.Instance.PlayAmbient(AudioClip clip, bool loop = true, float fadeDuration = 0f, float targetVolume = 1f)
    //- AudioController.Instance.StopAmbient(float fadeDuration = 0f)

    //💥 SOUND EFFECTS
    //- AudioController.Instance.PlaySFX(AudioClip clip, float vol = 1f)





}
