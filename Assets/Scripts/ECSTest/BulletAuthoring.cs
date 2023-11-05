using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public struct Bullet : IComponentData
{
    public float Speed;
    public float3 StartPos;
    public quaternion StartQuaternion;
    public float3 Dir;
    public float CurLifeTime;
    public float DestroyLifeTime;
    public float CurDistance;
    public float DestroyDistance;
}

public class BulletAuthoring : MonoBehaviour
{
    class Baker : Baker<BulletAuthoring>
    {
        public override void Bake(BulletAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<Bullet>(entity);
        }
    }
}

[BurstCompile]
public partial struct BulletSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var delta = SystemAPI.Time.DeltaTime;
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        var writer = ecb.AsParallelWriter();
        var bulletJob = new BulletJob
        {
            Delta = delta,
            Writer = writer
        };
        state.Dependency = bulletJob.ScheduleParallel(state.Dependency);
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

[BurstCompile]
public partial struct BulletJob : IJobEntity
{
    public float Delta;
    public EntityCommandBuffer.ParallelWriter Writer;

    public void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity, BulletAspect aspect)
    {
        if (aspect.IsMoving)
        {
            aspect.Move(Delta);
        }
        else
        {
            aspect.InitPos(Delta);
        }
        
        if (aspect.NeedDestroy)
        {
            Writer.DestroyEntity(chunkIndex, entity);
        }
    }
}

public readonly partial struct BulletAspect : IAspect
{
    public readonly RefRW<Bullet> Bullet;
    public readonly RefRW<LocalTransform> Transform;

    public bool NeedDestroy
    {
        get
        {
            var overDist = Bullet.ValueRO.CurDistance > Bullet.ValueRO.DestroyDistance;
            var overTime = Bullet.ValueRO.CurLifeTime > Bullet.ValueRO.DestroyLifeTime;
            return overDist || overTime;
        }
    }
    
    public bool IsMoving
    {
        get
        {
            var hasDist = Bullet.ValueRO.CurDistance > 0;
            var hasTime = Bullet.ValueRO.CurLifeTime > 0;
            return hasDist || hasTime;
        }
    }

    public void InitPos(float delta)
    {
        Transform.ValueRW.Position = Bullet.ValueRO.StartPos;
        Transform.ValueRW.Rotation = Bullet.ValueRO.StartQuaternion;
        Bullet.ValueRW.CurLifeTime += delta;
    }

    public void Move(float delta)
    {
        Transform.ValueRW.Position += Bullet.ValueRO.Dir * delta * Bullet.ValueRO.Speed;
        
        Bullet.ValueRW.CurLifeTime += delta;
        Bullet.ValueRW.CurDistance = math.length(Transform.ValueRO.Position - Bullet.ValueRO.StartPos);
    }
}
