using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame {
    public enum GEM_QUALITY { CHIPPED, FLAWED, NONE, POLISHED, FLAWLESS, PERFECT }
    public enum GEM_SIZE { TINY, SMALL, NORMAL, LARGE, HUGE }

    public class ItemGem : Item {
        public override ITEM_TYPE Type { get { return ITEM_TYPE.GEM; } }
        public GEM_QUALITY Quality { get; set; }
        public GEM_SIZE Size { get; set; }
        protected ItemGem(ItemGem template) : base(template) {
            Quality = (GEM_QUALITY)(GEM_QUALITY_VALUES.GetValue(Statics.Random.Next(GEM_QUALITY_VALUES.Length)));
            Size = (GEM_SIZE)(GEM_SIZE_VALUES.GetValue(Statics.Random.Next(GEM_SIZE_VALUES.Length)));
            Value = (int)(Value * GemQualityToValueMultiplier[Quality] * GemSizeToValueMultiplier[Size]);
            Name = PrefixString(this) + Name;
        }
        public ItemGem(XElement itemNode) : base(itemNode) { }
        public override Item Clone() {
            return new ItemGem(this);
        }

        #region Static
        private static Dictionary<GEM_SIZE, string> GemSizeToString = new Dictionary<GEM_SIZE, string>();
        private static Dictionary<string, GEM_SIZE> GemSizeStringToGemSize = new Dictionary<string, GEM_SIZE>();
        private static Dictionary<GEM_SIZE, float> GemSizeToValueMultiplier = new Dictionary<GEM_SIZE, float>();
        private static Dictionary<GEM_QUALITY, string> GemQualityToString = new Dictionary<GEM_QUALITY, string>();
        private static Dictionary<string, GEM_QUALITY> GemQualityStringToGemQuality = new Dictionary<string, GEM_QUALITY>();
        private static Dictionary<GEM_QUALITY, float> GemQualityToValueMultiplier = new Dictionary<GEM_QUALITY, float>();
        private static Array GEM_QUALITY_VALUES;
        private static Array GEM_SIZE_VALUES;

        static ItemGem() {
            GEM_QUALITY_VALUES = Enum.GetValues(typeof(GEM_QUALITY));
            GEM_SIZE_VALUES = Enum.GetValues(typeof(GEM_SIZE));

            GemSizeStringToGemSize.Add("tiny", GEM_SIZE.TINY);
            GemSizeStringToGemSize.Add("small", GEM_SIZE.SMALL);
            GemSizeStringToGemSize.Add("normal", GEM_SIZE.NORMAL);
            GemSizeStringToGemSize.Add("large", GEM_SIZE.LARGE);
            GemSizeStringToGemSize.Add("huge", GEM_SIZE.HUGE);

            GemSizeToString.Add(GEM_SIZE.TINY, "tiny");
            GemSizeToString.Add(GEM_SIZE.SMALL, "small");
            GemSizeToString.Add(GEM_SIZE.NORMAL, "normal");
            GemSizeToString.Add(GEM_SIZE.LARGE, "large");
            GemSizeToString.Add(GEM_SIZE.HUGE, "huge");

            GemSizeToValueMultiplier.Add(GEM_SIZE.TINY, 0.2f);
            GemSizeToValueMultiplier.Add(GEM_SIZE.SMALL, 0.5f);
            GemSizeToValueMultiplier.Add(GEM_SIZE.NORMAL, 1.0f);
            GemSizeToValueMultiplier.Add(GEM_SIZE.LARGE, 2.0f);
            GemSizeToValueMultiplier.Add(GEM_SIZE.HUGE, 3.0f);

            GemQualityStringToGemQuality.Add("chipped", GEM_QUALITY.CHIPPED);
            GemQualityStringToGemQuality.Add("flawed", GEM_QUALITY.FLAWLESS);
            GemQualityStringToGemQuality.Add("none", GEM_QUALITY.NONE);
            GemQualityStringToGemQuality.Add("polished", GEM_QUALITY.POLISHED);
            GemQualityStringToGemQuality.Add("flawless", GEM_QUALITY.FLAWLESS);
            GemQualityStringToGemQuality.Add("perfect", GEM_QUALITY.PERFECT);

            GemQualityToString.Add(GEM_QUALITY.CHIPPED, "chipped");
            GemQualityToString.Add(GEM_QUALITY.FLAWED, "flawed");
            GemQualityToString.Add(GEM_QUALITY.NONE, "none");
            GemQualityToString.Add(GEM_QUALITY.POLISHED, "polished");
            GemQualityToString.Add(GEM_QUALITY.FLAWLESS, "flawless");
            GemQualityToString.Add(GEM_QUALITY.PERFECT, "perfect");

            GemQualityToValueMultiplier.Add(GEM_QUALITY.CHIPPED, 0.2f);
            GemQualityToValueMultiplier.Add(GEM_QUALITY.FLAWED, 0.5f);
            GemQualityToValueMultiplier.Add(GEM_QUALITY.NONE, 1.0f);
            GemQualityToValueMultiplier.Add(GEM_QUALITY.POLISHED, 1.5f);
            GemQualityToValueMultiplier.Add(GEM_QUALITY.FLAWLESS, 2.0f);
            GemQualityToValueMultiplier.Add(GEM_QUALITY.PERFECT, 3.0f);
        }
        public static string PrefixString(ItemGem gem) {
            string _sizeString = gem.Size == GEM_SIZE.NORMAL ? string.Empty : GemSizeToString[gem.Size] + " ";
            string _qualityString = gem.Quality == GEM_QUALITY.NONE ? string.Empty : GemQualityToString[gem.Quality] + " ";
            return _sizeString + _qualityString;
        }
        #endregion
    }
}
