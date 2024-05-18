using TMPro;
using UnityEngine;

public class LeverTest : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;

    public void SetValue(float value)
    {
        text.text = $"Lever: {value}";
    }
}
