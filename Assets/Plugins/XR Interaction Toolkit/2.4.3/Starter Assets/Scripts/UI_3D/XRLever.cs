using System;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace UnityEngine.XR.Content.Interaction
{
    /// <summary>
    /// An interactable lever that snaps into an on or off position by a direct interactor
    /// </summary>
    public class XRLever : XRBaseInteractable
    {
        const float k_LeverDeadZone = 0.01f; // Prevents rapid switching between on and off states when right in the middle

        [SerializeField]
        [Tooltip("The object that is visually grabbed and manipulated")]
        Transform m_Handle = null;

        [SerializeField]
        [Tooltip("The value of the lever")]
        [Range(0f, 1f)]
        float m_Value = 0f;

        [SerializeField]
        [Tooltip("If enabled, the lever will snap to the binary value when released")]
        bool m_LockToBinary;
        
        [SerializeField]
        [Tooltip("If enabled, the lever will lerp to minimal angle when released")]
        bool m_BackToZero;
        
        [SerializeField]
        [Tooltip("Above property lerp speed")]
        float m_BackToZeroSpeed = 8f;

        [SerializeField]
        [Tooltip("Angle of the lever in the 'on' position")]
        [Range(-90.0f, 90.0f)]
        float m_MaxAngle = 90.0f;

        [SerializeField]
        [Tooltip("Angle of the lever in the 'off' position")]
        [Range(-90.0f, 90.0f)]
        float m_MinAngle = -90.0f;

        [SerializeField]
        [Tooltip("Events to trigger when the lever activates")]
        UnityEvent m_OnLeverActivate = new UnityEvent();

        [SerializeField]
        [Tooltip("Events to trigger when the lever deactivates")]
        UnityEvent m_OnLeverDeactivate = new UnityEvent();
        
        [SerializeField]
        [Tooltip("Events to trigger when the lever deactivates")]
        UnityEvent<float> m_OnLeverMove = new UnityEvent<float>();

        IXRSelectInteractor m_Interactor;

        /// <summary>
        /// The object that is visually grabbed and manipulated
        /// </summary>
        public Transform handle
        {
            get => m_Handle;
            set => m_Handle = value;
        }

        /// <summary>
        /// The value of the lever
        /// </summary>
        public float value
        {
            get => m_Value;
            set => SetValue(value, true);
        }

        /// <summary>
        /// If enabled, the lever will snap to the value position when released
        /// </summary>
        public bool lockToValue { get; set; }

        /// <summary>
        /// Angle of the lever in the 'on' position
        /// </summary>
        public float maxAngle
        {
            get => m_MaxAngle;
            set => m_MaxAngle = value;
        }

        /// <summary>
        /// Angle of the lever in the 'off' position
        /// </summary>
        public float minAngle
        {
            get => m_MinAngle;
            set => m_MinAngle = value;
        }

        /// <summary>
        /// Events to trigger when the lever activates
        /// </summary>
        public UnityEvent onLeverActivate => m_OnLeverActivate;

        /// <summary>
        /// Events to trigger when the lever deactivates
        /// </summary>
        public UnityEvent onLeverDeactivate => m_OnLeverDeactivate;
        
        /// <summary>
        /// Events to trigger when the lever moves
        /// </summary>
        public UnityEvent<float> onLeverMove => m_OnLeverMove;

        void Start()
        {
            SetValue(m_Value, true);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            selectEntered.AddListener(StartGrab);
            selectExited.AddListener(EndGrab);
        }

        protected override void OnDisable()
        {
            selectEntered.RemoveListener(StartGrab);
            selectExited.RemoveListener(EndGrab);
            base.OnDisable();
        }

        void StartGrab(SelectEnterEventArgs args)
        {
            m_Interactor = args.interactorObject;
        }

        void EndGrab(SelectExitEventArgs args)
        {
            SetValue(m_Value, true);
            m_Interactor = null;
        }

        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            base.ProcessInteractable(updatePhase);

            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            {
                if (isSelected)
                {
                    UpdateValue();
                }
            }
        }

        public void Update()
        {
            if (!isSelected && m_BackToZero && m_Value > Single.Epsilon)
            {
                float newValue = Mathf.Lerp(m_Value, 0f, Time.deltaTime * m_BackToZeroSpeed);
                if (newValue < k_LeverDeadZone)
                    newValue = 0f;
                
                SetValue(newValue, true);
            }
        }

        Vector3 GetLookDirection()
        {
            Vector3 direction = m_Interactor.GetAttachTransform(this).position - m_Handle.position;
            direction = transform.InverseTransformDirection(direction);
            direction.x = 0;

            return direction.normalized;
        }

        void UpdateValue()
        {
            Vector3 lookDirection = GetLookDirection();
            float lookAngle = Mathf.Atan2(lookDirection.z, lookDirection.y) * Mathf.Rad2Deg;

            // if (m_MinAngle < m_MaxAngle)
            // {
            //     lookAngle = Mathf.Clamp(lookAngle, m_MinAngle, m_MaxAngle);
            // }
            // else
            // {
            //     lookAngle = Mathf.Clamp(lookAngle, m_MaxAngle, m_MinAngle);
            // }

            float newValue = Mathf.InverseLerp(minAngle, maxAngle, lookAngle);
            newValue = Mathf.Clamp01(newValue);

            // var maxAngleDistance = Mathf.Abs(m_MaxAngle - lookAngle);
            // var minAngleDistance = Mathf.Abs(m_MinAngle - lookAngle);
            //
            // if (m_Value)
            //     maxAngleDistance *= (1.0f - k_LeverDeadZone);
            // else
            //     minAngleDistance *= (1.0f - k_LeverDeadZone);
            //
            // var newValue = (maxAngleDistance < minAngleDistance);
            //
            // SetHandleAngle(lookAngle);

            SetValue(newValue, true);
        }

        void SetValue(float newValue, bool forceRotation = false)
        {
            if (!isSelected && m_LockToBinary)
                newValue = Mathf.Round(newValue);
            
            if (forceRotation)
                SetHandleAngle(CalculateAngle(newValue));

            m_Value = newValue;

            if (!isSelected)
            {
                if (newValue > 0.5f)
                {
                    m_OnLeverActivate.Invoke();
                }
                else
                {
                    m_OnLeverDeactivate.Invoke();
                }
            }
            
            m_OnLeverMove.Invoke(newValue);
        }

        void SetHandleAngle(float angle)
        {
            if (m_Handle != null)
                m_Handle.localRotation = Quaternion.Euler(angle, 0.0f, 0.0f);
        }

        void OnDrawGizmosSelected()
        {
            var angleStartPoint = transform.position;

            if (m_Handle != null)
                angleStartPoint = m_Handle.position;

            const float k_AngleLength = 0.25f;

            var angleMaxPoint = angleStartPoint + transform.TransformDirection(Quaternion.Euler(m_MaxAngle, 0.0f, 0.0f) * Vector3.up) * k_AngleLength;
            var angleMinPoint = angleStartPoint + transform.TransformDirection(Quaternion.Euler(m_MinAngle, 0.0f, 0.0f) * Vector3.up) * k_AngleLength;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(angleStartPoint, angleMaxPoint);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(angleStartPoint, angleMinPoint);
        }

        void OnValidate()
        {
            SetHandleAngle(CalculateAngle(value));
        }

        float CalculateAngle(float newValue)
        {
            return Mathf.Lerp(m_MinAngle, m_MaxAngle, newValue);
        }
    }
}
