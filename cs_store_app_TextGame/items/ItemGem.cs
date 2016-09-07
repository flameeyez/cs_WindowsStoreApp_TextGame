using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs_store_app_TextGame {
    public enum GEM_QUALITY { CHIPPED, FLAWED, NONE, POLISHED, FLAWLESS, PERFECT }
    public enum GEM_SIZE { TINY, SMALL, NORMAL, LARGE, HUGE }

    [DataContract(Name = "ItemGem", Namespace = "cs_store_app_TextGame")]
    public class ItemGem : Item {
        public override ITEM_TYPE Type { get { return ITEM_TYPE.GEM; } }
        public ItemGem(XElement itemNode) : base(itemNode) {
            _quality = GemQualityStringToGemQuality[itemNode.Element("gem-quality").Value];
            _size = GemSizeStringToGemSize[itemNode.Element("size").Value];
            _valueBase = float.Parse(itemNode.Element("value-base").Value);
        }

        private GEM_SIZE _size;
        private string Size { get { return _size == GEM_SIZE.NORMAL ? string.Empty : GemSizeToString[_size] + " "; } }

        private GEM_QUALITY _quality;
        private string Quality { get { return _quality == GEM_QUALITY.NONE ? string.Empty : GemQualityToString[_quality] + " "; } }

        public string FullName { get { return Size + Quality + Name; } }

        private float _valueBase;
        public override int Value { get { return (int)(_valueBase * GemQualityToValueMultiplier[_quality]); } }

        #region Static
        private static Dictionary<GEM_SIZE, string> GemSizeToString = new Dictionary<GEM_SIZE, string>();
        private static Dictionary<string, GEM_SIZE> GemSizeStringToGemSize = new Dictionary<string, GEM_SIZE>();
        private static Dictionary<string, GEM_QUALITY> GemQualityStringToGemQuality = new Dictionary<string, GEM_QUALITY>();
        private static Dictionary<GEM_QUALITY, float> GemQualityToValueMultiplier = new Dictionary<GEM_QUALITY, float>();
        private static Dictionary<GEM_QUALITY, string> GemQualityToString = new Dictionary<GEM_QUALITY, string>();
        static ItemGem() {
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
        #endregion
    }
}
