using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSFXBehaviour : MonoBehaviour, IPointerEnterHandler
{ 
    [SerializeField] private UI_SoundsHandler sounds;

    public void OnPointerEnter(PointerEventData eventData)
    {
        sounds.OnPointerEnter_Sound();
    }
}
