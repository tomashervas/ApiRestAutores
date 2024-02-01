using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace apiAutores.Utilities
{
    public class SwagggerByVersion : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var nameSpaceController = controller.ControllerType.Namespace; //Controllers.V1
            var ApiVersion = nameSpaceController!.Split(".").Last().ToLower(); //V1
            controller.ApiExplorer.GroupName = ApiVersion;
        }
    }
}
