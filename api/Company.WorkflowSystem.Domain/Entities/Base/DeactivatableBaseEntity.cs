namespace Company.WorkflowSystem.Domain.Entities
{
    public abstract class DeactivatableBaseEntity : BaseEntity
    {
        public bool Active { get; set; } = true;
    }
}
