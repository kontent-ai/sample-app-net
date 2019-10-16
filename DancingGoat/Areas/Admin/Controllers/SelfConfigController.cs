using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using DancingGoat.Helpers;
using DancingGoat.Areas.Admin.Helpers;
using DancingGoat.Areas.Admin.Models;
using Kentico.Kontent.Delivery;

namespace DancingGoat.Areas.Admin.Controllers
{
    public class SelfConfigController : Controller
    {
        protected const string CAPTION_CONFIGURATION_WRITE_ERROR = "Configuration Save Error";
        protected const string CAPTION_DESERIALIZATION_ERROR = "API Response Deserialization Error";

        protected const string MESSAGE_SELECTED_PROJECT =
                "You've configured your app with a project ID \"{0}\". You can edit its contents at https://app.kontent.ai/."
            ;

        protected const string MESSAGE_SHARED_PROJECT =
            "You've configured your app to with a project ID of a shared Kentico Kontent project.";

        protected const string MESSAGE_INVALID_PROJECT_GUID = "ProjectId is not valid GUID.";

        public const int PROJECT_EXISTENCE_VERIFICATION_REQUIRED_ITEMS = 30;

        protected readonly SelfConfigManager _selfConfigManager;
        protected readonly IDeliveryClient client = DancingGoat.Controllers.ControllerBase.CreateDeliveryClient();

        public SelfConfigController()
        {
            _selfConfigManager = new SelfConfigManager();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new IndexViewModel());
        }

        [HttpGet]
        public ActionResult Done()
        {
            return RedirectHelpers.GetHomeRedirectResult(new MessageModel { Caption = null, Message = string.Format(MESSAGE_SELECTED_PROJECT, AppSettingProvider.ProjectId.Value), MessageType = MessageType.Info });
        }

        [HttpGet]
        public async Task<ActionResult> SampleProjectReady()
        {             
            var items = (await client.GetItemsAsync()).Items;
            return Json(items.Count >= PROJECT_EXISTENCE_VERIFICATION_REQUIRED_ITEMS, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UseShared()
        {
            return SetConfiguration(MESSAGE_SHARED_PROJECT, AppSettingProvider.DefaultProjectId.Value, null, false);
        }

        [HttpPost]
        public ActionResult UseSelected(IndexViewModel model)
        {
            if (!model.ProjectGuid.HasValue)
            {
                return View("Error", new MessageModel { Caption = CAPTION_CONFIGURATION_WRITE_ERROR, Message = MESSAGE_INVALID_PROJECT_GUID, MessageType = MessageType.Error });
            }

            return SetConfiguration(string.Format(MESSAGE_SELECTED_PROJECT, model.ProjectGuid.Value), model.ProjectGuid.Value, model.EndAt, model.NewlyGeneratedProject);
        }

        private ActionResult SetConfiguration(string message, Guid projectGuid, DateTime? endAt, bool isNew)
        {
            try
            {
                _selfConfigManager.SetProjectIdAndExpirationAsync(projectGuid, endAt?.ToUniversalTime());

                if (isNew)
                {
                    return View("Wait");
                }

                return RedirectHelpers.GetHomeRedirectResult(new MessageModel { Caption = null, Message = message, MessageType = MessageType.Info });
            }
            catch (ConfigurationErrorsException ex)
            {
                return View("Error", new MessageModel { Caption = CAPTION_CONFIGURATION_WRITE_ERROR, Message = ex.Message, MessageType = MessageType.Error });
            }
            catch (JsonSerializationException ex)
            {
                return View("Error", new MessageModel { Caption = CAPTION_DESERIALIZATION_ERROR, Message = ex.Message, MessageType = MessageType.Error });
            }
        }
    }
}
