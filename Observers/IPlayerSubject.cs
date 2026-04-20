namespace GameEngine2D.Observers
{
    public interface IPlayerSubject
    {
        void Attach(IPlayerObserver observer);
        void Detach(IPlayerObserver observer);
        void Notify();
        string GetState();
    }
}