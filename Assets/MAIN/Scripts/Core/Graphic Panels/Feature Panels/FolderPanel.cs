using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.WSA;

public class FolderPanel : MonoBehaviour
{
    public static FolderPanel Instance { get; private set; }

    [SerializeField] private CanvasGroup canvasGroup;    
    [SerializeField] private GameObject itemButtonPrefab;
    [SerializeField] private GridLayoutGroup buttonLayoutGroup;

    private CanvasGroupController cg = null;

    public bool isWaitingOnUserChoice { get; private set; } = false;

    private void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        cg = new CanvasGroupController(this, canvasGroup);
        cg.alpha = 0;
        cg.SetInteractableState(active: false);
    }

    public void Show()
    {
        isWaitingOnUserChoice = true;

        cg.Show();
        cg.SetInteractableState(active: true);
    }

    public void Hide()
    {
        isWaitingOnUserChoice = false;

        cg.Hide();
        cg.SetInteractableState(active: false);
    }

    public void CreateItemPrefab(Sprite item, string name)
    {
        GameObject newButtonObject = Instantiate(itemButtonPrefab, buttonLayoutGroup.transform);
        newButtonObject.SetActive(true);

        Button newButton = newButtonObject.GetComponent<Button>();

        newButton.onClick.RemoveAllListeners();
        newButton.onClick.AddListener(() => {
            ItemsPanel.Instance.Show(item);
            isWaitingOnUserChoice = true;
        });
        
        ItemsPanel.Instance.closeButton.onClick.AddListener(() => isWaitingOnUserChoice = false );

        TextMeshProUGUI text = newButton.GetComponentInChildren<TextMeshProUGUI>();
        text.text = name;
    }
}