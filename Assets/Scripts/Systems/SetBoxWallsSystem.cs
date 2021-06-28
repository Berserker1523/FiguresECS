using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;

[UpdateInGroup(typeof(InitializationSystemGroup))]
[UpdateAfter(typeof(CreateBoxSystem))]
public class SetBoxWallsSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities
             .WithAll<SpawnerTag>()
             .ForEach((
                 Entity entity,
                 int entityInQueryIndex,
                 DynamicBuffer<CurrentBoxBufferElement> currentBoxBuffer,
                 DynamicBuffer<CurrentWallBufferElement> currenWallBuffer,
                 ref SpawnerData spawnerData) =>
             {
                 if (!spawnerData.hasToSetBoxWalls || currentBoxBuffer.IsEmpty)
                 {
                     return;
                 }

                 Entity box = currentBoxBuffer[0].currentBox;
                 BufferFromEntity<Child> childBufferFromEntity = GetBufferFromEntity<Child>();
                 DynamicBuffer<Child> boxChildBuffer = childBufferFromEntity[box];

                 ComponentDataFromEntity<WorldRenderBounds> allRenderBounds = GetComponentDataFromEntity<WorldRenderBounds>(true);
                 currenWallBuffer.Clear();
                 for (int i = 0; i < boxChildBuffer.Length; i++)
                 {
                     if (allRenderBounds.HasComponent(boxChildBuffer[i].Value))
                     {
                         WorldRenderBounds entityRenderBounds = allRenderBounds[boxChildBuffer[i].Value];
                         currenWallBuffer.Add(new CurrentWallBufferElement
                         {
                             wallPosition = entityRenderBounds.Value.Center,
                             wallColliderExtents = entityRenderBounds.Value.Extents
                         });
                     }
                 }

                 spawnerData.hasToSetBoxWalls = false;
                 /*if(spawnerData.a < 3)
                 {
                     spawnerData.hasToCreateNewBox = true;
                     spawnerData.a += 1;
                 }*/
             }).Schedule();
    }
}
