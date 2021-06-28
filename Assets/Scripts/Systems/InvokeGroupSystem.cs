using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(InitializationSystemGroup))]
[UpdateAfter(typeof(SetBoxWallsSystem))]
public class InvokeGroupSystem : SystemBase
{
    private const float waitSeconds = 0.05f;
    private const int maxTries = 100;
    private EndInitializationEntityCommandBufferSystem commandBufferSystem;
    private BuildPhysicsWorld physicsWorldSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        commandBufferSystem = World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
        physicsWorldSystem = World.GetExistingSystem<BuildPhysicsWorld>();
    }

    private static int GetFigureKindToInstantiate(GroupObjects2SpawnBufferElement currentGroup, Random random)
    {
        NativeList<int> availableFigureKindIndexes = new NativeList<int>(Allocator.Temp);
        if (currentGroup.object0 > 0)
        {
            availableFigureKindIndexes.Add(0);
        }
        if (currentGroup.object1 > 0)
        {
            availableFigureKindIndexes.Add(1);
        }
        if (currentGroup.object2 > 0)
        {
            availableFigureKindIndexes.Add(2);
        }
        if (currentGroup.object3 > 0)
        {
            availableFigureKindIndexes.Add(3);
        }

        if (availableFigureKindIndexes.IsEmpty)
        {
            return -1;
        }

        for (int i = 0; i < availableFigureKindIndexes.Length; i++)
        {
            int temp = availableFigureKindIndexes[i];
            int randomIndex = random.NextInt(i, availableFigureKindIndexes.Length);
            availableFigureKindIndexes[i] = availableFigureKindIndexes[randomIndex];
            availableFigureKindIndexes[randomIndex] = temp;
        }

        int figureKindToInstantiate = availableFigureKindIndexes[0];
        availableFigureKindIndexes.Dispose();
        return figureKindToInstantiate;
    }

    private static float2 GetRandomPosition(
        float polygonExtentsX,
        float polygonExtentsY,
        CurrentWallBufferElement leftWall,
        CurrentWallBufferElement rightWall,
        CurrentWallBufferElement bottomWall,
        CurrentWallBufferElement topWall,
        Random random,
        float tolerance = 0.1f)
    {
        float spawnPositionX = random.NextFloat(leftWall.wallPosition.x + leftWall.wallColliderExtents.x + polygonExtentsX + tolerance,
             rightWall.wallPosition.x - rightWall.wallColliderExtents.x - tolerance);

        float spawnPositionY = random.NextFloat(bottomWall.wallPosition.y + bottomWall.wallColliderExtents.y + polygonExtentsY + tolerance,
            topWall.wallPosition.y - topWall.wallColliderExtents.y - polygonExtentsY - tolerance);

        return new float2(spawnPositionX, spawnPositionY);
    }

    private static bool CheckEmptyPosition(
        CollisionWorld collisionWorld,
        float2 spawnPosition,
        float polygonExtentsX,
        float polygonExtentsY)
    {
        OverlapAabbInput overlapAabbInput = new OverlapAabbInput()
        {
            Aabb = new Aabb()
            {
                //Min = new float3(spawnPosition.x - renderBoundsX + 0.1f, spawnPosition.y - renderBoundsY + 0.1f, -1),
                //Max = new float3(spawnPosition.x + renderBoundsX - 0.1f, spawnPosition.y + renderBoundsY - 0.1f, 1)
                Min = new float3(spawnPosition.x - polygonExtentsX, spawnPosition.y - polygonExtentsY, -1),
                Max = new float3(spawnPosition.x + polygonExtentsX, spawnPosition.y + polygonExtentsY, 1)
            },
            Filter = new CollisionFilter()
            {
                BelongsTo = ~0u,
                CollidesWith = ~0u, // all 1s, so all layers, collide with everything
                GroupIndex = 0
            }
        };

        NativeList<int> hitsIndices = new NativeList<int>(Allocator.Temp);
        bool haveHit =  collisionWorld.OverlapAabb(overlapAabbInput, ref hitsIndices);
        hitsIndices.Dispose();
        return haveHit;
    }

    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        Random random = new Random((uint)UnityEngine.Random.Range(int.MinValue, int.MaxValue));

        EntityCommandBuffer.ParallelWriter entityCommandBuffer = commandBufferSystem.CreateCommandBuffer().AsParallelWriter();
        CollisionWorld collisionWorld = physicsWorldSystem.PhysicsWorld.CollisionWorld;

        Entities
            .WithReadOnly(collisionWorld)
            .WithAll<SpawnerTag>()
            .ForEach((
                Entity entity,
                int entityInQueryIndex,
                DynamicBuffer<InstantiatedFigures0BufferElement> InstantiatedFigures0Buffer,
                DynamicBuffer<InstantiatedFigures1BufferElement> InstantiatedFigures1Buffer,
                DynamicBuffer<InstantiatedFigures2BufferElement> InstantiatedFigures2Buffer,
                DynamicBuffer<InstantiatedFigures3BufferElement> InstantiatedFigures3Buffer,
                DynamicBuffer<CurrentWallBufferElement> currenWallBuffer,
                ref SpawnerData spawnerData) =>
            {
                if (spawnerData.hasToWait && spawnerData.waitedSeconds < waitSeconds)
                {
                    spawnerData.waitedSeconds += deltaTime;
                    return;
                }
                else
                {
                    spawnerData.waitedSeconds = 0;
                    spawnerData.hasToWait = false;
                }

                if (spawnerData.currentGroupIndex == -1)
                {
                    return;
                }

                if (currenWallBuffer.IsEmpty)
                {
                    return;
                }

                if (InstantiatedFigures0Buffer.Length + InstantiatedFigures1Buffer.Length + InstantiatedFigures2Buffer.Length + InstantiatedFigures3Buffer.Length == 0)
                {
                    return;
                }

                int figureKindToInstantiate = GetFigureKindToInstantiate(spawnerData.currentGroup, random);
                if (figureKindToInstantiate == -1)
                {
                    return;
                }

                Entity instantiatedFigure;
                switch (figureKindToInstantiate)
                {
                    case 0:
                        instantiatedFigure = InstantiatedFigures0Buffer[0].figure;
                        break;
                    case 1:
                        instantiatedFigure = InstantiatedFigures1Buffer[0].figure;
                        break;
                    case 2:
                        instantiatedFigure = InstantiatedFigures2Buffer[0].figure;
                        break;
                    case 3:
                        instantiatedFigure = InstantiatedFigures3Buffer[0].figure;
                        break;
                    default:
                        instantiatedFigure = InstantiatedFigures3Buffer[0].figure;
                        break;
                }


                ComponentDataFromEntity<RenderBounds> allRenderBounds = GetComponentDataFromEntity<RenderBounds>(true);
                RenderBounds entityRenderBounds = allRenderBounds[instantiatedFigure];
                float polygonExtentsX = entityRenderBounds.Value.Extents.x;
                float polygonExtentsY = entityRenderBounds.Value.Extents.y;

                float2 spawnPosition;
                float tests = 0;
                bool haveHit = true;
                do
                {
                    spawnPosition = GetRandomPosition(polygonExtentsX, polygonExtentsY, currenWallBuffer[3], currenWallBuffer[2], currenWallBuffer[1], currenWallBuffer[0], random);
                    haveHit = CheckEmptyPosition(collisionWorld, spawnPosition, polygonExtentsX, polygonExtentsX);
                    tests++;
                }
                while (haveHit == true && tests < maxTries);

                if (tests == maxTries)
                {
                    spawnerData.hasToWait = true;
                    spawnerData.waitedSeconds = 0;
                    spawnerData.currentAttemptsToCreateNewBox++;
                    if (spawnerData.currentAttemptsToCreateNewBox == spawnerData.attemptsToCreateNewBox)
                    {
                        spawnerData.currentAttemptsToCreateNewBox = 0;
                        spawnerData.hasToCreateNewBox = true;
                    }
                }
                else
                {
                    spawnerData.currentAttemptsToCreateNewBox = 0;
                    entityCommandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, instantiatedFigure);
                    entityCommandBuffer.SetComponent(entityInQueryIndex, instantiatedFigure, new Translation
                    {
                        Value = new float3(spawnPosition.x, spawnPosition.y, 0)
                    });

                    switch (figureKindToInstantiate)
                    {
                        case 0:
                            InstantiatedFigures0Buffer.RemoveAt(0);
                            spawnerData.currentGroup.object0--;
                            break;
                        case 1:
                            InstantiatedFigures1Buffer.RemoveAt(0);
                            spawnerData.currentGroup.object1--;
                            break;
                        case 2:
                            InstantiatedFigures2Buffer.RemoveAt(0);
                            spawnerData.currentGroup.object2--;
                            break;
                        case 3:
                            InstantiatedFigures3Buffer.RemoveAt(0);
                            spawnerData.currentGroup.object3--;
                            break;
                        default:
                            InstantiatedFigures3Buffer.RemoveAt(0);
                            spawnerData.currentGroup.object3--;
                            break;
                    }
                    entityCommandBuffer.AppendToBuffer(entityInQueryIndex, entity, new CurrentBoxFiguresBufferElement
                    {
                        figure = instantiatedFigure
                    });

                }
            }).ScheduleParallel();

        commandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}
