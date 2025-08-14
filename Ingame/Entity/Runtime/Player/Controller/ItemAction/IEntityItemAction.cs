using System.Collections;
using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    public interface IEntityItemAction
    {
        public void UsePrimaryDown(PawnController pawnController, ItemModel itemModel) { }
        public void UsePrimary(PawnController pawnController, ItemModel itemModel) { }
        public void UsePrimaryUp(PawnController pawnController, ItemModel itemModel) { }

        public void UseSecondaryDown(PawnController pawnController, ItemModel itemModel) { }
        public void UseSecondary(PawnController pawnController, ItemModel itemModel) { }
        public void UseSecondaryUp(PawnController pawnController, ItemModel itemModel) { }
    }

    public class HPPortionItemAction : IEntityItemAction
    {
        public void UseSecondaryDown(PawnController pawnController, ItemModel itemModel)
        {
            ItemModel removeItemModel = ItemModelFactory.Create(new ItemModelState { itemID = itemModel.itemID, count = 1 });
            pawnController.DiscardItem(removeItemModel);

            pawnController.HealRatio(0.2f);
        }
    }

    public class WandItemAction : IEntityItemAction
    {
        public void UseSecondaryDown(PawnController pawnController, ItemModel itemModel)
        {
            EntityController aimtargetController = pawnController.GetAimtarget();
            if (aimtargetController == null) return;

            ProjectileContext context = new ProjectileContext
                .Builder(EntityType.PRJ_StunSphere)
                .SetOwner(pawnController.agentModel)
                .SetFollowTarget(aimtargetController.transform)
                .SetSpeed(10f)
                .SetLifeTime(3f)
                .SetDamage(1f)
                .HitHandler(new StunProjectileHitHandler())
                .Build();

            ProjectileController controller = ProjectileSystem.SpawnProjectile(context);
            controller.transform.position = pawnController.transform.position;
        }
    }

    public class DefaultItemAction : IEntityItemAction
    {

    }

    public class SwordItemAction : IEntityItemAction
    {

    }

    public class BowItemAction : IEntityItemAction
    {

    }

    public class ShovelItemAction : IEntityItemAction
    {
        public void UsePrimaryDown(PawnController pawnController, ItemModel itemModel)
        {
            var cam = Camera.main;
            if (cam == null) return;

            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            int landscapeLayer = 1 << LayerMask.NameToLayer("Landscape");

            if (Physics.Raycast(ray, out RaycastHit hit, 10f, landscapeLayer))
            {
                EntityServiceLocator.WorldHandler.Dig(new PSphere(hit.point, 1));
            }
        }
    }

    public class PickaxeItemAction : IEntityItemAction
    {
    }

    public class RopeItemAction : IEntityItemAction
    {
        public void UseSecondaryDown(PawnController pawnController, ItemModel itemModel)
        {
            GrapplingController grapplingController =
                pawnController.handController.Hand<GrapplingController>();

            if (grapplingController != null)
                grapplingController.UseSecondaryDown(pawnController);
        }

        public void UseSecondaryUp(PawnController pawnController, ItemModel itemModel)
        {
            GrapplingController grapplingController =
                pawnController.handController.Hand<GrapplingController>();

            if (grapplingController != null)
                grapplingController.UseSecondaryUp(pawnController);
        }
    }
}