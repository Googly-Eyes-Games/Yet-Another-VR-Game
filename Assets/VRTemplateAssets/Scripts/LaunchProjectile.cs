using System;
using UnityEngine;

namespace Unity.VRTemplate
{
    /// <summary>
    /// Apply forward force to instantiated prefab
    /// </summary>
    public class LaunchProjectile : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The projectile that's created")]
        GameObject m_ProjectilePrefab = null;

        [SerializeField]
        [Tooltip("The point that the project is created")]
        Transform m_StartPoint = null;

        [SerializeField]
        [Tooltip("The speed at which the projectile is launched")]
        float m_LaunchSpeed = 1.0f;

        public void Fire()
        {
            GameObject newObject = Instantiate(m_ProjectilePrefab, m_StartPoint.position, m_StartPoint.rotation, null);

            if (newObject.TryGetComponent(out Rigidbody rigidBody))
                ApplyForce(rigidBody);
            
            RaycastHit hit;
            if (Physics.SphereCast(m_StartPoint.position, 0.1f, m_StartPoint.forward, out hit, 1000f))
            {
                Rigidbody hitRB = hit.collider.GetComponent<Rigidbody>();
                if (hitRB)
                {
                    hitRB.AddForce(m_StartPoint.forward * 10f);
                    Debug.Log($"HIT: {hit.transform.name}");
                }
            }
        }

        void ApplyForce(Rigidbody rigidBody)
        {
            Vector3 force = m_StartPoint.forward * m_LaunchSpeed;
            rigidBody.AddForce(force);
        }
    }
}
