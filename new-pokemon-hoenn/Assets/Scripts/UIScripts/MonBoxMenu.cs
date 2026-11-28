using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonBoxMenu : MonoBehaviour {
    public GameObject monButtonPrefab;
    public GameObject boxListWrap;
    public GameObject partyListWrap;
    public Image selectImage;
    public TextMeshProUGUI selectNickname;
    public TextMeshProUGUI selectID;
    public GameObject buttonWrap;
    public SummaryScreen summaryScreen;
    public GameObject summaryButton;
    public Button boxButton;
    public Button partyButton;

    void Awake() {
        foreach(var partyMon in PlayerParty.Party.members) {
            CreateMonButton(partyListWrap.transform, partyMon, true);
        }

        foreach(var boxMon in ReferenceLib.BoxMons) {
            CreateMonButton(boxListWrap.transform, boxMon, false);
        }
    }

    void CreateMonButton(Transform parent, Pokemon p, bool inParty) {
        var monBtn = Instantiate(monButtonPrefab, Vector3.zero, Quaternion.identity, parent);
        monBtn.transform.GetChild(0).GetComponent<Image>().sprite = p.boxSprite;
        monBtn.GetComponent<Button>().onClick.AddListener(() => {
            SetSelectedMon(p, inParty, monBtn);
        });
    }

    void SetSelectedMon(Pokemon p, bool inParty, GameObject toDestroy) {
        selectImage.sprite = p.frontSprite;
        selectImage.gameObject.SetActive(true);

        selectNickname.text = p.nickName;
        selectID.text = "ID: " + p.numberID.ToString();

        var summaryCaller = summaryButton.GetComponent<OverlayTransitionCaller>();
        summaryCaller.action.RemoveAllListeners();
        summaryCaller.action.AddListener(() => summaryScreen.ShowSummaryScreen(p));

        boxButton.onClick.RemoveAllListeners();
        boxButton.gameObject.SetActive(false);

        partyButton.onClick.RemoveAllListeners();
        partyButton.gameObject.SetActive(false);
        if (inParty) {
            boxButton.onClick.AddListener(() => {
                SendToBox(p);
                Destroy(toDestroy);
                CreateMonButton(boxListWrap.transform, p, false);
            });
            boxButton.interactable = PlayerParty.CanSendToBox(p);
            boxButton.gameObject.SetActive(true);
        } else {
            partyButton.onClick.AddListener(() => {
                SendToParty(p);
                Destroy(toDestroy);
                CreateMonButton(partyListWrap.transform, p, true);
            });
            partyButton.interactable = PlayerParty.CanSendToParty(p);
            partyButton.gameObject.SetActive(true);
        }
        buttonWrap.SetActive(true);
    }

    void SendToParty(Pokemon p) {
        PlayerParty.Party.members.Add(p);
        ReferenceLib.BoxMons.Remove(p);
    }

    void SendToBox(Pokemon p) {
        ReferenceLib.BoxMons.Add(p);
        PlayerParty.Party.members.Remove(p);
    }
}
