using GameEngine2D.Entities.Characters;

namespace GameEngine2D.Observers
{
    public class PlayerHudObserver : IPlayerObserver
    {
        private readonly IPlayerSubject subject;
        private string observerState = string.Empty;

        public PlayerHudObserver(IPlayerSubject subject)
        {
            this.subject = subject;
        }

        public void Update()
        {
            observerState = subject.GetState();
            Console.WriteLine($"[Observer: HUD] Refreshing HUD for player action: {observerState}");
        }
    }
}