using Xunit;
using LodgeMasterWeb.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace LodgeMaster.Tests;

public class HomeControllerTests
{
    [Fact]
    public void Index_ReturnsViewResult()
    {
        // Arrange
        var controller = new HomeController();

        // Act
        var result = controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
    }
    [Fact]
    public void TestErrorUnitResult()
    {
        // Arrange
        var controller = new HomeController();

        // Act
        var result = controller.TestErrorUnit(10,2);
        var excepted=20;
        // Assert
        Assert.Equal(result,excepted);
    }
}