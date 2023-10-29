using PlaygroundModule.ModelPart;
using UnityEngine;

namespace PlaygroundModule.PresenterPart
{
    public class PlaygroundSpawner
    {
        public void SpawnPlayground(CellFactory cellFactory, PlaygroundModel model, Transform parent, float playgroundSizeHeight, float playgroundSizeWidth)
        {
            float cellSizeHeight = playgroundSizeHeight / model.Height;
            float cellSizeWidth = playgroundSizeWidth / model.Width;
            
            float xStartSpawnPosition = -1 * (model.Width / 2.0f + cellSizeWidth / 2.0f);
            float zStartSpawnPosition = model.Height / 2.0f - cellSizeHeight / 2.0f;

            for (int i = 0; i < model.Height; i++)
            {
                for (int j = 0; j < model.Width; j++)
                {
                    var cellInfo = model.GetCell(i, j).CellType;
                    var cellSpawnPosition = new Vector3(
                        xStartSpawnPosition + cellSizeWidth * j,
                        0f,
                        zStartSpawnPosition - cellSizeHeight * i
                    );
                    cellFactory.InstantiateCell(cellInfo, parent, cellSpawnPosition);
                }
            }
        }
    }
}