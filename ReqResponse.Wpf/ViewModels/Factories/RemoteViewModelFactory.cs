using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqResponse.Wpf.ViewModels.Factories
{

    public class RemoteViewModelFactory : IViewModelFactory<RemoteViewModel>
    {
        public RemoteViewModel CreateViewModel()
        {
            return new RemoteViewModel();
        }
    }
}
