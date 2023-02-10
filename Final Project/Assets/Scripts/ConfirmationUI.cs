using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//public class ConfirmationUI : MonoBehaviour
//{
//    private TextMeshProUGUI textMeshPro;
//    private Button yesBtn;
//    private Button noBtn;

//    private void Awake()
//    {
//        textMeshPro = transform.find("Text").GetComponent<TextMeshProUGUI>();
//        yesBtn = transform.find("YesBtn").GetComponent<TextMeshProUGUI>();
//        noBtn = transform.find("NoBtn").GetComponent<TextMeshProUGUI>();

//        ShowQuestion("Do you really want to quit the game?", () =>
//        {
//            Debug.Log("Yes");
//        }, () =>
//        {
//            Debug.Log("No");
//        });
//    }

//    public void ShowQuestion(string questionText, Action yesAction, Action noAction)
//    {
//        textMeshPro.text = questionText;
//        yesBtn.onClick.AddListener(new UnityEngine.Events.UnityAction(yesAction));
//        noBtn.onClick.AddListener(new UnityEngine.Events.UnityAction(noAction));

//    }

//}
