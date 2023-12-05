using Infrastructure.Providers;
using PlaygroundModule.ModelPart;
using PlaygroundModule.ViewPart;
using UnityEngine;


namespace PlaygroundModule.PresenterPart
{
    public class CellFactory
    {
        private readonly CellDataProvider _cellDataProvider;
        
        public CellFactory(CellDataProvider cellDataProvider)
        {
            _cellDataProvider = cellDataProvider;
        }
        
        public CellView InstantiateCell(CellType info, Transform parent, Vector3 spawnPosition)
        {
            CellView cellObject = Object.Instantiate(_cellDataProvider.GetCellPixelPrefabs(info)[0], parent);
            cellObject.Destroy += OnDestroy;
            cellObject.transform.position = new Vector3(spawnPosition.x, cellObject.transform.position.y, spawnPosition.z);
            
            return cellObject;
        }

        private void OnDestroy(CellView gameObject)
        {
            Object.Destroy(gameObject.gameObject);
        }
    }
}