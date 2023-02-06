using Features.LightSystem.Views;

namespace Features.LightSystem.Messages
{
    public static class LightSignals
    {
        public sealed class RegisterNewDirectionalLightHandler
        {
            public string Id { get; }
            public DirectionalLightHandlerView DirectionalLightHandlerView { get; }

            public RegisterNewDirectionalLightHandler(string id, DirectionalLightHandlerView directionalLightHandlerView)
            {
                Id = id;
                DirectionalLightHandlerView = directionalLightHandlerView;
            }
        }
    }
}