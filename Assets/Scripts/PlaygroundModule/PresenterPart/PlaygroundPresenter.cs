using Infrastructure.Providers;
using PlaygroundModule.ModelPart;
using UnityEngine;

namespace PlaygroundModule.PresenterPart
{
    public class PlaygroundPresenter
    {
        private readonly PlaygroundModel _model;

        private CellFactory _cellFactory;

        public PlaygroundPresenter(PlaygroundModel playgroundModel, CellPixelsPrefabsProvider cellPixelsPrefabsProvider)
        {
            _model = playgroundModel;
            _cellFactory = new CellFactory(cellPixelsPrefabsProvider, 1.0f, 1.0f);
        }

        public void SpawnPlayground(Transform parent)
        {
            CellPixelInfo[,] info = new CellPixelInfo[3, 3]
            {
                { CellPixelInfo.Forest, CellPixelInfo.DarkForest, CellPixelInfo.River },
                { CellPixelInfo.Plain, CellPixelInfo.Plain, CellPixelInfo.Plain },
                { CellPixelInfo.Mountain, CellPixelInfo.Hill, CellPixelInfo.Sea }
            };
            _cellFactory.InstantiateCell(info, parent);
        }
    }
}