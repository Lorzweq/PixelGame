using UnityEngine;

public class HpUIController : MonoBehaviour
{
    [Header("Flasks")]
    [SerializeField] private HpFlaskUI normalFlask;
    [SerializeField] private HpFlaskUI demonFlask;

    [Header("HP Values")]
    [SerializeField] private int normalMaxHp = 100;
    [SerializeField] private int demonMaxHp = 70;

    private int currentHp;
    private bool isDemon;

    private void Start()
    {
        currentHp = normalMaxHp;

        normalFlask.SetVisible(true);
        demonFlask.SetVisible(false);

        normalFlask.SetHp(currentHp, normalMaxHp);
    }

    public void SetDemonForm(bool demon)
    {
        isDemon = demon;

        if (isDemon)
        {
            // Switch to demon HP
            float pct = (float)currentHp / normalMaxHp;
            currentHp = Mathf.Clamp(Mathf.RoundToInt(pct * demonMaxHp), 1, demonMaxHp);

            normalFlask.SetVisible(false);
            demonFlask.SetVisible(true);
            demonFlask.SetHp(currentHp, demonMaxHp);
        }
        else
        {
            // Switch back to normal HP
            float pct = (float)currentHp / demonMaxHp;
            currentHp = Mathf.Clamp(Mathf.RoundToInt(pct * normalMaxHp), 1, normalMaxHp);

            demonFlask.SetVisible(false);
            normalFlask.SetVisible(true);
            normalFlask.SetHp(currentHp, normalMaxHp);
        }
    }

    public void ApplyDamage(int amount)
    {
        currentHp = Mathf.Max(currentHp - amount, 0);

        if (isDemon)
            demonFlask.SetHp(currentHp, demonMaxHp);
        else
            normalFlask.SetHp(currentHp, normalMaxHp);
    }

    public void Heal(int amount)
    {
        int max = isDemon ? demonMaxHp : normalMaxHp;
        currentHp = Mathf.Min(currentHp + amount, max);

        if (isDemon)
            demonFlask.SetHp(currentHp, demonMaxHp);
        else
            normalFlask.SetHp(currentHp, normalMaxHp);
    }
}
