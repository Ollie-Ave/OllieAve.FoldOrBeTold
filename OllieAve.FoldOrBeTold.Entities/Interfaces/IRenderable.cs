namespace OllieAve.FoldOrBeTold.Entities.Interfaces;

public interface IRenderable
{
    void Render();

    int RenderingOrder { get; }
}

