using UnityEngine;

namespace XRMultiplayer.MiniGames
{
    public class GameManager : MiniGameBase
    {
        public override void StartGame()
        {
            base.StartGame();

            // reset player score
        }

        public override void FinishGame(bool submitScore = true)
        {
            base.FinishGame(submitScore);

            // functionality for end of game
        }


    }
}