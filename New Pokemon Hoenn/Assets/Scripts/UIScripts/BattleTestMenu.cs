using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        if(PlayerParty.Instance.playerParty == null){
            PlayerParty.Instance.playerParty = new Party();
        }
        enemyParty = new Party();
        if(levelValue == 0){
            levelValue = 50;
        }
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
            if(PlayerParty.Instance.playerParty.party[i] != null){
                playerPartyButtons[i].GetComponent<Image>().sprite = PlayerParty.Instance.playerParty.party[i].frontSprite;
            }
            if(enemyParty.party[i] != null){
                enemyPartyButtons[i].GetComponent<Image>().sprite = enemyParty.party[i].frontSprite;
            }
        }
    }

    private Party RandomParty(){
        Party newParty = new Party();
        for(int i = 0; i < newParty.party.Length; i++){
            newParty.party[i] = new Pokemon(ReferenceLib.Instance.pokemonDefaultLib[Random.Range(0, ReferenceLib.Instance.pokemonDefaultLib.Count)], levelValue, false);
            newParty.party[i].moves = new List<GameObject>(4);
            List<GameObject> movePool = new List<GameObject>(allMoves);
            for(int j = 0; j < newParty.party[i].moves.Capacity; j++){
                GameObject randomMove = movePool[Random.Range(0,movePool.Count)];
                movePool.Remove(randomMove);
                newParty.party[i].moves.Add(randomMove);
                newParty.party[i].FillPP(j);
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
        levelValue = System.Int32.Parse(input.text);
        Debug.Log(levelValue);
    }

    public void ChoosePlayerMon(int which){
        partySlot = which;
        isEnemyParty = false;
        List<Button> monSlotButtons = new List<Button>(playerPartyButtons);
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
        List<Button> monSlotButtons = new List<Button>(playerPartyButtons);
        monSlotButtons.AddRange(enemyPartyButtons);
        for (int i = 0; i < monSlotButtons.Count; i++){
            monSlotButtons[i].GetComponentInChildren<Text>().color = Color.white;
            if(i % 6 == which && i != which){
                monSlotButtons[i].GetComponentInChildren<Text>().color = Color.green;
            }
        }
    }

    public void ApplyMoves(){
        if(selectedMoves.Count > 0){
            Pokemon activeMon = isEnemyParty ? enemyParty.party[partySlot] : PlayerParty.Instance.playerParty.party[partySlot];
            for(int i = 0; i < selectedMoves.Count; i++){
                activeMon.moves[i] = selectedMoves[i];
                activeMon.FillPP(i);
            }
        }
    }

    public void CreateMon(int whichMon){
        Pokemon newMon = new Pokemon(ReferenceLib.Instance.pokemonDefaultLib[whichMon], levelValue, false);
        if(isEnemyParty){
            enemyParty.party[partySlot] = newMon;
            enemyPartyButtons[partySlot].GetComponent<Image>().sprite = newMon.frontSprite;
        }
        else{
            PlayerParty.Instance.playerParty.party[partySlot] = newMon;
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

    private void HealParty(Party party){
        foreach(Pokemon p in party.party){
            if(p != null){
                p.primaryStatus = PrimaryStatus.None;
                p.toxic = false;
                p.CurrentHealth = p.stats[0];
                for(int i = 0; i < p.moves.Count; i++){
                    p.movePP[i] = p.moveMaxPP[i];
                }
            }
        }
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
        CombatLib.Instance.combatSystem.StartBattle(enemyParty, battleTypeDropdown.value == 1);
    }
}
