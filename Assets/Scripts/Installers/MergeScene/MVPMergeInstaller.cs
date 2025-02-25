using DI;
using MVP.Models;
using MVP.Models.Interface;
using MVP.Presenters;
using MVP.Views;
using MVP.Views.Interface;
using UnityEngine;

namespace Installers.MergeScene
{
    public class MVPMergeInstaller : Installer
    {
        [SerializeField] private GridView _gridView;
        [SerializeField] private MergeUIView _mergeUIView;
        [SerializeField] private InventoryView _inventoryUIView;

        protected override void InstallBindings()
        {
            Container.BindAsSingleNonLazy<IGridModel>(() => Container.Construct<GridModel>());
            Container.BindAsSingle<ITaskModel>(() => Container.Construct<TaskModel>());
            Container.BindAsSingle<IInventoryModel>(() => Container.Construct<InventoryModel>());
            Container.BindAsSingle<IGridView>(() => _gridView);
            Container.BindAsSingle<IMergeUIView>(() => _mergeUIView);
            Container.BindAsSingle<IInventoryView>(() => _inventoryUIView);

            Container.BindAsSingleNonLazy(() => Container.Construct<GridPresenter>());
            Container.BindAsSingleNonLazy(() => Container.Construct<TaskPresenter>());
            Container.BindAsSingleNonLazy(() => Container.Construct<InventoryPresenter>());
            Container.BindAsSingleNonLazy(() => Container.Construct<MergePresenter>());
        }
    }
}