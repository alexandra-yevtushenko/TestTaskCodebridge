using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using TestTaskCodebridge.DataAccess;
using TestTaskCodebridge.DataAccess.Entitites;
using TestTaskCodebridge.DTOs;

namespace TestTaskCodebridge.Services
{
    public class DogsService
    {
        private readonly DogsDbContext _context;

        public DogsService(DogsDbContext dogsDbContext)
        {
            _context = dogsDbContext;
        }


        public async Task<IEnumerable<DisplayDogDTO>> GetAllDogsAsync(string attribute, 
            string order, int pageNumber, int pageSize)
        {
            ValidateQueryParametersGetAllDogs(attribute, order, pageNumber, pageSize);

            return await _context
                .Dogs
                .AsNoTracking()
                .Select(d => new DisplayDogDTO 
                {
                    Name = d.Name, 
                    Color = d.Color, 
                    TailLength = d.TailLength, 
                    Weight = d.Weight 
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                //.OrderBy(d => )
                .ToListAsync();
        }

        private void ValidateQueryParametersGetAllDogs(string attribute,
            string order, int pageNumber, int pageSize)
        {
            List <string> sortingAttributes = new List<string>() { "name", "color", "tail_lengh", "weight"};
            List<string> sortingOrders = new List<string>() { "asc", "desc" };

            if (!sortingAttributes.Contains(attribute))
            {
                throw new IncorrectQueryArgumentException("GetAllDogs: Attribute argument is incorrect!");
            }
            if (!sortingOrders.Contains(order))
            {
                throw new IncorrectQueryArgumentException("GetAllDogs: Order argument is incorrect!");
            }
            if (pageNumber < 0)
            {
                throw new IncorrectQueryArgumentException("GetAllDogs: PageNumber argument is incorrect!");
            }
            if (pageSize <= 0)
            {
                throw new IncorrectQueryArgumentException("GetAllDogs: PageSize argument is incorrect!");
            }
        }

        public async Task<DisplayDogDTO> CreateNewDogAsync(CreateDogDTO createDogDTO)
        {
            await ValidateNewDogAsync(createDogDTO);

            DogEntity newDog = new DogEntity
            { 
                Name = createDogDTO.Name, 
                Color = createDogDTO.Color, 
                TailLength = createDogDTO.TailLength,
                Weight = createDogDTO.Weight
            };

            await _context.Dogs.AddAsync(newDog);
            await _context.SaveChangesAsync();

            return new DisplayDogDTO 
            {
                Name = newDog.Name, 
                Color = newDog.Color, 
                TailLength = newDog.TailLength, 
                Weight = newDog.Weight 
            };
        }

        private async Task ValidateNewDogAsync(CreateDogDTO createDogDTO)
        {
            if (createDogDTO.Name.IsNullOrEmpty())
            {
                throw new IncorrectObjectParameterException("CreateNewDog: Dog Name is incorrect!");
            }

            if (createDogDTO.TailLength < 0) 
            {
                throw new IncorrectObjectParameterException("CreateNewDog: Tail length is incorrect!");    
            }

            if (createDogDTO.Weight <= 0)
            {
                throw new IncorrectObjectParameterException("CreateNewDog: Weight is incorrect!");
            }

            var existentDogNames = await _context.Dogs.Select(d => d.Name).ToListAsync();
            if (existentDogNames.Contains(createDogDTO.Name))
            {
                throw new IncorrectObjectParameterException("CreateNewDog: Dog with this name is already exists!");
            }
        }


    }
}
