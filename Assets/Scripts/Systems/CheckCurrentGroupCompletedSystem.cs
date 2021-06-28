using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(InitializationSystemGroup))]
[UpdateAfter(typeof(SetBoxWallsSystem))]
public class CheckCurrentGroupCompletedSystem : SystemBase
{
    private float currentTime;

    protected override void OnUpdate()
    {
        currentTime += Time.DeltaTime;
        float timer = currentTime;

        Entities
            .WithAll<SpawnerTag>()
            .ForEach((
                DynamicBuffer<GroupObjects2SpawnBufferElement> groupObjects2SpawnBuffer,
                ref SpawnerData spawnerData) =>
            {
                if (spawnerData.currentGroupIndex == -1)
                {
                    return;
                }
                bool groupCompleted = spawnerData.currentGroup.object0 + spawnerData.currentGroup.object1 + spawnerData.currentGroup.object2 + spawnerData.currentGroup.object3 == 0;
                if (groupCompleted)
                {
                    if(!groupObjects2SpawnBuffer.IsEmpty)
                    {
                        groupObjects2SpawnBuffer.RemoveAt(spawnerData.currentGroupIndex);
                        if (!groupObjects2SpawnBuffer.IsEmpty)
                        {
                            spawnerData.currentGroupIndex = 0;
                            spawnerData.currentGroup = groupObjects2SpawnBuffer[0];
                        }
                    }
                    else
                    {
                        Debug.Log($"timer: {timer}");
                        Debug.Log($"boxes: {spawnerData.boxCounter}");
                        spawnerData.currentGroupIndex = -1;
                    }
                }
            }).ScheduleParallel();
    }
}
