namespace Company.WorkflowSystem.Domain.Entities
{
    public class SubFunctionalityInUserRole : BaseEntity
    {
        public int FunctionalityInUserRoleId { get; set; }
        public FunctionalityInUserRole FunctionalityInUserRole { get; set; }
        public int SubFunctionalityId { get; set; }
        public SubFunctionality SubFunctionality { get; set; }
    }
}
