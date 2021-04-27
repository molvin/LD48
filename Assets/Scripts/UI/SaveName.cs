using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveName : MonoBehaviour
{
    public const string NameKey = "player_name_xx_1337";

    public UnityEngine.UI.InputField NameField;


    private void Awake()
    {
        if (PlayerPrefs.HasKey(NameKey))
        {
            NameField.gameObject.SetActive(false);
        }
    }

    public static string GetPlayerName() => PlayerPrefs.GetString(NameKey, "Player");

    public void Save()
    {
        if (!PlayerPrefs.HasKey(NameKey))
        {
            string Name = NameField.text.Trim();
            if (Name.Length > 1)
            {
                PlayerPrefs.SetString(NameKey, Name);
                GameStateManager.Instance.LeaveMenu = true;
            }
        }
        else
        {
            GameStateManager.Instance.LeaveMenu = true;
        }
    }
}
