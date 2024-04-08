using System.Collections.Generic;
using CastleModule.PresenterPart;
using CharacterModule.PresenterPart;
using Infrastructure.Services;
using PlaygroundModule.PresenterPart;

namespace Infrastructure.SaveModule
{
    public delegate void SaveDelegate(PlaygroundPresenter playgroundPresenter, CastlePresenter castlePresenter,
        PlayerTeamPresenter playerTeamPresenter, List<EnemyTeamPresenter> enemyTeamPresenters,
        CharacterIdSetter characterIdSetter);
}