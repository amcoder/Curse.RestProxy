using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Curse.RestProxy.Controllers
{
    [RoutePrefix("api/addon/{addonId}")]
    public class AddOnFilesController: ApiController
    {
        private AddOnService.IAddOnService AddOnService { get; set; }

        public AddOnFilesController(AddOnService.IAddOnService addOnService)
        {
            AddOnService = addOnService;
        }

        [HttpGet]
        [Route("files", Name = "AddOnFiles")]
        public async Task<IHttpActionResult> Index(int addonId)
        {
            var result = await AddOnService.GetAllFilesForAddOnAsync(addonId);

            if(result == null)
                return NotFound();

            return Ok(new
            {
                Files = result
            });
        }

        [HttpGet]
        [Route("file/{fileId}", Name = "AddOnFile")]
        public async Task<IHttpActionResult> Get(int addonId, int fileId)
        {
            var result = await AddOnService.GetAddOnFileAsync(addonId, fileId);

            if(result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        [Route("file/{fileId}/changelog", Name = "AddOnFileChangelog")]
        public async Task<IHttpActionResult> Changelog(int addonId, int fileId)
        {
            var result = await AddOnService.v2GetChangeLogAsync(addonId, fileId);

            if(result == null)
                return NotFound();

            return Ok(result);
        }
    }
}