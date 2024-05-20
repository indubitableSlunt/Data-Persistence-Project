using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (string.IsNullOrWhiteSpace(txtName.text))
        {
            txtWarning.text = "Enter a name to start";
        }
        else
        {
            MenuManager.Instance.playerName = txtName.text;
            SceneManager.LoadScene("main");
        }
    }


}
