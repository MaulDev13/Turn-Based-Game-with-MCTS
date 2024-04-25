using UnityEngine;
using TMPro;

public class ArenaLastMove : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descText;

    public void Init(string _value)
    {
        

        descText.text = _value.ToString();
    }
}
