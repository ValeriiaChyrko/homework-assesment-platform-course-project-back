using System.Security.Cryptography;
using System.Text.Json;
using HomeworkAssignment.Services.Abstractions;

namespace HomeworkAssignment.Services;

public class CacheKeyManager : ICacheKeyManager
{
    public string CategoryListGroup()
        => $"category:list";
    public string EnrollmentListGroup(Guid userId) =>
        $"enrollment:list:{userId}";
    public string CourseListGroup(Guid userId)
        => $"course:list:{userId}";
    public string CourseSingleGroup(Guid courseId)
        => $"course:single:{courseId}";
    public string ChapterSingleGroup(Guid courseId, Guid chapterId)
        => $"course:list:{courseId}:chapters:{chapterId}";
    public string AssignmentSingleGroup(Guid courseId, Guid chapterId, Guid assignmentId)
        => $"course:list:{courseId}:chapters:{chapterId}:assignments:{assignmentId}";
    
    public string CategoryList()
        => $"category:list";
    
    public string CourseList(Guid userId, object filter)
        => $"course:list:{userId}:{ComputeHash(filter)}";
    public string CourseOwned(Guid userId)
        => $"course:list:owned:{userId}";
    public string CourseSingle(Guid courseId)
        => $"course:single:{courseId}";
    public string CourseAttachments(Guid courseId)
        => $"course:list:attachments:{courseId}";
    
    public string EnrollmentList(Guid userId)
        => $"enrollment:list:{userId}";
    public string Enrollment(Guid userId, Guid courseId)
        => $"enrollment:single:{userId}:{courseId}";
    
    public string ChapterSingle(Guid courseId, Guid chapterId)
        => $"course:single:{courseId}:chapter:single:{chapterId}";
    public string ChapterFirst(Guid courseId)
        => $"course:single:{courseId}:chapters:first";
    public string ChapterAttachments(Guid courseId, Guid chapterId)
        => $"course:single:{courseId}:chapter:single:attachments:{chapterId}";
    
    public string ChapterProgress(Guid userId, Guid courseId, Guid chapterId)
        => $"progress:single:{userId}:{courseId}:{chapterId}";
    
    public string AssignmentSingle(Guid courseId, Guid chapterId, Guid assignmentId)
        => $"course:single:{courseId}:chapter:single:{chapterId}:assignment:single:{assignmentId}";
    public string AssignmentList(Guid courseId, Guid chapterId)
        => $"course:single:{courseId}:chapter:single:{chapterId}:assignments";
    
    public string AssignmentProgress(Guid userId, Guid courseId, Guid chapterId, Guid assignmentId)
        => $"progress:single:{userId}:{courseId}:{chapterId}:{assignmentId}";
    
    private static string ComputeHash(object obj)
    {
        var json = JsonSerializer.Serialize(obj);
        var hashBytes = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(json));
        return Convert.ToBase64String(hashBytes)
            .Replace("/", "_")
            .Replace("+", "-")[..12]; 
    }
}