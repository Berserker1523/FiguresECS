/*using Unity.Entities;
using Unity.Burst;
using Unity.Physics.Systems;
using Unity.Physics;
using UnityEngine;
using Unity.Collections;
using Unity.Rendering;

//[UpdateInGroup(typeof(PresentationSystemGroup))]
//[UpdateBefore(typeof(BeginPresentationEntityCommandBufferSystem))]
[UpdateInGroup(typeof(SimulationSystemGroup))]
//[UpdateBefore(typeof(SpawnNewFigure))]
public class DeleteFigureIfOverlaps : SystemBase
{
    private BeginPresentationEntityCommandBufferSystem commandBufferSystem;
    private BuildPhysicsWorld physicsWorldSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        commandBufferSystem = World.GetOrCreateSystem<BeginPresentationEntityCommandBufferSystem>();
        physicsWorldSystem = World.GetExistingSystem<BuildPhysicsWorld>();
    }
    protected override void OnUpdate()
    {
        //EntityCommandBuffer entityCommandBuffer = commandBufferSystem.CreateCommandBuffer();
        //var ecb = commandBufferSystem.CreateCommandBuffer();//.AsParallelWriter();
        CollisionWorld collisionWorld = physicsWorldSystem.PhysicsWorld.CollisionWorld;
        Entities
            .WithAny<FigureTag>()
            .WithReadOnly(collisionWorld)
            //.WithBurst(FloatMode.Default, FloatPrecision.Standard, true)
            .ForEach((Entity entity, in RenderBounds b, in WorldRenderBounds a, in PhysicsCollider collider) =>
            {
                //entityCommandBuffer.Instantiate(spawnerData.entity2Spawn);
                //Entity newEntity = ecb.Instantiate(entityInQueryIndex, spawnerData.entity2Spawn);
                //Entity newEntity = ecb.Instantiate(spawnerData.entity2Spawn);
                //ComponentDataFromEntity<PhysicsCollider> allTranslations3 = GetComponentDataFromEntity<PhysicsCollider>(true);
                //Debug.Log($"b: {b.Value.Center}");
                //Debug.Log($"a: {a.Value.Center}");
                //BlobAssetReference<Unity.Physics.Collider> renderBounds3 = collider.Value;
                //Debug.Log(renderBounds3.Value.CalculateAabb().Center);
                Debug.Log($"{a.Value.Min}, {a.Value.Max}");
                OverlapAabbInput overlapAabbInput = new OverlapAabbInput()
                {
                    Aabb = new Aabb()
                    {
                        Min = a.Value.Min + 0.1f,
                        Max = a.Value.Max - 0.1f
                    },
                    Filter = new CollisionFilter()
                    {
                        BelongsTo = ~0u,
                        CollidesWith = ~0u, // all 1s, so all layers, collide with everything
                        GroupIndex = 0
                    }
                };
                
                OverlapAabbHit hit = new OverlapAabbHit();
                NativeList<int> hitsIndices = new NativeList<int>(Allocator.Temp);
                bool haveHit = collisionWorld.OverlapAabb(overlapAabbInput, ref hitsIndices);
                //Debug.Log(overlapAabbInput.Aabb.Center);
                //Debug.Log(overlapAabbInput.Aabb.Extents);
                //Debug.Log($"have hit: {haveHit}");
                Debug.Log($"lenght: {hitsIndices.Length}");
                //foreach (int a in hitsIndices)
                //{
                //    Debug.Log($"index: {a}");
                //}
                //if (hitsIndices.Length > 1)
                //{
                 //   ecb.DestroyEntity(entity);
                //}

            }).ScheduleParallel();

        commandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}*/
