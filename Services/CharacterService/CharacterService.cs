using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DotNet_Rpg.Dtos.Character;

namespace DotNet_Rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character>() 
        {
            new Character() { Id = 1, Name = "Sam", },
            new Character() {},
            new Character() { Id = 2, Name = "Don Quixote", }
        };

        private readonly IMapper _mapper;
        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = _mapper.Map<Character>(newCharacter);
            character.Id = characters.Max(c => c.Id) + 1;

            characters.Add(character);
            serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try 
            {
                var character = characters.FirstOrDefault(c => c.Id == updateCharacter.Id);

                if (character is null) 
                {
                    throw new Exception($"Character with ID: {updateCharacter.Id} not found.");
                }

                _mapper.Map(updateCharacter, character);
                
                // character.Name = updateCharacter.Name;
                // character.HitPoints = updateCharacter.HitPoints;
                // character.Strength = updateCharacter.Strength;
                // character.Defense = updateCharacter.Defense;
                // character.Intelligence = updateCharacter.Intelligence;
                // character.Class = updateCharacter.Class;

                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = true;
                serviceResponse.Message = ex.Message;
            }
            
           
            return serviceResponse;
        }


        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>
            {
                Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList()
            };

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var character = characters.FirstOrDefault(c => c.Id == id);

            var serviceResponse = new ServiceResponse<GetCharacterDto>
                {
                    Data = _mapper.Map<GetCharacterDto>(character)
                };

                return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try 
            {
                var character = characters.FirstOrDefault(c => c.Id == id);

                if (character is null) 
                {
                    throw new Exception($"Character with ID: {id} not found.");
                }

                characters.Remove(character);
                
                serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = true;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}