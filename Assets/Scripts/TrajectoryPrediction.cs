using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//modified to be 3D from original 2D code written by Ricky Willis of Space Ape Games on July 5, 2016. Accessed on 4/2/2022 at https://tech.spaceapegames.com/2016/07/05/trajectory-prediction-with-unity-physics/
public class TrajectoryPrediction
{
    public static Vector3[] Plot(Rigidbody rigidbody, Vector3 pos, Vector3 velocity, int steps)
    {
        Vector3[] results = new Vector3[steps];

        float timestep = Time.fixedDeltaTime;
        Vector3 gravityAccel = Physics.gravity * timestep * timestep;
        float drag = 1f - timestep * rigidbody.drag;
        Vector3 moveStep = velocity * timestep;

        for (int i = 0; i < steps; ++i)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;
        }

        return results;
    }
}
