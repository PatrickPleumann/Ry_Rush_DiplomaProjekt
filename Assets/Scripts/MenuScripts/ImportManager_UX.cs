using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImportManager_UX : MonoBehaviour
{
    [Header("Choose & Show Song UX/UI")]
    [SerializeField] public Button BrowseFiles_Button;
    [SerializeField] public TMP_Text YourSong_Name;
    [SerializeField] public string Message;
    [SerializeField] public Button ConfirmSong_Button;

    [Header("Choose & Confirm BPM UX/UI")]
    [SerializeField] public GameObject BPM_GO;
    [SerializeField] public TMP_InputField BPMInput_InputField;
    [SerializeField] public Button ConfirmBPM_Button;

    [Header("Choose & Confirm Async Value UX/UI")]
    [SerializeField] public GameObject AsyncValue_GO;
    [SerializeField] public Slider SampleOffset_Slider;
    [SerializeField] public Button ConfirmAsyncValue_Button;
    [SerializeField] public TMP_Text ShowAsyncSamples_Text;

    [SerializeField] public Button PlayPreview_Button;
    [SerializeField] public TMP_Dropdown songs_DropdownMenu;
                      
}
