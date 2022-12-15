using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectories : MonoBehaviour
{
    // Start is called before the first frame update


    public List<LineRenderer> TrajectoriesList;

    public List<Vector3> headTrajectoryPositions;

    public List<Vector3> leftHandTrajectoryPositions;

    public List<Vector3> rightHandTrajectoryPositions;

    public List<Color32> userColors;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetupTrajectories(int participantindex, List<Vector3> head, List<Vector3> leftHand, List<Vector3> rightHand)
    {
        headTrajectoryPositions = head;
        leftHandTrajectoryPositions = leftHand;
        rightHandTrajectoryPositions = rightHand;
        for (int i = 0; i < TrajectoriesList.Count; i++)
        {
            TrajectoriesList[i].startColor = userColors[participantindex * 3 + i];
            TrajectoriesList[i].endColor = userColors[participantindex * 3 + i];
        }
    }

    public void DrawTrajectories(int index)
    {
        int NumberOfTrajectories = 50;
        if (index > NumberOfTrajectories)
        {

            TrajectoriesList[0].positionCount = NumberOfTrajectories;
            TrajectoriesList[1].positionCount = NumberOfTrajectories;
            TrajectoriesList[2].positionCount = NumberOfTrajectories;
            for (int i = index - NumberOfTrajectories; i < index; i++)
            {
                TrajectoriesList[0].SetPosition(i - (index - NumberOfTrajectories), headTrajectoryPositions[i]);
                TrajectoriesList[1].SetPosition(i - (index - NumberOfTrajectories), leftHandTrajectoryPositions[i]);
                TrajectoriesList[2].SetPosition(i - (index - NumberOfTrajectories), rightHandTrajectoryPositions[i]);
            }
        }
        else
        {
            TrajectoriesList[0].positionCount = index;
            TrajectoriesList[1].positionCount = index;
            TrajectoriesList[2].positionCount = index;
            for (int i = 0; i < index; i++)
            {
                TrajectoriesList[0].SetPosition(i, headTrajectoryPositions[i]);
                TrajectoriesList[1].SetPosition(i, leftHandTrajectoryPositions[i]);
                TrajectoriesList[2].SetPosition(i, rightHandTrajectoryPositions[i]);
            }
        }

    }
}
