using System.Security.Cryptography;
using System.Text.Json;
using HomeworkAssignment.Services.Abstractions;

namespace HomeworkAssignment.Services;

public class CacheKeyManager : ICacheKeyManager
{
    public string EnrollmentListGroup(Guid userId) =>
        $"enrollment:list:{userId}";
    public string CourseListGroup(Guid userId)
        => $"course:list:{userId}";
    public string CourseSingleGroup(Guid courseId)
        => $"course:single:{courseId}";
    
    public string CourseList(Guid userId, object filter)
        => $"course:list:{userId}:{ComputeHash(filter)}";
    public string CourseOwned(Guid userId)
        => $"course:owned:{userId}";

    public string CourseSingle(Guid courseId)
        => $"course:single:{courseId}";

    public string CourseAttachments(Guid courseId)
        => $"course:attachments:{courseId}";
    
    public string EnrollmentList(Guid userId)
        => $"enrollment:list:{userId}";
    public string Enrollment(Guid userId, Guid courseId)
        => $"enrollment:single:{userId}:{courseId}";
    
    private static string ComputeHash(object obj)
    {
        var json = JsonSerializer.Serialize(obj);
        var hashBytes = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(json));
        return Convert.ToBase64String(hashBytes)
            .Replace("/", "_")
            .Replace("+", "-")[..12]; 
    }
}