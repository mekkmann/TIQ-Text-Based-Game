namespace Text_Based_Game.Classes
{
    enum PathStepType
    {
        Walking,
        PlayerTalk,
        MobFight,
        BossFight
    }
    internal class PathStep
    {
        public PathStepType Type;

        public PathStep(PathStepType type)
        {
            Type = type;
        }
    }
}
