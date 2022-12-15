using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class RigControl : MonoBehaviour
{
    public GameObject humanoid;
    public Vector3 bodyRotation = new Vector3(0, 0, 0);
    //RigBone leftUpperArm;

    //RigBone rightUpperArm;



    RigBone hips;
    RigBone spine;
    RigBone neck;
    RigBone head;
    RigBone leftshoulder;
    RigBone leftLowerArm;

    RigBone lefthand;
    RigBone rightshoulder;
    RigBone rightLowerArm;
    RigBone righthand;
    RigBone leftupperleg;
    RigBone leftlowerleg;
    RigBone leftfoot;
    RigBone lefttoes;
    RigBone rightUpperLeg;
    RigBone rightLowerLeg;
    RigBone rightfoot;
    RigBone righttoes;

    public List<RigBone> allbones = new List<RigBone>();

    public List<Vector4> rotationsBones;

    public List<Vector3> positionsBones;

    public List<Vector3> InitialPositions;

    public Transform firstPoint;

    public Transform secondPoint;

    public GameObject mainCar;

    public List<GameObject> test;

    public List<GameObject> AvatarModel;

    void Awake()
    {
        createBonesandAdd();
    }

    void Start()
    {




    }
    void Update()
    {
        // double t = Math.Sin(Time.time * Math.PI); // [-1, 1]
        // double s = (t + 1) / 2;                       // [0, 1]
        // double u = 1 - s / 2;                         // [0.5, 1]
        // leftUpperArm.set((float)(80 * t), 1, 0, 0);
        // leftLowerArm.set((float)(90 * s), 1, 0, 0);
        // rightUpperArm.set((float)(90 * t), 0, 0, 1);
        // //rightUpperLeg.set((float)(180 * u), 1, 0, 0);
        // rightLowerLeg.set((float)(90 * s), 1, 0, 0);
        // humanoid.transform.rotation
        //   = Quaternion.AngleAxis(bodyRotation.z, new Vector3(0, 0, 1))
        //   * Quaternion.AngleAxis(bodyRotation.x, new Vector3(1, 0, 0))
        //   * Quaternion.AngleAxis(bodyRotation.y, new Vector3(0, 1, 0));
    }

    public void setbonesAll()
    {
        //setInitialPositions();
        //transform.localRotation = Quaternion.identity;

        AvatarModel[0].transform.parent.transform.rotation = mainCar.transform.rotation * Quaternion.Euler(new Vector3(0, 180, 0));
        transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));


        if (positionsBones[0] == new Vector3(-1000f, -1000f, -1000f))
        {
            gameObject.SetActive(false);
        }
        else
        {
            if (gameObject.name == "Passenger")
            {
                gameObject.SetActive(true);
            }


            for (int i = 0; i < allbones.Count; i++)
            {

                if (i == 3 || i == 6 || i == 9 || i == 13 || i == 17)
                {
                    // Quaternion t = new Quaternion(rotationsBones[i].x, rotationsBones[i].y, rotationsBones[i].z, rotationsBones[i].w);
                    // allbones[i].offset(t);
                }
                else
                {

                    allbones[i].set(AvatarModel[i + 1], AvatarModel[i], positionsBones[i + 1], positionsBones[i], firstPoint, secondPoint, mainCar, i);
                    //transform.localRotation = mainCar.transform.rotation;
                }

            }
        }
        //transform.rotation.SetLookRotation(Vector3.forward);

        //transform.Rotate(new Vector3(0, 1, 0), 180);
    }

    public void setInitialPositions()
    {
        Animator animator = this.GetComponent<Animator>();
        InitialPositions.Add(animator.GetBoneTransform(HumanBodyBones.Hips).transform.position);
        InitialPositions.Add(animator.GetBoneTransform(HumanBodyBones.Spine).transform.position);
        InitialPositions.Add(animator.GetBoneTransform(HumanBodyBones.Neck).transform.position);
        InitialPositions.Add(animator.GetBoneTransform(HumanBodyBones.Head).transform.position);
        InitialPositions.Add(animator.GetBoneTransform(HumanBodyBones.LeftUpperArm).transform.position);
        InitialPositions.Add(animator.GetBoneTransform(HumanBodyBones.LeftLowerArm).transform.position);
        InitialPositions.Add(animator.GetBoneTransform(HumanBodyBones.LeftHand).transform.position);
        InitialPositions.Add(animator.GetBoneTransform(HumanBodyBones.RightUpperArm).transform.position);
        InitialPositions.Add(animator.GetBoneTransform(HumanBodyBones.RightLowerArm).transform.position);
        InitialPositions.Add(animator.GetBoneTransform(HumanBodyBones.RightHand).transform.position);
        InitialPositions.Add(animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg).transform.position);
        InitialPositions.Add(animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).transform.position);
        InitialPositions.Add(animator.GetBoneTransform(HumanBodyBones.LeftFoot).transform.position);
        InitialPositions.Add(animator.GetBoneTransform(HumanBodyBones.LeftToes).transform.position);
        InitialPositions.Add(animator.GetBoneTransform(HumanBodyBones.RightUpperLeg).transform.position);
        InitialPositions.Add(animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).transform.position);
        InitialPositions.Add(animator.GetBoneTransform(HumanBodyBones.RightFoot).transform.position);
        InitialPositions.Add(animator.GetBoneTransform(HumanBodyBones.RightToes).transform.position);
    }


    public void createBonesandAdd()
    {
        hips = new RigBone(humanoid, HumanBodyBones.Hips);
        spine = new RigBone(humanoid, HumanBodyBones.Spine);
        neck = new RigBone(humanoid, HumanBodyBones.Neck);
        head = new RigBone(humanoid, HumanBodyBones.Head);
        leftshoulder = new RigBone(humanoid, HumanBodyBones.LeftUpperArm);
        //leftUpperArm = new RigBone(humanoid, HumanBodyBones.LeftUpperArm);
        leftLowerArm = new RigBone(humanoid, HumanBodyBones.LeftLowerArm);
        lefthand = new RigBone(humanoid, HumanBodyBones.LeftHand);
        rightshoulder = new RigBone(humanoid, HumanBodyBones.RightUpperArm);
        rightLowerArm = new RigBone(humanoid, HumanBodyBones.RightLowerArm);
        //rightUpperArm = new RigBone(humanoid, HumanBodyBones.RightUpperArm);
        righthand = new RigBone(humanoid, HumanBodyBones.RightHand);
        leftupperleg = new RigBone(humanoid, HumanBodyBones.LeftUpperLeg);
        leftlowerleg = new RigBone(humanoid, HumanBodyBones.LeftLowerLeg);
        leftfoot = new RigBone(humanoid, HumanBodyBones.LeftFoot);
        lefttoes = new RigBone(humanoid, HumanBodyBones.LeftToes);
        rightUpperLeg = new RigBone(humanoid, HumanBodyBones.RightUpperLeg);
        rightLowerLeg = new RigBone(humanoid, HumanBodyBones.RightLowerLeg);
        rightfoot = new RigBone(humanoid, HumanBodyBones.RightFoot);
        righttoes = new RigBone(humanoid, HumanBodyBones.RightToes);
        allbones.Add(hips);
        allbones.Add(spine);
        allbones.Add(neck);
        allbones.Add(head);
        allbones.Add(leftshoulder);
        allbones.Add(leftLowerArm);
        allbones.Add(lefthand);
        allbones.Add(rightshoulder);
        allbones.Add(rightLowerArm);
        allbones.Add(righthand);
        allbones.Add(leftupperleg);
        allbones.Add(leftlowerleg);
        allbones.Add(leftfoot);
        allbones.Add(lefttoes);
        allbones.Add(rightUpperLeg);
        allbones.Add(rightLowerLeg);
        allbones.Add(rightfoot);
        allbones.Add(righttoes);
        setInitialPositions();

        GameObject firstpoint = new GameObject();
        firstpoint.transform.parent = transform;
        firstpoint.transform.position = transform.position;
        GameObject secondpoint = new GameObject();
        secondpoint.transform.parent = transform;
        secondpoint.transform.position = transform.position;
        firstPoint = firstpoint.transform;
        secondPoint = secondpoint.transform;
        test.Add(firstpoint);
        test.Add(secondpoint);
    }
}