using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DotNet_Rpg.Data;
using DotNet_Rpg.Dtos.Character;
using Microsoft.EntityFrameworkCore;

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
        private readonly DataContext _context;
        public CharacterService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters(int userId)
        {
            var dbCharacters = await _context.Characters.Where(c => c.User!.Id == userId).ToListAsync();

            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>
            {
                Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList()
            };

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);

            var serviceResponse = new ServiceResponse<GetCharacterDto>
                {
                    Data = _mapper.Map<GetCharacterDto>(dbCharacter)
                };

                return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = _mapper.Map<Character>(newCharacter);

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

           serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try 
            {
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updateCharacter.Id);

                if (character is null) 
                {
                    throw new Exception($"Character with ID: {updateCharacter.Id} not found.");
                }

                character.Name = updateCharacter.Name ;
                character.HitPoints = updateCharacter.HitPoints ;
                character.Strength = updateCharacter.Strength ;
                character.Defense = updateCharacter.Defense ;
                character.Intelligence = updateCharacter.Intelligence ;
                character.Class = updateCharacter.Class ;

                _mapper.Map<GetCharacterDto>(character);

                await _context.SaveChangesAsync();
                
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = true;
                serviceResponse.Message = ex.Message;
            }
            
           
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try 
            {
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);

                if (character is null) 
                {
                    throw new Exception($"Character with ID: {id} not found.");
                }

                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
                
                serviceResponse.Data = _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
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