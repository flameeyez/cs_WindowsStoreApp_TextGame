using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;

namespace cs_store_app_TextGame
{
    [DataContract(Name = "EntityHands", Namespace = "cs_store_app_TextGame")]
    public class EntityHands
    {
        [DataMember]
        public List<EntityHand> Hands = new List<EntityHand>();
        public bool Full
        {
            get
            {
                foreach (EntityHand hand in Hands)
                {
                    if (hand.Item == null) { return false; }
                }

                return true;
            }
        }
        public bool Empty
        {
            get
            {
                foreach (EntityHand hand in Hands)
                {
                    if (hand.Item != null) { return false; }
                }

                return true;
            }
        }
        
        // at least one hand must be holding a weapon
        //  or all hands must be empty
        public bool CanAttack
        {
            get
            {
                bool bHandsAreEmpty = true;

                foreach (EntityHand hand in Hands)
                {
                    if(hand.Item != null)
                    {
                        bHandsAreEmpty = false;
                        if (hand.Item.Type == ITEM_TYPE.WEAPON)
                        {
                            return true;
                        }
                    }
                }

                return bHandsAreEmpty;
            }
        }
        public int AttackPower
        {
            get
            {
                int nAttackPower = 0;

                foreach (EntityHand hand in Hands)
                {
                    if (hand.Item == null) { continue; }
                    if (hand.Item.Type == ITEM_TYPE.WEAPON)
                    {
                        nAttackPower += (hand.Item as ItemWeapon).AttackPower;
                    }
                }

                return nAttackPower;
            }
        }

        // simple List wrappers
        public void Add(EntityHand hand)
        {
            Hands.Add(hand);
        }
        public int Count
        {
            get
            {
                return Hands.Count;
            }
        }

        public Paragraph PlayerDisplayParagraph
        {
            get
            {
                Paragraph p = new Paragraph();

                EntityHand RightHand = Hands[0];
                EntityHand LeftHand = Hands[1];

                if (LeftHand.Item == null && RightHand.Item == null)
                {
                    p.Inlines.Add("You aren't holding anything.".ToRun());
                    return p;
                }

                if (RightHand.Item == null)
                {
                    // RightHand IS empty
                    // LeftHand IS NOT empty
                    p.Inlines.Add("You are holding ".ToRun());
                    p.Merge(LeftHand.Item.NameWithIndefiniteArticle);
                    p.Inlines.Add(" in your left hand.".ToRun());
                    return p;
                }
                else
                {
                    // RightHand IS NOT empty
                    // LeftHand MAY OR MAY NOT BE empty
                    p.Inlines.Add("You are holding ".ToRun());
                    p.Merge(RightHand.Item.NameWithIndefiniteArticle);
                    p.Inlines.Add(" in your right hand".ToRun());

                    // TODO: LeftHand switch here
                    if (LeftHand.Item == null)
                    {
                        // RightHand IS NOT empty
                        // LeftHand IS empty
                        p.Inlines.Add(".".ToRun());
                        return p;
                    }
                    else
                    {
                        // RightHand IS NOT empty
                        // LeftHand IS NOT empty
                        // append ...and a blah in its left hand.
                        p.Inlines.Add(" and ".ToRun());
                        p.Merge(LeftHand.Item.NameWithIndefiniteArticle);
                        p.Inlines.Add(" in your left hand.".ToRun());
                        return p;
                    }
                }
            }
        }

        // with dependency on entity, might make sense to move up, since entity already has hands (circular?)
        // otherwise, remove dependency
        public Paragraph NPCDisplayParagraph(EntityNPCBase entity)
        {
            // if the NPC has hands, assume two hands for now
            if (Hands.Count == 0) { return null; }

            Paragraph p = new Paragraph();

            EntityHand RightHand = Hands[0];
            EntityHand LeftHand = Hands[1];

            if (LeftHand.Item == null && RightHand.Item == null)
            {
                p.Inlines.Add("The ".ToRun());
                p.Merge(entity.NameAsParagraph);
                p.Inlines.Add(" isn't holding anything.".ToRun());
                return p;
            }

            if (RightHand.Item == null)
            {
                // RightHand IS empty
                // LeftHand IS NOT empty
                p.Inlines.Add("The ".ToRun());
                p.Merge(entity.NameAsParagraph);
                p.Inlines.Add(" is holding ".ToRun());
                p.Merge(LeftHand.Item.NameWithIndefiniteArticle);
                p.Inlines.Add(" in its left hand.".ToRun());
                return p;
            }
            else
            {
                // RightHand IS NOT empty
                // LeftHand MAY OR MAY NOT BE empty
                p.Inlines.Add("The ".ToRun());
                p.Merge(entity.NameAsParagraph);
                p.Inlines.Add(" is holding ".ToRun());
                p.Merge(RightHand.Item.NameWithIndefiniteArticle);
                p.Inlines.Add(" in its right hand".ToRun());

                // TODO: LeftHand switch here
                if (LeftHand.Item == null)
                {
                    // RightHand IS NOT empty
                    // LeftHand IS empty
                    p.Inlines.Add(".".ToRun());
                    return p;
                }
                else
                {
                    // RightHand IS NOT empty
                    // LeftHand IS NOT empty
                    // append ...and a blah in its left hand.
                    p.Inlines.Add(" and ".ToRun());
                    p.Merge(LeftHand.Item.NameWithIndefiniteArticle);
                    p.Inlines.Add(" in its left hand.".ToRun());
                    return p;
                }
            }
        }

        public EntityHand GetHandWithItem(string strKeyword, ITEM_TYPE type = ITEM_TYPE.ANY)
        {
            foreach (EntityHand hand in Hands)
            {
                if (hand.Item.IsKeyword(strKeyword) && (type == ITEM_TYPE.ANY || type == hand.Item.Type))
                {
                    return hand;
                }
            }

            return null;
        }
        public Item GetAnyItem(ITEM_TYPE type)
        {
            foreach(EntityHand hand in Hands)
            {
                if (hand.Item == null) { continue; }

                if(hand.Item.Type == type)
                {
                    return hand.Item;
                }
            }

            return null;
        }
        public Item GetItem(string strKeyword, bool bRemoveFromHand, ITEM_TYPE itemType = ITEM_TYPE.ANY)
        {
            foreach(EntityHand hand in Hands)
            {
                if (hand.Item == null) { continue; }
                if (itemType != ITEM_TYPE.ANY && itemType != hand.Item.Type) { continue; }
                if (!hand.Item.IsKeyword(strKeyword)) { continue; }

                // item found
                Item item = hand.Item;
                if (bRemoveFromHand) { hand.Item = null; }
                return item;
            }

            return null;
        }
        public EntityHand GetEmptyHand()
        {
            foreach(EntityHand hand in Hands)
            {
                if (hand.Item == null) { return hand; }
            }

            return null;
        }
    }
}
