using UnityEngine;
using Unity.Entities;

public class CurrentBoxBufferElementAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddBuffer<CurrentBoxBufferElement>(entity);
    }
}
