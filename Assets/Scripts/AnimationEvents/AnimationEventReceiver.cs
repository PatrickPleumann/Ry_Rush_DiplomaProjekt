using UnityEngine;
using System.Collections.Generic;

public class AnimationEventReceiver : MonoBehaviour
{
    [SerializeField] public List<AnimationEvent> AnimationEvents = new();

    public void OnAnimationEventTriggered(string eventName)
    {
        AnimationEvent matchingEvent = AnimationEvents.Find(se => se.eventName == eventName);
        matchingEvent?.OnAnimationEvent?.Invoke();
    }
}
