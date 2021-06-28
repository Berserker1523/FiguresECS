using Unity.Entities;

[GenerateAuthoringComponent]
public struct SpawnerData : IComponentData
{
    public Entity object0;
    public Entity object1;
    public Entity object2;
    public Entity object3;

    public int currentGroupIndex;
    public GroupObjects2SpawnBufferElement currentGroup;

    public int boxCounter;
    public Entity boxGameObject;
    public bool hasToCreateNewBox;
    public bool hasToSetBoxWalls;
    public int a;

    public bool hasToWait;
    public float waitedSeconds;
    public int currentAttemptsToCreateNewBox;
    public int attemptsToCreateNewBox;
}
