using System;
using UnityEngine;

public class CommandMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject commandMenu;


    public event Action<CommandType>
        OnCommandSelected;


    public void Show()
    {
        commandMenu.SetActive(true);
    }


    public void Hide()
    {
        commandMenu.SetActive(false);
    }


    public void SelectFight()
    {
        SelectCommand(
            CommandType.Fight
        );
    }


    public void SelectMagic()
    {
        SelectCommand(
            CommandType.Magic
        );
    }


    public void SelectItem()
    {
          SelectCommand(
            CommandType.Item
        );
    }


    private void SelectCommand(
        CommandType command)
    {
        Hide();

        OnCommandSelected?
            .Invoke(command);
    }
}