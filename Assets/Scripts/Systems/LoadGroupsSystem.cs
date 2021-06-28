using System;
using System.IO;
using UnityEngine;
using Unity.Entities;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public class LoadGroupsSystem : SystemBase
{
    private const string ConfigurationDataFileName = "data.csv";
    private bool ran = false;

    protected override void OnUpdate()
    {
        if(ran)
        {
            return;
        }

        Entities
            .WithoutBurst()
            .WithAll<SpawnerTag>()
            .ForEach((DynamicBuffer<GroupObjects2SpawnBufferElement> groupObjects2SpawnBuffer, ref SpawnerData spawnerData) =>
            {
                
                groupObjects2SpawnBuffer.Clear();
                StreamReader file = null;
                try
                {
                    file = File.OpenText(Path.Combine(Application.streamingAssetsPath, ConfigurationDataFileName));
                    string currentLine = file.ReadLine();
                    while (currentLine != null)
                    {
                        int[] group = Array.ConvertAll(currentLine.Split(','), int.Parse);
       
                        groupObjects2SpawnBuffer.Add(new GroupObjects2SpawnBufferElement 
                        { 
                            object0 = group[0],
                            object1 = group[1],
                            object2 = group[2],
                            object3 = group[3],
                        });
                        
                        currentLine = file.ReadLine();
                    }

                    spawnerData.currentGroupIndex = 0;
                    spawnerData.currentGroup = groupObjects2SpawnBuffer[0];
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                finally
                {
                    if (file != null)
                    {
                        file.Close();
                    }
                }
            }).ScheduleParallel();

        ran = true;
    }
}
