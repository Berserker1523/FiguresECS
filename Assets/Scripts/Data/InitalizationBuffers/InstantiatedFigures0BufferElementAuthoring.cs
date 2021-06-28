using UnityEngine;
using Unity.Entities;

public class InstantiatedFigures0BufferElementAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddBuffer<InstantiatedFigures0BufferElement>(entity);
    }
}
