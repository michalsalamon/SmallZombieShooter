using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private Vector2 minMaxVerticalAngle = new Vector2(-90, 90);

    public void LookVertical(float angle)
    {
        Vector3 currentAngle = transform.eulerAngles + Vector3.right * angle * Time.deltaTime;

        if (currentAngle.x >= minMaxVerticalAngle.y && currentAngle.x < 360 + minMaxVerticalAngle.x && angle > 0)
        {
            currentAngle.x = minMaxVerticalAngle.y;
        }
        else if (currentAngle.x <= 360 + minMaxVerticalAngle.x && currentAngle.x > minMaxVerticalAngle.y && angle < 0)
        {
            currentAngle.x = 360 + minMaxVerticalAngle.x;
        }

        transform.eulerAngles = currentAngle;
    }

    //private void Awake()
    //{
    //    player = FindObjectOfType<Player>().transform;
    //    cameraMaxDist = cameraOffset.magnitude;
    //}

    //private void Update()
    //{
    //    defaultCamPos = player.position + player.rotation * cameraOffset;

    //    Ray ray = new Ray(player.position, player.rotation * cameraOffset);
    //    RaycastHit hit;

    //    if (Physics.Raycast(ray, out hit, cameraMaxDist + 0.5f, cameraCheckMask))
    //    {
    //        float percent = (hit.point - player.position).magnitude / cameraMaxDist;
    //        transform.position = new Vector3(Mathf.Lerp(player.position.x, defaultCamPos.x, percent), transform.position.y, Mathf.Lerp(player.position.z, defaultCamPos.z, percent));
    //    }
    //    else
    //    {
    //        transform.position = defaultCamPos;
    //    }

    //    transform.LookAt(player.transform.position);
    //    transform.rotation = Quaternion.Euler(camRot.x, transform.eulerAngles.y, transform.eulerAngles.z);
    //}
}
