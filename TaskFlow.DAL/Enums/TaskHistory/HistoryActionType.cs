namespace TaskFlow.DAL.Enums.Task
{
    public enum HistoryActionType
    {
        StatusChange = 0,
        AssignmentChange = 1,
        PriorityChange = 2,
        DescriptionUpdate = 3,
        DeadlineChange = 4,
        ProgressUpdate = 5,
        CommentAdded = 6,
        AttachmentAdded = 7,
        TimeLogged = 8
    }
}