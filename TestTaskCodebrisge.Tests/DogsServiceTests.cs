using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System.Xml.Linq;
using TestTaskCodebridge.Controllers;
using TestTaskCodebridge.DataAccess;
using TestTaskCodebridge.DataAccess.Entitites;
using TestTaskCodebridge.DTOs;
using TestTaskCodebridge.Exceptions;
using TestTaskCodebridge.Services;
using Xunit;

namespace TestTaskCodebrisge.Tests
{
    public class DogsServiceTests
    {
        private DogsService _service;
        private DbContextOptions<DogsDbContext> _options;
        private DogsDbContext _context;
        public DogsServiceTests()
        {
            _options = new DbContextOptionsBuilder<DogsDbContext>()
                    .UseInMemoryDatabase("DogsDbTests")
                    .Options;
        }

        private async Task AddDogsAsync(DogsDbContext context)
        {
            context.Dogs.AddRange(
                new DogEntity { Name = "Neo", Color = "red&amber", TailLength = 22, Weight = 32 },
                new DogEntity { Name = "Jessy", Color = "black&white", TailLength = 7, Weight = 14 },
                new DogEntity { Name = "Buddy", Color = "brown", TailLength = 15, Weight = 20 },
                new DogEntity { Name = "Max", Color = "white", TailLength = 10, Weight = 18 },
                new DogEntity { Name = "Rocky", Color = "black", TailLength = 12, Weight = 22 },
                new DogEntity { Name = "Luna", Color = "gray", TailLength = 8, Weight = 16 }
            );

            await context.SaveChangesAsync();
        }


        [Fact]
        public async Task GetAllDogs_ReturnsCorrectPagination_1_5()
        {
            _context = new DogsDbContext(_options);
            await AddDogsAsync(_context);

            _service = new DogsService(_context);

            var result = await _service.GetAllDogsAsync("name", "asc", 1, 5);
            
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }


        [Fact]
        public async Task GetAllDogs_ReturnsCorrectSorting_weight_desc()
        {
            _context = new DogsDbContext(_options);
            await AddDogsAsync(_context);

            _service = new DogsService(_context);

            var result = await _service.GetAllDogsAsync("weight", "desc", 1, 10);

            Assert.Equal(6, result.Count());
            Assert.Equal(32, result.First().Weight);
            Assert.Equal(14, result.Last().Weight);
        }


        [Fact]
        public async Task GetAllDogs_ReturnsExpectedDogsList()
        {
            _context = new DogsDbContext(_options);
            await AddDogsAsync(_context);

            _service = new DogsService(_context);

            var expectedDogs = new[]
            {
                new DogEntity { Name = "Buddy", Color = "brown", TailLength = 15, Weight = 20 },
                new DogEntity { Name = "Jessy", Color = "black&white", TailLength = 7, Weight = 14 },
                new DogEntity { Name = "Luna", Color = "gray", TailLength = 8, Weight = 16 },
                new DogEntity { Name = "Max", Color = "white", TailLength = 10, Weight = 18 },
                new DogEntity { Name = "Neo", Color = "red&amber", TailLength = 22, Weight = 32 },
                new DogEntity { Name = "Rocky", Color = "black", TailLength = 12, Weight = 22 }
            };

            var resultList = (await _service.GetAllDogsAsync("name", "asc", 1, 10)).ToList();

            Assert.Equal(expectedDogs.Length, resultList.Count);

            for (int i = 0; i < expectedDogs.Length; i++)
            {
                Assert.Equal(expectedDogs[i].Name, resultList[i].Name);
                Assert.Equal(expectedDogs[i].Color, resultList[i].Color);
                Assert.Equal(expectedDogs[i].TailLength, resultList[i].TailLength);
                Assert.Equal(expectedDogs[i].Weight, resultList[i].Weight);
            }
        }

        [Fact]
        public async void GetAllDogs_ReturnsCorrectPagination_1_0()
        {
            _context = new DogsDbContext(_options);
            await AddDogsAsync(_context);

            _service = new DogsService(_context);

            await Assert.ThrowsAsync<IncorrectQueryArgumentException>(async () => await _service.GetAllDogsAsync("name", "asc", 1, 0));
        }

        [Fact]
        public async Task CreateNewDogAsync_ReturnsCorrectDisplayDogDTO()
        {
            _context = new DogsDbContext(_options);
            await AddDogsAsync(_context);

            _service = new DogsService(_context);

            var newDog = new CreateDogDTO
            {
                Name = "Charlie",
                Color = "gold",
                TailLength = 17,
                Weight = 25
            };

            var result = await _service.CreateNewDogAsync(newDog);

            Assert.NotNull(result);
            Assert.Equal("Charlie", result.Name);
            Assert.Equal("gold", result.Color);
            Assert.Equal(17, result.TailLength);
            Assert.Equal(25, result.Weight);
        }

        [Fact]
        public async Task CreateNewDogAsync_ThrowsExceptionWhenNameAlreadyExists()
        {
            _context = new DogsDbContext(_options);
            await AddDogsAsync(_context);

            _service = new DogsService(_context);

            _context.Dogs.Add(new DogEntity
            {
                Name = "Neo",
                Color = "red",
                TailLength = 15,
                Weight = 20
            });

            await _context.SaveChangesAsync();

            var service = new DogsService(_context);

            var duplicateDog = new CreateDogDTO
            {
                Name = "Neo",
                Color = "black",
                TailLength = 10,
                Weight = 18
            };

            await Assert.ThrowsAsync<IncorrectObjectParameterException>(async () => await service.CreateNewDogAsync(duplicateDog));
        }

    }
}
