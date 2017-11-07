using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPCamera : MonoBehaviour {
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private float speedX = 360f;
    [SerializeField]
    private float speedY = 180f;
    [SerializeField]
    private float zoomSpeed = 2f;
    [SerializeField]
    private GameObject axes;

    private float minX = -40f;
    private float maxX = 80f;

    private float rotateX = 0f;
    private float rotateY = 0f;

    private float minDistance = 2f;
    private float maxDistance = 8f;
    private float distance;
    // Use this for initialization
    void Start () {
        //添加1.3f的向上偏移将摄像机的参考点调整至角色眼睛的位置
        offset =  transform.position - (target.position + new Vector3(0, 1.3f, 0));
        //计算出相机位置与参考点位置的向量相对于（0，0，-1）的欧拉角，并将其设置为相机的初始欧拉角
        Vector3 offsetEuler = Quaternion.FromToRotation(Vector3.back, offset).eulerAngles;
        rotateX = offsetEuler.x;
        rotateY = offsetEuler.y;
        distance = offset.magnitude;
        //Debug.Log("transform :" + transform.rotation.eulerAngles);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        if (target != null && Input.GetMouseButton(1))
        {
            rotateX -= Input.GetAxis("Mouse Y") * speedX * 0.1f;
            rotateY += Input.GetAxis("Mouse X") * speedY * 0.1f;
            rotateX = ClampAngle(rotateX, minX, maxX);
        }
        distance = distance - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        Quaternion rotation = Quaternion.Euler(rotateX, rotateY, 0);
        Quaternion axesRotation = Quaternion.Euler(0, 0, rotateY);
        //根据欧拉角和距离计算出新的位置
        Vector3 tarPos = target.position + new Vector3(0, 1.3f, 0);
        Vector3 pos = rotation * new Vector3(0, 0, -distance) + tarPos;
        //Debug.Log("target :" + rotation);
        //Debug.Log("target :" + rotation.eulerAngles);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
        //transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
        transform.rotation = rotation;
        transform.position = pos;
        axes.transform.rotation = axesRotation;
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
