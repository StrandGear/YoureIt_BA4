using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerInterfaces : MonoBehaviour
{
    //
}

public interface ILayerObject 
{
    GameObject LayerGameObject { get; }

    Vector3 CurrentFixedPosition { get; set; }

    int ID { get; }

    string Name { get; set; }

    bool IsUsed { get; set; }

    public void ResetPosition();
}

public interface ITransparency
{
    float Transparency { get; set; }
}

public interface ILockable
{
    bool IsLocked { get; set; }

    public void LockLayer();
}

public interface ILayerOrder
{
    int LayerOrder { get; set; }
}

public interface ILinkable
{
    bool IsLinkable { get; set; }
}

public interface IHiding
{
    bool IsHidden { get; set; }

    Renderer ObjectRenderer { get; set; }

    public void UpdateHidingProperty();
}