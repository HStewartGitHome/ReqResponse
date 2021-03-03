using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqResponse.Wpf.ViewModels.Factories
{
    public class LocalViewModelFactory : IViewModelFactory<LocalViewModel>
    {
     
        public LocalViewModel CreateViewModel()
        {
            return new LocalViewModel();
        }
    }
}
