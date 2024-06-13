using erulathra;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HeartsHapticFeedback : MonoBehaviour
{
    private XRBaseController xrController;

    private void Awake()
    {
        xrController = GetComponent<XRBaseController>();
        
        HeartsSubsystem heartsSubsystem = SceneSubsystemManager.GetSubsystem<HeartsSubsystem>();
        heartsSubsystem.OnHeartsNumberChanged += HandleHeartsNumberChanged;
    }

    private void HandleHeartsNumberChanged(int hearts, int deltaHearts)
    {
        if (deltaHearts > 0)
            return;

        xrController.SendHapticImpulse(1f, 0.25f);
    }
}
