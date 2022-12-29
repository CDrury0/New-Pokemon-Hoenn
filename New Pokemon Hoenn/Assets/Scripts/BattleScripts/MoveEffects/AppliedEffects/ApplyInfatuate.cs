public class ApplyInfatuate : ApplyIndividualEffect
{
    public override string CheckMoveFail(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        return base.CheckMoveFail(user, target, moveData);
    }
}