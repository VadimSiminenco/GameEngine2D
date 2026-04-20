using GameEngine2D.Entities.Characters;

namespace GameEngine2D.Observers
{
    public class PlayerLogObserver : IPlayerObserver
    {
        private readonly IPlayerSubject subject;
        private string observerState = string.Empty;

        public PlayerLogObserver(IPlayerSubject subject)
        {
            this.subject = subject;
        }

        public void Update()
        {
            observerState = subject.GetState();
            Console.WriteLine($"[Observer: Log] Player state changed to: {observerState}");
        }
    }
}