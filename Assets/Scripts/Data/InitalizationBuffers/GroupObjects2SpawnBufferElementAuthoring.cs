using UnityEngine;
using Unity.Entities;

public class GroupObjects2SpawnBufferElementAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddBuffer<GroupObjects2SpawnBufferElement>(entity);
    }
}
