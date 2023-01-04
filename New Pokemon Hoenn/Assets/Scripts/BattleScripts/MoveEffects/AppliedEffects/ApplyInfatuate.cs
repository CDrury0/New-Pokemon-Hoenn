public class ApplyInfatuate : ApplyIndividualEffect
{
    public override bool ImmuneToEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        return base.ImmuneToEffect(user, target, moveData);
    }
}