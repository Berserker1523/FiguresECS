using Unity.Entities;
using Unity.Transforms;

[UpdateBefore(typeof(TransformSystemGroup))]
public class FreezeTranslationAndRotationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities
            .WithAll<FigureTag>()
            .ForEach((ref Translation translation, ref Rotation rotation) =>
            {
                translation.Value.z = 0;
                rotation.Value.value.x = 0;
                rotation.Value.value.y = 0;
            }).ScheduleParallel();
    }
}
