namespace Paint.Grid.Interaction
{
    public interface iInteractableObject 
    {
        bool IsSelected { get; }

        void Select();
        void Unselect();
    }
}
