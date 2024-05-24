using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private UIDocument characterUIDocument;
    [SerializeField] private UIDocument messagesUIDocument;

    private VisualElement characterLayout;
    private ProgressBar abyssEnergyBar;
    private ProgressBar abyssKnowledgeBar;

    public static bool HideCharacterUi = false;

    private void OnEnable()
    {
        var root = characterUIDocument.rootVisualElement;
        abyssEnergyBar = root.Q<ProgressBar>("AeBar");
        abyssKnowledgeBar = root.Q<ProgressBar>("AkBar");
        characterLayout = root.Q<VisualElement>("CharacterLayout");

        PlayerCharacter.AbyssEnergyPercentChanged += UpdateAeBar;
        PlayerCharacter.AbyssKnowledgePercentChanged += UpdateAkBar;
        // subscribe to events after 2 seconds by use of coroutines, not Invoke()

        // StartCoroutine(SubscribeToEvents());
    }

    private void Start()
    {
        UIWizard.instance.MessageShowed += HideCharacterUI;
        UIWizard.instance.MessageHide += ShowCharacterUI;
    }

    private IEnumerator SubscribeToEvents()
    {
        yield return new WaitForSeconds(0.5f);
        UIWizard.instance.MessageShowed += HideCharacterUI;
        UIWizard.instance.MessageHide += ShowCharacterUI;
    }

    private void HideCharacterUI()
    {
        // characterLayout.style.display = DisplayStyle.None;
        abyssEnergyBar.style.display = DisplayStyle.None;
        abyssKnowledgeBar.style.display = DisplayStyle.None;
    }

    private void ShowCharacterUI()
    {
        // characterLayout.style.display = DisplayStyle.Flex;
        abyssEnergyBar.style.display = DisplayStyle.Flex;
        abyssKnowledgeBar.style.display = DisplayStyle.Flex;
    }

    private void UpdateAeBar(int energy)
    {
        abyssEnergyBar.value = energy;
        // abyssEnergyBar.title = energy.ToString();
    }

    private void UpdateAkBar(int knowledge)
    {
        abyssKnowledgeBar.value = knowledge;
        // abyssKnowledgeBar.title = knowledge.ToString();
    }

    private void OnDisable()
    {
        PlayerCharacter.AbyssEnergyChanged -= UpdateAeBar;
        PlayerCharacter.AbyssKnowledgeChanged -= UpdateAkBar;
    }
}
