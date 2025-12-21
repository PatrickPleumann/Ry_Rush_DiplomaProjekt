using UnityEngine;
using UnityEngine.Events;

public class AnimationEventStateBehaviour : StateMachineBehaviour
{
    public string eventName;
    [Range(0f, 1f)] public float triggerTime;

    bool hasTriggered;
    float currentTime;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasTriggered = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime = stateInfo.normalizedTime % 1f;

        if (hasTriggered == false && currentTime >= triggerTime)
        {
            NotifyReceiver(animator);
            hasTriggered = true;
        }
    }

    private void NotifyReceiver(Animator _animator)
    {
        AnimationEventReceiver receiver = _animator.GetComponent<AnimationEventReceiver>();

        if (receiver != null)
        {
            receiver.OnAnimationEventTriggered(eventName);
        }
    }
}
