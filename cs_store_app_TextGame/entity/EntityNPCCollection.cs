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
        public List<EntityNPCBase> Entities = new List<EntityNPCBase>();
        
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
        
        public void Add(EntityNPCBase npc)
        {
            Entities.Add(npc);
        }
        public void Remove(EntityNPCBase npc)
        {
            Entities.Remove(npc);
        }
        public void Update()
        {
            foreach(EntityNPCBase npc in Entities)
            {
                npc.Update();
            }
        }
        
        public EntityNPCBase GetRandomHostile(EntityNPCBase source, bool bMustBeAlive = false)
        {
            List<EntityNPCBase> hostiles = GetListOfHostiles(source, bMustBeAlive);
            if (hostiles.Count == 0) { return null; }
            return hostiles.Random();
        }
        private List<EntityNPCBase> GetListOfHostiles(EntityNPCBase source, bool bMustBeAlive = false)
        {
            List<EntityNPCBase> hostiles = new List<EntityNPCBase>();

            foreach (EntityNPCBase npc in Entities)
            {
                // ignore entities of same type
                if (npc.Faction.Equals(source.Faction)) { continue; }

                int relationship = EntityRelationshipTable.GetRelationship(source.Faction, npc.Faction);
                if (relationship < 0)
                {
                    if (!bMustBeAlive || !npc.IsDead)
                    {
                        hostiles.Add(npc);
                    }
                }
            }

            return hostiles;
        }
        
        public void Cleanup()
        {
            for (int i = Entities.Count - 1; i >= 0; i--)
            {
                if (Entities[i].IsDead) { Entities.RemoveAt(i); }
            }
        }

        public EntityNPCBase Find(string strWord, int ordinal = 0)
        {
            // TODO: closest match?
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
        public EntityNPCBase FindDead(string strWord, int ordinal = 0)
        {
            int nOccurrences = -1;

            for (int i = Entities.Count() - 1; i >= 0; i--)
            {
                if (!Entities[i].IsKeyword(strWord)) { continue; }
                if (!Entities[i].IsDead) { continue; }

                nOccurrences++;
                if (nOccurrences == ordinal)
                {
                    return Entities[i];
                }
            }

            return null;
        }
        public EntityNPCBase FindLiving(string strWord, int ordinal = 0)
        {
            int nOccurrences = -1;

            for (int i = Entities.Count() - 1; i >= 0; i--)
            {
                if (!Entities[i].IsKeyword(strWord)) { continue; }
                if (Entities[i].IsDead) { continue; }

                nOccurrences++;
                if (nOccurrences == ordinal)
                {
                    return Entities[i];
                }
            }

            return null;
        }

        #region OLD
        //public List<EntityNPC> EntitiesOLD = new List<EntityNPC>();
        //public string RoomDisplayStringOLD
        //{
        //    get
        //    {
        //        if (Entities.Count == 0) { return ""; }

        //        string strReturn = "You also see ";

        //        if (Entities.Count > 2)
        //        {
        //            for (int i = Entities.Count() - 1; i >= 0; i--)
        //            {
        //                strReturn += "a";
        //                if ((Entities[i].Name[0]).IsVowel())
        //                {
        //                    strReturn += "n";
        //                }
        //                strReturn += " ";
        //                strReturn += Entities[i].Name;
        //                if (i == 1)
        //                {
        //                    strReturn += ", and ";
        //                }
        //                else if (i > 0)
        //                {
        //                    strReturn += ", ";
        //                } 
        //            }
        //        }
        //        else if (Entities.Count == 2)
        //        {
        //            strReturn += "a";
        //            if ((Entities[1].Name[0]).IsVowel())
        //            {
        //                strReturn += "n";
        //            }
        //            strReturn += " ";
        //            strReturn += Entities[1].Name;
        //            strReturn += " and a";
        //            if ((Entities[0].Name[0]).IsVowel())
        //            {
        //                strReturn += "n";
        //            }
        //            strReturn += " ";
        //            strReturn += Entities[0].Name;
        //        }
        //        else if (Entities.Count == 1)
        //        {
        //            strReturn += "a";
        //            if ((Entities[0].Name[0]).IsVowel())
        //            {
        //                strReturn += "n";
        //            }
        //            strReturn += " ";
        //            strReturn += Entities[0].Name;
        //        }

        //        strReturn += ".";

        //        return strReturn;
        //    }
        //}
        //public Paragraph RoomDisplayParagraphOLD
        //{
        //    get
        //    {
        //        if (Entities.Count == 0) { return null; }

        //        Paragraph p = new Paragraph();
        //        p.Inlines.Add(("You also see ").ToRun());

        //        if (Entities.Count > 2)
        //        {
        //            string strA = "";
        //            for (int i = Entities.Count() - 1; i >= 0; i--)
        //            {
        //                strA += "a";
        //                if ((Entities[i].Name[0]).IsVowel())
        //                {
        //                    strA += "n";
        //                }
        //                strA += " ";

        //                p.Inlines.Add(strA.ToRun());
        //                p.Inlines.Add(Entities[i].NameAsRun);
        //                if (i == 1)
        //                {
        //                    strA = ", and ";
        //                }
        //                else if (i > 0)
        //                {
        //                    strA = ", ";
        //                }
        //            }
        //        }
        //        else if (Entities.Count == 2)
        //        {
        //            string strA = "a";
        //            if ((Entities[1].Name[0]).IsVowel())
        //            {
        //                strA += "n";
        //            }
        //            strA += " ";

        //            p.Inlines.Add(strA.ToRun());
        //            p.Inlines.Add(Entities[1].NameAsRun);

        //            strA = " and a";
        //            if ((Entities[0].Name[0]).IsVowel())
        //            {
        //                strA += "n";
        //            }
        //            strA += " ";

        //            p.Inlines.Add(strA.ToRun());
        //            p.Inlines.Add(Entities[0].NameAsRun);
        //        }
        //        else if (Entities.Count == 1)
        //        {
        //            string strA = "a";
        //            if ((Entities[0].Name[0]).IsVowel())
        //            {
        //                strA += "n";
        //            }
        //            strA += " ";
        //            p.Inlines.Add(strA.ToRun());
        //            p.Inlines.Add(Entities[0].NameAsRun);
        //        }

        //        p.Inlines.Add((".\n").ToRun());

        //        return p;
        //    }
        //}
        //public void AddOLD(EntityNPC npc)
        //{
        //    Entities.Add(npc);
        //}
        //public void RemoveOLD(EntityNPC npc)
        //{
        //    Entities.Remove(npc);
        //}
        //public List<Handler> UpdateOLD()
        //{
        //    List<Handler> handlers = new List<Handler>();

        //    for (int i = Entities.Count - 1; i >= 0; i--)
        //    {
        //        Handler handler = Entities[i].Update();
        //        if (handler != null) { handlers.Add(handler); }
        //    }

        //    return handlers;
        //}
        //public EntityNPC GetRandomHostileOLD(EntityNPC source, bool bMustBeAlive = false)
        //{
        //    List<EntityNPC> hostiles = GetListOfHostiles(source, bMustBeAlive);
        //    if (hostiles.Count == 0) { return null; }
        //    return hostiles.Random();
        //}
        //private List<EntityNPC> GetListOfHostilesOLD(EntityNPC source, bool bMustBeAlive = false)
        //{
        //    List<EntityNPC> hostiles = new List<EntityNPC>();

        //    foreach (EntityNPC npc in Entities)
        //    {
        //        // ignore entities of same type
        //        if (npc.Faction.Equals(source.Faction)) { continue; }

        //        int relationship = EntityRelationshipTable.GetRelationship(source.Faction, npc.Faction);
        //        if (relationship < 0)
        //        {
        //            if (!bMustBeAlive || npc.CurrentHealth > 0)
        //            {
        //                hostiles.Add(npc);
        //            }
        //        }
        //    }

        //    return hostiles;
        //}
        //public void CleanupOLD()
        //{
        //    for (int i = Entities.Count - 1; i >= 0; i--)
        //    {
        //        if (Entities[i].IsDead) { Entities.RemoveAt(i); }
        //    }

        //    if (Entities.Count > 5)
        //    {
        //        Entities.Clear();
        //    }
        //}
        //public EntityNPC FindOLD(string strWord, int ordinal = 0)
        //{
        //    // TODO: closest match?
        //    int nOccurrences = -1;

        //    for (int i = Entities.Count() - 1; i >= 0; i--)
        //    {
        //        if (Entities[i].IsKeyword(strWord))
        //        {
        //            nOccurrences++;
        //            if (nOccurrences == ordinal)
        //            {
        //                return Entities[i];
        //            }
        //        }
        //    }

        //    return null;
        //}
        //public EntityNPC FindDeadOLD(string strWord, int ordinal = 0)
        //{
        //    int nOccurrences = -1;

        //    for (int i = Entities.Count() - 1; i >= 0; i--)
        //    {
        //        if (!Entities[i].IsKeyword(strWord)) { continue; }
        //        if (!Entities[i].IsDead) { continue; }

        //        nOccurrences++;
        //        if (nOccurrences == ordinal)
        //        {
        //            return Entities[i];
        //        }
        //    }

        //    return null;
        //}
        //public EntityNPC FindLivingOLD(string strWord, int ordinal = 0)
        //{
        //    int nOccurrences = -1;

        //    for (int i = Entities.Count() - 1; i >= 0; i--)
        //    {
        //        if (!Entities[i].IsKeyword(strWord)) { continue; }
        //        if (Entities[i].IsDead) { continue; }

        //        nOccurrences++;
        //        if (nOccurrences == ordinal)
        //        {
        //            return Entities[i];
        //        }
        //    }

        //    return null;
        //}
        #endregion
    }
}