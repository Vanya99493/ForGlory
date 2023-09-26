using Infrastructure.Providers;
using PlaygroundModule.ModelPart;
using PlaygroundModule.ViewPart;
using UnityEngine;


namespace PlaygroundModule.PresenterPart
{
    public class CellFactory
    {
        private readonly CellPixelsPrefabsProvider _cellPixelsPrefabsProvider;

        private readonly float _cellSizeHeight;
        private readonly float _cellSizeWidth;
        
        private readonly float _startXPosition;
        private readonly float _startZPosition;
        
        public CellFactory(CellPixelsPrefabsProvider cellPixelsPrefabsProvider, float cellSizeHeight, float cellSizeWidth)
        {
            _cellPixelsPrefabsProvider = cellPixelsPrefabsProvider;
            _cellSizeHeight = cellSizeHeight;
            _cellSizeWidth = cellSizeWidth;

            _startXPosition = cellSizeWidth / -2.0f;
            _startZPosition = cellSizeHeight / 2.0f;
        }
        
        public CellView InstantiateCell(CellPixelInfo[,] info, Transform parent)
        {
            CellView cellObject = new GameObject().AddComponent<CellView>();
            cellObject.Destroy += OnDestroy;
            cellObject.name = "Cell";
            cellObject.transform.SetParent(parent);

            (float cellHeight, float cellWidth) = (
                    _cellSizeHeight / info.GetLength(0),
                    _cellSizeWidth / info.GetLength(1)
                    );
            Vector3 cellPixelSize = new Vector3(
                _cellSizeWidth / info.GetLength(1), 
                1f, 
                _cellSizeHeight / info.GetLength(0)
                );

            for (int heightIndex = 0; heightIndex < info.GetLength(0); heightIndex++)
            {
                for (int widthIndex = 0; widthIndex < info.GetLength(1); widthIndex++)
                {
                    CellPixelView cellPixelView =
                        InstantiateCellPixel(info[heightIndex, widthIndex], cellObject.transform, cellPixelSize);
                    cellPixelView.transform.position = GetCellPixelPosition(cellHeight, cellWidth, heightIndex, widthIndex);
                }
            }
            
            
            return cellObject;
        }

        private CellPixelView InstantiateCellPixel(CellPixelInfo info, Transform parent, Vector3 size)
        {
            CellPixelView cellPixelObject = Object.Instantiate(_cellPixelsPrefabsProvider.GetCellPixelPrefab(info), parent);
            cellPixelObject.transform.localScale = size;
            return cellPixelObject;
        }

        private Vector3 GetCellPixelPosition(float height, float width, int heightIndex, int widthIndex)
        {
            return new Vector3(
                _startXPosition + (width / 2.0f + widthIndex * width),
                0,
                _startZPosition - (height / 2.0f + heightIndex * height)
                );
        }

        private void OnDestroy(CellView gameObject)
        {
            Object.Destroy(gameObject.gameObject);
        }
    }
}