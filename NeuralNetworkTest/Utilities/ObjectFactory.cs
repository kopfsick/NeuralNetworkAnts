using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace NeuralNetworkTest.Utilities
{
    public interface IObjectFactory<out T> : IDisposable
    {
        T CreateInstance();
    }

    [Export(typeof(IObjectFactory<>))]
    public class ObjectFactory<T> : IObjectFactory<T>
    {
        private readonly ExportFactory<T> _factory;
        private readonly List<ExportLifetimeContext<T>> _exports;

        [ImportingConstructor]
        public ObjectFactory(ExportFactory<T> factory)
        {
            _factory = factory;
            _exports = new List<ExportLifetimeContext<T>>();
        }

        public T CreateInstance()
        {
            var exportLifetimeContext = _factory.CreateExport();
            _exports.Add(exportLifetimeContext);

            return exportLifetimeContext.Value;
        }

        public void Dispose()
        {
            foreach (var context in _exports)
                context.Dispose();

            _exports.Clear();
        }
    }
}