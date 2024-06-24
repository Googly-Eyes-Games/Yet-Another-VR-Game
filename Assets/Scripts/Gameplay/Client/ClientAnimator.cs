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
    
    [Foldout("Setup")]
    [SerializeField]
    private Transform meshRoot;
    
    [Foldout("Setup")]
    [SerializeField]
    private Transform mugTransform;

    private Client client;

    private Vector3 defaultMeshRootPosition;
    private Quaternion defaultMeshRootRotation;

    private bool isWoman = false;

    public bool IsWoman
    {
        get => isWoman;
        set
        {
            isWoman = value;
            animator.SetBool(AnimatorLookUp.IsWoman, value);
        }
    }

    private void Awake()
    {
        client = GetComponent<Client>();

        client.OnInitialize += HandleClientInitialized;
        client.OnClientStateChanged += HandleClientStateChanged;
    }

    private void Start()
    {
        defaultMeshRootPosition = meshRoot.localPosition;
        defaultMeshRootRotation = meshRoot.localRotation;
    }

    private void HandleClientInitialized()
    {
        if (client.IsRightClient)
        {
            rightArm.weight = 1.0f;
            leftArm.weight = 0.0f;
        }
        else
        {
            rightArm.weight = 0.0f;
            leftArm.weight = 1.0f;
        }

        animator.SetFloat(AnimatorLookUp.WalkSpeed, client.WalkSpeed * walkSpeedFactor);
        mugTransform.gameObject.SetActive(false);
    }

    private void HandleClientStateChanged(ClientState state)
    {
        if (state == ClientState.WantsBeer)
            return;

        bool hasMug = client.CollectedMug;

        animator.SetFloat(AnimatorLookUp.WalkSpeed, client.ReturnSpeed * walkSpeedFactor);
        mugTransform.gameObject.SetActive(hasMug);
        rightArm.weight = state == ClientState.DrinkingBeer ? 1.0f : 0.0f ;
        leftArm.weight = 0.0f;
    }

    public void ResetRootMotion()
    {
        meshRoot.localPosition = defaultMeshRootPosition;
        meshRoot.localRotation = defaultMeshRootRotation;
    }

    private static class AnimatorLookUp
    {
        public static readonly int WalkSpeed = Animator.StringToHash("WalkSpeed");
        public static readonly int IsWoman = Animator.StringToHash("IsWoman");
    }
}
