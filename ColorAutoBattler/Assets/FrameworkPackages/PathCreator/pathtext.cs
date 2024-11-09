using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathtext : MonoBehaviour
{
    public PathCreator PathCr;
    public EndOfPathInstruction end;

    public float mltp = 1;

    float t;
    float tt = 35;

    // Start is called before the first frame update
    void Start()
    {
        end = EndOfPathInstruction.Stop;

        transform.position = PathCr.path.GetPoint(0.5f);
    }

    private void Update()
    {
        t += Time.deltaTime * mltp;

        transform.position = PathCr.path.GetPoint(t / tt, end);
        transform.rotation = PathCr.path.GetRotation(t / tt, end);
        Quaternion rot = PathCr.path.GetRotation(t / tt, end);

        //Debug.Log(rot.eulerAngles);
        //transform.localEulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y - 90, rot.eulerAngles.z);
        //transform.rotation = rot * Quaternion.Euler(0, -90, 0);
    }
}
