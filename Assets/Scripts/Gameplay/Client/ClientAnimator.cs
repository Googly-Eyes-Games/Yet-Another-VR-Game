using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ClientAnimator : MonoBehaviour
{
    [Foldout("Config")]
    [SerializeField]
    private float walkSpeedFactor = 1.6f;
    
    [Foldout("Setup")]
    [SerializeField]
    private TwoBoneIKConstraint leftArm;
    
    [Foldout("Setup")]
    [SerializeField]
    private TwoBoneIKConstraint rightArm;

    [Foldout("Setup")]
    [SerializeField]
    private Animator animator;

    private Client client;

    private void Awake()
    {
        client = GetComponent<Client>();
        if (client.IsRightClient)
        {
            rightArm.weight = 1.0f;
        }
        else
        {
            leftArm.weight = 1.0f;
        }

        client.OnInitialize += () =>
        {
            animator.SetFloat(AnimatorLookUp.WalkSpeed, client.WalkSpeed * walkSpeedFactor);
        };
        client.OnClientStateChanged += state =>
        {
            if (state != ClientState.WantsBeer)
                animator.SetFloat(AnimatorLookUp.WalkSpeed, client.ReturnSpeed * walkSpeedFactor);
        };
    }

    private static class AnimatorLookUp
    {
        public static readonly int WalkSpeed = Animator.StringToHash("WalkSpeed");
    }
}
