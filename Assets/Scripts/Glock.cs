using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class Glock : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    
    [SerializeField]
    private Transform muzzlePoint;

    [SerializeField]
    private float startBulletSpeed;
    
    private XRGrabInteractable grabInteractable;
    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        
        grabInteractable.activated.AddListener(Shoot);
    }

    private void Update()
    {
    }

    private void Shoot(ActivateEventArgs args)
    {
        GameObject newBullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
        Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(muzzlePoint.forward * startBulletSpeed, ForceMode.VelocityChange);
    }
}
