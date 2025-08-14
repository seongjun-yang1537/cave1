namespace Ingame
{
    public interface IEntityInteractable
    {
        public string Description { get; }
        public void Interact(EntityController entity, PlayerController playerController);
    }
}