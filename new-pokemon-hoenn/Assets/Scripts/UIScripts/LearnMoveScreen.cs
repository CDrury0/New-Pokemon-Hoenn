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

    [SerializeField] private GameObject mainMenuGO;
    [SerializeField] private SummaryMovePlate moveToLearnDisplay;
    [SerializeField] private SummaryMovePlate[] movesKnownDisplays;
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private TextMeshProUGUI moveDetailsText;
    [SerializeField] private AudioClip learnMoveSound;

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

    public IEnumerator DoLearnMoveScreen(Pokemon p, GameObject move, System.Func<string, IEnumerator> messageOutput) {
        MoveData moveData = move.GetComponent<MoveData>();
        
        if(p.moves.Contains(null)){
            MoveReplaced = p.moves.IndexOf(null);
            LearnMove(p, move);
        }
        else{
            moveToLearnDisplay.SetMoveInfo(moveData.maxPP, moveData.maxPP, moveData, p);
            promptText.text = p.nickName + " is trying to learn " + moveData.moveName + ", but already knows 4 moves.";
            moveDetailsText.text = moveData.moveDescription;

            for (int i = 0; i < movesKnownDisplays.Length; i++){
                movesKnownDisplays[i].SetMoveInfo(p.movePP[i], p.moveMaxPP[i], p.moves[i].GetComponent<MoveData>(), p);
            }

            mainMenuGO.SetActive(true);
            yield return new WaitUntil(() => Proceed);
            mainMenuGO.SetActive(false);

            if (MoveReplaced < p.moves.Count){
                LearnMove(p, move);
            }
        }

        if(messageOutput != null){
            bool learnedMove = MoveReplaced < p.moves.Count;
            string message = p.nickName + (learnedMove ? " learned " + moveData.moveName + "!" : " did not learn " + moveData.moveName);
            if(learnedMove){
                AudioManager.Instance.PlaySoundEffect(learnMoveSound, -0.1f);
            }
            yield return StartCoroutine(messageOutput(message));
        }
    }

    /// <summary>
    /// Returns null if no valid move can be learned, otherwise returns the move
    /// </summary>
    public static GameObject GetValidMoveToLearn(Pokemon p){
        GameObject tentativeMove = p.pokemonDefault.learnedMoves[p.level];
        return tentativeMove != null && !p.moves.Contains(tentativeMove) ? tentativeMove : null;
    }
}
