using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LearnMoveScreen : MonoBehaviour
{
    public int MoveReplaced { get; private set; }
    private bool _proceed;
    private bool Proceed {
        get {
            if(_proceed){
                _proceed = false;
                return true;
            }
            return _proceed;
        }
        set {
            _proceed = value;
        }
    }

    [SerializeField] private GameObject background;
    [SerializeField] private SummaryMovePlate moveToLearnDisplay;
    [SerializeField] private SummaryMovePlate[] movesKnownDisplays;
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private TextMeshProUGUI moveDetailsText;

    private void SetProceed() {
        Proceed = true;
    }

    ///<summary>Setting the argument to 4 (or any value larger than the largest index of pokemon.moves[]) indicates the move was not learned</summary>
    public void SetLearnedMove(int moveReplaced) {
        this.MoveReplaced = moveReplaced;
        SetProceed();
    }

    private void LearnMove(Pokemon p, GameObject move) {
        MoveData moveData = move.GetComponent<MoveData>();
        p.moves[MoveReplaced] = move;
        p.movePP[MoveReplaced] = moveData.maxPP;
        p.moveMaxPP[MoveReplaced] = moveData.maxPP;
    }

    public IEnumerator DoLearnMoveScreen(Pokemon p, GameObject move) {
        if(p.moves.Contains(null)){
            MoveReplaced = p.moves.IndexOf(null);
            LearnMove(p, move);
            yield break;
        }

        MoveData moveData = move.GetComponent<MoveData>();
        moveToLearnDisplay.SetMoveInfo(moveData.maxPP, moveData.maxPP, moveData);
        promptText.text = p.nickName + " is trying to learn " + moveData.moveName + ", but already knows 4 moves.";
        moveDetailsText.text = "Select a move to learn more about it.";

        for (int i = 0; i < movesKnownDisplays.Length; i++){
            movesKnownDisplays[i].SetMoveInfo(p.movePP[i], p.moveMaxPP[i], p.moves[i].GetComponent<MoveData>());
        }

        background.SetActive(true);
        yield return new WaitUntil(() => Proceed);
        background.SetActive(false);
        
        if(MoveReplaced < p.moves.Count){
            LearnMove(p, move);
        }
    }
}