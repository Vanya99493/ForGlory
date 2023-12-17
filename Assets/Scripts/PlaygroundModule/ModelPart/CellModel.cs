using System;
using System.Collections.Generic;
using CharacterModule.PresenterPart;
using PlaygroundModule.ViewPart;
using UnityEngine;

namespace PlaygroundModule.ModelPart
{
    public class CellModel
    {
        public event Action<List<TeamPresenter>> TeamsCollisionAction;
        
        private CellType _cellType;
        private CellView _view;
        private TeamPresenter _preTeamOnCell; 

        public CellType CellType => _cellType;
        
        public int CellHeightId { get; private set; }
        public int CellWidthId { get; private set; }
        public List<TeamPresenter> TeamsOnCell { get; private set; }
        public Vector3 MoveCellPosition => _view.MoveCellPosition;

        public CellModel(CellType cellType, int cellHeightId, int cellWidthId)
        {
            _cellType = cellType;
            CellHeightId = cellHeightId;
            CellWidthId = cellWidthId;

            TeamsOnCell = new List<TeamPresenter>();
        }

        public void IntitializeView(CellView view)
        {
            _view = view;
        }

        public bool PreSetCharacterOnCell(TeamPresenter team)
        {
            //Debug.Log($"TRY set on {CellHeightId}:{CellWidthId}");
            if (team is PlayerTeamPresenter && _preTeamOnCell == null)
            {
                if (TeamsOnCell.Count == 0 || (TeamsOnCell.Count == 1 && TeamsOnCell[0] is EnemyTeamPresenter))
                {
                    _preTeamOnCell = team;
                    return true;
                }
            }
            else if (team is EnemyTeamPresenter && _preTeamOnCell == null)
            {
                if (TeamsOnCell.Count == 0 || (TeamsOnCell.Count == 1 && TeamsOnCell[0] is PlayerTeamPresenter))
                {
                    _preTeamOnCell = team;
                    return true;
                }
            }

            return false;
        }

        public bool SetCharacterOnCell(TeamPresenter team, bool isFirstInitialization = false)
        {
            if (isFirstInitialization && TeamsOnCell.Count > 0)
            {
                return false;
            }
            
            TeamsOnCell.Add(team);
            _preTeamOnCell = null;
            //Debug.Log($"Set on {CellHeightId}:{CellWidthId}");

            if (TeamsOnCell.Count >= 2)
            {
                TeamsCollisionAction?.Invoke(TeamsOnCell);
            }
            
            return true;
        }

        public bool CheckCellOnCharacters()
        {
            return TeamsOnCell.Count > 0;
        }
        
        public void RemoveCharacterFromCell(TeamPresenter team)
        {
            TeamsOnCell.Remove(team);
        }
    }
}