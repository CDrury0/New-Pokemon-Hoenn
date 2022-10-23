using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatLib
{
    public enum Type { None, Normal, Fire, Water, Electric, Grass, Ice, Fighting, Poison, Ground, Flying, Psychic, Bug, Rock, Ghost, Dragon, Dark, Steel, Fairy  }
    public enum Matchup {Immunity, Weakness, Resistance}
    public enum EggGroup { Monster, Amphibious, Bug, Avian, Field, Fairy, Plantoid, Humanoid, Aquatic, Inorganic, Amorphous, Piscine, Dragon, None }

    public enum Ability
    {   Stench,
        Drizzle,
        Speed_Boost,
        Battle_Armor,
        Sturdy,
        Damp,
        Sand_Veil,
        Static,
        Volt_Absorb,
        Water_Absorb,
        Oblivious,
        Cloud_Nine,
        Compound_Eyes,
        Insomnia,
        Color_Change,
        Poison_Immunity,
        Flash_Fire,
        Shield_Dust,
        Own_Tempo,
        Suction_Cups,
        Intimidate,
        Shadow_Tag,
        Rough_Skin,
        Wonder_Guard,
        Levitate,
        Effect_Spore,
        Synchronize,
        Clear_Body,
        Natural_Cure,
        Lightning_Rod,
        Serene_Grace,
        Swift_Swim,
        Chlorophyll,
        Illuminate,
        Trace,
        Huge_Power,
        Poison_Point,
        Inner_Focus,
        Magma_Armor,
        Water_Veil,
        Magnet_Pull,
        Soundproof,
        Rain_Dish,
        Sand_Stream,
        Pressure,
        Thick_Fat,
        Early_Bird,
        Flame_Body,
        Runaway,
        Keen_Eye,
        Hyper_Cutter,
        Pickup,
        Truant,
        Hustle,
        Cute_Charm,
        Plus,
        Minus,
        Forecast,
        Sticky_Hold,
        Shed_Skin,
        Guts,
        Marvel_Scale,
        Liquid_Ooze,
        Overgrow,
        Blaze,
        Torrent,
        Swarm,
        Rock_Head,
        Drought,
        Arena_Trap,
        Vital_Spirit,
        White_Smoke,
        Pure_Power,
        Shell_Armor,
        Air_Lock,
        Competitive,
        No_Guard,
        Sniper,
        Quick_Feet,
        Poison_Heal,
        Solid_Rock,
        Tangled_Feet,
        Anticipation,
        Solar_Power,
        Super_Luck,
        Ice_Body,
        Hydration,
        Snow_Cloak,
        Regenerator,
        None 
    }

    public static int CalculateExperienceAtLevel(GrowthRate growthRate, int level)
    {
        switch (growthRate)
        {
            case GrowthRate.Fast:
                return (int)(Mathf.Pow(level, 3) * 4 / 5);
            case GrowthRate.Medium:
                return (int)Mathf.Pow(level, 3);
            case GrowthRate.Slow:
                return (int)(Mathf.Pow(level, 3) * 5 / 4);
            default:
                return 0;
        }
    }

    public static string GetAbilityDescription(Ability ability)
    {
        switch (ability)
        {
            case Ability.Air_Lock:
                return "Prevents all weather effects while in battle";
            case Ability.Anticipation:
                return "Senses whether or not the foe knows a highly dangerous move";
            case Ability.Arena_Trap:
                return "Prevents Ground-type or otherwise grounded Pokémon from escaping or switching out";
            case Ability.Battle_Armor:
                return "Ignores bonus damage from critical hits";
            case Ability.Blaze:
                return "Boosts the power of Fire-type moves by 50% while at low HP";
            case Ability.Chlorophyll:
                return "Speed is boosted by 50% during intense sunlight";
            case Ability.Clear_Body:
                return "Prevents other Pokémon from lowering this Pokémon's stats";
            case Ability.Cloud_Nine:
                return "Ignores the effects of the weather";
            case Ability.Color_Change:
                return "The Pokémon inherits the type of the move it selected";
            case Ability.Competitive:
                return "Boosts special attack by 2 stages when another Pokémon lowers one of its stats";
            case Ability.Compound_Eyes:
                return "Boosts the accuracy of moves used by this Pokémon by 30%, and increases the chance that a wild Pokémon will be holding an item";
            case Ability.Cute_Charm:
                return "Has a 30% chance to infatuate any Pokémon each time a move makes contact with it, if it is the opposite gender";
            case Ability.Damp:
                return "Prevents any Pokémon from self-destructing while in battle";
            case Ability.Drizzle:
                return "Causes rain to fall while in battle";
            case Ability.Drought:
                return "Causes intense sunlight while in battle";
            case Ability.Early_Bird:
                return "Wakes from sleep more quickly";
            case Ability.Effect_Spore:
                return "Has a 30% chance to apply a status condition to the attacker each time a move makes contact with this Pokémon";
            case Ability.Flame_Body:
                return "Has a 30% chance to burn the attacker each time a move makes contact with this Pokémon";
            case Ability.Flash_Fire:
                return "This Pokémon restores 20% of its max HP when hit by a Fire-type move instead of taking damage";
            case Ability.Forecast:
                return "Changes form and type to match the weather";
            case Ability.Guts:
                return "Boosts the power of physical moves by 50% when affected by a status condition";
            case Ability.Huge_Power:
                return "Doubles the effective attack stat of this Pokémon";
            case Ability.Hustle:
                return "Boosts the damage of physical moves by 50%, but lowers accuracy by 20%";
            case Ability.Hydration:
                return "Heals major status conditions at the end of the turn if it is raining";
            case Ability.Hyper_Cutter:
                return "Prevents attack from being lowered";
            case Ability.Ice_Body:
                return "Restores HP slightly each turn while in a hail storm";
            case Ability.Illuminate:
                return "Attracts wild Pokémon when leading the party by shining brightly";
            case Ability.Inner_Focus:
                return "Prevents this Pokémon from flinching";
            case Ability.Insomnia:
                return "Prevents this Pokémon from falling asleep";
            case Ability.Intimidate:
                return "When entering the battle, all opponents' attack stat is reduced by 1 stage, and helps keep weak wild Pokémon away if it leads the party";
            case Ability.Keen_Eye:
                return "Prevents accuracy from being lowered";
            case Ability.Levitate:
                return "This Pokémon is immune to Ground-type moves, as well as any effects on the ground";
            case Ability.Lightning_Rod:
                return "Provides immunity to Electric-type moves and redirects all Electric-type attacks to this Pokémon";
            case Ability.Liquid_Ooze:
                return "Energy draining abilities targeted at this Pokémon damage the user";
            case Ability.Magma_Armor:
                return "Prevents this Pokémon from being frozen and significantly reduces the amount of time it takes an egg to hatch while in the party";
            case Ability.Magnet_Pull:
                return "Prevents Steel-type Pokémon from escaping or switching out";
            case Ability.Marvel_Scale:
                return "Boosts defense if affected by a status condition";
            case Ability.Minus:
                return "Boosts special attack by 50% if its partner has Plus as its ability";
            case Ability.Natural_Cure:
                return "Cures this Pokémon of its status condition when it switches out or the battle ends";
            case Ability.No_Guard:
                return "All moves used by or against this Pokémon will not miss";
            case Ability.Oblivious:
                return "This Pokémon cannot be infatuated or taunted";
            case Ability.Overgrow:
                return "Boosts the power of Grass-type moves by 50% while at low HP";
            case Ability.Own_Tempo:
                return "Prevents this Pokémon from being confused";
            case Ability.Pickup:
                return "Has a chance to pick up an item while exploring";
            case Ability.Plus:
                return "Boosts special attack by 50% if its partner has Minus as its ability";
            case Ability.Poison_Heal:
                return "Restores HP equal to the damage that would have been taken while poisoned";
            case Ability.Poison_Immunity:
                return "Prevents poisoning";
            case Ability.Poison_Point:
                return "Has a 30% chance to poison the attacker each time a move makes contact with this Pokémon";
            case Ability.Pressure:
                return "Opponents' moves deplete 1 more PP than normal when used";
            case Ability.Pure_Power:
                return "Doubles the effective attack stat of this Pokémon";
            case Ability.Quick_Feet:
                return "Boosts speed by 50% while affected by a status condition";
            case Ability.Rain_Dish:
                return "Restores 10% of this Pokémon's max HP every turn while it is raining";
            case Ability.Regenerator:
                return "Heals for 30% of its max HP when switched out";
            case Ability.Rock_Head:
                return "Prevents recoil damage, unless struggling";
            case Ability.Rough_Skin:
                return "Attackers take 8% of their max HP as damage each time they make contact with this Pokémon";
            case Ability.Runaway:
                return "This Pokémon can always escape from wild battles";
            case Ability.Sand_Stream:
                return "Causes a sandstorm to rage while in battle";
            case Ability.Sand_Veil:
                return "Prevents damage from sandstorms and reduces the accuracy of moves used against this Pokémon during a sandstorm by 20%";
            case Ability.Serene_Grace:
                return "Doubles the chance of applying a move's additional effects";
            case Ability.Shadow_Tag:
                return "Prevents wild Pokémon from escaping";
            case Ability.Shed_Skin:
                return "Has a chance to cure itself of a status condition at the end of every turn";
            case Ability.Shell_Armor:
                return "Ignores bonus damage from critical hits";
            case Ability.Shield_Dust:
                return "Ignores the additional effects of damaging moves";
            case Ability.Sniper:
                return "Critical hits deal 30% more damage than usual";
            case Ability.Snow_Cloak:
                return "Moves targeted at this Pokémon are 20% less accurate during hail";
            case Ability.Solar_Power:
                return "Boosts special attack by 30% in intense sunlight";
            case Ability.Solid_Rock:
                return "This Pokémon takes 25% less damage from super-effective moves";
            case Ability.Soundproof:
                return "This Pokémon is unaffected by sound-based moves";
            case Ability.Speed_Boost:
                return "Boosts speed by 1 stage at the end of every turn";
            case Ability.Static:
                return "Has a 30% chance to paralyze the attacker every time a move makes contact with this Pokémon";
            case Ability.Stench:
                return "Has a 10% chance to cause the target to flinch when using a move that makes contact, and lowers the rate at which wild Pokémon are encountered while leading the party";
            case Ability.Sticky_Hold:
                return "Prevents a held item from being removed or stolen, and increases the chance of nabbing something while fishing while leading the party";
            case Ability.Sturdy:
                return "Any move that would otherwise cause this Pokémon to faint from full HP instead leaves it with 1";
            case Ability.Suction_Cups:
                return "Prevents this Pokémon from being forced to switch out or flee";
            case Ability.Super_Luck:
                return "Boosts critical hit ratio by 1 stage";
            case Ability.Swarm:
                return "Boosts the power of Bug-type moves by 50% while at low HP";
            case Ability.Swift_Swim:
                return "Doubles the effective speed of this Pokémon while it is raining";
            case Ability.Synchronize:
                return "When this Pokémon is burned, poisoned, or paralyzed, the Pokémon that inflicted it is also affected. Guarantees that wild Pokémon will have the same nature while leading the party";
            case Ability.Tangled_Feet:
                return "While this Pokémon is confused, all moves against it have their accuracy halved";
            case Ability.Thick_Fat:
                return "Reduces damage taken from Fire and Ice-type moves by 50%";
            case Ability.Torrent:
                return "Boosts the power of Water-type moves by 50% while at low HP";
            case Ability.Trace:
                return "Copies the foe's ability for the duration of the battle";
            case Ability.Truant:
                return "Can only do something every other turn";
            case Ability.Vital_Spirit:
                return "Prevents this Pokémon from falling asleep, and causes all wild Pokémon to be the highest level possible in the area while leading the party";
            case Ability.Volt_Absorb:
                return "This Pokémon restores 20% of its max HP when hit by an Electric-type move instead of taking damage";
            case Ability.Water_Absorb:
                return "This Pokémon restores 20% of its max HP when hit by a Water-type move instead of taking damage";
            case Ability.Water_Veil:
                return "Reduces damage taken from Fire-type moves by 25% and prevents burns";
            case Ability.White_Smoke:
                return "Prevents other Pokémon from lowering this Pokémon's stats";
            case Ability.Wonder_Guard:
                return "This Pokémon only takes damage from super-effective moves, status conditions, and recoil";
            default:
                return "your mom";
        }
    }
}
