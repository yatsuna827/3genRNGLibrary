using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonPRNG.LCG32;
using PokemonPRNG.LCG32.StandardLCG;
using PokemonStandardLibrary;
using PokemonStandardLibrary.PokeDex.Gen3;
using PokemonStandardLibrary.CommonExtension;

namespace Pokemon3genRNGLibrary
{
    class FRLGStationaryGenerator : IGeneratable<Pokemon.Individual>
    {
        private readonly GBASlot slot;
        internal FRLGStationaryGenerator(string name, uint lv) => slot = new GBASlot(-1, name, lv, 1);

        public Pokemon.Individual Generate(uint seed) => slot.Generate(seed, 
            StandardLvGenerator.GetInstance(), 
            GenerateMethod.Standard, 
            NullNatureGenerator.GetInstance(), 
            NullGenderGenerator.GetInstance(),  
            out _);

        public static FRLGStationaryGenerator Moltres = new FRLGStationaryGenerator("ファイヤー", 50);
        public static FRLGStationaryGenerator Zapdos = new FRLGStationaryGenerator("サンダー", 50);
        public static FRLGStationaryGenerator Articuno = new FRLGStationaryGenerator("フリーザー", 50);
        public static FRLGStationaryGenerator Mewtwo = new FRLGStationaryGenerator("ミュウツー", 70);

        public static FRLGStationaryGenerator Eevee = new FRLGStationaryGenerator("イーブイ", 25);
    }



    class WildGenerationArgument
    {
        // FieldAbility
        public FieldAbility FieldAbility { get; set; }

        /// <summary>
        /// サファリに設置したポロックの味を指定します.
        /// サファリ以外では無視されます.
        /// </summary>
        public PokeBlock PokeBlock { get; set; } = PokeBlock.Plain;

        public IIVsGenerator GenerateMethod { get; set;} = Pokemon3genRNGLibrary.GenerateMethod.Standard;

        /// <summary>
        /// エンカウント判定を無視するかどうかを指定します.
        /// </summary>
        public bool ForceEncount { get; set; }
        // 
    }

    class FRLGMap
    {
        public readonly string MapName;
        public readonly uint BasicEncounterRate;
        private readonly EncounterTable encounterTable;
        public SlotGenerator GetSlotGenerator(WildGenerationArgument arg)
        {
            return null;
        }
        public ILvGenerator GetLvGenerator(WildGenerationArgument arg)
        {
            return null;
        }
        public INatureGenerator GetNatureGenerator(WildGenerationArgument arg)
        {
            return null;
        }
        public IGenderGenerator GetGenderGenerator(WildGenerationArgument arg)
        {
            return null;
        }
    }

    static class GenerateExtensions
    {
        internal static bool CheckNature(this uint pid, Nature fixedNature) 
            => fixedNature == Nature.other || (pid % 25) == (uint)fixedNature;
        internal static bool CheckGender(this uint pid, GenderRatio genderRatio, Gender fixedGender) 
            => genderRatio.IsFixed() || fixedGender == Gender.Genderless || pid.GetGender(genderRatio) == fixedGender;

        internal static Gender GetGender(this uint pid, GenderRatio ratio)
        {
            if (ratio == GenderRatio.Genderless) return Gender.Genderless;
            return (pid & 0xFF) < (uint)ratio ? Gender.Female : Gender.Male;
        }
        internal static bool IsShiny(this uint pid, uint tsv) => (tsv ^ (pid & 0xFFFF) ^ (pid >> 16)) < 8;
        internal static ShinyType GetShinyType(this uint pid, uint tsv)
        {
            var v = tsv ^ (pid & 0xFFFF) ^ (pid >> 16);
            if (v >= 8) return ShinyType.NotShiny;
            if (v == 0) return ShinyType.Square;
            return ShinyType.Star;
        }
    }
}
