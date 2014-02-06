using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iEve8Lib.BLL
{
    public sealed class Skill
    {
        public int Id { get; set; }
        public string SkillName { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int SkillPoints { get; set; }
        public int Level { get; set; }
        public string LevelImage { get; set; }
        public string Time { get; set; }
    }

   
}
