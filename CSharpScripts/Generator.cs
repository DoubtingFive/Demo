using UnityEngine;

public class Generator : MonoBehaviour
{
    public Transform shoot;
    public GameObject obj;

    public void Shoot(Vector3 customDir = default(Vector3), Vector3 customPos = default(Vector3))
    {
        RaycastHit hit;
        if (customDir.magnitude != 0) {
            Physics.Raycast(customPos, customDir, out hit, 100f);
        } else Physics.Raycast(shoot.position, shoot.forward, out hit, 100f);
        if (hit.collider != null)
        {
            Instantiate(obj, hit.point, Quaternion.identity);
        }
        else
        {
            if (customDir.magnitude != 0)
            {
                Instantiate(obj, customPos+ customDir* 100, Quaternion.identity);
            }
            else Instantiate(obj, transform.position + shoot.forward * 100, Quaternion.identity);
        }
    }
}
