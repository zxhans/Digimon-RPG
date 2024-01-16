using Digimon_Project.Database.Results;

namespace Digimon_Project.Utils
{
    public static class Skill
    {
        public static Game.Entities.Skill GetSkill(int id)
        {
            SkillResult result = Emulator.Enviroment.Database.Select<SkillResult>("s.skill_id AS id"
                + ", s.name AS Name, s.required_level AS Lvl, s.units AS Units, s.range_type AS range_type"
                + ", s.vp, s.power"
                , "digimon_skills s", "WHERE skill_id=@id", new Database.QueryParameters() { { "id", id } });

            return result.skill;
        }
    }
}
