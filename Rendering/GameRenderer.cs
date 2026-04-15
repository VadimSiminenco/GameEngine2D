namespace GameEngine2D.Rendering
{
    public class GameRenderer
    {
        public void Render(IGameDrawable drawable)
        {
            drawable.Draw();
        }
    }
}