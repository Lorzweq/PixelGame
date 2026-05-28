using UnityEngine;

public class CloseUI : MonoBehaviour
{
    [SerializeField] private GameObject uiRoot; // esim InventoryCanvas tai WindowPanel
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            Toggle();
    }


    public void Close()
    {
        if (uiRoot != null)
            uiRoot.SetActive(false);
    }

    public void Open()
    {
        if (uiRoot != null)
            uiRoot.SetActive(true);
    }

    public void Toggle()
    {
        if (uiRoot != null)
            uiRoot.SetActive(!uiRoot.activeSelf);
    }
}
