namespace ClinicSystem.Application.DTOs.Common;

public class PagedRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? OrderBy { get; set; }
    public bool Ascending { get; set; } = true;
}