using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RigBone
{
    public GameObject gameObject2;
    public HumanBodyBones bone;
    public bool isValid;

    public Vector3 initialPos;

    public Quaternion initialRot;
    Animator animator;
    Quaternion savedValue;
    public RigBone(GameObject g, HumanBodyBones b)
    {
        gameObject2 = g;
        bone = b;
        isValid = false;
        animator = gameObject2.GetComponent<Animator>();

        if (animator == null)
        {
            Debug.Log("no Animator Component");
            return;
        }
        Avatar avatar = animator.avatar;
        if (avatar == null || !avatar.isHuman || !avatar.isValid)
        {
            Debug.Log("Avatar is not Humanoid or it is not valid");
            return;
        }
        isValid = true;
        //savedValue = animator.GetBoneTransform(bone).localRotation;
        savedValue = animator.GetBoneTransform(bone).transform.rotation;
        initialPos = animator.GetBoneTransform(bone).transform.position;
        initialRot = animator.GetBoneTransform(bone).transform.rotation;
    }
    // public void set(float a, float x, float y, float z)
    // {
    //     set(Quaternion.AngleAxis(a, new Vector3(x, y, z)));
    // }
    public void set(GameObject nextPosition, GameObject oldPosition, Vector3 v1, Vector3 v2, Transform firstPoint, Transform secondPoint, GameObject mainCar, int index)
    {
        // Vector3 firstPoint = new Vector3();
        // Vector3 secondPoint = new Vector3();
        // Transform firstPoint = Avatar[0].transform.parent.GetChild(2);
        // Transform secondPoint = Avatar[0].transform.parent.GetChild(3);

        Vector3 newDirectionVector = new Vector3(0, 0, 0);





        firstPoint.localPosition = v1;
        secondPoint.localPosition = v2;

        // firstPoint.position = mainCar.transform.position + v1;
        // secondPoint.position = mainCar.transform.position + v2;
        newDirectionVector = firstPoint.position - secondPoint.position;


        Quaternion rot = Quaternion.FromToRotation((nextPosition.transform.position) - (oldPosition.transform.position), newDirectionVector);

        Quaternion q = (rot * oldPosition.transform.rotation) * Quaternion.Euler(new Vector3(0, 180, 0));
        //Quaternion q = (rot * oldPosition.transform.rotation);
        animator.GetBoneTransform(bone).rotation = q;


        //savedValue = q;

        // if (index == 0)
        // {
        //     animator.GetBoneTransform(bone).localRotation = mainCar.transform.rotation;
        // }
        //animator.GetBoneTransform(bone).rotation = (rot * initialRot);
        //gameObject2.transform.rotation = (rot * initialRot) * Quaternion.Euler(new Vector3(0, 180, 0));
        //animator.GetBoneTransform(bone).localRotation = q;
        // savedValue = q;
    }

    public void mul(float a, float x, float y, float z)
    {
        mul(Quaternion.AngleAxis(a, new Vector3(x, y, z)));
    }
    public void mul(Quaternion q)
    {
        Transform tr = animator.GetBoneTransform(bone);
        tr.localRotation = q * tr.localRotation;
    }
    public void offset(float a, float x, float y, float z)
    {
        offset(Quaternion.AngleAxis(a, new Vector3(x, y, z)));
    }
    public void offset(Quaternion q)
    {
        animator.GetBoneTransform(bone).localRotation = q * savedValue;
    }
    public void changeBone(HumanBodyBones b)
    {
        bone = b;
        savedValue = animator.GetBoneTransform(bone).localRotation;
    }
}
