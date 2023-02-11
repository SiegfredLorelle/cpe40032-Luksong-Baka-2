using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTopRight : MonoBehaviour
{
    public MoveLeft moveLeftScript;

    // Start is called before the first frame update
    void Start()
    {
        moveLeftScript = GetComponent<MoveLeft>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveLeftScript.isThrown)
        { 
            transform.Translate(new Vector3(1, 1.25f) * 20.0f * Time.deltaTime, relativeTo: Space.World);
            transform.Rotate(new Vector3(0, 0.5f, 1) * 500.0f * Time.deltaTime, relativeTo: Space.World);
        }
    }
}
