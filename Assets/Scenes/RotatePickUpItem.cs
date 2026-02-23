using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class RotatePickUpItem : MonoBehaviour
{
    private Vector3 rotationVec = Vector3.down;
    CancellationTokenSource cts;
    private void Start()
    {
        cts = new();

        StartRotation();
    }

    private async void StartRotation()
    {
        await RotateObjectAsync(cts.Token);
    }
    private async UniTask RotateObjectAsync(CancellationToken token)
    {
        
        while (!token.IsCancellationRequested)
        {
            await UniTask.WaitForEndOfFrame();
            token.ThrowIfCancellationRequested();
            gameObject.transform.Rotate(rotationVec, 1f);
        }
    }

    private void OnDestroy()
    {
        cts.Cancel();
    }
}


