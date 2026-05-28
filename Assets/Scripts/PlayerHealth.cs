using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private HpFlaskUI normalFlaskUI;
    [SerializeField] private HpFlaskUI demonFlaskUI; // optional (can be null)

    [Header("Normal HP")]
    [SerializeField] private int normalMaxHp = 100;

    [Header("Demon HP")]
    [SerializeField] private int demonMaxHp = 70;

    [Header("Demon Damage Debt")]
    [SerializeField] private int debtMultiplier = 2;     // 2x damage when reverting
    [SerializeField] private int maxDebtCap = 0;         // 0 = no cap, else cap debt

    public int Hp { get; private set; }
    public int MaxHp => isDemon ? demonMaxHp : normalMaxHp;

    private bool isDemon = false;
    private int storedDebt = 0;
    private bool applyingDebt = false;

    private void Awake()
    {
        Hp = normalMaxHp;

        // start in normal mode
        SetDemonForm(false, applyDebtOnExit: false);
        UpdateFlaskUI();
    }

    public void TakeDamage(int dmg)
    {
        if (Hp <= 0) return;
        if (dmg <= 0) return;

        // In demon form: take NO damage, store debt instead
        if (isDemon && !applyingDebt)
        {
            int add = dmg * debtMultiplier;
            storedDebt += add;

            if (maxDebtCap > 0)
                storedDebt = Mathf.Min(storedDebt, maxDebtCap);

            // Optional debug
            Debug.Log($"Demon ignored {dmg} dmg. Stored debt now: {storedDebt}");
            return;
        }

        Hp -= dmg;
        if (Hp < 0) Hp = 0;

        Debug.Log($"Player took {dmg}. HP: {Hp}/{MaxHp}");

        UpdateFlaskUI();

        if (Hp == 0)
            Debug.Log("Player died");
    }

    public void Heal(int amount)
    {
        if (Hp <= 0) return;
        if (amount <= 0) return;

        Hp += amount;
        if (Hp > MaxHp) Hp = MaxHp;

        UpdateFlaskUI();
    }

    /// <summary>
    /// Call this from your demon toggle script.
    /// </summary>
    public void SetDemonForm(bool demon, bool applyDebtOnExit = true)
    {
        if (isDemon == demon) return;

        // Keep same HP percentage when switching forms (recommended)
        int oldMax = MaxHp;
        float pct = oldMax > 0 ? (float)Hp / oldMax : 1f;

        isDemon = demon;

        // swap UI visibility
        if (normalFlaskUI != null) normalFlaskUI.SetVisible(!isDemon);
        if (demonFlaskUI != null) demonFlaskUI.SetVisible(isDemon);

        // convert HP to new max (same %)
        int newMax = MaxHp;
        Hp = Mathf.Clamp(Mathf.RoundToInt(pct * newMax), 1, newMax);

        UpdateFlaskUI();

        // When returning to normal: apply stored debt
        if (!isDemon && applyDebtOnExit && storedDebt > 0)
            ApplyDebt();
    }

    private void ApplyDebt()
    {
        applyingDebt = true;

        int debt = storedDebt;
        storedDebt = 0;

        Debug.Log($"Applying stored demon debt: {debt}");

        // Apply as real damage now (normal form)
        TakeDamage(debt);

        applyingDebt = false;
    }

    private void UpdateFlaskUI()
    {
        // Update the currently active flask with current hp/maxhp
        if (isDemon)
        {
            if (demonFlaskUI != null)
                demonFlaskUI.SetHp(Hp, MaxHp);
        }
        else
        {
            if (normalFlaskUI != null)
                normalFlaskUI.SetHp(Hp, MaxHp);
        }
    }
}
