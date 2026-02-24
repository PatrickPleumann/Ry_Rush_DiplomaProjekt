using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class RotatePickUpItem : MonoBehaviour
{
    [SerializeField] private CentralizedValues values;
    private Vector3 rotationVec = Vector3.down;
    CancellationTokenSource cts = new();
    private void Start()
    {
        StartRotation();
    }

    private async void StartRotation()
    {
        await RotateObjectAsync(cts.Token);
    }
    private async UniTask RotateObjectAsync(CancellationToken token)
    {
        while (values.SessionIsOver == false)
        {
            token.ThrowIfCancellationRequested();
            await UniTask.WaitForEndOfFrame();
            gameObject.transform.Rotate(rotationVec, 1f);
        }
        cts.Cancel();
        cts.Dispose();
    }
}


