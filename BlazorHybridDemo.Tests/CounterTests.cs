using System.Diagnostics.Metrics;
using BlazorHybridDemo.Components.Pages;
using Bunit;

namespace BlazorHybridDemo.Tests;

public class CounterTests : TestContext
{
    [Fact]
    public void ComponentWorks()
    {
        // Arrange
        var ctx = new TestContext();
        var cut = ctx.RenderComponent<Counter>();

        // Act
        cut.Find("button").Click();

        // Assert
        cut.Find("p").MarkupMatches("<p role=\"status\">Current count: 1</p>");
    }

    [Fact]
    public void Counter_Should_Increment_When_Button_Clicked()
    {
        // Arrange
        var cut = RenderComponent<Counter>();

        // Act - Click the button once
        cut.Find("button").Click();

        // Assert - Count should be 1
        var paragraph = cut.Find("p[role='status']");
        paragraph.MarkupMatches("<p role=\"status\">Current count: 1</p>");
    }

    [Fact]
    public void Counter_Should_Start_At_Zero()
    {
        // Arrange & Act
        var cut = RenderComponent<Counter>();

        // Assert
        var paragraph = cut.Find("p[role='status']");
        paragraph.MarkupMatches("<p role=\"status\">Current count: 0</p>");
    }

    [Fact]
    public void Counter_Should_Increment_Multiple_Times()
    {
        // Arrange
        var cut = RenderComponent<Counter>();

        // Act - Click the button 5 times
        for (int i = 0; i < 5; i++)
        {
            cut.Find("button").Click();
        }

        // Assert - Count should be 5
        var paragraph = cut.Find("p[role='status']");
        paragraph.MarkupMatches("<p role=\"status\">Current count: 5</p>");
    }
}