using FluentAssertions;
using Moq;
using StokYonetimAPI.Application.DTOs;
using StokYonetimAPI.Application.Interfaces;
using StokYonetimAPI.Application.Parameters;
using StokYonetimAPI.Application.Services;
using StokYonetimAPI.Domain;
using StokYonetimAPI.Tests;

namespace StokYonetimAPI.Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repoMock;
    private readonly ProductService _sut;

    public ProductServiceTests()
    {
        _repoMock = new Mock<IProductRepository>();
        _sut = new ProductService(_repoMock.Object);
    }


    [Fact]
    public async Task CreateAsync_ValidInput()
    {
        var dto = ProductTestData.ValidCreateDto();
        var createdProduct = new Product { Id = dto.Id, Name = dto.Name, StockQuantity = dto.StockQuantity, UnitPrice = dto.UnitPrice, UserID = 1 };

        _repoMock.Setup(r => r.CreateAsync(It.IsAny<Product>())).ReturnsAsync(createdProduct);

        var result = await _sut.CreateAsync(dto, userId: 1);

        result.Should().NotBeNull();
        result.Name.Should().Be("Orange");
        result.UserID.Should().Be(1);

    }

    [Fact]
    public async Task CreateAsync_EmptyName()
    {
        var dto = ProductTestData.ValidCreateDto();
        dto.Name = "";

        Func<Task> act = () => _sut.CreateAsync(dto, userId: 1);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CreateAsync_NegativePrice()
    {
        var dto = ProductTestData.ValidCreateDto();
        dto.UnitPrice = -1;
        Func<Task> act = () => _sut.CreateAsync(dto, userId: 1);

        await act.Should().ThrowAsync<ArgumentException>();

    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public async Task CreateAsync_InvalidID(int invalidId)
    {
        var dto = ProductTestData.ValidCreateDto();
        dto.Id = invalidId;
        Func<Task> act = () => _sut.CreateAsync(dto, userId: 1);
    }

    [Fact]
    public async Task GetAllAsync()
    {
        var parameters = new ProductParameters { PageNumber = 1, PageSize = 10 };
        _repoMock.Setup(r => r.GetAllAsync(It.IsAny<ProductParameters>(), It.IsAny<int?>())).ReturnsAsync(ProductTestData.GetPagedProducts());

        var result = await _sut.GetAllAsync(parameters, userId: null);

        result.Items.Should().HaveCount(3);
        result.TotalCount.Should().Be(3);
        result.Items.First().Name.Should().Be("Apple");
    }
    [Fact]
    public async Task GetByIdAsync_ExistingId()
    {
        var product = ProductTestData.GetSampleProducts().First();
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

        var result = await _sut.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Apple");
    }
    [Fact]
    public async Task GetTaskAsync_NotFound() {
        _repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product?)null);
        var result = await _sut.GetByIdAsync(999);
        result.Should().BeNull();
    }


    [Fact]
    public async Task DeleteAsync_ExistingId()
    {
        _repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);
        var result = await _sut.DeleteAsync(1);
        result.Should().BeTrue();
        _repoMock.Verify(r => r.DeleteAsync(1), Times.Once());

    }
    [Fact]
    public async Task DeleteAsync_InvalidId()
    {
        Func<Task> act = () => _sut.DeleteAsync(-1);
        await act.Should().ThrowAsync<ArgumentException>();
    }
}

