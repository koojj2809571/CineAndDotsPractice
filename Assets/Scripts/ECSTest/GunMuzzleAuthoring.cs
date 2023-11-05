using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public struct GunMuzzle : IComponentData
{
    public Entity BulletGo;
    public float FireDuration;
    public float BulletSpeed;
    public float DestroyLifeTime;
    public float DestroyDistance;
}

public class GunMuzzleAuthoring : MonoBehaviour
{
    public GameObject bulletGo;
    public float fireDuration;
    public float bulletSpeed;
    public float destroyLifeTime;
    public float destroyDistance;

    class Baker: Baker<GunMuzzleAuthoring>
    {
        public override void Bake(GunMuzzleAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new GunMuzzle
            {
                BulletGo = GetEntity(authoring.bulletGo, TransformUsageFlags.Dynamic),
                FireDuration = authoring.fireDuration,
                BulletSpeed = authoring.bulletSpeed,
                DestroyLifeTime = authoring.destroyLifeTime,
                DestroyDistance = authoring.destroyDistance
            });
        }
    }
}

public readonly partial struct GunMuzzleAspect : IAspect
{
    public readonly RefRW<LocalToWorld> Ltw;
    public readonly RefRO<GunMuzzle> Muzzle;

    public Entity Bullet => Muzzle.ValueRO.BulletGo;
    public float Duration => Muzzle.ValueRO.FireDuration;
    public float BulletSpeed => Muzzle.ValueRO.BulletSpeed;
    public float3 CurPos => Ltw.ValueRO.Position;
    public quaternion CurQuaternion => Ltw.ValueRO.Rotation;
    public float3 Dir => Ltw.ValueRO.Forward;
}

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public partial struct GunMuzzleSystem : ISystem
{

    private float _timer;
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GunMuzzle>();
        _timer = 0;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var delta = SystemAPI.Time.DeltaTime;
        
        foreach (var aspect in SystemAPI.Query<GunMuzzleAspect>())
        {
            _timer += delta;
            if (_timer < aspect.Duration) return;
            Entity bullet = state.EntityManager.Instantiate(aspect.Bullet);
            state.EntityManager.SetComponentData(bullet, new Bullet
            {
                Speed = aspect.BulletSpeed,
                StartPos = aspect.CurPos,
                StartQuaternion = aspect.CurQuaternion,
                Dir = aspect.Dir,
                CurDistance = 0,
                DestroyDistance = aspect.Muzzle.ValueRO.DestroyDistance,
                CurLifeTime = 0,
                DestroyLifeTime = aspect.Muzzle.ValueRO.DestroyLifeTime
            });
            _timer = 0;
        }
        
    }
}


