namespace ReqResponse.Middleware.Services.Client.Factories
{
    public class TestRequesteServiceClientFactory
    {
        public static IRequestService _service;

        public static void SetService(IRequestService service)
        {
            _service = service;
        }

        public static ITestRequestServiceClient CreateService()
        {
            return new TestRequestServiceClient(_service);
        }
    }
}