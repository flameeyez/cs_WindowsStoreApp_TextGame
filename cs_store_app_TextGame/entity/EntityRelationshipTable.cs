using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace cs_store_app_TextGame
{
    public enum FACTION
    {
        GOBLIN,
        ORC,
        TROLL,
        VILLAGER
    }
    public static class EntityRelationshipTable
    {
        private static Dictionary<FACTION, Dictionary<FACTION, int>> Relationships = new Dictionary<FACTION, Dictionary<FACTION, int>>();
        static EntityRelationshipTable() { }
            
        public static void Test()
        {
            Dictionary<FACTION, int> orcRelationships = Relationships[FACTION.ORC];
            int OrcOpinionOfTroll = orcRelationships[FACTION.TROLL];

            if (OrcOpinionOfTroll < 0)
            {
                // orc hates troll and will attack
            }
        }

        // foreach entity_type, look for xml match
        // if relationship with self, continue
        // if found, add relationship
        // if not found, add default value (friendly, 100)
        // rogue xml is ignored (bad entity_type, should probably warn)
        public static async Task Load()
        {
            foreach(FACTION type in Enum.GetValues(typeof(FACTION)))
            {
                Relationships.Add(type, new Dictionary<FACTION, int>());                
            }

            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("xml\\entity_relationships");
            var file = await folder.GetFileAsync("entity_relationships.xml");
            var stream = await file.OpenStreamForReadAsync();

            XDocument document = XDocument.Load(stream);
            await stream.FlushAsync();

            var entityNodes = from entities in document
                                    .Elements("entities")
                                    .Elements("entity")
                               select entities;
            foreach (var entityNode in entityNodes)
            {
                FACTION entity_type = (FACTION)(Enum.Parse(typeof(FACTION), entityNode.Element("id").Value));
                Dictionary<FACTION, int> r = Relationships[entity_type];

                foreach (FACTION target_type in Enum.GetValues(typeof(FACTION)))
                {
                    if (entity_type.Equals(target_type)) { continue; }
                    var relationshipNodes = from relationship in entityNode
                                        .Elements("relationships")
                                        .Elements("relationship")
                                        where relationship.Element("id").Value == target_type.ToString()
                                           select relationship;

                    if(relationshipNodes.Count() == 0)
                    {
                        r[target_type] = 100;
                    }
                    else
                    {
                        var relationshipNode = relationshipNodes.First();
                        int value = int.Parse(relationshipNode.Element("value").Value);
                        r[target_type] = value;
                    }
                }

                //var relationshipNodes = from relationships in entityNode
                //                        .Elements("relationships")
                //                        .Elements("relationship")
                //                            select relationships;

                //// TODO: 
                //foreach (var relationshipNode in relationshipNodes)
                //{
                //    ENTITY_TYPE target_type = (ENTITY_TYPE)Enum.Parse(typeof(ENTITY_TYPE), relationshipNode.Element("id").Value);
                //    int target_value = int.Parse(relationshipNode.Element("value").Value);
                //    r.Add(target_type, target_value);
                //}
            }
        }

        // a relationship is defined as what entity e1 think of e2
        // a negative number means that e1 hates e2 and will act as behavior warrants (attack, run away)
        public static int GetRelationship(FACTION e1, FACTION e2)
        {
            return Relationships[e1][e2];
            //Dictionary<FACTION, int> r = Relationships[e1];
            //return r[e2];
        }

        public static string DebugDisplayString()
        {
            string str = "";

            foreach (FACTION t1 in Enum.GetValues(typeof(FACTION)))
            {
                Dictionary<FACTION, int> d = Relationships[t1];
                str += t1.ToString() + "\n";
                
                foreach (FACTION t2 in Enum.GetValues(typeof(FACTION)))
                {
                    if (t1.Equals(t2)) { continue; }
                    int value = d[t2];

                    str += "\t" + t2.ToString() + ": " + value.ToString() + "\n";
                }
            }

            return str;
        }
    }
}
