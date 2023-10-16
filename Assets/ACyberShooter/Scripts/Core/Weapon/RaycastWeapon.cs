using Configs;
using EnemySystem;
using RootMotion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core
{

    public class RaycastWeapon 
    {

        class Bullet 
        {

            public Vector3 initPosition;
            public Vector3 initVelocity;
            public float time = 0.0f;
            public BulletConfig config;
            public TrailRenderer tracer; 
        }

       
        private List<Bullet> _bullets = new();

        private Transform _rayDestination;
        private Transform _rayOrigin;
        private ParticleSystem _hitEffect; // need create a bullet config  (tracer can be there)
        
        Ray ray;
        RaycastHit hit;
        LayerMask _ignoreRaycastLayerMask;
         

        public RaycastWeapon(
            Transform rayDirection,
            Transform rayOrigin ,
            LayerMask ignoreRaycastLayerMask,
            ParticleSystem hitEffect
            ){

            _hitEffect = hitEffect;
            _rayDestination = rayDirection;
            _rayOrigin = rayOrigin;
            _ignoreRaycastLayerMask = ignoreRaycastLayerMask;
        }


        public void Fire(BulletConfig config)
        => FireBullet(_hitEffect, config);
        
         
        public void UpdateBullets(float deltaTime)
        {

            SimulationBullets(deltaTime);
            DestroyBullets(deltaTime);
        }


        private Vector3 GetPosition(Bullet bullet)
        {
            // pos + (velocity * time) + .5f * gravity * time * time

            Vector3 gravity = Vector3.down * bullet.config.Drop;
            return (bullet.initPosition) + (bullet.initVelocity * bullet.time) + (.5f * gravity * bullet.time * bullet.time);

        }


        private Bullet CreateBullet(
            Vector3 initPosition,
            Vector3 initVelocity,
            BulletConfig config){
            ///[Mehod]\\\
            
            var bullet = new Bullet();
            bullet.initPosition = initPosition;
            bullet.initVelocity = initVelocity;
            bullet.time = 0.0f;
            bullet.config = config;

            if (bullet.config.Traicer != null)
            {

                bullet.tracer = GameObject.Instantiate(config.Traicer, initPosition, Quaternion.identity);
                bullet.tracer.AddPosition(initPosition);
            }
            return bullet;
        }

         
        private void SimulationBullets(float deltaTime)
        {

            _bullets.ForEach(bullet =>
            {
                Vector3 p0 = GetPosition(bullet);
                bullet.time += deltaTime;

                Vector3 p1 = GetPosition(bullet);
                RaycastSegment(p0, p1, bullet);
            });
        }


        private void DestroyBullets(float deltaTime)
        => _bullets.RemoveAll(bullet => bullet.time >= bullet.config.MaxTime);
       

        private void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
        {

            Vector3 direction = end - start;
            float distance = direction.magnitude;

            ray.origin = start;
            ray.direction = direction;

            if (Physics.Raycast(ray, out hit, distance ))
            {
                //we can check material collider and sets hit effect from config (need create)
                TryInstantiateHitEffect();
                TryTakeDamage(hit, bullet.config);
                TryInstantiateTracer(bullet);

                bullet.time = bullet.config.MaxTime;
            }
            else if(bullet.tracer != null) bullet.tracer.transform.position = end;
        }

        private void TryTakeDamage(RaycastHit hit, BulletConfig config)
        {

            if(hit.collider.TryGetComponent<EnemyBoneView>(out var enemyView))
            {
                enemyView.EnemyView.TakeDamage(config.Damage);
            }


        }


        private void TryInstantiateHitEffect()
        {
            if (_hitEffect != null)
            {
                CheckHitLayers(out var canActiveHit);

                if (canActiveHit)
                {
                    var effect = GameObject.Instantiate(_hitEffect);
                    effect.transform.position = hit.point;
                    effect.transform.forward = hit.normal;
                    effect.Emit(1);
                }
            }
        }


        private void TryInstantiateTracer(Bullet bullet)
        {
            if (bullet.tracer != null)
            {
                bullet.tracer.transform.position = hit.point;
            }
        }


        private void CheckHitLayers(out bool canActiveHit)
        {

            canActiveHit = true;
            int[] ignoreHitEffectsLayers = _ignoreRaycastLayerMask.MaskToNumbers();

            foreach (int layerIndex in ignoreHitEffectsLayers)
            {
                if (hit.collider.gameObject.layer == layerIndex)
                {
                    canActiveHit = false;
                }
            }
        }

        private void FireBullet(ParticleSystem hitEffect, BulletConfig config)
        {

            Vector3 velocity = (_rayDestination.position - _rayOrigin.position).normalized * config.Speed;
            var bullet = CreateBullet(_rayOrigin.position, velocity, config);
            _bullets.Add(bullet);
        }
    }
}