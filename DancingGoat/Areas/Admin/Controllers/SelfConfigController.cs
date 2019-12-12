using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DancingGoat.Areas.Admin.Abstractions;
using DancingGoat.Areas.Admin.Infrastructure;
using DancingGoat.Areas.Admin.Models;
using DancingGoat.Helpers;
using Kentico.Kontent.Delivery;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DancingGoat.Areas.Admin.Controllers
{
    public class SelfConfigController : Controller
    {
        private readonly IAppSettingProvider _settingProvider;
        protected const string CAPTION_CONFIGURATION_WRITE_ERROR = "Configuration Save Error";
        protected const string CAPTION_DESERIALIZATION_ERROR = "API Response Deserialization Error";

        protected const string MESSAGE_SELECTED_PROJECT =
                "You've configured your app with a project ID \"{0}\". You can edit its contents at https://app.kontent.ai/."
            ;

        protected const string MESSAGE_SHARED_PROJECT =
            "You've configured your app to with a project ID of a shared Kentico Kontent project.";

        protected const string MESSAGE_INVALID_PROJECT_GUID = "ProjectId is not valid GUID.";

        public const int PROJECT_EXISTENCE_VERIFICATION_REQUIRED_ITEMS = 30;

        protected readonly ISelfConfigManager _selfConfigManager;
        protected IDeliveryClient client;

        public SelfConfigController(IAppSettingProvider settingProvider, IDeliveryClient deliveryClient, ISelfConfigManager selfConfigManager)
        {
            _settingProvider = settingProvider;
            _selfConfigManager = selfConfigManager;
            client = deliveryClient;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new IndexViewModel());
        }

        [HttpGet]
        public ActionResult Done()
        {
            return RedirectHelpers.GetHomeRedirectResult(new MessageModel { Caption = null, Message = string.Format(MESSAGE_SELECTED_PROJECT, _settingProvider.GetProjectId().Value), MessageType = MessageType.Info });
        }

        [HttpGet]
        public async Task<ActionResult> SampleProjectReady()
        {
            var items = (await client.GetItemsAsync()).Items;
            return Json(items.Count >= PROJECT_EXISTENCE_VERIFICATION_REQUIRED_ITEMS);
        }

        [HttpPost]
        public ActionResult UseShared()
        {
            return SetConfiguration(MESSAGE_SHARED_PROJECT, _settingProvider.GetDefaultProjectId().Value, null, false);
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
            catch (JsonSerializationException ex)
            {
                return View("Error", new MessageModel { Caption = CAPTION_DESERIALIZATION_ERROR, Message = ex.Message, MessageType = MessageType.Error });
            }
            catch (Exception ex)
            {
                return View("Error", new MessageModel { Caption = CAPTION_CONFIGURATION_WRITE_ERROR, Message = ex.Message, MessageType = MessageType.Error });
            }

        }
    }
}