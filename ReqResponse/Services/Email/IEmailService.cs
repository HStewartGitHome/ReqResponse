﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReqResponse.Services.Email
{
    public interface IEmailService
    {
        Task EmailErrorReportStrings(List<string> strs);
    }
}