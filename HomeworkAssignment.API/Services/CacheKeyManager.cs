using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using HomeworkAssignment.Services.Abstractions;

public class CacheKeyManager : ICacheKeyManager
{
    // === CATEGORY ===
    public string CategoryListGroup()
    {
        return "group:category:list";
    }

    public string CategoryList()
    {
        return "category:list";
    }

    // === COURSE ===
    public string CourseListGroup(Guid userId)
    {
        return $"group:course:list:{userId}";
    }

    public string CourseList(Guid userId, object filter)
    {
        return $"course:list:{userId}:{ComputeHash(filter)}";
    }

    public string CourseSingleGroup(Guid courseId)
    {
        return $"group:course:{courseId}";
    }

    public string CourseSingle(Guid courseId)
    {
        return $"course:{courseId}";
    }

    public string CourseSingleStudent(Guid userId, Guid courseId)
    {
        return $"course:{courseId}:student:{userId}";
    }

    public string CourseOwned(Guid userId)
    {
        return $"course:owned:{userId}";
    }

    public string CourseAttachments(Guid courseId)
    {
        return $"course:{courseId}:attachments";
    }

    // === ENROLLMENT ===
    public string EnrollmentListGroup(Guid userId)
    {
        return $"group:enrollment:list:{userId}";
    }

    public string EnrollmentsAnalytics(Guid userId)
    {
        return $"enrollment:analytics:{userId}";
    }

    public string Enrollment(Guid userId, Guid courseId)
    {
        return $"enrollment:{userId}:{courseId}";
    }

    // === CHAPTER ===
    public string ChapterSingleGroup(Guid courseId, Guid chapterId)
    {
        return $"group:course:{courseId}:chapter:{chapterId}";
    }
    
    public string ChapterList(Guid courseId)
    {
        return $"course:{courseId}:chapter:list";
    }

    public string ChapterSingle(Guid courseId, Guid chapterId)
    {
        return $"course:{courseId}:chapter:{chapterId}";
    }

    public string ChapterFirst(Guid courseId)
    {
        return $"course:{courseId}:chapter:first";
    }

    public string ChapterNext(Guid courseId, Guid chapterId)
    {
        return $"course:{courseId}:chapter:{chapterId}:next";
    }

    public string ChapterAttachments(Guid courseId, Guid chapterId)
    {
        return $"course:{courseId}:chapter:{chapterId}:attachments";
    }

    // === PROGRESS ===
    public string ChapterProgress(Guid userId, Guid courseId, Guid chapterId)
    {
        return $"progress:{userId}:course:{courseId}:chapter:{chapterId}";
    }

    // === ASSIGNMENT ===
    public string AssignmentSingleGroup(Guid courseId, Guid chapterId, Guid assignmentId)
    {
        return $"group:course:{courseId}:chapter:{chapterId}:assignment:{assignmentId}";
    }

    public string AssignmentSingle(Guid courseId, Guid chapterId, Guid assignmentId)
    {
        return $"course:{courseId}:chapter:{chapterId}:assignment:{assignmentId}";
    }

    public string AssignmentList(Guid courseId, Guid chapterId)
    {
        return $"course:{courseId}:chapter:{chapterId}:assignments";
    }
    
    public string AssignmentAnalytics(Guid userId, Guid courseId, Guid chapterId, Guid assignmentId)
    {
        return $"assignment:analytics:{userId}:course:{courseId}:chapter:{chapterId}:assignments";
    }

    public string AssignmentProgress(Guid userId, Guid courseId, Guid chapterId, Guid assignmentId)
    {
        return $"progress:{userId}:course:{courseId}:chapter:{chapterId}:assignment:{assignmentId}";
    }

    // === ATTEMPT ===
    public string AttemptList(Guid courseId, Guid chapterId, Guid assignmentId)
    {
        return $"course:{courseId}:chapter:{chapterId}:assignment:{assignmentId}:attempts";
    }

    // === HASHING ===
    private static string ComputeHash(object obj)
    {
        var json = JsonSerializer.Serialize(obj);
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(json));
        return Convert.ToBase64String(hashBytes)
            .Replace("/", "_")
            .Replace("+", "-")[..12];
    }
}