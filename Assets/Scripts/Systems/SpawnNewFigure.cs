/*using Unity.Entities;
using Unity.Burst;
using Unity.Physics.Systems;
using Unity.Physics;
using UnityEngine;
using Unity.Collections;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public class SpawnNewFigure : SystemBase
{
    private BeginInitializationEntityCommandBufferSystem commandBufferSystem;
    private BuildPhysicsWorld physicsWorldSystem;
    private int figures2Spawn = 0;
    protected override void OnCreate()
    {
        base.OnCreate();
        commandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        physicsWorldSystem = World.GetExistingSystem<BuildPhysicsWorld>();
    }
    protected override void OnUpdate()
    {
        //EntityCommandBuffer entityCommandBuffer = commandBufferSystem.CreateCommandBuffer();
        var ecb = commandBufferSystem.CreateCommandBuffer();//.AsParallelWriter();
        CollisionWorld collisionWorld = physicsWorldSystem.PhysicsWorld.CollisionWorld;
        Entities
            .WithAny<SpawnerTag>()
            .WithoutBurst()
            //.WithBurst(FloatMode.Default, FloatPrecision.Standard, true)
            .ForEach((Entity entity, int entityInQueryIndex, in SpawnerData spawnerData) =>
            {
                if(figures2Spawn > 10)
                {
                    return;
                }
                //entityCommandBuffer.Instantiate(spawnerData.entity2Spawn);
                //Entity newEntity = ecb.Instantiate(entityInQueryIndex, spawnerData.entity2Spawn);
                Entity newEntity = ecb.Instantiate(spawnerData.entity2Spawn);
                figures2Spawn++;*/
                //ComponentDataFromEntity<PhysicsCollider> allTranslations3 = GetComponentDataFromEntity<PhysicsCollider>(true);
                /*BlobAssetReference<Unity.Physics.Collider> renderBounds3 = allTranslations3[newEntity].Value;
                renderBounds3.Value.CalculateAabb();

                OverlapAabbInput overlapAabbInput = new OverlapAabbInput()
                {
                    Aabb = renderBounds3.Value.CalculateAabb(),
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
                foreach(int a in hitsIndices)
                {
                    Debug.Log(a);
                }*/

            /*}).Run();

        commandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}*/
