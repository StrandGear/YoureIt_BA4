using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IInteractable
{
    public BoxCollider InteractionRadius { get; set; }
    public void Interact();

    public bool CanInteract();
}
