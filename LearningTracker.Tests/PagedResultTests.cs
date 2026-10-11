using LearningTracker.Api.Dto;

namespace LearningTracker.Tests;

public class PagedResultTests
{
    [Theory]
    [InlineData(13, 10, 2)] // всего, размер страницы, ожидаемое число страниц
    [InlineData(0, 10, 0)]
    [InlineData(10, 10, 1)]
    public void TotalPages_ReturnsCeilingOfDivision(int total, int pageSize, int expected)
    {
        var paged = new PagedResult<int> { TotalCount = total, PageSize = pageSize};

        Assert.Equal(expected, paged.TotalPages);
    }
}
