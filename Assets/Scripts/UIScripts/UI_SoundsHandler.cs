using UnityEngine;

public class UI_SoundsHandler : MonoBehaviour
{
    [SerializeField] private AudioSource ui_Sounds;

    [SerializeField] public AudioClip buttonClicked;
    [SerializeField] public AudioClip buttonHovered;
    [SerializeField] public AudioClip startGameButtonClicked;
    [SerializeField] public AudioClip swapMenuSound;
    [SerializeField] public AudioClip returnToPreviousMenuSound;

    private void Awake()
    {
        ui_Sounds = GetComponent<AudioSource>();
    }

    public void OnPointerEnter_Sound()
    {
        ui_Sounds.clip = buttonHovered;
        ui_Sounds.Play();
    }
    public void OnPointerClick_Sound()
    {
        ui_Sounds.clip = buttonClicked;
        ui_Sounds.Play();
    }

    public void SwapScreenButtonClicked()
    {
        ui_Sounds.clip = swapMenuSound;
        ui_Sounds.Play();
    }
    public void StartGameButtonClicked()
    {
        ui_Sounds.clip = startGameButtonClicked;
        ui_Sounds.Play();
    }
    public void ReturnButtonClicked()
    {
        ui_Sounds.clip = returnToPreviousMenuSound;
        ui_Sounds.Play();
    }
}
