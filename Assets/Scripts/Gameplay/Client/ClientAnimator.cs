using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ClientAnimator : MonoBehaviour
{
    [Foldout("Setup")]
    [SerializeField]
    private TwoBoneIKConstraint leftArm;
    
    [Foldout("Setup")]
    [SerializeField]
    private TwoBoneIKConstraint rightArm;

    private void Awake()
    {
        Client client = GetComponent<Client>();
        if (client.IsRightClient)
        {
            rightArm.weight = 1.0f;
        }
        else
        {
            leftArm.weight = 1.0f;
        }
    }
}
