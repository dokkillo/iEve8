using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iEve8Lib.BLL
{
    public sealed class Character
    {
        public int CharacterId { get; set; }
        public string CharacterName { get; set; }
        public int CorpId { get; set; }
        public string CorporationName { get; set; }
        public Skill SkillTraining { get; set; }
        public string Image { get; set; }
        public string Balance { get; set; }
        public string Race { get; set; }
        public string BloodLine { get; set; }
        public string AllianceName { get; set; }
        public Account account { get; set; }
        public string totalSkillPoints { get; set; }
        public IList<Skill> SkillQueue { get; set; }
        public IList<Skill> SkillList { get; set; }
    }
}
