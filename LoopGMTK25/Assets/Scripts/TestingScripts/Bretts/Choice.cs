using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Events;

// Choice.cs
[System.Serializable]
public class Choice
{
    public string text;
    public TimelineAsset timeline; // The timeline to play when this choice is selected
    public UnityEvent onSelected; // Optional: for additional actions
}
