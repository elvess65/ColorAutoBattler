using UnityEngine;
using UnityEngine.UI;

public class UIWindow_CharacterSelection : MonoBehaviour
{
    public System.Action<int, int, int> OnSelectionFinished;

    public RectTransform CharactersParent;
    public RectTransform AttacksParent;
    public RectTransform ResistsParent;
    public RectTransform ButtonSelect;
    public RectTransform ButtonBack;
    public Text TextCharacter;
    public Text TextPlayer;

    public Button[] CharacterButtons;
    public Button[] AttckButtons;
    public Button[] ResistsButtons;

    public enum CharacterTypes { Melee, Range, Fly }

    private int m_SelectedCharacter;
    private int m_SelectedAttack;
    private int m_SelectedResist;


    public void SetSelectingPlayer(int id)
    {
        TextPlayer.text = string.Format("Player " + id + " is selecting");
        ToCharacterSelectionState();
    }


    void ToCharacterSelectionState()
    {
        m_SelectedCharacter = m_SelectedAttack = m_SelectedResist = -1;

        CharactersParent.gameObject.SetActive(true);
        AttacksParent.gameObject.SetActive(false);
        ResistsParent.gameObject.SetActive(false);
        ButtonSelect.gameObject.SetActive(false);
        ButtonBack.gameObject.SetActive(false);
        TextCharacter.gameObject.SetActive(false);

        for (int i = 0; i < CharacterButtons.Length; i++)
            CharacterButtons[i].image.color = Color.white;

        RestoreButtons();
    }

    void ToParamsSelectionState()
    {
        CharactersParent.gameObject.SetActive(false);

        AttacksParent.gameObject.SetActive(true);
        ResistsParent.gameObject.SetActive(true);
        ButtonBack.gameObject.SetActive(true);
        TextCharacter.gameObject.SetActive(true);  
    }

    void ToSelectionFinishedState()
    {
        ButtonSelect.gameObject.SetActive(true);
    }


    public void CharacterButtonPressHandler(int type)
    {
        m_SelectedCharacter = type;
        CharacterTypes t = (CharacterTypes)type;
        TextCharacter.text = t.ToString();

        ToParamsSelectionState();
    }

    public void AttckButtonPressHandler(int type)
    {
        m_SelectedAttack = type;

        RestoreNotSelectedButtons();

        AttckButtons[type].image.color = Color.green;
        ResistsButtons[type].image.color = GetReducedAlphaColor(ResistsButtons[type].image.color);
        ResistsButtons[type].enabled = false;

        if (SelectionFinished())
            ToSelectionFinishedState();
    }

    public void ResistButtonPressHandler(int type)
    {
        m_SelectedResist = type;
        RestoreNotSelectedButtons();

        ResistsButtons[type].image.color = Color.green;
        AttckButtons[type].image.color = GetReducedAlphaColor(AttckButtons[type].image.color);
        AttckButtons[type].enabled = false;

        if (SelectionFinished())
            ToSelectionFinishedState();
    }


    public void BackButtonPresshandler() => ToCharacterSelectionState();

    public void SelectButtonPressHandler() => OnSelectionFinished?.Invoke(m_SelectedCharacter, m_SelectedAttack, m_SelectedResist);


    bool SelectionFinished() => m_SelectedAttack >= 0 && m_SelectedResist >= 0;

    void RestoreButtons()
    {
        for (int i = 0; i < AttckButtons.Length; i++)
        {
            AttckButtons[i].image.color = Color.white;
            AttckButtons[i].enabled = true;
        }

        for (int i = 0; i < ResistsButtons.Length; i++)
        {
            ResistsButtons[i].image.color = Color.white;
            ResistsButtons[i].enabled = true;
        }
    }

    void RestoreNotSelectedButtons()
    {
        for (int i = 0; i < AttckButtons.Length; i++)
        {
            if (i != m_SelectedResist && i != m_SelectedAttack)
            {
                AttckButtons[i].image.color = Color.white;
                AttckButtons[i].enabled = true;
            }
        }

        for (int i = 0; i < ResistsButtons.Length; i++)
        {
            if (i != m_SelectedResist && i != m_SelectedAttack)
            {
                ResistsButtons[i].image.color = Color.white;
                ResistsButtons[i].enabled = true;
            }
        }
    }

    Color GetReducedAlphaColor(Color color)
    {
        color.a = 0.5f;
        return color;
    }
}
