using UnityEngine;
using Unity.Entities;

public class CurrentWallBufferElementAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddBuffer<CurrentWallBufferElement>(entity);
    }
}
