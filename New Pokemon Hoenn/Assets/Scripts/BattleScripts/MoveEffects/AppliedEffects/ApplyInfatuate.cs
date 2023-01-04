public class ApplyInfatuate : ApplyIndividualEffect
{
    public override bool ImmuneToEffect(BattleTarget user, BattleTarget target, MoveData moveData)
    {
        if(target.pokemon.gender == Gender.None || target.pokemon.gender == user.pokemon.gender){
            return true;
        }
        //oblivious?
        return false;
    }
}