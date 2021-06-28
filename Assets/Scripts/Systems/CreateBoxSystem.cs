using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;

[UpdateInGroup(typeof(InitializationSystemGroup))]
[UpdateAfter(typeof(InstantiateFiguresSystem))]
public class CreateBoxSystem : SystemBase
{
    private EndInitializationEntityCommandBufferSystem commandBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        commandBufferSystem = World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var entityCommandBuffer = commandBufferSystem.CreateCommandBuffer().AsParallelWriter();

        Entities
             .WithAll<SpawnerTag>()
             .ForEach((
                 Entity entity,
                 int entityInQueryIndex,
                 DynamicBuffer<CurrentBoxFiguresBufferElement> CurrentBoxFiguresBuffer,
                 DynamicBuffer<CurrentBoxBufferElement> CurrentBoxBuffer,
                 DynamicBuffer<CurrentWallBufferElement> currenWallBuffer,
                 ref SpawnerData spawnerData,
                 in Translation translation) =>
             {
                 if (!spawnerData.hasToCreateNewBox)
                 {
                     return;
                 }

                 for (int i = 0; i < CurrentBoxFiguresBuffer.Length; i++)
                 {
                     entityCommandBuffer.RemoveComponent<PhysicsCollider>(entityInQueryIndex, CurrentBoxFiguresBuffer[i].figure);
                     entityCommandBuffer.RemoveComponent<PhysicsMass>(entityInQueryIndex, CurrentBoxFiguresBuffer[i].figure);
                     entityCommandBuffer.RemoveComponent<PhysicsVelocity>(entityInQueryIndex, CurrentBoxFiguresBuffer[i].figure);
                     entityCommandBuffer.RemoveComponent<PhysicsDamping>(entityInQueryIndex, CurrentBoxFiguresBuffer[i].figure);
                     entityCommandBuffer.RemoveComponent<FigureTag>(entityInQueryIndex, CurrentBoxFiguresBuffer[i].figure);
                     //entityCommandBuffer.RemoveComponent<RenderBounds>(entityInQueryIndex, CurrentBoxFiguresBuffer[i].figure);
                     //entityCommandBuffer.RemoveComponent<WorldRenderBounds>(entityInQueryIndex, CurrentBoxFiguresBuffer[i].figure);
                     //entityCommandBuffer.RemoveComponent<RenderMesh>(entityInQueryIndex, CurrentBoxFiguresBuffer[i].figure);
                 }
                 CurrentBoxFiguresBuffer.Clear();

                 if (CurrentBoxBuffer.IsEmpty)
                 {
                     Entity newBox = entityCommandBuffer.Instantiate(entityInQueryIndex, spawnerData.boxGameObject);
                     Translation newBoxtranslation = new Translation
                     {
                         Value = translation.Value
                     };
                     entityCommandBuffer.SetComponent(entityInQueryIndex, newBox, newBoxtranslation);
                     CurrentBoxBuffer.Clear();
                     entityCommandBuffer.AppendToBuffer(entityInQueryIndex, entity, new CurrentBoxBufferElement
                     {
                         currentBox = newBox
                     });
                 }
                 else
                 {
                     float wall0PositionX = 0;
                     float wall0PositionY = 0;
                     float wall1PositionX = 0;
                     for (int i = 2; i < currenWallBuffer.Length; i++)
                     {
                         if(i == 3)
                         {
                             wall0PositionX = currenWallBuffer[i].wallPosition.x;
                             wall0PositionY = currenWallBuffer[i].wallPosition.y;
                         }
                         else
                         {
                             wall1PositionX = currenWallBuffer[i].wallPosition.x;
                         }
                     }

                     Entity newBox = entityCommandBuffer.Instantiate(entityInQueryIndex, spawnerData.boxGameObject);
                     Translation newBoxtranslation = new Translation
                     {
                         Value = new float3(2 * wall1PositionX - wall0PositionX, wall0PositionY, 0)
                     };
                     entityCommandBuffer.SetComponent(entityInQueryIndex, newBox, newBoxtranslation);
                     CurrentBoxBuffer.Clear();
                     entityCommandBuffer.AppendToBuffer(entityInQueryIndex, entity, new CurrentBoxBufferElement
                     {
                         currentBox = newBox
                     });
                 }

                 spawnerData.boxCounter += 1;
                 spawnerData.hasToCreateNewBox = false;
                 spawnerData.hasToSetBoxWalls = true;

             }).ScheduleParallel();

        commandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}