namespace ReqResponse.Middleware.Services.Client.Factories
{
    public class TestModelRequestServiceClientFactory
    {
        public static IRequestService _service;

        public static void SetService(IRequestService service)
        {
            _service = service;
        }

        public static ITestModelRequestServiceClient CreateService()
        {
            ITestRequestServiceClient client = new TestRequestServiceClient(_service);
            return new TestModelRequestServiceClient(client);
        }
    }
}