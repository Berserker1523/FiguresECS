using UnityEngine;
using Unity.Entities;

public class InstantiatedFigures2BufferElementAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddBuffer<InstantiatedFigures2BufferElement>(entity);
    }
}
