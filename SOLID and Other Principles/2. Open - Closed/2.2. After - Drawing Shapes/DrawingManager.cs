namespace OpenClosedDrawingShapesAfter
{
    using OpenClosedDrawingShapesAfter.Contracts;

    public class DrawingManager : IDrawingManager
    {
        private ISurface surface = new ScreenSurface(); //not dependency injection :)

        public void Draw(IShape shape)
        {
            shape.DrawOnSurface(surface);
        }
    }
}
