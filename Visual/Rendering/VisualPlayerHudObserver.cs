using GameEngine2D.Observers;

namespace GameEngine2D.Visual.Rendering
{
    public class VisualPlayerHudObserver : IPlayerObserver
    {
        private readonly IPlayerSubject subject;

        public string CurrentPlayerState { get; private set; }

        public VisualPlayerHudObserver(IPlayerSubject subject)
        {
            this.subject = subject;
            CurrentPlayerState = subject.GetState();
        }

        public void Update()
        {
            CurrentPlayerState = subject.GetState();
        }
    }
}