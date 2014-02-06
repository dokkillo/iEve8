using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using iEve8Lib.BLL;
//using Newtonsoft.Json;

namespace iEve8Lib
{
    public sealed class EveAccounts
    {
      
        public EveAccounts()
        {
           
        }

        public List<Character> getAccountCharacters(string jsonAccount)
        {
            List<Account> AccountList = new List<Account>();
            List<Character> CharacterList = new List<Character>();
            List<Skill> GenericSkills = new List<Skill>();

            if (!string.IsNullOrEmpty(jsonAccount))
            {

                #region data
                //Name: One
//AccountId: 1067769 
//Key": twPME2uN3bUavSCMm189wsVudHBIQo4zWqxl2flg8KQsP1o8WEps0fwXpk8bK1iw
//,
//Name: Two
//AccountId: 1070601
//Key: ySg1sYIVkjHrk0d8x846qMRV28jHADxsMh9Kbu4UavlkaoQeUkfZ0XpHI24kDzU7    
//hello, 

//Some webservices changed, and my app is crashing now. I didnt have time to fix right now.  I need to hidden this app from market for a while.


//Name: three
//AccountId: 1073189
                //Key: T8yBqNSRcQySP0PDCHPUGEQnOWPxsIrSolszV4CMqOplby7dTaCicMiXxnd0rssr
                #endregion
                

                Account acc = new Account();
                acc.account = "1067769";
                acc.key = "twPME2uN3bUavSCMm189wsVudHBIQo4zWqxl2flg8KQsP1o8WEps0fwXpk8bK1iw";


                AccountList.Add(acc);

                foreach (var t in AccountList)
                {
                    int account;
                    if (Int32.TryParse(t.account, out account))
                    {
                        CharacterList.AddRange(returnAccountCharacters(account, t.key));
                    }
                }
            }

            return CharacterList;
        }

        public string ConfirmAccount(string jsonAccount)
        {
            List<Account> AccountList = new List<Account>();
            string result = "Error, please test later";
            if (!string.IsNullOrEmpty(jsonAccount))
            {

                Account acc = new Account();
                acc.account = "1067769";
                acc.key = "twPME2uN3bUavSCMm189wsVudHBIQo4zWqxl2flg8KQsP1o8WEps0fwXpk8bK1iw";
                AccountList.Add(acc);


                foreach (var t in AccountList)
                {
                    int account;
                    if (Int32.TryParse(t.account, out account))
                    {
                        result = confirmAccountValues(account, t.key);
                    }
                }
            }


            return result;
        }
        /// <summary>
        /// Method for return the name of all Skills
        /// </summary>
        private List<Skill> getSkills()
        {
            List<Skill> SkillList = new List<Skill>();
            var xml = GetSkillsXml();
            var lv1s = (from t in xml.Descendants("result").Descendants("rowset").Descendants("row").Descendants("rowset").Descendants("row") where t.Attribute("typeName") != null select t);
            foreach (var t in lv1s)
            {
                Skill oSkill = new Skill();
                oSkill.GroupId = Convert.ToInt32(t.Attribute("groupID").Value);
                oSkill.GroupName = GetGroupSkillName(oSkill.GroupId);
                oSkill.Id = Convert.ToInt32(t.Attribute("typeID").Value);
                oSkill.SkillName = t.Attribute("typeName").Value.ToString();
                SkillList.Add(oSkill);
            }
            return SkillList;
        }
    
        /// <summary>
        /// Method for return the characters of one account
        /// </summary>
        /// <param name="KeyId"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        private List<Character> returnAccountCharacters(int KeyId, string Key)
        {

            #region xml sample

//            <?xml version="1.0" encoding="UTF-8"?>
//-         <eveapi version="2">
            //<currentTime>2012-06-18 16:19:44</currentTime>-
            //<result>-
            //<rowset columns="name,characterID,corporationName,corporationID" key="characterID" name="characters">
            //<row name="dokkillo" corporationID="98087081" corporationName="Tr0pa de elite." characterID="274419359"/>
            //    <row name="Camionera" corporationID="1000080" corporationName="Ministry of War" characterID="279582876"/>
            //        <row name="Lobezno" corporationID="1000111" corporationName="Aliastra" characterID="396427738"/>
            //            </rowset>
            //                </result>
            //<cachedUntil>2012-06-18 17:06:38</cachedUntil>
            //    </eveapi>

            #endregion

           var xml = GetCharacterListXml(KeyId, Key);
           var lv1s = (from lv1 in xml.Descendants("result").Descendants("rowset").Descendants("row") select lv1);
           List<Character> CharacterList = new List<Character>();
           foreach (var t in lv1s)
           {
               int CharacterId = (int)t.Attribute("characterID");
               Character oChar = new Character();
               oChar = GetCharacterSheet(KeyId, Key, CharacterId);
              
              CharacterList.Add(oChar);
           }
            
           return CharacterList;

        }

        private string confirmAccountValues(int KeyId, string Key)
        {
            var xml = GetCharacterListXml(KeyId, Key);
            var lv1s = (from lv1 in xml.Descendants("result").Descendants("rowset").Descendants("row") select lv1).FirstOrDefault();
            return lv1s.ToString();



        }


        /// <summary>
        /// Method for return the actual skill is training the character
        /// </summary>
        /// <param name="KeyId"></param>
        /// <param name="Key"></param>
        /// <param name="CharacterId"></param>
        /// <returns></returns>
        private Skill GetReturnSkill(int KeyId, string Key, int CharacterId, List<Skill> GenericSkillList)
        {
            Skill oSkill = new Skill();
            var xml = GetCharacterSkillsXml(KeyId, Key, CharacterId);
            var lv1s = (from lv1 in xml.Descendants("result").Descendants() select lv1);
            if (lv1s.Count() > 2)
            {
                
                DateTime EndDate = Convert.ToDateTime(lv1s.ElementAt(1).Value);
                DateTime BeginDate = Convert.ToDateTime(lv1s.ElementAt(0).Value);               
                var typeID =lv1s.ElementAt(3).Value;
                if (GenericSkillList != null)
                 {
                     var skill = (from t in GenericSkillList where t.Id == Convert.ToInt32(typeID) select t).FirstOrDefault();
                     oSkill.Level = Convert.ToInt32(lv1s.ElementAt(6).Value);
                     oSkill.LevelImage = "/images/level" + oSkill.Level + ".gif";
                     oSkill.SkillName = skill.SkillName + " " + oSkill.Level;
                     oSkill.Id = skill.Id;
                 }
                TimeSpan ts = (EndDate - BeginDate);
                oSkill.Time = string.Format(" {0} days {1} Hours {2} minutes {2} seconds", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
               
            }
            else
            {
             oSkill.Id = 0;
              oSkill.Time = " -";
              oSkill.SkillName = "No training skill";
            }

            return oSkill;
          }       
              

        private Character GetCharacterSheet(int KeyId, string Key, int CharacterId)
        {
            List<Skill> GenericSkills = getSkills();
            Character oChar = new Character();
            oChar.account = new Account();
            oChar.account.account = "Characters";
            oChar.account.key = KeyId.ToString();
            //oChar.account.Type = "Characters";
            oChar.CharacterId = CharacterId;
            oChar.SkillTraining = new Skill();
            oChar.SkillTraining = GetReturnSkill(KeyId, Key, CharacterId, GenericSkills);
            oChar.Image = "http://image.eveonline.com/Character/" + CharacterId + "_256.jpg";

            var xml = GetCharacterSheetXML(KeyId, Key,oChar.CharacterId);
            var lv1s = (from lv1 in xml.Descendants("result") select lv1).FirstOrDefault();

            oChar.AllianceName = lv1s.ToString();
            if (null != lv1s)
            {
                oChar.Race = (string)lv1s.Element("race");
                oChar.BloodLine = (string)lv1s.Element("bloodLine");
                oChar.AllianceName = (string)lv1s.Element("allianceName");
                oChar.CharacterName = (string)lv1s.Element("name");
                oChar.CorpId = (int)lv1s.Element("corporationID");
                oChar.CorporationName = (string)lv1s.Element("corporationName");
                oChar.Balance = BalanceToString((decimal)lv1s.Element("balance"));
            }

            var skills = (from t in xml.Descendants("result").Descendants("rowset").Descendants("row") select t);
            List<Skill> charSkillList = new List<Skill>();

            if (GenericSkills != null && GenericSkills.Count > 0)
            {
                foreach (var t in skills)
                {
                    if (null != t.Attribute("typeID"))
                    {
                        Skill oSkill = (from x in GenericSkills where x.Id == (int)t.Attribute("typeID") select x).FirstOrDefault();
                        if (null != oSkill)
                        {       
                            oSkill.Level = (int)t.Attribute("level");
                            oSkill.LevelImage = "images/level" + oSkill.Level + ".gif";
                            oSkill.SkillPoints = (int)t.Attribute("skillpoints");
                            charSkillList.Add(oSkill);
                        }
                    }
                }
            }

            oChar.totalSkillPoints = BalanceToString(charSkillList.Sum(x => x.SkillPoints));
            oChar.SkillList = charSkillList.OrderBy(x => x.GroupName).ToList();
            

            return oChar;
        }

        #region XML Methods

        private XDocument GetCharacterListXml(int KeyId, string Key)
        {
            var xml = XDocument.Load("https://api.eveonline.com/account/Characters.xml.aspx?keyID=" + KeyId + " &vCode=" + Key + "");
            return xml;
        }

        private XDocument GetSkillsXml()
        {
            var xml = XDocument.Load("https://api.eveonline.com/eve/SkillTree.xml.aspx");

            return xml;
        }

        private XDocument GetCharacterSkillsXml(int KeyId, string Key, int CharacterId)
        {
            var xml = XDocument.Load("https://api.eveonline.com/char/SkillInTraining.xml.aspx?keyID=" + KeyId + "&vCode=" + Key + "&characterID=" + CharacterId + "");
            return xml;
        }

        private XDocument GetCharacterSheetXML(int KeyId, string Key, int CharacterId)
        {
            var xml = XDocument.Load("https://api.eveonline.com/char/CharacterSheet.xml.aspx?keyID=" + KeyId + "&vCode=" + Key + "&characterID=" + CharacterId + "");
            return xml;
        }


        private XDocument GetCharacterSkillQueue(int KeyId, string Key, int CharacterId)
        {
            var xml = XDocument.Load("https://api.eveonline.com//char/SkillQueue.xml.aspx?keyID=" + KeyId + "&vCode=" + Key + "&characterID=" + CharacterId + "");
            return xml;
        }

        #endregion

        #region Util Methods

        private string GetGroupSkillName(int Id)
        {
            Dictionary<int,string> dictNameGroupSkill = new Dictionary<int,string>();
            dictNameGroupSkill = createDictGroupSkillName();
            try
            {
                return dictNameGroupSkill[Id];
            }
            catch (Exception)
            {

                return "void";
            }
            

        }

        private Dictionary<int, string>  createDictGroupSkillName()
        {
            Dictionary<int, string> dictNameGroupSkill = new Dictionary<int,string>();
            dictNameGroupSkill.Add(255, "Gunnery");
            dictNameGroupSkill.Add(256, "Missile Launcher Operation");
            dictNameGroupSkill.Add(257, "Spaceship Command");
            dictNameGroupSkill.Add(258, "Leadership");
            dictNameGroupSkill.Add(266, "Corporation Management");
            dictNameGroupSkill.Add(268, "Industry");
            dictNameGroupSkill.Add(269, "Mechanic");
            dictNameGroupSkill.Add(270, "Science");
            dictNameGroupSkill.Add(271, "Engineering");
            dictNameGroupSkill.Add(272, "Electronics");
            dictNameGroupSkill.Add(273, "Drones");
            dictNameGroupSkill.Add(274, "Trade");
            dictNameGroupSkill.Add(275, "Navigation");
            dictNameGroupSkill.Add(278, "Social");
            dictNameGroupSkill.Add(505, "Fake Skills");
            dictNameGroupSkill.Add(989, "Subsystems");
            dictNameGroupSkill.Add(1044, "Planet Management");
            return dictNameGroupSkill;
        }

        //private Character GetCreateCharacterMenu()
        //{
        //    Character oCharacter = new Character();
        //    oCharacter.CharacterId = 0;
        //    oCharacter.Image = "http://image.eveonline.com/Character/0_256.jpg";
        //    oCharacter.CharacterName = "Account Managment";
        //    oCharacter.SkillTraining = new Skill();
        //    oCharacter.account = new Account();
        //    oCharacter.account.key = "AccountMenu";
        //    oCharacter.account.Type = "AccountMenu";
        //    oCharacter.account.Title = "Account Managment";
        //    return oCharacter;

        //}

        private string BalanceToString(decimal value)
        {          
                return value.ToString("0,0.0");         
        }

        #endregion
    }
}
