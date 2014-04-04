using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Documents;

namespace cs_store_app_TextGame
{
    public class EntityNPCCollection
    {
        public List<EntityNPC> Entities = new List<EntityNPC>();
        public string RoomDisplayString
        {
            get
            {
                if (Entities.Count == 0) { return ""; }

                string strReturn = "You also see ";

                if (Entities.Count > 2)
                {
                    for (int i = Entities.Count() - 1; i >= 0; i--)
                    {
                        strReturn += "a";
                        if ((Entities[i].Name[0]).IsVowel())
                        {
                            strReturn += "n";
                        }
                        strReturn += " ";
                        strReturn += Entities[i].Name;
                        if (i == 1)
                        {
                            strReturn += ", and ";
                        }
                        else if (i > 0)
                        {
                            strReturn += ", ";
                        } 
                    }
                    //for (int i = 0; i < NPCs.Count; i++)
                    //{
                    //}
                }
                else if (Entities.Count == 2)
                {
                    strReturn += "a";
                    if ((Entities[1].Name[0]).IsVowel())
                    {
                        strReturn += "n";
                    }
                    strReturn += " ";
                    strReturn += Entities[1].Name;
                    strReturn += " and a";
                    if ((Entities[0].Name[0]).IsVowel())
                    {
                        strReturn += "n";
                    }
                    strReturn += " ";
                    strReturn += Entities[0].Name;
                }
                else if (Entities.Count == 1)
                {
                    strReturn += "a";
                    if ((Entities[0].Name[0]).IsVowel())
                    {
                        strReturn += "n";
                    }
                    strReturn += " ";
                    strReturn += Entities[0].Name;
                }

                strReturn += ".";

                return strReturn;
            }
        }
        public Paragraph RoomDisplayParagraph
        {
            get
            {
                if (Entities.Count == 0) { return null; }

                Paragraph p = new Paragraph();
                p.Inlines.Add(("You also see ").ToRun());

                if (Entities.Count > 2)
                {
                    string strA = "";
                    for (int i = Entities.Count() - 1; i >= 0; i--)
                    {
                        strA += "a";
                        if ((Entities[i].Name[0]).IsVowel())
                        {
                            strA += "n";
                        }
                        strA += " ";

                        p.Inlines.Add(strA.ToRun());
                        p.Inlines.Add(Entities[i].NameAsRun);
                        if (i == 1)
                        {
                            strA = ", and ";
                        }
                        else if (i > 0)
                        {
                            strA = ", ";
                        }
                    }
                }
                else if (Entities.Count == 2)
                {
                    string strA = "a";
                    if ((Entities[1].Name[0]).IsVowel())
                    {
                        strA += "n";
                    }
                    strA += " ";

                    p.Inlines.Add(strA.ToRun());
                    p.Inlines.Add(Entities[1].NameAsRun);

                    strA = " and a";
                    if ((Entities[0].Name[0]).IsVowel())
                    {
                        strA += "n";
                    }
                    strA += " ";

                    p.Inlines.Add(strA.ToRun());
                    p.Inlines.Add(Entities[0].NameAsRun);
                }
                else if (Entities.Count == 1)
                {
                    string strA = "a";
                    if ((Entities[0].Name[0]).IsVowel())
                    {
                        strA += "n";
                    }
                    strA += " ";
                    p.Inlines.Add(strA.ToRun());
                    p.Inlines.Add(Entities[0].NameAsRun);
                }

                p.Inlines.Add((".\n").ToRun());

                return p;
            }
        }

        public EntityNPC Find(string strWord, int ordinal = 0)
        {
            int nOccurrences = -1;

            for (int i = Entities.Count() - 1; i >= 0; i--)
            {
                if (Entities[i].IsKeyword(strWord))
                {
                    nOccurrences++;
                    if (nOccurrences == ordinal)
                    {
                        return Entities[i];
                    }
                }
            }

            return null;
        }
        public void Add(EntityNPC npc)
        {
            Entities.Add(npc);
        }
        public void Remove(EntityNPC npc)
        {
            Entities.Remove(npc);
        }
        public List<Handler> Update()
        {
            List<Handler> handlers = new List<Handler>();

            for (int i = Entities.Count - 1; i >= 0; i--)
            {
                Handler handler = Entities[i].Update();
                if (handler != null) { handlers.Add(handler); }
            }

            return handlers;
        }
        public EntityNPC GetRandomHostile(EntityNPC source, bool bMustBeAlive = false)
        {
            List<EntityNPC> hostiles = GetListOfHostiles(source, bMustBeAlive);
            if (hostiles.Count == 0) { return null; }
            return hostiles.Random();
        }

        private List<EntityNPC> GetListOfHostiles(EntityNPC source, bool bMustBeAlive = false)
        {
            List<EntityNPC> hostiles = new List<EntityNPC>();

            foreach(EntityNPC npc in Entities)
            {
                // ignore entities of same type
                if (npc.Type.Equals(source.Type)) { continue; }

                int relationship = EntityRelationshipTable.GetRelationship(source.Type, npc.Type);
                if(relationship < 0)
                {
                    if (!bMustBeAlive || npc.CurrentHealth > 0)
                    {
                        hostiles.Add(npc);
                    }
                }
            }

            return hostiles;
        }
    }
}
