using Infrastructure.Providers;
using PlaygroundModule.ModelPart;
using PlaygroundModule.ViewPart;
using UnityEngine;


namespace PlaygroundModule.PresenterPart
{
    public class CellFactory
    {
        private readonly CellPrefabsProvider _cellPrefabsProvider;
        
        public CellFactory(CellPrefabsProvider cellPrefabsProvider)
        {
            _cellPrefabsProvider = cellPrefabsProvider;
        }
        
        public CellView InstantiateCell(CellType info, Transform parent, Vector3 spawnPosition)
        {
            CellView cellObject = Object.Instantiate(_cellPrefabsProvider.GetCellPixelPrefabs(info)[0], parent);
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