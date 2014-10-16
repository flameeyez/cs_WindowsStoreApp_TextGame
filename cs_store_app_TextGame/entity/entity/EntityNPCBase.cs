using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.UI;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace cs_store_app_TextGame
{
    // TODO: rename to EntityNPC
    [DataContract(Name = "EntityNPCBase", Namespace = "cs_store_app_TextGame")]
    public class EntityNPCBase : EntityBase
    {
        #region Name
        [DataMember]
        protected string _name { get; set; }
        public virtual string Name
        {
            get
            {
                return (IsDead ? "dead " : "") + _name;
            }
            set
            {
                _name = value;
            }
        }
        public virtual string NameBase
        {
            get
            {
                return _name;
            }
        }
        public virtual Run NameAsRun
        {
            get
            {
                return new Run { Foreground = new SolidColorBrush(Statics.LevelDeltaToColor(Level)), Text = Name };
            }
        }
        public virtual Paragraph NameAsParagraph
        {
            get
            {
                Paragraph p = new Paragraph();
                p.Inlines.Add(NameAsRun);
                return p;
            }
        }
        public virtual Paragraph NameBaseAsParagraph
        {
            get
            {
                Paragraph p = new Paragraph();
                p.Inlines.Add(new Run { Foreground = new SolidColorBrush(Statics.LevelDeltaToColor(Level)), Text = NameBase });
                return p;
            }
        }
        public virtual string NameIndefiniteArticle(bool bCapitalize)
        {
            return _name.IndefiniteArticle(bCapitalize);
        }
        public Paragraph NameWithIndefiniteArticle(bool bCapitalize = false)
        {
            Paragraph p = new Paragraph();

            p.Inlines.Add((NameIndefiniteArticle(bCapitalize)).ToRun());
            p.Inlines.Add(NameAsRun);

            return p;
        }
        #endregion
        #region NPC-specific attributes
        
        [DataMember]
        public DateTime LastActionTime = DateTime.Now;
        [DataMember]
        public int ActionPulse { get; set; }
        [DataMember]
        public List<string> Keywords = new List<string>();
        [DataMember]
        public FACTION Faction { get; set; }
        public Paragraph LookParagraph
        {
            get
            {
                if (IsDead)
                {
                    Paragraph p = new Paragraph();
                    p.Inlines.Add(("The ").ToRun());
                    p.Merge(NameBaseAsParagraph);
                    p.Inlines.Add((" is dead.").ToRun());
                    return p;
                }

                // TODO: fix so that we don't need to pass 'this' into NPCDisplayParagraph
                Paragraph p1 = Hands.NPCDisplayParagraph(this).Clone();
                p1.Inlines.Add("\n".ToRun());
                Paragraph p2 = InventoryParagraph.Clone();
                p1.Merge(p2);
                return p1;
            }
        }

        #region Behavior
        [DataMember]
        public EntityBehavior Behavior = new EntityBehavior();
        public virtual void ProcessBehavior()
        {
            List<EntityBehaviorAction> DesiredActions = Behavior.GetDesiredBehavior();

            // TODO: declare return code variable; declare return code enum? handled/unhandled?

            // loop until return code is satisfactory
            foreach (EntityBehaviorAction desiredAction in DesiredActions)
            {
                // for current action, call corresponding method
                // if return code satisfactory, break
                switch (desiredAction.Action)
                {
                    case ACTION_ENUM.ATTACK:
                        // returnCode = DoAttack();
                        break;
                    case ACTION_ENUM.NONE:
                        // returnCode = success;
                        break;
                    case ACTION_ENUM.MOVE_BASIC:
                        // returnCode = DoMoveBasic();
                        break;

                    // etc...

                    default:
                        break;
                }
            }
        }
        #endregion

        public override Paragraph InventoryParagraph
        {
            get
            {
                Paragraph p = new Paragraph();
                List<Run> inventory = new List<Run>();

                foreach (EntityBodyPart bodyPart in Body.BodyParts)
                {
                    // TODO: containers

                    if (bodyPart.Item != null)
                    {
                        inventory.Add(bodyPart.Item.NameIndefiniteArticle.ToRun());
                        inventory.Add(bodyPart.Item.NameAsRun);
                    }
                }

                p.Inlines.Add(("The ").ToRun());
                p.Inlines.Add(NameAsRun);
                p.Inlines.Add((" is wearing ").ToRun());

                switch (inventory.Count / 2)
                {
                    case 0:
                        p.Inlines.Clear();
                        p.Inlines.Add(("The ").ToRun());
                        p.Inlines.Add(NameAsRun);
                        p.Inlines.Add((" isn't wearing anything special.").ToRun());
                        return p;
                    case 1:
                        p.Inlines.Add(inventory[0]);
                        p.Inlines.Add(inventory[1]);
                        break;
                    case 2:
                        p.Inlines.Add(inventory[0]);
                        p.Inlines.Add(inventory[1]);
                        p.Inlines.Add((" and ").ToRun());
                        p.Inlines.Add(inventory[2]);
                        p.Inlines.Add(inventory[3]);
                        break;
                    default:
                        for (int i = 0; i < inventory.Count - 2; i += 2)
                        {
                            p.Inlines.Add(inventory[i]);
                            p.Inlines.Add(inventory[i + 1]);
                            p.Inlines.Add((", ").ToRun());
                        }

                        p.Inlines.Add(("and ").ToRun());
                        p.Inlines.Add(inventory[inventory.Count - 2]);
                        p.Inlines.Add(inventory[inventory.Count - 1]);
                        break;
                }

                p.Inlines.Add((".").ToRun());
                return p;
            }
        }
        #endregion
        #region NPC-specific methods
        public bool IsKeyword(string strWord)
        {
            foreach (string keyword in Keywords)
            {
                if (keyword == strWord) { return true; }
            }

            return false;
        }
        public void Update()
        {
            if (IsDead && HasBeenSearched)
            {
                // TODO: replace this once rooms have new NPC collections
                // CurrentRoom.NPCs.Remove(this);

                // TODO: replace this with direct display message
                // return Handler.HANDLED(MESSAGE_ENUM.DEBUG_REMOVE, NameAsParagraph);
                return;
            }

            DateTime now = DateTime.Now;
            TimeSpan delta = now - LastActionTime;
            if (delta.TotalMilliseconds >= ActionPulse)
            {
                // check behavior
                LastActionTime = now;
                
                // TODO: replace this with new behavior
                // handler = DoAction();
            }
        }
        #endregion

        public EntityNPCBase() : base() { }
        public EntityNPCBase(XElement entityNPCBaseElement) : base(entityNPCBaseElement)
        {
            Name = entityNPCBaseElement.Element("name").Value;
            Faction = (FACTION)Enum.Parse(typeof(FACTION), entityNPCBaseElement.Element("faction").Value);

            // keywords
            foreach (XElement keywordElement in entityNPCBaseElement.Element("keywords").Elements("keyword"))
            {
                Keywords.Add(keywordElement.Value);
            }

            // behavior
            var behaviorNode = entityNPCBaseElement.Element("behavior");
            if (behaviorNode != null)
            {
                foreach (XElement behaviorActionElement in behaviorNode.Elements())
                {
                    ACTION_ENUM action = TranslatedInput.StringToAction[behaviorActionElement.Name.LocalName];
                    int percentage = int.Parse(behaviorActionElement.Value);
                    Behavior.PossibleActions.Add(new EntityBehaviorAction(action, percentage));
                }
            }
        }
        //public EntityNPCBase Clone()
        //{
        //    // TODO: finish
        //    throw new NotImplementedException();
        //}
    }
}
