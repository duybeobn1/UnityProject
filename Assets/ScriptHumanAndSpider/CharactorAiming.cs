using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class CharactorAiming : MonoBehaviour
{
    public float turnSpeed = 15;
    public float aimDuration = 0.1f;
    public Rig aimLayer;
    Camera mainCamera;
    RayCastWeapon weapon;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        weapon = GetComponentInChildren<RayCastWeapon>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
    }

    private void ShootRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit))
        {
            // Check if the hit object has a simpleController script (i.e., it's the spider)
            simpleController spiderController = hit.collider.GetComponent<simpleController>();
            if (spiderController != null)
            {
                // Reduce spider's health
                spiderController.takeDamage(1); // Assuming 1 damage per hit
            }
        }
    }
    private void LateUpdate()
    {
    if(aimLayer)
        {
            if(Input.GetButton("Fire2"))
            {
                aimLayer.weight += Time.deltaTime / aimDuration;
            }
            else
            {
                aimLayer.weight -= Time.deltaTime / aimDuration;
            }
        }
        if(Input.GetButtonDown("Fire1")&&Input.GetButton("Fire2")) 
        {
            weapon.StartFiring();
            ShootRaycast();
        }
        else weapon.StopFiring();
        if (weapon.isFiring)
        {
            weapon.UpdateFiring(Time.deltaTime);
        }
        weapon.UpdateBullet(Time.deltaTime);
   
        


    }
}
