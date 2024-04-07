using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastWeapon : MonoBehaviour
{

    class Bullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }
    public simpleController creatureController; // ICI
    public WormBuilder wb; // boss
    public bool isFiring = false;
    public ParticleSystem[] muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect;
    public Transform raycastOrigin;
    public Transform raycastDestination;
    public int fireRate = 25;
    public float bulletSpeed = 1000.0f;
    public float bulletDrop = 300.0f;
    float maxLifeTime = 3.0f;
    Ray ray;
    RaycastHit hitInfo;
    float accumulatedTime;
    List<Bullet> bullets = new List<Bullet>();

    Vector3 GetPosition(Bullet bullet)
    {
        Vector3 gravity = Vector3.down * bulletDrop;
        return bullet.initialPosition + bullet.initialVelocity * bullet.time + 0.5f * gravity * bullet.time * bullet.time;
    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity)
{
    Bullet bullet = new Bullet();
    bullet.initialPosition = position;
    bullet.initialVelocity = velocity;
    bullet.time = 0.0f;
    bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity); // Assurez-vous que tracerEffect est correctement défini
    bullet.tracer.AddPosition(position);
    return bullet;
}


    public void StartFiring()
    {
        isFiring = true;
        accumulatedTime = 0.0f;
        FireBullet();
    }

   public void UpdateFiring(float deltaTime)
    {
        accumulatedTime += deltaTime;
        float fireInterval = 1.0f / fireRate;
        while(accumulatedTime >= 0.0f) {
            FireBullet();
            accumulatedTime -= fireInterval;
        }
    }

    public void UpdateBullet(float deltaTime) 
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RayCastSegment(p0, p1, bullet);
        });
    }
    void DestroyBullets()
    {
        bullets.RemoveAll(bullet => bullet.time >= maxLifeTime);
    }
    void RayCastSegment(Vector3 start, Vector3 end, Bullet bullet)
{
    Vector3 direction = end - start;
    float distance = direction.magnitude;
    ray.origin = start;
    ray.direction = direction;
    
    if (Physics.Raycast(ray, out hitInfo, distance))
    {
        if (hitInfo.collider.CompareTag("Creature")) // Assurez-vous que la balise est correcte
        {
            //creatureController.takeDamage(10); // Appel à la méthode takeDamage() de simpleController
        }
        
        if (hitInfo.collider.CompareTag("Creature"))
        {
            wb.takeDamage(5); // Appel à la méthode takeDamage() de WormBuilder
        }

        hitEffect.transform.position = hitInfo.point;
        hitEffect.transform.forward = hitInfo.normal;
        hitEffect.Emit(10);

        // Vérifiez si le bullet.tracer est null avant d'essayer d'y accéder
        if (bullet.tracer != null)
        {
            bullet.tracer.transform.position = hitInfo.point;
        }
        
        bullet.time = maxLifeTime;
    }
    else
    {
        // Vérifiez si le bullet.tracer est null avant d'essayer d'y accéder
        if (bullet.tracer != null)
        {
            bullet.tracer.transform.position = end;
        }
    }
}



    private void FireBullet()
    {
        foreach (var particle in muzzleFlash)
        {
            particle.Emit(1);
        }


        Vector3 velocity = (raycastDestination.position - raycastOrigin.position).normalized * bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);    

        }

        public void StopFiring()
    {
        isFiring= false;
    }

}
