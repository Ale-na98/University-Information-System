using System.Linq;
using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace module_10
{
    [ApiController]
    [Route("/api/hometasks")]
    public class HometaskController : ControllerBase
    {
        private readonly IHometasksService _hometasksService;

        public HometaskController(IHometasksService hometasksService)
        {
            _hometasksService = hometasksService;
        }

        [HttpPost]
        public ActionResult<string> Create(HometaskDto hometaskDto)
        {
            var id = _hometasksService.Create(hometaskDto);
            return Ok($"api/hometasks/{id}");
        }

        [HttpGet("{id}")]
        public ActionResult<HometaskDto> Get(int id)
        {
            return Ok(_hometasksService.Get(id));
        }

        [HttpGet]
        [HttpGet(".{format}"), FormatFilter]
        public ActionResult<IReadOnlyCollection<HometaskDto>> GetAll()
        {
            return Ok(_hometasksService.GetAll());
        }

        [HttpPut("{id}")]
        public ActionResult<string> Update(int id, HometaskDto hometaskDto)
        {
            _hometasksService.Update(id, hometaskDto with { Id = id });
            return Ok($"api/hometasks/{id}");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _hometasksService.Delete(id);
            return Ok($"The hometask with Id {id} was deleted.");
        }
    }
}