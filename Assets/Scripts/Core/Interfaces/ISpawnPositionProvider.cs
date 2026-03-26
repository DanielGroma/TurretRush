using UnityEngine;

public interface ISpawnPositionProvider
{
    Vector3 GetPosition();
    Vector3 GetPositionInView();
    Vector3 GetPositionOutOfView();

}