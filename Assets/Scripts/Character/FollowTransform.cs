using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public Transform target; // GameObject cần follow
    public float smoothTime = 0.3f; // Thời gian để di chuyển mượt
    private Vector3 velocity = Vector3.zero; // Vận tốc giúp SmoothDamp hoạt động

    void Update()
    {
        if (target == null) return; // Kiểm tra target có tồn tại không

        // Tính toán vị trí mới với độ mượt
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
    }
}
