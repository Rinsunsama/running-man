using UnityEngine;
using System.Collections;
public class Zhangai_Transform : MonoBehaviour
{
    int Direction = Random.Range(0, 2);
    float Speed = Random.Range(4, 10);
    float Rotation_X = Random.Range(0, 360);
    float Rotation_Y = Random.Range(0, 360);
    float Rotation_Z = Random.Range(0, 360);
    void Start()
    {
        Quaternion Rotation = Quaternion.Euler(Rotation_X, Rotation_Y, Rotation_Z);
        transform.rotation = Rotation;
    }
    void Update()
    {
        if (Direction == 0)
            transform.Translate(Vector3.left * Time.deltaTime * Speed);
        else
            transform.Translate(Vector3.right * Time.deltaTime * Speed);
    }
}

