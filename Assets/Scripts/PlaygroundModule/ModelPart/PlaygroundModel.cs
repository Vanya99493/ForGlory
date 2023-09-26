using PlaygroundModule.ViewPart;

namespace PlaygroundModule.ModelPart
{
    public class PlaygroundModel
    {
        private PlaygroundView _view;
        

        public PlaygroundModel(PlaygroundView playgroundView)
        {
            _view = playgroundView;
        }
    }
}