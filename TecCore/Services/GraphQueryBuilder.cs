namespace TecCore.Services;

public class GraphQueryBuilder
{
    private readonly List<string> _filters = [];

    /// <summary>
    /// Filters users by domain.
    /// </summary>
    public GraphQueryBuilder FilterByEmailDomain(string domain)
    {
        _filters.Add($"endswith(mail, '{domain}')");
        return this;
    }

    /// <summary>
    /// Filters users by surname.
    /// </summary>
    public GraphQueryBuilder FilterBySurname(string surname)
    {
        _filters.Add($"surname eq '{surname}'");
        return this;
    }

    /// <summary>
    /// Filters users by job title.
    /// </summary>
    public GraphQueryBuilder FilterByJobTitle(string jobTitle)
    {
        _filters.Add($"jobTitle eq '{jobTitle}'");
        return this;
    }

    /// <summary>
    /// Filters users by department.
    /// </summary>
    public GraphQueryBuilder FilterByDepartment(string department)
    {
        _filters.Add($"department eq '{department}'");
        return this;
    }

    /// <summary>
    /// Filters users by multiple years.
    /// </summary>
    public GraphQueryBuilder FilterByYearsRange(int startYear, int endYear)
    {
        var yearConditions = Enumerable.Range(startYear, endYear - startYear + 1)
            .Select(year => $"department eq '{year}'");
        _filters.Add($"({string.Join(" or ", yearConditions)})");
        return this;
    }

    /// <summary>
    /// Builds the final OData filter query.
    /// </summary>
    public string Build()
    {
        return string.Join(" and ", _filters);
    }
}