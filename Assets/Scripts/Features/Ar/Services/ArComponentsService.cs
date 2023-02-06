using System;
using Features.Ar.Factories;
using Features.Ar.Models;
using Features.ImageTracking.Views;
using Zenject;

namespace Features.Ar.Services
{
    public class ArComponentsService : IDisposable
    {
        private readonly ArComponentsModel _arComponentsModel;
        private ArComponentsView _arComponentsView;

        private readonly ArComponentsViewFactory _arComponentsViewFactory;


        public ArComponentsService(ArComponentsModel arComponentsModel, ArComponentsViewFactory arComponentsViewFactory)
        {
            _arComponentsModel = arComponentsModel;
            _arComponentsViewFactory = arComponentsViewFactory;
        }

        public void Enable()
        {
            _arComponentsView = _arComponentsView ? _arComponentsView : _arComponentsViewFactory.Create();
            _arComponentsModel.Bind(_arComponentsView);
        }

        public void Disable()
        {
            _arComponentsModel.UnBind();

            Dispose();
        }

        public void Dispose()
        {
            if (_arComponentsView == null) return;
            _arComponentsView.Dispose();
            _arComponentsView = null;
        }
    }
}