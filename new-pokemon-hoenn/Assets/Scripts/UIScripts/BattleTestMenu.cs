using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class BattleTestMenu : MonoBehaviour
{
    public GameObject layoutElementPrefab;
    public GameObject menuButtonPrefab;
    public GameObject monListParent;
    public GameObject moveListParent;
    public Text moveDescriptionText;
    public TMP_Dropdown battleTypeDropdown;
    public GameObject[] allMoves;
    [SerializeField] private Button[] playerPartyButtons;
    [SerializeField] private Button[] enemyPartyButtons;
    [SerializeField] private Text[] selectedMoveTexts;
    private Party enemyParty;
    private List<GameObject> selectedMoves;
    private int levelValue;
    private int partySlot;
    private bool isEnemyParty;

    void Awake(){
        ClearSelectedMoves();
        PopulateMoveList();
        PopulatePokemonList();
        PlayerParty.Instance.playerParty ??= new Party();
        PlayerParty.Party.members = new(){null, null, null, null, null, null};
        enemyParty = new Party(){
            members = new(){null, null, null, null, null, null},
        };
        if(levelValue == 0)
            levelValue = 50;
    }

    public void ClearPlayerParty(){
        ClearParty(true);
    }

    public void ClearEnemyParty(){
        ClearParty(false);
    }

    public void RandomPlayerParty(){
        PlayerParty.Instance.playerParty = RandomParty();
        UpdateButtonSprites();
    }

    public void RandomEnemyParty(){
        enemyParty = RandomParty();
        UpdateButtonSprites();
    }

    private void UpdateButtonSprites(){
        for(int i = 0; i < playerPartyButtons.Length; i++){
            var ally = PlayerParty.Party.members.ElementAtOrDefault(i);
            if(ally is not null)
                playerPartyButtons[i].GetComponent<Image>().sprite = ally.frontSprite;

            var enemy = enemyParty.members.ElementAtOrDefault(i);
            if(enemy is not null)
                enemyPartyButtons[i].GetComponent<Image>().sprite = enemy.frontSprite;
        }
    }

    private Party RandomParty(){
        Party newParty = new();
        for(int i = 0; i < newParty.members.Capacity; i++){
            var newMon = new Pokemon(ReferenceLib.Instance.pokemonDefaultLib[Random.Range(0, ReferenceLib.Instance.pokemonDefaultLib.Count)], levelValue, false);
            newParty.members.Add(newMon);
            newMon.moves = new List<GameObject>(4);
            List<GameObject> movePool = new(allMoves);
            for(int j = 0; j < newMon.moves.Capacity; j++){
                GameObject randomMove = movePool[Random.Range(0,movePool.Count)];
                movePool.Remove(randomMove);
                newMon.moves.Add(randomMove);
                newMon.FillPP(j);
            }
        }
        return newParty;
    }

    private void ClearParty(bool playerParty){
        Button[] b = playerParty ? playerPartyButtons : enemyPartyButtons;
        if(playerParty){
            PlayerParty.Instance.playerParty = new Party();
        }
        else{
            enemyParty = new Party();
        }
        foreach(Button button in b){
            button.GetComponent<Image>().sprite = null;
        }
    }

    public void ChangeLevel(InputField input){
        levelValue = int.Parse(input.text);
        Debug.Log(levelValue);
    }

    public void ChoosePlayerMon(int which){
        partySlot = which;
        isEnemyParty = false;
        List<Button> monSlotButtons = new(playerPartyButtons);
        monSlotButtons.AddRange(enemyPartyButtons);
        for (int i = 0; i < monSlotButtons.Count; i++){
            monSlotButtons[i].GetComponentInChildren<Text>().color = Color.white;
            if(i == which){
                monSlotButtons[i].GetComponentInChildren<Text>().color = Color.green;
            }
        }
    }

    public void ChooseEnemyMon(int which){
        partySlot = which;
        isEnemyParty = true;
        List<Button> monSlotButtons = new(playerPartyButtons);
        monSlotButtons.AddRange(enemyPartyButtons);
        for (int i = 0; i < monSlotButtons.Count; i++){
            monSlotButtons[i].GetComponentInChildren<Text>().color = Color.white;
            if(i % 6 == which && i != which){
                monSlotButtons[i].GetComponentInChildren<Text>().color = Color.green;
            }
        }
    }

    public void ApplyMoves(){
        if(selectedMoves.Count <= 0)
            return;

        var activeList = isEnemyParty ? enemyParty.members : PlayerParty.Party.members;
        Pokemon activeMon = activeList.ElementAtOrDefault(partySlot);
        if(activeMon is null)
            return;

        for(int i = 0; i < selectedMoves.Count; i++){
            activeMon.moves[i] = selectedMoves[i];
            activeMon.FillPP(i);
        }
    }

    public void CreateMon(int whichMon){
        Pokemon newMon = new(ReferenceLib.Instance.pokemonDefaultLib[whichMon], levelValue, false);
        if(isEnemyParty){
            enemyParty.members.RemoveAt(whichMon);
            enemyParty.members.Insert(whichMon, newMon);

            enemyParty.members[partySlot] = newMon;
            enemyPartyButtons[partySlot].GetComponent<Image>().sprite = newMon.frontSprite;
        }
        else {
            PlayerParty.Instance.playerParty.members[partySlot] = newMon;
            playerPartyButtons[partySlot].GetComponent<Image>().sprite = newMon.frontSprite;
        }
    }

    private void PopulatePokemonList(){
        for(int i = 0; i < ReferenceLib.Instance.pokemonDefaultLib.Count; i++){
            GameObject g = Instantiate(layoutElementPrefab, monListParent.transform);
            g.GetComponent<Image>().sprite = ReferenceLib.Instance.pokemonDefaultLib[i].normalFront;
            g.GetComponentInChildren<Text>().text = ReferenceLib.Instance.pokemonDefaultLib[i].pokemonName;
            int blahblah = i;
            g.GetComponent<Button>().onClick.AddListener(() => CreateMon(blahblah));
        }
    }

    private void PopulateMoveList(){
        for(int i = 0; i < allMoves.Length; i++){
            GameObject g = Instantiate(menuButtonPrefab, moveListParent.transform);
            MoveData moveData = allMoves[i].GetComponent<MoveData>();
            g.GetComponentInChildren<TextMeshProUGUI>().text = moveData.moveName;
            MouseOverMoveButton mouseOverComponent = g.AddComponent<MouseOverMoveButton>();
            mouseOverComponent.moveData = moveData;
            mouseOverComponent.moveDescriptionText = moveDescriptionText;
            int blahblah = i;
            g.GetComponent<Button>().onClick.AddListener(() => SelectMove(blahblah));
        }
    }

    public void ClearSelectedMoves(){
        selectedMoves = new List<GameObject>(4);
        UpdateSelectedMoveText();
    }

    public void HealAll(){
        HealParty(PlayerParty.Instance.playerParty);
        HealParty(enemyParty);
    }

    private void HealParty(Party party) {
        foreach(Pokemon p in party.members)
            p.HealComplete();
    }

    private void UpdateSelectedMoveText(){
        for(int i = 0; i < selectedMoves.Capacity; i++){
            selectedMoveTexts[i].text = i < selectedMoves.Count ? selectedMoves[i].GetComponent<MoveData>().moveName : " --- ";
        }
    }

    public void SelectMove(int whichMove){
        GameObject representedMove = allMoves[whichMove];
        if(selectedMoves.Count != selectedMoves.Capacity && !selectedMoves.Contains(representedMove)){
            selectedMoves.Add(representedMove);
            UpdateSelectedMoveText();
        }
    }

    public void StartBattle(){
        gameObject.SetActive(false);
        CombatLib.CombatSystem.StartBattle(enemyParty, battleTypeDropdown.value == 1);
    }
}
