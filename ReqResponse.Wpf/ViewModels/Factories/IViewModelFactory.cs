using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqResponse.Wpf.ViewModels.Factories
{
    public interface  IViewModelFactory<T> where T : BaseViewModel
    {
        T CreateViewModel();
    }
}
