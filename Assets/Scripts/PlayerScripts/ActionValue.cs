using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "ActionValue", menuName = "Scriptable Objects/ActionValue")]

public class ActionValue : ScriptableObject
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