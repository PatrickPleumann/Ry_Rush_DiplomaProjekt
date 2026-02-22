using Cysharp.Threading.Tasks;
using UnityEngine;

public class AnimationEventStateBehaviour : StateMachineBehaviour
{
    public string eventName;
    [Range(0f, 1f)] public float TriggerTime;

    private bool hasTriggered;
    private float currentTime;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasTriggered = false;
    }

    public override async void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime = stateInfo.normalizedTime % 1f;

        if (hasTriggered == false && currentTime >= TriggerTime)
        {
            NotifyReceiver(animator);
            hasTriggered = true;
            await ResetTimer(stateInfo.length);
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

    private async UniTask ResetTimer(float _timeInSeconds)
    {
        await UniTask.Delay((int)(_timeInSeconds * 1000));
        hasTriggered = false;
    }
}
