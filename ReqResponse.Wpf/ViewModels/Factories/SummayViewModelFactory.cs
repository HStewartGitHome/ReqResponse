using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqResponse.Wpf.ViewModels.Factories
{

    public class SummaryViewModelFactory : IViewModelFactory<SummaryViewModel>
    {
        public SummaryViewModel CreateViewModel()
        {
            return new SummaryViewModel();
        }
    }
}
