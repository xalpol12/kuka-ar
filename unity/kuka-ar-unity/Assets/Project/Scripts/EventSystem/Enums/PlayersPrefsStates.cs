namespace Project.Scripts.EventSystem.Enums
{
    public class PlayersPrefsStates
    {
        public int FirstRun { get; private set; }
        public int NthRun { get; private set; }

        public PlayersPrefsStates()
        {
            FirstRun = 0;
            NthRun = 1;
        }
    }
}