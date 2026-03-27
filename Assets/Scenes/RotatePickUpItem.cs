using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class RotatePickUpItem : MonoBehaviour
{
    [SerializeField] private CentralizedValues values;
    private Vector3 rotationVec = Vector3.down;
    CancellationTokenSource cts = new();
    private void Start()
    {
        RotateObject();
    }

    private async void RotateObject()
    {
        await RotateObjectAsync(cts.Token);
    }

    private async UniTask RotateObjectAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        while (values.SessionIsOver == false)
        {
            gameObject.transform.Rotate(rotationVec, 1f);
            await UniTask.WaitForEndOfFrame();
        }
    }

    private void OnDestroy()
    {
        cts.Cancel();
        cts.Dispose();
    }
}


