﻿using Paint.Character.Weapon;
using Paint.Characters;
using Paint.Match;
using UnityEngine;
using UnityEngine.UI;

public class UIWindow_CharacterSelection : MonoBehaviour
{
    public System.Action<CharacterTypes, WeaponTypes, WeaponTypes> OnSelectionFinished;

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

    public Text Melee_Attack;
    public Text Melee_HP;
    public Text Range_Attack;
    public Text Range_HP;
    public Text Fly_Attack;
    public Text Fly_HP;

    private CharacterTypes m_SelectedCharacter;
    private WeaponTypes m_SelectedAttack;
    private WeaponTypes m_SelectedResist;

    private Color32 m_SELECTED_COLOR = new Color32(158, 255, 170, 200);

    public void SetSelectingPlayer(int id, Color color)
    {
        Melee_Attack.text = $"{MatchManager.MELEE_DAMAGE}";
        Melee_HP.text = $"{MatchManager.MELEE_HP}";
        Range_Attack.text = $"{MatchManager.RANGE_DAMAGE}";
        Range_HP.text = $"{MatchManager.RANGE_HP}";
        Fly_Attack.text = $"{MatchManager.FLY_DAMAGE}";
        Fly_HP.text = $"{MatchManager.FLY_HP}";

        TextPlayer.text = string.Format("Player {0}", id);
        TextPlayer.color = color;
        ToCharacterSelectionState();
    }


    void ToCharacterSelectionState()
    {
        m_SelectedCharacter = CharacterTypes.Max;
        m_SelectedAttack = WeaponTypes.Max;
        m_SelectedResist = WeaponTypes.Max;

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
        m_SelectedCharacter = (CharacterTypes)type;
        TextCharacter.text = m_SelectedCharacter.ToString();

        ToParamsSelectionState();
    }

    public void AttckButtonPressHandler(int type)
    {
        m_SelectedAttack = (WeaponTypes)type;

        RestoreNotSelectedButtons();

        int index = (int)m_SelectedAttack;
        AttckButtons[index].image.color = m_SELECTED_COLOR;
        ResistsButtons[index].image.color = GetReducedAlphaColor(ResistsButtons[index].image.color);
        ResistsButtons[index].enabled = false;

        if (SelectionFinished())
            ToSelectionFinishedState();
    }

    public void ResistButtonPressHandler(int type)
    {
        m_SelectedResist = (WeaponTypes)type;
        RestoreNotSelectedButtons();

        int index = (int)m_SelectedResist;
        ResistsButtons[index].image.color = m_SELECTED_COLOR;
        AttckButtons[index].image.color = GetReducedAlphaColor(AttckButtons[index].image.color);
        AttckButtons[index].enabled = false;

        if (SelectionFinished())
            ToSelectionFinishedState();
    }


    public void BackButtonPresshandler() => ToCharacterSelectionState();

    public void SelectButtonPressHandler() => OnSelectionFinished?.Invoke(m_SelectedCharacter, m_SelectedAttack, m_SelectedResist);


    bool SelectionFinished() => m_SelectedAttack != WeaponTypes.Max && m_SelectedResist != WeaponTypes.Max;

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
            if (i != (int)m_SelectedResist && i != (int)m_SelectedAttack)
            {
                AttckButtons[i].image.color = Color.white;
                AttckButtons[i].enabled = true;
            }
        }

        for (int i = 0; i < ResistsButtons.Length; i++)
        {
            if (i != (int)m_SelectedResist && i != (int)m_SelectedAttack)
            {
                ResistsButtons[i].image.color = Color.white;
                ResistsButtons[i].enabled = true;
            }
        }
    }

    Color GetReducedAlphaColor(Color color)
    {
        color.a = 0.1f;
        return color;
    }
}
