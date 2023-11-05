using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public struct Turret : IComponentData
{
    public float RotateSpeed;
}

public class TurretAuthoring : MonoBehaviour
{

    public float rotationSpeed;
    
    class Baker: Baker<TurretAuthoring>
    {
        public override void Bake(TurretAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Turret
            {
                RotateSpeed = math.radians(authoring.rotationSpeed),
            });
        }
    }
}

public readonly partial struct TurretAspect : IAspect
{
    public readonly RefRW<LocalTransform> LocalTrans;
    public readonly RefRO<Turret> TurretValue;

    public void Rotate(float delta)
    {
        LocalTrans.ValueRW = LocalTrans.ValueRW.RotateY(TurretValue.ValueRO.RotateSpeed * delta);
    }
}

public partial struct TurretSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Turret>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var delta = SystemAPI.Time.DeltaTime;
        foreach (var aspect in SystemAPI.Query<TurretAspect>())
        {
            aspect.Rotate(delta);
        }
    }
}