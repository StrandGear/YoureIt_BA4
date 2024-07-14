using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class IKEnemy : MonoBehaviour
{
    [SerializeField] private int chainLength = 2;
    
    
    [SerializeField] private Transform target;
    [SerializeField] private Transform pole;
    
    
    [SerializeField] private int iterations = 10;
    
    [SerializeField] private float delta = 0.001f;
    
    [Range(0,1)]
    //[SerializeField] private float snapBackStrength = 1f;

    protected float[] bonesLength;
    protected float completeLength;
    protected Transform[] bones;
    protected Vector3[] positions;
    protected Vector3[] startDirectionSucc;
    protected Quaternion[] startRotationBone;
    protected Quaternion startRotationTarget;
    protected Quaternion startRotationRoot;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        //initial array
        bones = new Transform[chainLength + 1];
        positions = new Vector3[chainLength + 1];
        bonesLength = new float[chainLength];
        startDirectionSucc = new Vector3[chainLength + 1];
        startRotationBone = new Quaternion[chainLength + 1];

        //init target
        if (target == null)
        {
            target = new GameObject(gameObject.name + " Target").transform;
            target.position = transform.position;
        }
        startRotationTarget = target.rotation;
        
        completeLength = 0;
        
        
        
        //init data
        var current = transform;
        for (var i = bones.Length - 1; i >= 0; i--)
        {
            bones[i] = current;
            startRotationBone[i] = current.rotation;
            
            if (i == bones.Length - 1)
            {
                //leaf
                startDirectionSucc[i] = target.position - current.position;
               
            }
            else
            {
                //mid bone
                startDirectionSucc[i] = bones[i +1].position - current.position;
                bonesLength[i] = (bones[i + 1].position - current.position).magnitude;
                completeLength += bonesLength[i];
            }
            
            current = current.parent;
        }
    }

    private void LateUpdate()
    {
        ResolveIK();

    }

    private void ResolveIK()
    {
        if (target == null)
        {
            return;
        }

        if (bonesLength.Length != chainLength)
        {
            Init();
        }
        
        //get position
        for (int i = 0; i < bones.Length; i++)
            positions[i] = bones[i].position;

        var rootRot = (bones[0].parent != null) ? bones[0].parent.rotation : Quaternion.identity;
        var rootRotDiff = rootRot * Quaternion.Inverse(startRotationRoot);

        if ((target.position - bones[0].position).sqrMagnitude >= completeLength * completeLength)
        {
            
            var direction = (target.position - positions[0]).normalized;
            //set everything after root
            for (int i = 1; i < positions.Length; i++)
                positions[i] = positions[i - 1] + direction * bonesLength[i - 1];
        }
        else
        {
            for (int iteration = 0; iteration < iterations; iteration++)
            {
                //back
                for (int i = positions.Length - 1; i > 0; i--)
                {
                    if (i == positions.Length - 1)
                        positions[i] = target.position; //set it to target
                    else
                    {
                        positions[i] = positions[i + 1] + (positions[i] - positions[i + 1]).normalized * bonesLength[i]; //set in line on distance
                    }
                }

                //forward
                for (int i = 1; i < positions.Length; i++)
                {
                    positions[i] = positions[i - 1] + (positions[i] - positions[i - 1]).normalized * bonesLength[i - 1];
                }
                
                //close enough?
                if ((positions[positions.Length - 1] - target.position).sqrMagnitude < delta * delta)
                    break;
            }
        }
        
        //move towards pole
        if (pole != null)
        {
            
            for (int i = 1; i < positions.Length - 1; i++)
            {
                var plane = new Plane(positions[i + 1] - positions[i - 1], positions[i - 1]);
                var projectedPole = plane.ClosestPointOnPlane(pole.position);
                var projectedBone = plane.ClosestPointOnPlane(positions[i]);
                var angle = Vector3.SignedAngle(projectedBone - positions[i - 1], projectedPole - positions[i - 1], plane.normal);
                positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (positions[i] - positions[i - 1]) + positions[i - 1];
            }
        }
        
        //set position & rotation
        for (int i = 0; i < positions.Length; i++)
        {
            if (i == positions.Length - 1)
            {
                bones[i].rotation = target.rotation * Quaternion.Inverse(startRotationTarget) * startRotationBone[i];
            }
            else
            {
                bones[i].rotation = Quaternion.FromToRotation(startDirectionSucc[i], positions[i+1] - positions[i]) * startRotationBone[i];
            }

            bones[i].position = positions[i];
        }
    }


    private void OnDrawGizmos()
    {
        //Draws each bone in the IK chain
        var current = this.transform;
        for (int i = 0; i < chainLength && current != null && current.parent != null; i++)
        {
            var scale = Vector3.Distance(current.position, current.parent.position) * 0.1f;
            Handles.matrix = Matrix4x4.TRS(current.position, Quaternion.FromToRotation(Vector3.up, current.parent.position - current.position), new Vector3(scale, Vector3.Distance(current.parent.position, current.position), scale));
            Handles.color = Color.green;
            Handles.DrawWireCube(Vector3.up * 0.5f, Vector3.one);
            current = current.parent;
        }
    }
}
