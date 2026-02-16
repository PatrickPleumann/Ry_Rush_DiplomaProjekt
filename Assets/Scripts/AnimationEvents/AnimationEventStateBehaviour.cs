using System.Threading.Tasks;
using UnityEngine;

public class AnimationEventStateBehaviour : StateMachineBehaviour
{
    public string eventName;
    [Range(0f, 1f)] public float triggerTime;
    [SerializeField] private float resetTriggerTime;
    

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
            ResetTimer(stateInfo.length);
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

    private async void ResetTimer(float _timeInSeconds) //void is dangerous, but no issues with just a specialized timer
    {
        //works perfect with "stateinfo.Lenght"
        await Task.Delay((int)(_timeInSeconds * 1000));
        hasTriggered = false;
    }
}
