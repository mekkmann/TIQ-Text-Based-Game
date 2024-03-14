
namespace Text_Based_Game.Classes
{
    internal class GameManager
    {
        Player Player = new();
        GamePath? currentPath;
        int MaxPathLength = 20;
        int MinPathLength = 10;
        bool IsGameRunning = true;
        // METHODS

        /// <summary>
        /// 
        /// </summary>
        public void StartGame()
        {
            // instantiate Tutorial Path
            currentPath = new("the Tutorial Path", 7, Player, PathDifficulty.Easy);
            currentPath.Start();

            while (IsGameRunning)
            {
                // to keep console open
                Console.ReadLine();
            }
        }

        //(string name, int pathLength, Player player, PathDifficulty difficulty)
        public void GeneratePath(PathDifficulty chosenDifficulty)
        {
            Random random = new();

            string[] possiblePathNames = ["the dark road", "this trashed alley", "wherever this cat is going..."];
            string randomPathName = possiblePathNames[random.Next(possiblePathNames.Length)];

            int randomPathLength = random.Next(MinPathLength, MaxPathLength + 1);

            GamePath newPath = new(
                randomPathName,
                randomPathLength,
                Player,
                chosenDifficulty
                );

            currentPath = newPath;
        }
    }
}
