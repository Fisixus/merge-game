using DI;
using MVP.Models;
using MVP.Models.Interface;
using MVP.Presenters;
using MVP.Presenters.Handlers;
using MVP.Views;
using MVP.Views.Interface;
using UnityEngine;

namespace Installers.MergeScene
{
    public class MVPMergeInstaller : Installer
    {
        [SerializeField] private GridView _gridView;
        [SerializeField] private MergeUIView _mergeUIView;

        protected override void InstallBindings()
        {
            Container.BindAsSingle<IGridModel>(() => Container.Construct<GridModel>());
            Container.BindAsSingle<ITaskModel>(() => Container.Construct<TaskModel>());
            Container.BindAsSingle<IGridView>(() => _gridView);
            Container.BindAsSingle<IMergeUIView>(() => _mergeUIView);
            
            Container.BindAsSingleNonLazy(() => Container.Construct<GridPresenter>());
            Container.BindAsSingleNonLazy(() => Container.Construct<TaskPresenter>());
            Container.BindAsSingleNonLazy(() => Container.Construct<MergePresenter>());
        }
    }
}