using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public GameObject destroyedObject;
    public float durability = 0;
    bool spawned = false;
    private void OnCollisionEnter(Collision collision)
    {
        Vector3 v = collision.impulse/Time.fixedDeltaTime;
        durability += v.magnitude;
        if (!spawned && durability > 200)
        {
            Destroy(gameObject);
            Transform _g = Instantiate(destroyedObject, transform.position, transform.rotation).transform;

            for (int i = 0; i < _g.childCount; i++)
            {
                Rigidbody childRb = _g.GetChild(i).GetComponent<Rigidbody>();
                childRb.AddForce(-collision.contacts[0].normal*v.magnitude, ForceMode.Force);
            }
            Manager.DerbisDestroy();
            Destroy(_g.gameObject, 150f);
            spawned = true;
        }
    }
}
