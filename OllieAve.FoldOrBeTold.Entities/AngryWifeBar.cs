using System.Numerics;
using OllieAve.FoldOrBeTold.Entities.Interfaces;
using OllieAve.FoldOrBeTold.Entities.Models;
using Raylib_cs;

namespace OllieAve.FoldOrBeTold.Entities;

public class AngryWifeBar : EntityBase, IEntity, IUiRenderable
{
    private const string textureName = "WashingBasket.png";

    private Vector2 position;
    private Shader shader;

    public void Render()
    {
    }

    public void Update(UpdateState updateState)
    {
        throw new NotImplementedException();
    }
}
