using Unity.Entities;

[UpdateInGroup(typeof(InitializationSystemGroup))]
[UpdateAfter(typeof(LoadGroupsSystem))]
public class InstantiateFiguresSystem : SystemBase
{
    private bool ran = false;
    private EndInitializationEntityCommandBufferSystem commandBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        commandBufferSystem = World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        if (ran)
        {
            return;
        }

        void SetEntityData(EntityCommandBuffer.ParallelWriter entityCommandBuffer, int entityInQueryIndex, Entity newFigure)
        {
            entityCommandBuffer.AddComponent<Disabled>(entityInQueryIndex, newFigure);
        }

        var entityCommandBuffer = commandBufferSystem.CreateCommandBuffer().AsParallelWriter();

        Entities
            .WithAll<SpawnerTag>()
            .ForEach((
                Entity entity,
                int entityInQueryIndex,
                DynamicBuffer<GroupObjects2SpawnBufferElement> groupObjects2SpawnBuffer,
                DynamicBuffer<InstantiatedFigures0BufferElement> InstantiatedFigures0Buffer,
                DynamicBuffer<InstantiatedFigures1BufferElement> InstantiatedFigures1Buffer,
                DynamicBuffer<InstantiatedFigures2BufferElement> InstantiatedFigures2Buffer,
                DynamicBuffer<InstantiatedFigures3BufferElement> InstantiatedFigures3Buffer,
                in SpawnerData spawnerData) =>
            {
                for (int i = 0; i < groupObjects2SpawnBuffer.Length; i++)
                {
                    for (int j = 0; j < groupObjects2SpawnBuffer[i].object0; j++)
                    {
                        Entity newFigure = entityCommandBuffer.Instantiate(entityInQueryIndex, spawnerData.object0);
                        SetEntityData(entityCommandBuffer, entityInQueryIndex, newFigure);
                        entityCommandBuffer.AppendToBuffer(entityInQueryIndex, entity, new InstantiatedFigures0BufferElement
                        {
                            figure = newFigure
                        });
                    }

                    for (int j = 0; j < groupObjects2SpawnBuffer[i].object1; j++)
                    {
                        Entity newFigure = entityCommandBuffer.Instantiate(entityInQueryIndex, spawnerData.object1);
                        SetEntityData(entityCommandBuffer, entityInQueryIndex, newFigure);
                        entityCommandBuffer.AppendToBuffer(entityInQueryIndex, entity, new InstantiatedFigures1BufferElement
                        {
                            figure = newFigure
                        });
                    }

                    for (int j = 0; j < groupObjects2SpawnBuffer[i].object2; j++)
                    {
                        Entity newFigure = entityCommandBuffer.Instantiate(entityInQueryIndex, spawnerData.object2);
                        SetEntityData(entityCommandBuffer, entityInQueryIndex, newFigure);
                        entityCommandBuffer.AppendToBuffer(entityInQueryIndex, entity, new InstantiatedFigures2BufferElement
                        {
                            figure = newFigure
                        });
                    }

                    for (int j = 0; j < groupObjects2SpawnBuffer[i].object3; j++)
                    {
                        Entity newFigure = entityCommandBuffer.Instantiate(entityInQueryIndex, spawnerData.object3);
                        SetEntityData(entityCommandBuffer, entityInQueryIndex, newFigure);
                        entityCommandBuffer.AppendToBuffer(entityInQueryIndex, entity, new InstantiatedFigures3BufferElement
                        {
                            figure = newFigure
                        });
                    }
                }
            }).ScheduleParallel(); ;

        ran = true;
        commandBufferSystem.AddJobHandleForProducer(Dependency);
    }

}
