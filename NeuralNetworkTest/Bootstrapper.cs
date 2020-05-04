using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Registration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace NeuralNetworkTest
{
    public class Bootstrapper
    {
        public void Run()
        {
            var container = CreateContainer();

            Initialize(container);

            StartShell(container);
        }

        private void Initialize(CompositionContainer container)
        {
            var controller = container.GetExportedValue<IMainController>();
            controller.Setup();
            controller.Start();
        }

        private static void StartShell(CompositionContainer container)
        {
            var mainViewModel = container.GetExportedValue<IMainViewModel>();
            new Shell {Content = mainViewModel}.Show();
        }

        private CompositionContainer CreateContainer()
        {
            var regBuilder = new RegistrationBuilder();
            var catalog = new AssemblyCatalog(GetType().Assembly);
            var provider = new CatalogExportProvider(catalog, true);
            var container = new CompositionContainer(provider);
            provider.SourceProvider = container;

            return container;
        }
    }
}
