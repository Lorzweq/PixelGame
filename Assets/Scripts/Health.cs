using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHp = 30;
    public int currentHp;

    void Awake()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int dmg)
    {
        currentHp -= dmg;
        Debug.Log(gameObject.name + " took " + dmg);

        if (currentHp <= 0)
            Destroy(gameObject);
    }

}
