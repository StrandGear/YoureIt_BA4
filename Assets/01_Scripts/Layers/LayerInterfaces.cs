using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerInterfaces : MonoBehaviour
{
    //
}

public interface ILayerObject 
{
    bool IsVisible { get; set; }
}

public interface ITransparency
{
    float Transparency { get; set; }
}

public interface ILockable
{
    bool IsLocked { get; set; }
}

public interface ILayerOrder
{
    int LayerOrder { get; set; }
}

public interface ILinkable
{
    bool IsLinkable { get; set; }
}