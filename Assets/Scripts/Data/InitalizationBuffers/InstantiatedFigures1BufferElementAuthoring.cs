using UnityEngine;
using Unity.Entities;

public class InstantiatedFigures1BufferElementAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddBuffer<InstantiatedFigures1BufferElement>(entity);
    }
}
