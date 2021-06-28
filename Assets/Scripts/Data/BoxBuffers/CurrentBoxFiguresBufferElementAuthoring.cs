using UnityEngine;
using Unity.Entities;

public class CurrentBoxFiguresBufferElementAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddBuffer<CurrentBoxFiguresBufferElement>(entity);
    }
}
