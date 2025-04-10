namespace HomeworkAssignment.Services.Abstractions;

public interface ICacheKeyManager
{
    string CourseList(Guid userId, object filter);
    string CourseListGroup(Guid userId);
    string CourseOwned(Guid userId);
    string CourseSingle(Guid courseId);
    string CourseSingleGroup(Guid courseId);
    string CourseAttachments(Guid courseId);
    string EnrollmentListGroup(Guid userId);
    string Enrollment(Guid userId, Guid courseId);
    string EnrollmentList(Guid userId);
}