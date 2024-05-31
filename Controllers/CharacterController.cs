global using DotNet_Rpg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNet_Rpg.Services.CharacterService;
using Microsoft.AspNetCore.Mvc;

namespace DotNet_Rpg.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController(ICharacterService characterService) : ControllerBase
    {
        private readonly ICharacterService characterService = characterService;

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<Character>>>> Get() 
        {
            return Ok(await characterService.GetAllCharacters());
        }

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<ServiceResponse<Character>>> GetSingle(int id) 
        {
            return Ok(await characterService.GetCharacterById(id));
        }

        [HttpPost("AddCharacter")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto c)
        {
            await characterService.AddCharacter(c);

            return Ok(characterService.GetAllCharacters());
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            var response = await characterService.UpdateCharacter(updateCharacter);

            if (response.Data is null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> DeleteCharacter(int id)
        {
             var response = await characterService.DeleteCharacter(id);

            if (response.Data is null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}