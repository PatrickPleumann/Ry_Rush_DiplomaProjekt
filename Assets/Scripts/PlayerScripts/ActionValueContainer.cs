using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ActionValueContainer", menuName = "Scriptable Objects/ActionValueContainer")]
public class ActionValueContainer : ScriptableObject
{
    [SerializeField] private float m_value;
    [HideInInspector] public UnityEvent<float> OnValueChanged;

    public float Value
    {
        get => m_value;
        set
        {
            if (m_value != value)
            {
                m_value = value;
                OnValueChanged.Invoke(m_value);
            }
        }
    }
}
