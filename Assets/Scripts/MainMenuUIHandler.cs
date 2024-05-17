using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuUIHandler : MonoBehaviour
{
    [SerializeField] TMP_InputField txtName;
    [SerializeField] TMP_Text txtWarning;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void btnStartClicked()
    {
        string playerName = txtName.text;
        if (string.IsNullOrWhiteSpace(playerName))
        {
            txtWarning.text = "Enter a name to start";
        }
        else
        {
            MainManager.Instance.PlayerName = playerName;
        }
    }


}
