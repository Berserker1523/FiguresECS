using UnityEngine;
using Unity.Entities;

public class InstantiatedFigures3BufferElementAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddBuffer<InstantiatedFigures3BufferElement>(entity);
    }
}
