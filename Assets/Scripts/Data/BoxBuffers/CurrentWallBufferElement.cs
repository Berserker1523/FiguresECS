using Unity.Entities;
using Unity.Mathematics;

[InternalBufferCapacity(4)]
public struct CurrentWallBufferElement : IBufferElementData
{
    public float3 wallPosition;
    public float3 wallColliderExtents;
}
