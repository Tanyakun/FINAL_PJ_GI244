using System.Collections;
using UnityEngine;

public class BossSpecialSkill : MonoBehaviour
{
    public Transform platform; // ฉาก FloatingIsland หรือ Island
    public float floatHeight = 5f;
    public float tiltAngle = 25f;
    public float skillDuration = 5f;
    public float cooldown = 30f;
    public float floatSpeed = 2f;

    private Vector3 originalPos;
    private Quaternion originalRot;
    private bool isUsingSkill = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // ปิดฟิสิกส์ตอนเริ่ม
        originalPos = transform.position;
        originalRot = platform.rotation;
        StartCoroutine(SkillLoop());
    }

    IEnumerator SkillLoop()
    {
        while (true)
        {
            if (!isUsingSkill)
            {
                isUsingSkill = true;
                yield return StartCoroutine(UseSkill());
                yield return new WaitForSeconds(cooldown);
                isUsingSkill = false;
            }
            yield return null;
        }
    }

    IEnumerator UseSkill()
    {
        rb.isKinematic = true; // ปิดฟิสิกส์ตอนเริ่มลอย
        // ลอยขึ้น
        Vector3 targetPos = originalPos + Vector3.up * floatHeight;
        float t = 0;
        while (t < 1)
        {
            transform.position = Vector3.Lerp(originalPos, targetPos, t);
            t += Time.deltaTime * floatSpeed;
            yield return null;
        }

        // เอียงฉาก
        platform.rotation = Quaternion.Euler(tiltAngle, 0, 0);
        yield return new WaitForSeconds(skillDuration);

        // คืนฉาก
        platform.rotation = originalRot;

        // ตกลงมา
        t = 0;
        while (t < 1)
        {
            transform.position = Vector3.Lerp(targetPos, originalPos, t);
            t += Time.deltaTime * floatSpeed;
            yield return null;
        }
        
        rb.isKinematic = false; // เปิดฟิสิกส์กลับมา
    }
}
