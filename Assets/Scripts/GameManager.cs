using UnityEngine;

namespace XRMultiplayer.MiniGames
{
    // game variables:
    // timer
    // message to be displayed
    // leaderboard - at the end

    public class GameManager : MiniGameBase
    {
        int currentPlayerScore = 0;
        public override void StartGame()
        {
            base.StartGame();

            // reset player score
        }

        public override void FinishGame(bool submitScore = true)
        {
            base.FinishGame(submitScore);

            // functionality for end of game
            // show congratulations for end of game
        }

        // TODO - create method to track player score
        public void LocalPlayerIncreaseScore(int score)
        {
            if(m_MiniGameManager.currentNetworkedGameState == MiniGameManager.GameState.InGame)
            {
                currentPlayerScore += score;
            }
        }
    }
}