using UnityEngine;

public class UI_SoundsHandler : MonoBehaviour
{
    [SerializeField] private AudioSource ui_Sounds;

    [SerializeField] private AudioClip buttonClicked;
    [SerializeField] private AudioClip buttonHovered;
    [SerializeField] private AudioClip startGameButtonClicked;
    [SerializeField] private AudioClip swapMenuSound;
    [SerializeField] private AudioClip returnToPreviousMenuSound;
    [SerializeField] private AudioClip saveSongButton;
    [SerializeField] private AudioClip resetValuesSound;
    [SerializeField] private AudioClip assignBooleanValueSound;
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
    public void SaveSongValuesButton()
    {
        ui_Sounds.clip = saveSongButton;
        ui_Sounds.Play();
    }

    public void ResetValuesButton()
    {
        ui_Sounds.clip = resetValuesSound;
        ui_Sounds.Play();
    }

    public void AssignBooleanBalue()
    {
        ui_Sounds.clip = assignBooleanValueSound;
        ui_Sounds.Play();
    }
}
